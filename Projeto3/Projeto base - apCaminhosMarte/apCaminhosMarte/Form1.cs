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

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Buscar caminhos entre cidades selecionadas");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

            foreach (Cidade ci in dadosCidade)
            {
                lsbOrigem.Items.Add($"{ci.IdCidade} - {ci.NomeCidade}");
                lsbDestino.Items.Add($"{ci.IdCidade} - {ci.NomeCidade}");
            }

            pbMapa.Invalidate();
            CriarGrafo();
        }

        void CriarGrafo()
        {
            int[,] matAdjacencias = new int[qtosDados, qtosDados];
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

        void DesenharPontos(Graphics g)
        {
            SolidBrush preenchimento = new SolidBrush(Color.Black);
            Pen caneta = new Pen(Color.Black);
            int x = 0;
            int y = 0;
            for(int i = 0; i < qtosDados; i++)
            {
                Cidade ci = dadosCidade[i];
                x = Convert.ToInt32((ci.CoordenadaX * pbMapa.Width) / 4096);
                y = Convert.ToInt32((ci.CoordenadaY * pbMapa.Height) / 2048);
                g.FillEllipse(preenchimento, x, y, 10, 10);
                g.DrawString(ci.NomeCidade, new Font("Comic Sans", 10),
                              new SolidBrush(Color.Black), x - 10, y + 15);
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
            DesenharPontos(g);
        }
    }
}
