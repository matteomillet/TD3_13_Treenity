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
        public int vitessePerso = 4;
        Ennemies[] ennemies = new Ennemies[10];
        Rect[] obstacleHitbox = new Rect[2];
        private static DispatcherTimer minuterie;
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
        }


        private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
        {
            imgPerso.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            ScaleTransform fliptrans = new ScaleTransform();
            imgPerso.RenderTransform = fliptrans;

            if ((e.Key == Key.Right || e.Key == Key.D) && MethodeColision.ColisionAvecObstacles(obstacleHitbox, rectangleJoueur) != "droite")
            {
                fliptrans.ScaleX = 1;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + vitessePerso);
                rectangleJoueur.X += vitessePerso;
            }


            if ((e.Key == Key.Left || e.Key == Key.Q) && MethodeColision.ColisionAvecObstacles(obstacleHitbox, rectangleJoueur) != "droite")
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
                Canvas.SetTop(imgPerso, Canvas.GetTop(imgPerso) + vitessePerso);
                rectangleJoueur.Y += vitessePerso;
            }

            Console.WriteLine($"Position du joueur : {Canvas.GetLeft(imgPerso)}, {Canvas.GetTop(imgPerso)}");
            Console.WriteLine($"Position de la hitbox du joueur (rectangle joueur) : {rectangleJoueur.X}, {rectangleJoueur.Y}");
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
            foreach (Ennemies ennemie in ennemies)
            {
                ennemie.MoveEnnemie(rectangleJoueur, ennemie.rectangle);
                Console.WriteLine(ennemie.posLeft);
            }

            


            MethodeColision.ColisionAvecEnnemies(ennemies, rectangleJoueur);
        }



        

        
    }
}


