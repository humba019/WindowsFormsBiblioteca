using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsBiblioteca
{
    public partial class Biblioteca : Form
    {
        MySqlConnection conMySQL = new MySqlConnection("server=localhost;user id=root;pwd='1234';database=biblioteca");
        MySqlCommand cmdMySQL = new MySqlCommand();
        MySqlDataReader reader;
        string status;


        public Biblioteca()
        {
            InitializeComponent();
            cmdMySQL.Connection = conMySQL;
        }
        void carregaListaLivros()
        {
            try
            {
                lvLivros.Items.Clear();
                if (txtPesquisar.Text == "")
                {
                    cmdMySQL.CommandText = "SELECT * FROM livro ORDER BY nomeLivro ASC";
                }
                else
                {
                    cmdMySQL.CommandText = "SELECT * FROM livro WHERE nomeLivro LIKE '" + txtPesquisar.Text + "%' ORDER BY nomeLivro ASC";
                }
                reader = cmdMySQL.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem list = new ListViewItem(reader[0].ToString());
                    list.SubItems.Add(reader[1].ToString());
                    list.SubItems.Add(reader[2].ToString());
                    list.SubItems.Add(reader[3].ToString());
                    lvLivros.Items.AddRange(new ListViewItem[] { list });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Biblioteca_Load(object sender, EventArgs e)
        {
            {
                try
                {
                    conMySQL.Open();
                    status = "novo";
                    carregaListaLivros();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            }
        }

        private void lvLivros_Click(object sender, EventArgs e)
        {
            try
            {
                txtCodigo.Text = lvLivros.Items[lvLivros.FocusedItem.Index].SubItems[0].Text;
                txtNome.Text = lvLivros.Items[lvLivros.FocusedItem.Index].SubItems[1].Text;
                txtAutor.Text = lvLivros.Items[lvLivros.FocusedItem.Index].SubItems[2].Text;
                txtEditora.Text = lvLivros.Items[lvLivros.FocusedItem.Index].SubItems[3].Text;
                status = "editar";
            }
            catch (Exception) { MessageBox.Show("Não existem registros na lista.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

        }
        private void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            carregaListaLivros();
        }


        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja encerrar a aplicação?", "Encerrar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    cmdMySQL.CommandText = "DELETE FROM livro WHERE idLivro='" + lvLivros.Items[lvLivros.FocusedItem.Index].Text + "'";
                    cmdMySQL.ExecuteNonQuery();
                    MessageBox.Show("Registro deletado com sucesso.", "Deletar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    carregaListaLivros();
                    btnLimpar.PerformClick();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }


        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNome.Text == "")
                {
                    MessageBox.Show("Informe o nome do livro.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNome.Focus();
                }

                else if (txtAutor.Text == "")
                {
                    MessageBox.Show("Informe o autor do livro.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAutor.Focus();
                }

                else if (txtEditora.Text == "")
                {
                    MessageBox.Show("Informe o editora do livro.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEditora.Focus();
                }

                else
                {
                    if (status == "novo")
                    {
                        cmdMySQL.CommandText = "INSERT INTO livro(nomeLivro,autorLivro,editoraLivro) VALUES('" + txtNome.Text + "','" + txtAutor.Text + "','" + txtEditora.Text + "')";
                        cmdMySQL.ExecuteNonQuery();
                        cmdMySQL.Dispose();
                        MessageBox.Show("Registro salvo com sucesso.", "Salvar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else if (status == "editar")
                    {
                        cmdMySQL.CommandText = "UPDATE Contatos SET nomeLivro='" + txtNome.Text + "', autorLivro='" + txtAutor.Text + "', editoraLivro='" + txtEditora.Text +
                           "' WHERE idLivro='" + lvLivros.Items[lvLivros.FocusedItem.Index].Text + "'";
                        cmdMySQL.ExecuteNonQuery();
                        MessageBox.Show("Registro atualizado com sucesso.", "Atualizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    carregaListaLivros();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtNome.Text = "";
            txtAutor.Text = "";
            txtEditora.Text = "";
            status = "novo";
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja encerrar a aplicação?", "Encerrar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.ExitThread();
            }

        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            txtPesquisar.Text = "";
            carregaListaLivros();
        }
    }
}
