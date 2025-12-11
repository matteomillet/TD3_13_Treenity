using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



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
        
        public UCJeu()
        {
            InitializeComponent();
            InitializeJoueur();
            InitializeEnnemies();  
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
                ennemies[i] = new Ennemies();
                AffichageEntite(ennemies[i]);
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
        }

        public void AffichageEntite(Ennemies entite)
        {
            Image ennemieImg = new Image();
            ennemieImg.Source = Ennemies.imageEnnemie;
            canvasJeu.Children.Add(ennemieImg);
            Canvas.SetLeft(ennemieImg, entite.posLeft);
            Canvas.SetTop(ennemieImg, entite.posTop);
        }

        public bool Colision(Rect[] entites, Rect joueur)
        {
            /*
             Le rectangle du joueur entre en colision avec rectangle dans liste si oui = true sinon = false

             */


            bool colision = true;

            return colision;
        }
    }
}


