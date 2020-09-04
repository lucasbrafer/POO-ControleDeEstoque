using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controle_de_Estoque
{
    public class Produto
    {
        public string Código_Produto { get; set; }
        public string Nome { get; set; }
        public double Preço_Unitário { get; set; }
        public int Quantidade { get; set; }
        public int Quantidade_Mínima { get; set; }

        private Categoria cat;
        public string Categoria
        {
            get 
            {
                return cat.Nome;
            }
            set
            {
                cat = Model.RetornaCategoria(value);
            }
        }

        public string Data_de_Validade { get; set; }



        public Produto()
        {
            string Result = "";

            DateTime x = new DateTime();
            x = DateTime.Now;

            Result += x.Date.ToString().Substring(0, 2);
            Result += x.Minute.ToString();
            Result += x.Second.ToString();

            for (int i = Result.Length - 1; i >= 0; i--)
                Código_Produto += Result[i];
           
        }
    }

    
}
