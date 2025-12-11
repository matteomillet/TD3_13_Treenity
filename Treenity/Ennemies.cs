using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Treenity
{
    public class Ennemies
    {
        public String name = "Elfe";
        public BitmapImage imageEnnemie = new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/pinguin.png"));
        public int pv = 12;
        public int strength = 15;
        public int speed = 10;
        public int posTop = new Random().Next(0, 1080);
        public int posLeft = new Random().Next(0, 1920);

        public void MoveEnnemie(Rect joueur)
        {
            int distance = (int)(joueur.X - posLeft);
            posLeft += speed * (distance / Math.Abs(distance));
        }
    }
}
