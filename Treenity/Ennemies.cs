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
using System.Drawing;


namespace Treenity
{
    public class Ennemies
    {
        private static Random rand = new Random();

        public static String name = "Elfe";
        public static BitmapImage imageEnnemie = new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/pinguin.png"));

        public int pv = 12;
        public int degats = 15;
        public int vitesse = 1;

        public int posTop;
        public int posLeft;

        public Image ennemieImg = new Image();

        public Rect rectangle;
        public System.Windows.Shapes.Rectangle hitboxRect;

        public Ennemies(Canvas canvas)
        {
            posLeft = rand.Next(0, (int)(canvas.ActualWidth - imageEnnemie.PixelWidth));
            posTop = rand.Next(0, (int)(canvas.ActualHeight - imageEnnemie.PixelHeight));

            ennemieImg.Source = imageEnnemie;
            ennemieImg.Width = imageEnnemie.PixelWidth; 
            ennemieImg.Height = imageEnnemie.PixelHeight;

            Canvas.SetTop(ennemieImg, posTop);
            Canvas.SetLeft(ennemieImg, posLeft);

            canvas.Children.Add(ennemieImg);
            rectangle = new Rect(posLeft, posTop, imageEnnemie.PixelWidth, imageEnnemie.PixelHeight);

            hitboxRect = new System.Windows.Shapes.Rectangle
            {
                Width = ennemieImg.Width,
                Height = ennemieImg.Height,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            Canvas.SetLeft(hitboxRect, posLeft);
            Canvas.SetTop(hitboxRect, posTop);

            canvas.Children.Add(hitboxRect);
        }

        public void MoveEnnemie(Rect joueur, Rect entite)
        {
            int sens = 0;

            int joueurBordDroit = (int)(joueur.X + joueur.Width);
            int ennemiBordDroit = (int)(posLeft + rectangle.Width);

            if (ennemiBordDroit < joueur.X)
                sens = 1;
            else if (posLeft > joueurBordDroit)
                sens = -1;

            posLeft += vitesse * sens;

            rectangle.X = posLeft;
            rectangle.Y = posTop;


            Canvas.SetLeft(ennemieImg , posLeft);

            Canvas.SetLeft(hitboxRect, posLeft);

            Console.WriteLine($"Posiont hitbox ennemie : {entite.X} {entite.Y}");  
        }
    }
}
