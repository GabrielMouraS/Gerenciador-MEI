using System;
using System.Drawing; 
using System.IO;      
using System.Windows.Forms;

namespace GerenciadorMei.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.Resize += MainForm_Resize;

            
            sairToolStripMenuItem.Click += (s, e) => Application.Exit();

            
            escuroToolStripMenuItem.Click += (s, e) => AplicarTema("Escuro");
            claroToolStripMenuItem.Click += (s, e) => AplicarTema("Claro");

            cadastrarProdutoToolStripMenuItem.Click += cadastrarProdutoToolStripMenuItem_Click;
        }

        // --- EVENTOS DE CARREGAMENTO E VISUAL ---

        private void MainForm_Load(object sender, EventArgs e)
        {
            CentralizarLogo();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            CentralizarLogo();
        }

        private void CentralizarLogo()
        {
          
            if (panelCentral != null)
            {
                panelCentral.Left = (this.ClientSize.Width - panelCentral.Width) / 2;
                panelCentral.Top = (this.ClientSize.Height - panelCentral.Height) / 2;
            }
        }

        

        private void listarProdutoToolStripMenuItem_Click(object sender, EventArgs e)
        {
           ListarProdutos listarProdutos = new ListarProdutos();
            listarProdutos.ShowDialog();
        }

        private void cadastrarProdutoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroProduto cadastroProduto = new CadastroProduto();
            cadastroProduto.ShowDialog();
        }

        

        private void listarServiçosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TelaServicos telaServicos = new TelaServicos();
            telaServicos.ShowDialog();
        }

        private void novoServiçoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NovoServico novoServico = new NovoServico();
            novoServico.ShowDialog();
        }

        
        private void agendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NovoServico novoServico = new NovoServico();
            novoServico.ShowDialog();
        }

      

        private void cadastroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CadastroCliente cadastroCliente = new CadastroCliente();
            cadastroCliente.ShowDialog();
        }

        private void listarClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarClientes listarClientes = new ListarClientes();
            listarClientes.ShowDialog();
        }

        

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Selecione a pasta para salvar o Backup";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string dbOrigem = "gerenciador.db";
                        
                        string nomeArquivo = $"backup_mei_{DateTime.Now:yyyy-MM-dd_HH-mm}.db";
                        string caminhoDestino = Path.Combine(fbd.SelectedPath, nomeArquivo);

                        if (File.Exists(dbOrigem))
                        {
                            File.Copy(dbOrigem, caminhoDestino, true);
                            MessageBox.Show("Backup realizado com sucesso!\nLocal: " + caminhoDestino,
                                            "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Erro: Banco de dados não encontrado.",
                                            "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao fazer backup: " + ex.Message,
                                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

       

        
        private void label2_Click(object sender, EventArgs e) { }

        private void AplicarTema(string tema)
        {
            if (tema == "Escuro")
            {
                this.BackColor = Color.FromArgb(40, 40, 40);
                panelCentral.BackColor = Color.FromArgb(60, 60, 60);
                menuStrip1.BackColor = Color.Gray;
                menuStrip1.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = SystemColors.Desktop;
                panelCentral.BackColor = SystemColors.Control;
                menuStrip1.BackColor = SystemColors.Control;
                menuStrip1.ForeColor = Color.Black;
            }
        }
    }
}