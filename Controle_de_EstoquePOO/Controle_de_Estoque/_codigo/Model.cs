using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Controle_de_Estoque
{
    public static class Model
    {
        public static List<Categoria> categorias = new List<Categoria>();
        public static List<Produto> produtos = new List<Produto>();

        public static void Iniciar()
        {
            LerXMLCategoria();
            LerXMLProduto();
        }

        public static void CadastraCat(Categoria x)
        {
            categorias.Add(x);
        }
        public static List<Categoria> RetornaCategorias()
        {
            return categorias;
        }

        public static ArrayList RetornaNomeCategorias()
        {
            ArrayList x = new ArrayList();
            foreach(Categoria i in categorias)
            {
                x.Add(i.Nome);
            }
            return x;
        }

        public static void CadastraProd(Produto x)
        {
            produtos.Add(x);
        }

        public static List<Produto> RetornaProdutos()
        {
            return produtos;
        }

        public static ArrayList RetornaProdutosVencidos()
        {
            ArrayList x = new ArrayList();
            foreach(Produto i in produtos)
            {
                if (DateTime.Parse(i.Data_de_Validade) < DateTime.Today)
                    x.Add(i);
            }
            return x;
        }

        public static ArrayList RetornaProdutosAbaixoQtdMin()
        {
            ArrayList x = new ArrayList();
            foreach (Produto i in produtos)
            {
                if (i.Quantidade < i.Quantidade_Mínima)
                    x.Add(i);
            }
            return x;
        }

        public static string RetornaCategoriaReferente(string nome)
        {
            foreach(Categoria i in categorias)
            {
                if(i.Nome == nome)
                {
                    return i.Código_Categoria;
                }
            }
            return "";
        }
        public static bool RemoveCategoria(string cod)
        {
            foreach(Categoria x in categorias)
            {
                if(x.Código_Categoria == cod)
                {
                    for (int i = 0; i < produtos.Count; i++)
                    {
                        if (produtos[i].Categoria == x.Nome)
                        {
                            produtos.Remove(produtos[i]);
                            i--;
                        }
                    }
                    categorias.Remove(x);
                    
                    
                    return true;
                }
            }
            return false;
        }
        public static bool RemoveProduto(string cod)
        {
            foreach (Produto x in produtos)
            {
                if (x.Código_Produto == cod)
                {
                    produtos.Remove(x);
                    return true;
                }
            }
            return false;
        }

        public static bool AlteraCategoria(string cod, string nome)
        {
            foreach(Categoria x in categorias)
            {
                if(x.Código_Categoria == cod)
                {
                    categorias[categorias.IndexOf(x)].Nome = nome.ToUpper();
                    return true;
                }
            }
            return false;
        }

        public static bool AlterarProduto(Produto x)
        {
            int j = -1;
            foreach(Produto i in produtos)
            {
                if(i.Código_Produto == x.Código_Produto)
                {
                    j = produtos.IndexOf(i);
                }
            }

            if(j != -1)
            {
                if (x.Nome != null)
                    produtos[j].Nome = x.Nome;

                if (x.Quantidade != null)
                    produtos[j].Quantidade = x.Quantidade;

                if (x.Quantidade_Mínima != null)
                    produtos[j].Quantidade_Mínima = x.Quantidade_Mínima;

                if (x.Preço_Unitário != null)
                    produtos[j].Preço_Unitário = x.Preço_Unitário;

                if (x.Data_de_Validade != null)
                    produtos[j].Data_de_Validade = x.Data_de_Validade;

                if (x.Categoria != null)
                    produtos[j].Categoria = Model.RetornaCategoriaReferente(x.Categoria);

                return true;
            }
            return false;
        }
        

        public static Categoria RetornaCategoria(string cod)
        {
            foreach(Categoria x in categorias)
            {
                if(x.Código_Categoria == cod || x.Nome == cod)
                {
                    return x;
                }
            }
            return new Categoria();
        }

        public static double RetornaValorEstoque()
        {
            double i = 0;
            foreach(Produto x in produtos)
            {
                i += x.Preço_Unitário * x.Quantidade;
            }
            return i;
        }

        public static int RetornaProdutosaVencer()
        {
            int i = 0;
            foreach(Produto x in produtos)
            {
                if (DateTime.Parse(x.Data_de_Validade) >= DateTime.Today &&
                    DateTime.Parse(x.Data_de_Validade) <= DateTime.Today.AddDays(3))
                    i++;
            }
            return i;
        }

        public static int SalvarXMLCategoria()
        {
            TextWriter MeuWriter = new StreamWriter(@"ArquivoCategorias.xml");

            Categoria[] ListaCategoriaVetor = categorias.ToArray();

            XmlSerializer MinhaSerialização = new XmlSerializer(ListaCategoriaVetor.GetType());

            MinhaSerialização.Serialize(MeuWriter, ListaCategoriaVetor);

            MeuWriter.Close();

            return categorias.Count;
        }
        public static int SalvarXMLProduto()
        {
            TextWriter MeuWriter = new StreamWriter(@"ArquivoProdutos.xml");

            Produto[] ListaProdutoVetor = produtos.ToArray();

            XmlSerializer MinhaSerialização = new XmlSerializer(ListaProdutoVetor.GetType());

            MinhaSerialização.Serialize(MeuWriter, ListaProdutoVetor);

            MeuWriter.Close();

            return categorias.Count;
        }

        public static int LerXMLCategoria()
        {
            FileStream Arquivo = new FileStream(@"ArquivoCategorias.xml", FileMode.OpenOrCreate);
            try
            {
                

                Categoria[] ListaCategoriaVetor = categorias.ToArray();

                XmlSerializer MinhaSerialização = new XmlSerializer(ListaCategoriaVetor.GetType());

                ListaCategoriaVetor = (Categoria[])MinhaSerialização.Deserialize(Arquivo);

                categorias.Clear();

                categorias.AddRange(ListaCategoriaVetor);

                Arquivo.Close();

                return categorias.Count;
            }
            catch (Exception)
            {
                return categorias.Count;
            }
            finally
            {
                Arquivo.Close();
            }
        }

        public static int LerXMLProduto()
        {
            FileStream Arquivo = new FileStream(@"ArquivoProdutos.xml", FileMode.OpenOrCreate);
            try
            {
                
                Produto[] ListaProdutoVetor = produtos.ToArray();

                XmlSerializer MinhaSerialização = new XmlSerializer(ListaProdutoVetor.GetType());

                ListaProdutoVetor = (Produto[])MinhaSerialização.Deserialize(Arquivo);

                produtos.Clear();

                produtos.AddRange(ListaProdutoVetor);

                return produtos.Count;
            }
            catch (Exception)
            {
                return produtos.Count;
            }
            finally
            {
                Arquivo.Close();
            }
        }
    }
}
