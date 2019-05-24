using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

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

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {
           
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
                var arq = new StreamReader(dlgAbrir.FileName);
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

        private void desenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
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
                // sleep(100);
                desenhaArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                                                 incremento * 0.60, comprimento * 0.8, g);
                desenhaArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
                                                  incremento * 0.60, comprimento * 0.8, g);
                // sleep(100);
                SolidBrush preenchimento = new SolidBrush(Color.Blue);
                g.FillEllipse(preenchimento, xf - 25, yf - 15, 40, 40);
                g.DrawString(Convert.ToString(raiz.Info.IdCidade), new Font("Comic Sans", 12),
                              new SolidBrush(Color.Yellow), xf - 15, yf - 10);
                g.DrawString(Convert.ToString(raiz.Info.NomeCidade), new Font("Comic Sans", 12),
                              new SolidBrush(Color.Black), xf - 35, yf + 40);
            }
        }

        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            desenhaArvore(true, arvore.Raiz, (int)pnlArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 425, g);
            bool balanceada = false;
            lblAltura.Text = "Altura : " + Convert.ToString(
                           arvore.Altura(ref balanceada));
            chkBalanceada.Checked = balanceada;
        }
    }
}
