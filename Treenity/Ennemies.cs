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
        private Canvas canvasJeu;

        public static String name = "Elfe";
        private BitmapImage imageEnnemie;
        public static int pvMax = 50;

        public int pv;
        public int degats;
        public int vitesse;

        public int posTop;
        public int posLeft;

        public Image ennemieImg = new Image();

        public Rect rectangle;
        public System.Windows.Shapes.Rectangle hitboxRect;

        public System.Windows.Shapes.Rectangle barrePVMax;
        public System.Windows.Shapes.Rectangle barrePV;

        public Ennemies(Canvas canvas, int pvInitial, int degatsInitial, int vitesseInitial, BitmapImage image)
        {
            canvasJeu = canvas;

            pv = pvInitial;
            degats = degatsInitial;
            vitesse = vitesseInitial;


            imageEnnemie = image;

            ennemieImg.Source = imageEnnemie;
            ennemieImg.Width = imageEnnemie.PixelWidth; 
            ennemieImg.Height = imageEnnemie.PixelHeight;

            posLeft = rand.Next(0, (int)(canvasJeu.ActualWidth - imageEnnemie.PixelWidth));
            posTop = rand.Next(0, (int)(canvasJeu.ActualHeight - imageEnnemie.PixelHeight));

            Canvas.SetTop(ennemieImg, posTop);
            Canvas.SetLeft(ennemieImg, posLeft);

            canvasJeu.Children.Add(ennemieImg);
            rectangle = new Rect(posLeft, posTop, imageEnnemie.PixelWidth, imageEnnemie.PixelHeight);

            barrePVMax = new System.Windows.Shapes.Rectangle
            {
                Width = 80,
                Height = 10,
                Fill = Brushes.Black,
            };

            Canvas.SetLeft(barrePVMax, posLeft + (ennemieImg.Width - barrePVMax.Width) / 2);
            Canvas.SetTop(barrePVMax, posTop + ennemieImg.Height + 5);

            canvasJeu.Children.Add(barrePVMax);

            barrePV = new System.Windows.Shapes.Rectangle
            {
                Width = 80,
                Height = 10,
                Fill = Brushes.Green,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
            };

            Canvas.SetLeft(barrePV, posLeft + (ennemieImg.Width - barrePV.Width) / 2);
            Canvas.SetTop(barrePV, posTop + ennemieImg.Height + 5);

            canvasJeu.Children.Add(barrePV);

            hitboxRect = new System.Windows.Shapes.Rectangle
            {
                Width = ennemieImg.Width,
                Height = ennemieImg.Height,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            Canvas.SetLeft(hitboxRect, posLeft);
            Canvas.SetTop(hitboxRect, posTop);

            canvasJeu.Children.Add(hitboxRect);
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

            Canvas.SetLeft(ennemieImg, posLeft);

            Canvas.SetLeft(hitboxRect, posLeft);

            Canvas.SetLeft(barrePVMax, posLeft + (ennemieImg.Width - barrePVMax.Width) / 2);
            Canvas.SetLeft(barrePV, posLeft + (ennemieImg.Width - barrePVMax.Width) / 2);

            Console.WriteLine($"Posiont hitbox ennemie : {entite.X} {entite.Y}");
        }

        public void RecevoirDegats(int degat)
        {
            pv -= degat;
            if(pv < 0)
            {
                pv = 0;
                Mourir();
            }

            double pourcentage = (double)pv / pvMax;

            barrePV.Width = 80 * pourcentage;

            if (pourcentage > 0.5)
                barrePV.Fill = Brushes.Green;
            else if (pourcentage > 0.25)
                barrePV.Fill = Brushes.Orange;
            else
                barrePV.Fill = Brushes.Red;
        }

        public void Mourir()
        {
            canvasJeu.Children.Remove(ennemieImg);
            canvasJeu.Children.Remove(barrePV);
            canvasJeu.Children.Remove(barrePVMax);
            canvasJeu.Children.Remove(hitboxRect);
        }

        public void FaireTomberEnnemie()
        {
            Console.WriteLine("detection0");
            if(!MethodeColision.EntiteToucheSol(rectangle))
            {
                Console.WriteLine("tomber");
                rectangle.Y += 3;
                posTop = (int)rectangle.Y;
                Canvas.SetTop(ennemieImg, rectangle.Y);
                Canvas.SetTop(hitboxRect, rectangle.Y);

                Canvas.SetTop(barrePV, posTop + 40);
                Canvas.SetTop(barrePVMax, posTop + 40);
            }
        }
    }
}
