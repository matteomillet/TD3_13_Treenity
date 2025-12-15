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
        public List<Ennemies> ennemies;
        public Joueur joueur;
        Rect[] obstacleHitbox = new Rect[2];
        private static DispatcherTimer minuterie;
        public Ellipse cercleDebug;
        public double vitesseY = 0; 
        public const double gravite = 3;
        public const double FORCE_SAUT = -40;
        public UCJeu()
        {
            InitializeComponent();
            InitializeJoueur();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += canvasJeu_KeyDown;
            Application.Current.MainWindow.KeyUp += canvasJeu_KeyUp;

            InitializeEnnemies();
            InitializeTimer();
        }

        private void InitializeJoueur()
        {
            BitmapImage joueurImg = new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/bucheron.png"));
            joueur = new Joueur(canvasJeu, 100, 5, 4, joueurImg);

            // Initialisation d'un cercle de debug pour visualiser le rayon d'attaque
            /*
            cercleDebug = new Ellipse();
            cercleDebug.Width = rayonAttaque * 2;
            cercleDebug.Height = rayonAttaque * 2;
            cercleDebug.Stroke = Brushes.Red;
            cercleDebug.StrokeThickness = 2;

            canvasJeu.Children.Add(cercleDebug);
            */
        }

        private void InitializeEnnemies()
        {
            Random rand = new Random();
            ennemies = new List<Ennemies>();
            BitmapImage imageEnnemie = new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/pinguin.png"));
            for(int i = 0; i < 1;  i++)
            {        
                ennemies.Add(new Ennemies(canvasJeu, 50, 15, 1, imageEnnemie));
            }
        }

        private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D ||
        e.Key == Key.Left || e.Key == Key.Q)
            {
                joueur.vitesseX = 0;
            }
        }

        private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                joueur.DirectionRegard = 1;
                joueur.vitesseX = 4;
            }
            if (e.Key == Key.Left || e.Key == Key.Q)
            {
                joueur.DirectionRegard = -1;
                joueur.vitesseX = -4;
            }
            if (e.Key == Key.Space && MethodeColision.EntiteToucheSol(joueur.hitboxLogi))
            {
                joueur.vitesseY += FORCE_SAUT;
            }
            if (e.Key == Key.Enter)
            {
                foreach(Ennemies ennemie in ennemies)
                {
                    joueur.Attaque(ennemie);
                }
            }

            //Console.WriteLine($"Position du joueur : {Canvas.GetLeft(imgPerso)}, {Canvas.GetTop(imgPerso)}");
            Console.WriteLine($"Position de la hitbox du joueur (rectangle joueur) : {joueur.hitboxLogi.X}, {joueur.hitboxLogi.Y}");
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


        private int nbTick = 0;
        private void Jeu(object? sender, EventArgs e)
        {
            nbTick++;
            
            for(int i = ennemies.Count - 1; i >= 0; i--)
            {
                if (ennemies[i].pv <= 0)
                {
                    ennemies[i].Mourir();
                    ennemies.RemoveAt(i);

                    continue;
                }

                //Console.WriteLine("l'ennemie va peut etre tomber");
                //Console.WriteLine("Tomber Y = " + ennemies[i].hitboxLogi.Y);
                ennemies[i].MoveEnnemie(joueur);
                //Console.WriteLine("MoveEnnemie Y = " + ennemies[i].hitboxLogi.Y);
            }
            

            joueur.hitboxLogi.X += joueur.vitesseX;
            for (int i = ennemies.Count - 1; i >= 0; i--)
            {
                ennemies[i].AppliquerGravite();
            }
            joueur.AppliquerGravite();

            string colision = MethodeColision.ColisionAvecEnnemies(ennemies, joueur.hitboxLogi);

            if (colision != "pas colision")
            {
                Console.WriteLine("Colision detecter");
                joueur.RecevoirRecul(colision);
            }




            // Cercle de debug pour apercevoire le rayon d'attaque
            /*
            double centreX = rectangleJoueur.X + (rectangleJoueur.Width / 2);
            double centreY = rectangleJoueur.Y + (rectangleJoueur.Height / 2);

            double left = centreX - rayonAttaque;
            double top = centreY - rayonAttaque;

            Canvas.SetLeft(cercleDebug, left);
            Canvas.SetTop(cercleDebug, top);
            */

            joueur.UpdateVisu();
            for (int i = ennemies.Count - 1; i >= 0; i--)
            {
                ennemies[i].UpdateVisu();   
            }
        }
    }
}


