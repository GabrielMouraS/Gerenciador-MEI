using System;
using System.Windows.Forms;
using GerenciadorMei.Models;
using GerenciadorMei.Repositories;
using System.Text.RegularExpressions; 

namespace GerenciadorMei.UI
{
    public partial class CadastroProduto : Form
    {
        private readonly ProdutoRepository _repo = new ProdutoRepository();
        private int _idProduto = 0; 

        
        public CadastroProduto()
        {
            InitializeComponent();
            ConfigurarEventos();
            lblTitulo.Text = "Novo Produto";
        }

        
        public CadastroProduto(Produto p)
        {
            InitializeComponent();
            ConfigurarEventos();

            
            _idProduto = p.Id;
            txtNome.Text = p.Nome;

            
            txtPreco.Text = p.Preco.ToString("C2");

            lblTitulo.Text = "Editar Produto";
        }

        private void ConfigurarEventos()
        {
            
            btnSalvar.Click += BtnSalvar_Click;
            btnCancelar.Click += (s, e) => Close();

           
            txtPreco.Leave += TxtPreco_Leave;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome do produto é obrigatório.");
                txtNome.Focus();
                return;
            }

           
            string apenasNumeros = Regex.Replace(txtPreco.Text, "[^0-9]", "");

            decimal precoFinal = 0;
            if (!string.IsNullOrEmpty(apenasNumeros))
            {
                
                precoFinal = decimal.Parse(apenasNumeros) / 100;
            }

           
            var produto = new Produto
            {
                Id = _idProduto,
                Nome = txtNome.Text,
                Preco = precoFinal
            };

            try
            {
                
                if (_idProduto == 0)
                {
                    _repo.Inserir(produto);
                }
                else
                {
                    _repo.Atualizar(produto); 
                }

                
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Novo Produto adiconado: ");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }
        }

       
        private void TxtPreco_Leave(object sender, EventArgs e)
        {
            string apenasNumeros = Regex.Replace(txtPreco.Text, "[^0-9]", "");

            if (!string.IsNullOrEmpty(apenasNumeros))
            {
                decimal valor = decimal.Parse(apenasNumeros) / 100;
                txtPreco.Text = valor.ToString("C2");
            }
        }
    }
}