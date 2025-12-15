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
        public List<Ennemies> ennemies;
        public Rect[] obstacleHitbox = new Rect[2];
        private static DispatcherTimer minuterie;
        public static int rayonAttaque = 150;
        public int directionRegard = 1;
        public Rectangle hitboxJoueur;
        public Ellipse cercleDebug;
        public double vitesseY = 0; 
        public const double gravite = 3; 
        public const double forceSaut = -40;
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

            hitboxJoueur = new Rectangle
            {
                Width = imgPerso.Width,
                Height = imgPerso.Height,
                Stroke = Brushes.Cyan,
                StrokeThickness = 2
            };

            canvasJeu.Children.Add(hitboxJoueur);

            // Initialisation d'un cercle de debug pour visualiser le rayon d'attaque
            cercleDebug = new Ellipse();
            cercleDebug.Width = rayonAttaque * 2;
            cercleDebug.Height = rayonAttaque * 2;
            cercleDebug.Stroke = Brushes.Red;
            cercleDebug.StrokeThickness = 2;

            canvasJeu.Children.Add(cercleDebug);
        }

        private void InitializeEnnemies()
        {
            Random rand = new Random();
            ennemies = new List<Ennemies>();
            BitmapImage imageEnnemie = new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/pinguin.png"));
            for(int i = 0; i < 10;  i++)
            {
                
                ennemies.Add(new Ennemies(canvasJeu, 50, 15, 1, imageEnnemie));
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

            if ((e.Key == Key.Right || e.Key == Key.D) && MethodeColision.ColisionAvecObstacles(obstacleHitbox, rectangleJoueur, canvasJeu) != "droite")
            {
                directionRegard = 1;
                fliptrans.ScaleX = 1;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + vitessePerso);
                rectangleJoueur.X += vitessePerso;
            }


            if ((e.Key == Key.Left || e.Key == Key.Q) && MethodeColision.ColisionAvecObstacles(obstacleHitbox, rectangleJoueur, canvasJeu) != "gauche")
            {
                directionRegard = -1;
                fliptrans.ScaleX = -1;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) - vitessePerso);
                rectangleJoueur.X -= vitessePerso;
            }

            if (e.Key == Key.Space && MethodeColision.EntiteToucheSol(rectangleJoueur))
            {
                vitesseY = forceSaut;

            }



            if (e.Key == Key.Enter)
                Attaque(ennemies, rectangleJoueur, 2, directionRegard);

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
                ennemies[i].AppliquerGravite();
                //Console.WriteLine("Tomber Y = " + ennemies[i].rectangle.Y);
                ennemies[i].MoveEnnemie(rectangleJoueur, ennemies[i].rectangle);
                //Console.WriteLine("MoveEnnemie Y = " + ennemies[i].rectangle.Y);
            }

            string colision = MethodeColision.ColisionAvecEnnemies(ennemies, rectangleJoueur);
            string directionColision = colision.Substring(0, 1);
            Console.WriteLine(directionColision);
            if (( directionColision == "X") && nbTick >=32)
            {
                Console.WriteLine("Colision detecter lateral");
                DeplacerJoueur(colision,ref rectangleJoueur, imgPerso, canvasJeu);
                nbTick = 0;
            }
            if (directionColision == "Y")
            {
                Console.WriteLine("Colision detecter vertical");
                DeplacerJoueur(colision, ref rectangleJoueur, imgPerso, canvasJeu);
            }

            Canvas.SetLeft(hitboxJoueur, rectangleJoueur.X);
            Canvas.SetTop(hitboxJoueur, rectangleJoueur.Y);

            //faire tomber joueur
            AppliquerGravite(ref rectangleJoueur, imgPerso,ref hitboxJoueur);

            

            // Cercle de debug pour apercevoire le rayon d'attaque
            double centreX = rectangleJoueur.X + (rectangleJoueur.Width / 2);
            double centreY = rectangleJoueur.Y + (rectangleJoueur.Height / 2);

            double left = centreX - rayonAttaque;
            double top = centreY - rayonAttaque;

            Canvas.SetLeft(cercleDebug, left);
            Canvas.SetTop(cercleDebug, top);
            
        }

        private static void DeplacerJoueur(String direction, ref Rect joueurHitbox, Image imgPerso, Canvas canvasJeu)
        {
            string abssice = direction.Substring(0, 1);
            int recul = int.Parse(direction.Substring(1,direction.Length-1));
           
            if (abssice == "X" && joueurHitbox.X + recul >0 && joueurHitbox.X + recul < canvasJeu.ActualWidth)
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

        private void AppliquerGravite(ref Rect joueur, Image imgJoueur, ref System.Windows.Shapes.Rectangle hitboxVisuel)
        {
            vitesseY += gravite;
            joueur.Y += vitesseY;


            if (MethodeColision.EntiteToucheSol(joueur))
            {
                joueur.Y = 900 - joueur.Height;
                vitesseY = 0;

            }


            Canvas.SetTop(imgJoueur, joueur.Y);

        }

        private static void Attaque(List<Ennemies> ennemies, Rect joueur, int degats, int direction)
        {
            int joueurCentreX = (int)(joueur.X + (joueur.Width / 2));
            int joueurCentreY = (int)(joueur.Y + (joueur.Height / 2));

            foreach (Ennemies ennemie in ennemies)
            {
                double ennemiCentreX = ennemie.posLeft + (ennemie.ennemieImg.Width / 2);
                double ennemiCentreY = ennemie.posTop + (ennemie.ennemieImg.Height / 2);

                double distanceX = joueurCentreX - ennemiCentreX;
                double distanceY = joueurCentreY - ennemiCentreY;

                double distanceCarre = (distanceX * distanceX) + (distanceY * distanceY);

                bool estDevant = false;

                if (direction == 1)
                {
                    if (ennemiCentreX > joueurCentreX) estDevant = true;
                }
                else
                    if (ennemiCentreX < joueurCentreX) estDevant = true;

                if (distanceCarre <= (rayonAttaque * rayonAttaque) && estDevant)
                {
                    ennemie.RecevoirDegats(degats);
                    Console.WriteLine("Ennemi touché dans le rayon !");
                }
            }
        }
    }
}


