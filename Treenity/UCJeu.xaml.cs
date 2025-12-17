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
        public static int pvJoueur = 150;
        public static int pvGodMode = 10000;
        public static int degatsJoueur = 10;
        public static int degatsGodMode = 10000;
        public static int vitesseJoueur = 4;
        public double vitesseY = 0;
        public int nbEnnemis = 4;
        public int nbNiveau = 9;
        public const double gravite = 3;
        public const double FORCE_SAUT = -40;
        public bool godMode = false;

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

            Application.Current.MainWindow.MouseLeftButtonDown += canvasJeu_MouseLeftButtonUp;

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
                if (element is Image image && !( image.Name == "background1" || image.Name == "background2"))
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
            joueur = new Joueur(canvasJeu, pvJoueur, degatsJoueur, vitesseJoueur, joueurImg);
        }

        private void InitializeEnnemies()
        {
            Random rand = new Random();
            ennemies = new List<Ennemies>();
            for(int i = 0; i < nbEnnemis; i++)
            {        
                double choixEnnemi = rand.NextDouble();
                string ennemi = "";
                foreach(KeyValuePair<String, double> chance in App.chanceEnnemis)
                {
                    if(choixEnnemi < chance.Value)
                    {
                        ennemi = chance.Key;
                        break;
                    }
                }
                ennemies.Add(new Ennemies(canvasJeu, App.statsEnnemis[ennemi][0], App.statsEnnemis[ennemi][1], App.statsEnnemis[ennemi][2], ennemi));
            }


        }

        private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
        {
            Key toucheDroite, toucheGauche, toucheSaut;

            if (App.ModeZQSD == true)
            {
                toucheDroite = Key.D;
                toucheGauche = Key.Q;
                toucheSaut = Key.Z;
            }
            else
            {
                toucheDroite = Key.Right;
                toucheGauche = Key.Left;
                toucheSaut = Key.Up;
            }

            if (e.Key == toucheDroite || e.Key == toucheGauche)
            {
                joueur.vitesseX = 0;
            }
            

            if (e.Key == toucheSaut && MethodeColision.EntiteToucheSol(joueur.hitboxLogi))
            {
                joueur.vitesseY += FORCE_SAUT;
            }

            if (e.Key == Key.G)
            {
                if(godMode == false)
                {
                    godMode = true;
                    joueur.pv = pvGodMode;
                    joueur.degats = degatsGodMode;
                }
                else
                {
                    godMode = false;
                    joueur.pv = pvJoueur;
                    joueur.degats = degatsJoueur;
                }
            }
        }

        private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
        {

            Key toucheDroite, toucheGauche;

            if (App.ModeZQSD == true)
            {
                toucheDroite = Key.D;
                toucheGauche = Key.Q;
            }
            else
            {
                toucheDroite = Key.Right;
                toucheGauche = Key.Left;
            }

            if (e.Key == toucheDroite)
            {
                joueur.directionRegard = 1;
                joueur.vitesseX = 4;
                joueur.attaque = false;
            }
            if (e.Key == toucheGauche)
            {
                joueur.directionRegard = -1;
                joueur.vitesseX = -4;
                joueur.attaque = false;
            }

            
            if (e.Key == Key.P)
            {
                ProchainNiveau();
            }


            //Console.WriteLine($"Position du joueur : {Canvas.GetLeft(imgPerso)}, {Canvas.GetTop(imgPerso)}");
            Console.WriteLine($"Position de la hitbox du joueur (rectangle joueur) : {joueur.hitboxLogi.X}, {joueur.hitboxLogi.Y}");
        }

        private void canvasJeu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (joueur.attaque == false)
            {
                foreach (Ennemies ennemie in ennemies)
                    joueur.Attaque(ennemie);

                joueur.attaque = true;
                Console.WriteLine(joueur.attaque);
            }
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

        private void MettreAJourInterface()
        {
            double pourcentage = (double)joueur.pv / joueur.pvMax;

            if (pourcentage < 0) pourcentage = 0;

            rectVieJoueur.Width = 200 * pourcentage;

            txtVieJoueur.Text = $"{joueur.pv} / {joueur.pvMax}";
        }
        private void GameOver()
        {
            // 1. On arrête le temps
            minuterie.Stop();

            // 2. On affiche l'écran de fin
            GridGameOver.Visibility = Visibility.Visible;
        }

        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            minuterie.Stop();
            GridMenuPause.Visibility = Visibility.Visible;
        }

        private void ReprendreClick(object sender, RoutedEventArgs e)
        {
            GridMenuPause.Visibility = Visibility.Collapsed;
            minuterie.Start();
            canvasJeu.Focus();
        }

        private void MenuClick(object sender, RoutedEventArgs e)
        {
            minuterie.Stop();
        }

        private int nbTick = 0;
        private void Jeu(object? sender, EventArgs e)
        {
            nbTick++;

            joueur.AnimerAttaque();
            for(int i = ennemies.Count - 1; i >= 0; i--)
            {
                if (ennemies[i].pv <= 0)
                {
                    ennemies[i].Mourir();
                    ennemies.RemoveAt(i);
                    // Console.WriteLine($"Ennemis restants : {ennemies.Count}");

                    if (ennemies.Count == 0)
                    {
                        if(nbNiveau < 10)
                        {
                        minuterie.Stop();
                        ProchainNiveau();
                        minuterie.Start();
                        }
                        else if(nbNiveau == 10)
                        {
                            minuterie.Stop();
                            titreFin.Text = "VICTOIRE";
                            titreFin.Foreground = Brushes.Green;
                            texteFin.Text = "Félicitations héros,\nvous êtes venu à bout de l'arbre sacré !";
                            effetFin.Fill = Brushes.Green;
                            GameOver();
                        }
                    }
                    continue;
                }

                //Console.WriteLine("l'ennemie va peut etre tomber");
                //Console.WriteLine("Tomber Y = " + ennemies[i].hitboxLogi.Y);
                ennemies[i].Decision(joueur);
                //Console.WriteLine("MoveEnnemie Y = " + ennemies[i].hitboxLogi.Y);
            }

            if(joueur.pv <= 0)
            {
                GameOver();
            }


            joueur.AppliquerGravite(obstacleHitbox);
            if (joueur.cooldownReculActuel > 0)
                joueur.cooldownReculActuel--;

            double deplacementTotalX = joueur.vitesseX + joueur.vitesseXRecul;
            if (deplacementTotalX != 0 && joueur.attaque == false)
                joueur.AnimerMouvement();

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
                if (ennemies[i].cooldownReculActuel > 0)
                    ennemies[i].cooldownReculActuel--;
            }
            
            

            string colision = MethodeColision.ColisionAvecEnnemies(ennemies, joueur.hitboxLogi);

            if (colision != "pas colision")
            {
                //Console.WriteLine("Colision detecter");
            }

            
            joueur.UpdateVisu();
            MettreAJourInterface();
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
                    if (godMode == true)
                        joueur.pv = pvGodMode;
                    else
                        joueur.pv = joueur.pvMax;
                    nbEnnemis += 2;
                    if (nbNiveau == 9)
                    {
                        nbEnnemis = 1;
                        App.chanceEnnemis["boss"] = 1;
                    }
                    nbNiveau += 1;
                    numeroNiveau.Text = $"Niveau {nbNiveau}";
                    InitializeEnnemies();

                    foreach (UIElement element in canvasJeu.Children)
                    {
                        if (element is Image image && (image.Name == "background1" || image.Name == "background2"))
                        {
                            double top = Canvas.GetTop(image);
                            if (top >= canvasJeu.ActualHeight) // si complètement en bas
                            {
                                Canvas.SetTop(image, -image.Height); // le replacer en haut
                            }
                        }
                    }
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


