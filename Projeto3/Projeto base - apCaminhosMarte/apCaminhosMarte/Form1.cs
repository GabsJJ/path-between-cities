using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace apCaminhosMarte
{
    public partial class Form1 : Form
    {
        Cidade[] dadosCidade = new Cidade[10000];
        int qtosDados = 0;
        Arvore<Cidade> arvore;
        Caminho[,] matAdjacencias;
        PilhaLista<PilhaLista<Caminho>> caminhosDescobertos;

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Buscar caminhos entre cidades selecionadas");
            BuscarCaminhos(lsbOrigem.SelectedIndex, lsbDestino.SelectedIndex);
        }
        void BuscarCaminhos(int cdOrigem, int cdDestino)
        {
            if (cdOrigem != cdDestino)
            {
                bool achouTodos = false;
                bool[] passouCidade = new bool[qtosDados + 1];
                int c = 1;
                PilhaLista<Caminho> caminhos = new PilhaLista<Caminho>();
                caminhosDescobertos = new PilhaLista<PilhaLista<Caminho>>();

                for (int i = 0; i < qtosDados; i++)
                    passouCidade[i] = false;

                do
                {
                    if (cdOrigem == cdDestino || c >= qtosDados)
                    {
                        if (caminhos.EstaVazia())
                        {
                            achouTodos = true;
                        }
                        else
                        {
                            if (cdOrigem == cdDestino)
                                caminhosDescobertos.Empilhar(caminhos.Copia());
                            Caminho caminho = caminhos.Desempilhar();
                            cdOrigem = caminho.IdCidadeOrigem;
                            c = caminho.IdCidadeDestino + 1;
                        }
                    }
                    if (c < qtosDados && matAdjacencias[cdOrigem, c] != null)
                    {
                        if (passouCidade[c] != true)
                        {
                            passouCidade[cdOrigem] = true;
                            caminhos.Empilhar(matAdjacencias[cdOrigem, c]);
                            cdOrigem = c;
                            c = 0; // recebe 0, pois incrementa logo após
                        }
                    }
                    c++;
                }
                while (!achouTodos);
                ExibirCaminhos(caminhosDescobertos);
            }
            else
                if (cdOrigem != -1 && cdDestino != -1)
                MessageBox.Show("Você já está na cidade!");

        }
        void ExibirCaminhos(PilhaLista<PilhaLista<Caminho>> caminhos)
        {
            dgvCaminhos.Rows.Clear();
            dgvCaminhos.RowCount = caminhos.Tamanho();
            dgvCaminhos.ColumnCount = 10;
            var pilhaCopia = caminhos.Copia();
            Color[] cores = { Color.Black, Color.Blue, Color.Red, Color.Green, Color.Orange, Color.Yellow };
            
            for (int i = 0; i < dgvCaminhos.RowCount; i++)
            {
                PilhaLista<Caminho> pcAtual = caminhos.Desempilhar();
                var aux = new PilhaLista<Caminho>();
                
                while (!pcAtual.EstaVazia())
                    aux.Empilhar(pcAtual.Desempilhar());

                int c = 0;
                Caminho cAtual = null;
                dgvCaminhos.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                
                dgvCaminhos.Rows[i].DefaultCellStyle.BackColor = cores[i];
                while (!aux.EstaVazia())
                {
                    cAtual = aux.Desempilhar();
                    dgvCaminhos.Rows[i].Cells[c].Value = dadosCidade[cAtual.IdCidadeOrigem].NomeCidade;
                    c++;
                }
                dgvCaminhos.Rows[i].Cells[c].Value = dadosCidade[cAtual.IdCidadeDestino].NomeCidade;
            }
            caminhosDescobertos = pilhaCopia;
            pbMapa.Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            caminhosDescobertos = new PilhaLista<PilhaLista<Caminho>>();
            arvore = new Arvore<Cidade>();
            if (dlgAbrir.ShowDialog() == DialogResult.OK)
            {
                var arq = new StreamReader(dlgAbrir.FileName, Encoding.UTF7);
                while (!arq.EndOfStream)
                    dadosCidade[qtosDados++] = new Cidade(arq.ReadLine());
                arq.Close();

                NoArvore<Cidade> primeiroNo = null;
                ParticionarVetorEmArvore(0, qtosDados - 1, ref primeiroNo);
                arvore.Raiz = primeiroNo;

                pnlArvore.Invalidate();
                bool balanceada = true;
                lblAltura.Text = "Altura : " + Convert.ToString(
                                     arvore.Altura(ref balanceada));
                chkBalanceada.Checked = balanceada;
            }
            for (int i = 0; i < qtosDados; i++)
            {
                lsbOrigem.Items.Add($"{dadosCidade[i].IdCidade} - {dadosCidade[i].NomeCidade}");
                lsbDestino.Items.Add($"{dadosCidade[i].IdCidade} - {dadosCidade[i].NomeCidade}");
            }

            CriarGrafo();
            pbMapa.Invalidate();
        }

        void CriarGrafo()
        {
            matAdjacencias = new Caminho[qtosDados, qtosDados];

            var arquivo = new StreamReader(@"CaminhosEntreCidadesMarte.txt");

            while (!arquivo.EndOfStream)
            {
                Caminho c = new Caminho(arquivo.ReadLine());
                matAdjacencias[c.IdCidadeOrigem, c.IdCidadeDestino] = c;
            }
        }

        void ParticionarVetorEmArvore(int inicio, int fim, ref NoArvore<Cidade> noAtual)
        {
            if (inicio > fim)   // saída da recursão
                noAtual = null;
            else
            {
                int meio = (inicio + fim) / 2;
                noAtual = new NoArvore<Cidade>(dadosCidade[meio], null, null);
                NoArvore<Cidade> esq = null;
                ParticionarVetorEmArvore(inicio, meio - 1, ref esq);
                noAtual.Esq = esq;
                NoArvore<Cidade> dir = null;
                ParticionarVetorEmArvore(meio + 1, fim, ref dir);
                noAtual.Dir = dir;
            }
        }

        void DesenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
                       int x, int y, double angulo, double incremento,
                       double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);

                DesenhaArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                                                 incremento * 0.60, comprimento * 0.8, g);
                DesenhaArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
                                                  incremento * 0.60, comprimento * 0.8, g);

                SolidBrush preenchimento = new SolidBrush(Color.Blue);
                g.FillEllipse(preenchimento, xf - 25, yf - 15, 40, 40);
                g.DrawString(Convert.ToString(raiz.Info.IdCidade), new Font("Comic Sans", 12),
                              new SolidBrush(Color.Yellow), xf - 15, yf - 10);
                g.DrawString(Convert.ToString(raiz.Info.NomeCidade), new Font("Comic Sans", 12),
                              new SolidBrush(Color.Black), xf - 35, yf + 40);
            }
        }

        void DesenharPontos(NoArvore<Cidade> atual, SolidBrush preenchimento, Pen caneta, Graphics g)
        {
            int x = 0;
            int y = 0;
            if (atual != null)
            {
                x = Convert.ToInt32((atual.Info.CoordenadaX * pbMapa.Width) / 4096);
                y = Convert.ToInt32((atual.Info.CoordenadaY * pbMapa.Height) / 2048);
                g.FillEllipse(preenchimento, x, y, 10, 10);
                g.DrawString(atual.Info.NomeCidade, new Font("Comic Sans ms", 10),
                              new SolidBrush(Color.Black), x - 10, y + 15);

                DesenharPontos(atual.Esq, preenchimento, caneta, g);
                DesenharPontos(atual.Dir, preenchimento, caneta, g);
            }
        }
        void DesenharCaminhos(PilhaLista<PilhaLista<Caminho>> caminhosDescobertos, SolidBrush preenchimento, Pen caneta, Graphics g)
        {
            int xOrigem = 0;
            int yOrigem = 0;
            int xDestino = 0;
            int yDestino = 0;
            Color[] cores = { Color.Black, Color.Blue, Color.Red, Color.Green, Color.Orange, Color.Yellow};
            int i = 0;
            while (!caminhosDescobertos.EstaVazia())
            {
                var umCaminho = caminhosDescobertos.Desempilhar();
                preenchimento = new SolidBrush(cores[i]);
                caneta = new Pen(cores[i],3);
                while (!umCaminho.EstaVazia())
                {
                    var caminho = umCaminho.Desempilhar();
                    xOrigem = (dadosCidade[caminho.IdCidadeOrigem].CoordenadaX * pbMapa.Width) / 4096;
                    yOrigem = (dadosCidade[caminho.IdCidadeOrigem].CoordenadaY * pbMapa.Height) / 2048;
                    xDestino = (dadosCidade[caminho.IdCidadeDestino].CoordenadaX * pbMapa.Width) / 4096;
                    yDestino = (dadosCidade[caminho.IdCidadeDestino].CoordenadaY * pbMapa.Height) / 2048;

                    g.DrawLine(caneta, xOrigem, yOrigem, xDestino, yDestino);
                }
                i++;
            }
            
        }
        void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DesenhaArvore(true, arvore.Raiz, (int)pnlArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 425, g);
            bool balanceada = false;
            lblAltura.Text = "Altura : " + Convert.ToString(
                           arvore.Altura(ref balanceada));
            chkBalanceada.Checked = balanceada;
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush preenchimento = new SolidBrush(Color.Black);
            Pen caneta = new Pen(Color.Black);
            DesenharPontos(arvore.Raiz, preenchimento, caneta, g);
            if (!caminhosDescobertos.EstaVazia())
                DesenharCaminhos(caminhosDescobertos, preenchimento, caneta, g);
        }

        private void pbMapa_Resize(object sender, EventArgs e)
        {
            BuscarCaminhos(lsbOrigem.SelectedIndex, lsbDestino.SelectedIndex);
        }
    }
}
