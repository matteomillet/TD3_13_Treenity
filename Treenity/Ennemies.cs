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
        public String name = "Elfe";
        public BitmapImage imageEnnemie = new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/pinguin.png"));
        public int pv = 12;
        public int strength = 15;
        public int posTop = new Random().Next(0, 1080);
        public int posLeft = new Random().Next(0, 1920);


        public void MoveX(int x)
        {
            posLeft += x;
        }

        public void MoveY(int y)
        {
            posTop += y;
        }
    }
}
