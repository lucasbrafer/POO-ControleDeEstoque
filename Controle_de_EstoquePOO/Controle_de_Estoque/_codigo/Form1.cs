using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controle_de_Estoque
{  
    public partial class Form1 : Form
    {
        Categoria MinhaCategoria;
        Produto MeuProduto;
        public Form1()
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
           
        }

        private void AtualizaDataGrid()
        {
            List<Produto> x = Model.RetornaProdutos();
            dataGridViewrelat.Rows.Clear();
            for (int i = 0; i < x.Count; i++)
            {               
                dataGridViewrelat.Rows.Add(x[i].Código_Produto, x[i].Nome,
                    x[i].Preço_Unitário, x[i].Quantidade, x[i].Quantidade_Mínima,
                    x[i].Categoria, x[i].Data_de_Validade);
            }
            dataGridViewrelat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void cmbselect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbselect.SelectedIndex == 0)
            {
                if(pnlcadprod.Visible == true)
                    pnlcadprod.Visible = false;

                pnlcategoria.Visible = true;
                cmbCategoria.SelectedItem = null;
                MinhaCategoria = new Categoria();
                txtcodcat.Text = MinhaCategoria.Código_Categoria;
            }

            if(cmbselect.SelectedIndex == 1)
            {
                if (pnlcategoria.Visible == true)
                    pnlcategoria.Visible = false;

                pnlcadprod.Visible = true;
                MeuProduto = new Produto();
                txtcodprod.Text = MeuProduto.Código_Produto;

                ArrayList nomes = Model.RetornaNomeCategorias();
               
                cmbCategoria.DataSource = nomes;
            }
        }

        private void btncat_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNomeCat.Text != "")
                {
                    MinhaCategoria.Nome = txtNomeCat.Text.ToUpper();

                    Model.CadastraCat(MinhaCategoria);
                    Model.SalvarXMLCategoria();
                    txtNomeCat.Text = "";
                    pnlcategoria.Visible = false;
                    cmbselect.SelectedIndex = -1;
                    MessageBox.Show("Categoria cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Preencha todos os campos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtNomeCat.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Falha ao cadastrar a categoria!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                pnlcategoria.Visible = false;
                cmbselect.SelectedIndex = -1;
            }
           
        }

        private void btncadprod_Click(object sender, EventArgs e)
        {
            try
            {
                if(cmbCategoria.Items.Count == 0)
                {
                    MessageBox.Show("É necessário cadastrar uma categoria!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cmbselect.SelectedIndex = 0;
                }
                else if(txtnomeprod.Text == "" || txtqtd.Text == "" || txtqtdmin.Text == "" || txtprecounit.Text == ""
                    || mskdatavalidade.Text == "")
                {
                    MessageBox.Show("Preencha todos os campos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MeuProduto.Nome = txtnomeprod.Text.ToUpper();
                    MeuProduto.Quantidade = int.Parse(txtqtd.Text);
                    MeuProduto.Quantidade_Mínima = int.Parse(txtqtdmin.Text);
                    MeuProduto.Preço_Unitário = double.Parse(txtprecounit.Text);
                    MeuProduto.Data_de_Validade = DateTime.Parse(mskdatavalidade.Text).ToShortDateString();
                    MeuProduto.Categoria = Model.RetornaCategoriaReferente(cmbCategoria.SelectedItem.ToString());

                    Model.CadastraProd(MeuProduto);
                    Model.SalvarXMLProduto();
                    AtualizaDataGrid();

                    txtnomeprod.Text = "";
                    txtqtd.Text = "";
                    txtqtdmin.Text = "";
                    txtprecounit.Text = "";
                    mskdatavalidade.Text = "";
                    pnlcadprod.Visible = false;
                    cmbselect.SelectedIndex = -1;
                    MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Falha ao cadastrar o produto!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                pnlcadprod.Visible = false;
                cmbselect.SelectedIndex = -1;
            }

            
        }

        private void btnsair_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Deseja Sair?","Sair",MessageBoxButtons.YesNo,MessageBoxIcon.Question) 
                == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }

        private void mskdatavalidade_Click(object sender, EventArgs e)
        {
            int i;
            for(i = 0; i <mskdatavalidade.Text.Length; i++)
            {
                if(mskdatavalidade.Text[i] == ' ')
                {
                    break;
                }
            }
            mskdatavalidade.Select(i, 0);
        }


        private void cmbfiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbfiltros.SelectedIndex == 0)
            {
                AtualizaDataGrid();

            }
            if(cmbfiltros.SelectedIndex == 1)
            {
                if (Model.RetornaProdutosVencidos().Count > 0)
                {
                    List<Produto> x = Model.RetornaProdutos();
                    dataGridViewrelat.Rows.Clear();
                    for (int i = 0; i < x.Count; i++)
                    {
                        if (DateTime.Parse(x[i].Data_de_Validade) < DateTime.Today)
                        {
                            dataGridViewrelat.Rows.Add(x[i].Código_Produto, x[i].Nome,
                            x[i].Preço_Unitário, x[i].Quantidade, x[i].Quantidade_Mínima,
                            x[i].Categoria, x[i].Data_de_Validade);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Nenhum produto vencido", "Informação", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            if(cmbfiltros.SelectedIndex == 2)
            {
                if (Model.RetornaProdutosAbaixoQtdMin().Count > 0)
                {
                    List<Produto> x = Model.RetornaProdutos();
                    dataGridViewrelat.Rows.Clear();
                    for (int i = 0; i < x.Count; i++)
                    {
                        if (x[i].Quantidade_Mínima > x[i].Quantidade)
                        {
                            dataGridViewrelat.Rows.Add(x[i].Código_Produto, x[i].Nome,
                            x[i].Preço_Unitário, x[i].Quantidade, x[i].Quantidade_Mínima,
                            x[i].Categoria, x[i].Data_de_Validade);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum produto abaixo da quantidade mínima", "Informação",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl.SelectedIndex == 2)
            {
                AtualizaDataGrid();
                cmbfiltros.SelectedIndex = 0;
                txtvalortot.Text = "R$" + Model.RetornaValorEstoque().ToString();
            }
            if(tabControl.SelectedIndex == 3)
            {
                cmbcatalterprod.DataSource = Model.RetornaNomeCategorias();
                cmbcatalterprod.SelectedItem = null;
            }
        }

        private void cmbRemoverAlterar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbRemoverAlterar.SelectedIndex == 0)
            {
                if (pnlremovprod.Visible == true)
                    pnlremovprod.Visible = false;
                if (pnlaltercat.Visible == true)
                    pnlaltercat.Visible = false;
                if (pnlalterprod.Visible == true)
                    pnlalterprod.Visible = false;

                pnlremovcat.Visible = true;
                MessageBox.Show("Se excluir uma categoria, todos os produtos pertencentes a ela também serão excluídos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridViewcat.Visible = true;
                List<Categoria> x = Model.RetornaCategorias();
                dataGridViewcat.Rows.Clear();
                for(int i = 0; i < x.Count; i++)
                {
                    dataGridViewcat.Rows.Add(x[i].Nome, x[i].Código_Categoria);
                }
                dataGridViewcat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            }
            if(cmbRemoverAlterar.SelectedIndex == 1)
            {
                if (pnlremovcat.Visible == true)
                    pnlremovcat.Visible = false;
                if (pnlaltercat.Visible == true)
                    pnlaltercat.Visible = false;
                if (pnlalterprod.Visible == true)
                    pnlalterprod.Visible = false;
                if (dataGridViewcat.Visible == true)
                    dataGridViewcat.Visible = false;
                pnlremovprod.Visible = true;
            }
            if(cmbRemoverAlterar.SelectedIndex == 2)
            {
                if (pnlremovprod.Visible == true)
                    pnlremovprod.Visible = false;
                if (pnlremovcat.Visible == true)
                    pnlremovcat.Visible = false;
                if (pnlalterprod.Visible == true)
                    pnlalterprod.Visible = false;

                pnlaltercat.Visible = true;
                MinhaCategoria = new Categoria();

                dataGridViewcat.Visible = true;
                List<Categoria> x = Model.RetornaCategorias();
                dataGridViewcat.Rows.Clear();
                for (int i = 0; i < x.Count; i++)
                {
                    dataGridViewcat.Rows.Add(x[i].Nome, x[i].Código_Categoria);
                }
                dataGridViewcat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            if(cmbRemoverAlterar.SelectedIndex == 3)
            {
                if (pnlremovprod.Visible == true)
                    pnlremovprod.Visible = false;
                if (pnlaltercat.Visible == true)
                    pnlaltercat.Visible = false;
                if (pnlremovcat.Visible == true)
                    pnlremovcat.Visible = false;
                if (dataGridViewcat.Visible == true)
                    dataGridViewcat.Visible = false;

                pnlalterprod.Visible = true;

                cmbcatalterprod.DataSource = Model.RetornaNomeCategorias();
                cmbcatalterprod.SelectedItem = null;
            }
        }

        private void btnremovcat_Click(object sender, EventArgs e)
        {
            string cod;
            if (txtremvcodcat.Text != "")
            {

                cod = txtremvcodcat.Text;
                if (Model.RemoveCategoria(cod))
                {
                    MessageBox.Show("Categoria removida com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtremvcodcat.Text = "";
                    Model.SalvarXMLCategoria();
                    Model.SalvarXMLProduto();
                    pnlremovcat.Visible = false;
                    dataGridViewcat.Visible = false;
                    cmbRemoverAlterar.SelectedIndex = -1;
                }   
                else
                {
                    MessageBox.Show("Código de categoria inválido!", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
            else
            {
                MessageBox.Show("Preencha todos os campos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtremvcodcat.Focus();
            }
                
        }

        private void btnremovprod_Click(object sender, EventArgs e)
        {
            string cod;
            if (txtremvprod.Text != "")
            {
                cod = txtremvprod.Text;
                if (Model.RemoveProduto(cod))
                {
                    MessageBox.Show("Produto removido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtremvprod.Text = "";
                    Model.SalvarXMLProduto();
                    pnlremovprod.Visible = false;
                    cmbRemoverAlterar.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Código de Produto inválido!", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            else
            {
                MessageBox.Show("Preencha todos os campos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtremvprod.Focus();
            }
        }

        private void btnaltercat_Click(object sender, EventArgs e)
        {
            string cod;
            string nome;
            if (txtcodaltercat.Text != "")
            {
                cod = txtcodaltercat.Text;
                nome = txtalternomecat.Text;
                if (Model.AlteraCategoria(cod, nome))
                {
                    MessageBox.Show("Categoria alterada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtremvprod.Text = "";
                    Model.SalvarXMLCategoria();
                    Model.SalvarXMLProduto();
                    pnlaltercat.Visible = false;
                    dataGridViewcat.Visible = false;
                    cmbRemoverAlterar.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Código de Categoria inválido!", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            else
            {
                MessageBox.Show("Preencha todos os campos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtremvprod.Focus();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Model.Iniciar();
            timer.Start();
        }

        private void btnalterprod_Click(object sender, EventArgs e)
        {
            try
            {
                Produto x = new Produto();
                if (txtcodalterprod.Text != "")
                {
                    x.Código_Produto = txtcodalterprod.Text;

                    if (txtnomealterprod.Text != "")
                        x.Nome = txtnomealterprod.Text.ToUpper();

                    if (txtalterqtdprod.Text != "")
                        x.Quantidade = int.Parse(txtalterqtdprod.Text);

                    if (txtalterqrdminprod.Text != "")
                        x.Quantidade_Mínima = int.Parse(txtalterqrdminprod.Text);

                    if (txtprecoalterprod.Text != "")
                        x.Preço_Unitário = double.Parse(txtprecoalterprod.Text);

                    if (mskdataalterprod.Text.Length > 6)
                        x.Data_de_Validade = DateTime.Parse(mskdataalterprod.Text).ToShortDateString();

                    if (cmbcatalterprod.SelectedItem != null)
                        x.Categoria = Model.RetornaCategoriaReferente(cmbcatalterprod.SelectedItem.ToString());

                    if (Model.AlterarProduto(x))
                    {
                        MessageBox.Show("Produto alterado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtnomealterprod.Text = "";
                        txtcodalterprod.Text = "";
                        txtalterqtdprod.Text = "";
                        txtalterqrdminprod.Text = "";
                        txtprecoalterprod.Text = "";
                        mskdataalterprod.Text = "";
                        cmbcatalterprod.SelectedItem = null;

                        Model.SalvarXMLProduto();
                        pnlalterprod.Visible = false;
                        cmbRemoverAlterar.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("Código de Categoria inválido!", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                else
                {
                    MessageBox.Show("Preencha todos os campos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtremvprod.Focus();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao alterar produto!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void mskdataalterprod_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < mskdataalterprod.Text.Length; i++)
            {
                if (mskdataalterprod.Text[i] == ' ')
                {
                    break;
                }
            }
            mskdataalterprod.Select(i, 0);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            string texto;
            int ProdutosaVencer = Model.RetornaProdutosaVencer();
            timer.Interval = 300000;
            if (ProdutosaVencer > 0)
            {
                texto = ProdutosaVencer.ToString() + " Produto(s) a vencer nos próximos 3 dias";
                notifyIcon.ShowBalloonTip(4000, "Produtos a vencer", texto, ToolTipIcon.Info);
                timer.Interval = 3600000;
            }
            

        }
    }
}
