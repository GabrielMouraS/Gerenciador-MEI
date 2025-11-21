using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GerenciadorMei.Models;
using GerenciadorMei.Repositories;

namespace GerenciadorMei.UI
{
    public partial class ListarProdutos : Form
    {
        
        private readonly ProdutoRepository _repo = new ProdutoRepository();

        private List<Produto> _listaOriginal;

        public ListarProdutos()
        {
            InitializeComponent();
            ConfigurarLogica();
        }

        private void ConfigurarLogica()
        {
           
            this.Load += ListarProdutos_Load;
            btnNovo.Click += BtnNovo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnExcluir.Click += BtnExcluir_Click;
            txtBusca.TextChanged += TxtBusca_TextChanged;
        }

        private void ListarProdutos_Load(object sender, EventArgs e)
        {
            CarregarDados();
        }

        private void CarregarDados()
        {
            try
            {
                // Busca tudo do banco
                _listaOriginal = _repo.GetAll();

                // Joga na tela
                gridProdutos.DataSource = _listaOriginal;

                // Gambiarra para o preco
                if (gridProdutos.Columns["Preco"] != null)
                {
                    gridProdutos.Columns["Preco"].DefaultCellStyle.Format = "C2";
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
            }
        }

        // --- BOTÃO NOVO ---
        private void BtnNovo_Click(object sender, EventArgs e)
        {
           
            CadastroProduto form = new CadastroProduto();

            
            if (form.ShowDialog() == DialogResult.OK)
            {
                CarregarDados();
            }
        }

        
        private void BtnEditar_Click(object sender, EventArgs e)
        {
           
            if (gridProdutos.SelectedRows.Count > 0)
            {
                
                var produtoSelecionado = (Produto)gridProdutos.SelectedRows[0].DataBoundItem;

               
                CadastroProduto form = new CadastroProduto(produtoSelecionado);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    CarregarDados();
                }
            }
            else
            {
                MessageBox.Show("Selecione um produto na lista para editar.", "Atenção");
            }
        }

        // --- BOTÃO EXCLUIR ---
        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (gridProdutos.SelectedRows.Count > 0)
            {
                var produto = (Produto)gridProdutos.SelectedRows[0].DataBoundItem;

                var resposta = MessageBox.Show(
                    $"Tem certeza que deseja excluir '{produto.Nome}'?",
                    "Excluir Produto",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (resposta == DialogResult.Yes)
                {
                    try
                    {
                        _repo.Excluir(produto.Id);
                        CarregarDados(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um produto para excluir.", "Atenção");
            }
        }

        
        private void TxtBusca_TextChanged(object sender, EventArgs e)
        {
            
            if (_listaOriginal == null) return;

            string filtro = txtBusca.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                
                gridProdutos.DataSource = _listaOriginal;
            }
            else
            {
               
                var listaFiltrada = _listaOriginal
                                    .Where(p => p.Nome.ToLower().Contains(filtro))
                                    .ToList();

                gridProdutos.DataSource = listaFiltrada;
            }
        }
    }
}