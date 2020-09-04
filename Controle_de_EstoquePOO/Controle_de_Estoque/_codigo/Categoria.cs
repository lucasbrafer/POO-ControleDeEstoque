using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controle_de_Estoque
{
    public class Categoria
    {
        public string Código_Categoria { get; set; }
        public string Nome { get; set; }


        public Categoria()
        {
            string Result = "";

            DateTime x = new DateTime();
            x = DateTime.Now;

            Result += x.Date.ToString().Substring(0, 2);
            Result += x.Minute.ToString();
            Result += x.Second.ToString();

            Código_Categoria = Result;
        }
    }
}
