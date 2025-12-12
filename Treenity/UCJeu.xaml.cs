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
        Ennemies[] ennemies = new Ennemies[1];
        Rect[] obstacleHitbox = new Rect[2];
        private static DispatcherTimer minuterie;
        public System.Windows.Shapes.Rectangle hitboxJoueur;
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

            hitboxJoueur = new System.Windows.Shapes.Rectangle
            {
                Width = imgPerso.Width,
                Height = imgPerso.Height,
                Stroke = Brushes.Cyan,
                StrokeThickness = 2
            };

            canvasJeu.Children.Add(hitboxJoueur);
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

            //Console.WriteLine($"Position du joueur : {Canvas.GetLeft(imgPerso)}, {Canvas.GetTop(imgPerso)}");
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
                Console.WriteLine("l'ennemie va peut etre tomber");
                ennemie.FaireTomberEnnemie(ennemie);
                Console.WriteLine("Tomber Y = " + ennemie.rectangle.Y);
            }

            foreach (Ennemies ennemie in ennemies)
            {
                ennemie.MoveEnnemie(rectangleJoueur, ennemie.rectangle);
                Console.WriteLine("MoveEnnemie Y = " + ennemie.rectangle.Y);


            }


            string colision = MethodeColision.ColisionAvecEnnemies(ennemies, rectangleJoueur);

            if (colision != "pas colision")
            {
                Console.WriteLine("Colision detecter");
                DeplacerJoueur(colision,ref rectangleJoueur, imgPerso);
            }

            

            Canvas.SetLeft(hitboxJoueur, rectangleJoueur.X);
            Canvas.SetTop(hitboxJoueur, rectangleJoueur.Y);

            //faire tomber joueur
            FaireTomberJoueur(ref rectangleJoueur, imgPerso,ref hitboxJoueur);

            

            
        }

        private static void DeplacerJoueur(String direction, ref Rect joueurHitbox, Image imgPerso)
        {
            string abssice = direction.Substring(0, 1);
            int recul = int.Parse(direction.Substring(1,direction.Length-1));
           
            if (abssice == "X")
            {
                joueurHitbox.X = joueurHitbox.X + recul;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + recul);
            }
            else
            {
                joueurHitbox.Y = joueurHitbox.Y + recul;
                Canvas.SetTop(imgPerso, Canvas.GetTop(imgPerso) + recul);
            }
        }

        private static void FaireTomberJoueur(ref Rect entite, Image imgEntite, ref System.Windows.Shapes.Rectangle hitboxVisuel)
        {
            if (!MethodeColision.EntiteToucheSol(entite))
            {
                Console.WriteLine("L'entite tombe");
                entite.Y = entite.Y + 3;
                Canvas.SetTop(imgEntite, Canvas.GetTop(imgEntite) + 3);
            }

            Canvas.SetLeft(hitboxVisuel, entite.X);
            Canvas.SetTop(hitboxVisuel, entite.Y);
        }

        

       
        

        
    }
}


