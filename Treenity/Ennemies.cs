using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media.Imaging;

namespace Treenity
{
    public class Ennemies
    {
        public String name="Elfe";
        public string style = "pack://application:,,,/Ressources/Images/pinguin.png";
        public int pv =12;
        public int strength =15;
        public int posTop = new Random().Next(100,401);
        public int posLeft = new Random().Next(100,401);
    }
}
