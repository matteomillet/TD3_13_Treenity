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
        public List<Rect> obstacleHitbox;
        private static DispatcherTimer minuterie;
        public Ellipse cercleDebug;
        public double vitesseY = 0; 
        public const double gravite = 3;
        public const double FORCE_SAUT = -40;

        Dictionary<String, double> chanceEnnemis = new Dictionary<String, double>()
        {
            {"citrouille", 0},{"vers", 0},{"fantome", 0.1},{"lapin", 0.4},{"volatile", 1}
        };

        private bool scrollNiveauEnCours = false;
        private double scrollRestant = 0;
        private double scrollVitesse = 5;
        public UCJeu()
        {
            InitializeComponent();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += canvasJeu_KeyDown;

            Application.Current.MainWindow.KeyUp += canvasJeu_KeyUp;

            InitializeObstacleHitbox();
            InitializeJoueur();
            InitializeEnnemies();
            InitializeTimer();
        }

        private void InitializeObstacleHitbox()
        {
            obstacleHitbox = new List<Rect>();
            foreach (UIElement element in canvasJeu.Children)
            {
                if (element is Image image && image.Name != "background")
                {
                    Rect obstacle = new Rect
                    (
                        Canvas.GetLeft(image),
                        Canvas.GetTop(image),
                        image.ActualWidth,
                        image.ActualHeight
                    );
                    obstacleHitbox.Add( obstacle ); 
                }
            }

            for (int i = 0; i < obstacleHitbox.Count; i++)
            {
                Rect r = obstacleHitbox[i];
                Console.WriteLine($"Obstacle #{i + 1} -> Position: ({r.X}, {r.Y}), Taille: {r.Width}x{r.Height}");
            }
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
            for(int i = 0; i < 1;  i++)
            {        
                double choixEnnemi = rand.NextDouble();
                string ennemi = "";
                foreach(KeyValuePair<String, double> chance in chanceEnnemis)
                {
                    if(choixEnnemi < chance.Value)
                    {
                        ennemi = chance.Key;
                        break;
                    }
                }
                ennemies.Add(new Ennemies(canvasJeu, 50, 10, 1, ennemi));
            }


        }

        private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D || e.Key == Key.Left || e.Key == Key.Q)
            {
                joueur.vitesseX = 0;
            }
            if (e.Key == Key.Enter)
            {
                foreach (Ennemies ennemie in ennemies)
                {
                    joueur.Attaque(ennemie);
                }
            }

            if (e.Key == Key.Space && MethodeColision.EntiteToucheSol(joueur.hitboxLogi))
            {
                joueur.vitesseY += FORCE_SAUT;
            }
        }

        private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                joueur.directionRegard = 1;
                joueur.vitesseX = 4;
            }
            if (e.Key == Key.Left || e.Key == Key.Q)
            {
                joueur.directionRegard = -1;
                joueur.vitesseX = -4;
            }
           

            if (e.Key == Key.P)
            {
                ProchainNiveau();
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
                    // Console.WriteLine($"Ennemis restants : {ennemies.Count}");

                    //if (ennemies.Count == 0)
                        //ProchainNiveau();
                    continue;
                }

                //Console.WriteLine("l'ennemie va peut etre tomber");
                //Console.WriteLine("Tomber Y = " + ennemies[i].hitboxLogi.Y);
                ennemies[i].Decision(joueur);
                //Console.WriteLine("MoveEnnemie Y = " + ennemies[i].hitboxLogi.Y);
            }


            joueur.AppliquerGravite(obstacleHitbox);

            double deplacementTotalX = joueur.vitesseX + joueur.vitesseXRecul;

            //futur déplacement du joueur
            Rect futurJoueur = joueur.hitboxLogi;
            futurJoueur.X += deplacementTotalX;

            //est ce que le futur déplacement du joueur va être en colision avec un obstacle 
            string colisionObstacle = MethodeColision.ColisionAvecObstacles(
                obstacleHitbox,
                futurJoueur,
                canvasJeu
            );

            bool dansLimites =
                futurJoueur.X > 0 &&
                futurJoueur.X < canvasJeu.ActualWidth - joueur.entiteImg.Width;

            //futur déplacement du joueur est dans les limite de la map et n'entre pas en colision avec un obstacle donc on applique le déplacement
            if (dansLimites && colisionObstacle == "pas colision")
            {
                joueur.hitboxLogi.X = futurJoueur.X;
            }
            else
            {
                // Blocage par l'obstacle
                joueur.vitesseX = 0;
            }

            //gère la fluidité du recul, si il y en a un
            if (joueur.vitesseXRecul > 0)
            {
                joueur.vitesseXRecul -= 4;
                if (joueur.vitesseXRecul < 0)
                    joueur.vitesseXRecul = 0;
            }
            else
            {
                joueur.vitesseXRecul += 4;
                if (joueur.vitesseXRecul > 0)
                    joueur.vitesseXRecul = 0;
            }


            for (int i = ennemies.Count - 1; i >= 0; i--)
            {
                ennemies[i].AppliquerGravite(obstacleHitbox);
            }
            

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

            if (scrollNiveauEnCours)
            {
                double increment = Math.Min(scrollVitesse, scrollRestant);

                foreach (UIElement element in canvasJeu.Children)
                {
                    if (element is Image image)
                    {
                        double top = Canvas.GetTop(image);
                        if (double.IsNaN(top)) top = 0;
                        Canvas.SetTop(image, top + increment);
                    }
                }

                for (int i = 0; i < obstacleHitbox.Count; i++)
                {
                    Rect rectangle = obstacleHitbox[i];
                    rectangle.Y += increment;
                    obstacleHitbox[i] = rectangle;
                }

                scrollRestant -= increment;
                if (scrollRestant <= 0)
                {
                    scrollNiveauEnCours = false;
                }
            }
        }

        private void ProchainNiveau()
        {
            scrollRestant = 1080; // distance totale à scroller
            scrollNiveauEnCours = true;
        }
    }
}


