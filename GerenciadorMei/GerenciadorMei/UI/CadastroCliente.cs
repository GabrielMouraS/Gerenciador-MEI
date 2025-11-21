using System;
using System.Windows.Forms;
using GerenciadorMei.Models;
using GerenciadorMei.Repositories;

namespace GerenciadorMei.UI
{
    public partial class CadastroCliente : Form
    {
        private readonly ClienteRepository _repo = new ClienteRepository();
        private int _clienteId = 0; // 0 = Novo, >0 = Edição

        // --- CONSTRUTOR 1: NOVO CLIENTE ---
        public CadastroCliente()
        {
            InitializeComponent();
            ConfigurarEventos();
            lblTitulo.Text = "Novo Cliente";
        }

        // --- CONSTRUTOR 2: EDITAR CLIENTE ---
        public CadastroCliente(Cliente cliente)
        {
            InitializeComponent();
            ConfigurarEventos();

            // Preenche os campos com os dados existentes
            _clienteId = cliente.Id;
            lblTitulo.Text = "Editar Cliente";

            txtNome.Text = cliente.Nome;
            txtTelefone.Text = cliente.Telefone;
            txtEmail.Text = cliente.Email;
            txtCpf.Text = cliente.Cpf;
            txtCnpj.Text = cliente.Cnpj;
            txtEndereco.Text = cliente.Endereco;

            // Lógica da Data de Nascimento
            if (cliente.DataNascimento != null)
            {
                dtpDataNasc.Value = cliente.DataNascimento.Value;
                checkSemData.Checked = false;
            }
            else
            {
                checkSemData.Checked = true;
                dtpDataNasc.Enabled = false;
            }
        }

        // --- LIGA OS BOTÕES AO CÓDIGO ---
        private void ConfigurarEventos()
        {
            // Vincula o clique dos botões às funções abaixo
            btnSalvar.Click += BtnSalvar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            // Lógica visual do Checkbox de Data
            checkSemData.CheckedChanged += CheckSemData_CheckedChanged;
        }

        // --- BOTÃO SALVAR ---
        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            // Validação simples
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O campo Nome é obrigatório.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            // Cria o objeto com os dados da tela
            var cliente = new Cliente
            {
                Id = _clienteId,
                Nome = txtNome.Text,
                Telefone = txtTelefone.Text,
                Email = txtEmail.Text,
                Cpf = txtCpf.Text,
                Cnpj = txtCnpj.Text,
                Endereco = txtEndereco.Text,
                // Se marcou "Não informar", salva como null. Senão, salva a data.
                DataNascimento = checkSemData.Checked ? (DateTime?)null : dtpDataNasc.Value
            };

            try
            {
                if (_clienteId == 0)
                {
                    _repo.Inserir(cliente); // Cria Novo
                }
                else
                {
                    _repo.Atualizar(cliente); // Atualiza Existente
                }

                // Fecha a tela e avisa que deu certo (OK)
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- BOTÃO CANCELAR ---
        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- CHECKBOX DA DATA ---
        private void CheckSemData_CheckedChanged(object sender, EventArgs e)
        {
            // Se marcou "sem data", desabilita o calendário
            dtpDataNasc.Enabled = !checkSemData.Checked;
        }
    }
}