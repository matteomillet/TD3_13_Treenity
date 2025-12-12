using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;



namespace Treenity
{
    /// <summary>
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    public partial class UCJeu : UserControl
    {
        public Rect rectangleJoueur = new Rect();
        public int vitessePerso = 2;
        Ennemies[] ennemies = new Ennemies[10];
        private static DispatcherTimer minuterie;
        public UCJeu()
        {
            InitializeComponent();
            InitializeJoueur();
            InitializeEnnemies();  
            InitializeTimer();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += canvasJeu_KeyDown;
            Application.Current.MainWindow.KeyUp += canvasJeu_KeyUp;
        }

        private void InitializeJoueur()
        {
            rectangleJoueur.X = (int)Canvas.GetLeft(imgPerso);
            rectangleJoueur.Y = (int)Canvas.GetTop(imgPerso);
            rectangleJoueur.Height = (int)imgPerso.Height;
            rectangleJoueur.Width = (int)imgPerso.Width;
        }

        private void InitializeEnnemies()
        {
            for (int i = 0; i < ennemies.Length; i++)
            {
                ennemies[i] = new Ennemies(canvasJeu);
            }

            InitializeTimer();
        }


        private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
        {
            imgPerso.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            ScaleTransform fliptrans = new ScaleTransform();
            imgPerso.RenderTransform = fliptrans;

            if (e.Key == Key.Right || e.Key == Key.D)
            {
                fliptrans.ScaleX = 1;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + vitessePerso);
                rectangleJoueur.X += vitessePerso;
            }


            if (e.Key == Key.Left || e.Key == Key.Q )
            {
                fliptrans.ScaleX = -1;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) - vitessePerso);
                rectangleJoueur.X -= vitessePerso;
            }

            if (e.Key == Key.Space || e.Key == Key.Z)
            {
                Canvas.SetTop(imgPerso, Canvas.GetTop(imgPerso) - vitessePerso);
                rectangleJoueur.Y -= vitessePerso;
            }

            if (e.Key == Key.Down || e.Key == Key.S)
            {
                Canvas.SetTop(imgPerso, Canvas.GetTop(imgPerso) - vitessePerso);
                rectangleJoueur.Y -= vitessePerso;
            }

            Console.WriteLine($"Position du joueur : {Canvas.GetLeft(imgPerso)}, {Canvas.GetTop(imgPerso)}");
            Console.WriteLine($"Position de la hitbox du joueur (rectangle joueur) : {rectangleJoueur.X}, {rectangleJoueur.Y}");
        }

        public bool Colision(Ennemies[] entites, Rect joueur)
        {
            /*
             Le rectangle du joueur entre en colision avec rectangle dans liste si oui = true sinon = false

             */


            bool colision = false;

            //Console.WriteLine("Detection des colision avec les ennemies : start");
            for (int i = 0; i < entites.Length; i++)
            {
                //Console.WriteLine("Detection de la colision avec l'ennemie numero : " + i);
                if (joueur.IntersectsWith(entites[i].rectangle))
                {
                    //Console.WriteLine("Colision avec l'ennemie numero " + i);
                    
                    Colision(entites, joueur);
                    return true;
                }
            }

            return colision;
        }

        public string Colision(Ennemies entite, Rect joueur)
        {
            
            string directionColision = "";
            double distanceRect = Math.Sqrt(Math.Pow((joueur.X - entite.rectangle.X), 2) + Math.Pow((joueur.Y - entite.rectangle.Y), 2));
            Rect  rectIntersect = Rect.Intersect(entite.rectangle, joueur);

            if (rectIntersect.Height > rectIntersect.Width)
            {
                if (joueur.Y > entite.rectangle.Y)
                    directionColision = "droite";
                else
                    directionColision = "gauche";
            }
            else
            {
                if (joueur.X < entite.rectangle.X)
                    directionColision = "bas";
                else
                    directionColision = "haut";
            }

            Console.WriteLine($"Position de la hitbox ( rectangle) de l'ennemie {entite.rectangle.X}, {entite.rectangle.Y}");
            Console.WriteLine($"Position de la hitbox du joueur (rectangle joueur) : {rectangleJoueur.X}, {rectangleJoueur.Y}");
            Console.WriteLine($"direction colision: {directionColision}");
            return directionColision;
        }

        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer();
            // configure l'intervalle du Timer
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            // associe l’appel de la méthode Jeu à la fin de la minuterie
            minuterie.Tick += Jeu;
            // lancement du timer
            minuterie.Start();
        }

        private void Jeu(object? sender, EventArgs e)
        {
            foreach(Ennemies ennemie in ennemies)
            {
                ennemie.MoveEnnemie(rectangleJoueur, ennemie.rectangle);
                Console.WriteLine(ennemie.posLeft);
            }
        }
    }
}


