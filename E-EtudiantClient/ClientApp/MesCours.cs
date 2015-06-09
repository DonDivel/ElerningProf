using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class MesCours
    {
        public String id { get; set; }
        public String Image { get; set; }
        public String Text { get; set; }
        
        public String Date { get; set; }
        public int Duree { get; set; }
        public string NameProf { get; set; }
        public string etat { get; set; }
        public String Prix { get; set; }
    }
}
