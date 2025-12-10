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
using System.Drawing;



namespace Treenity
{
    /// <summary>
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    public partial class UCJeu : UserControl
    {
        Ennemies ennemie1 = new Ennemies();
        public System.Drawing.Rectangle rectangleJoueur = new System.Drawing.Rectangle();
        public UCJeu()
        {
            InitializeComponent();
            AffichageEntite(ennemie1);

            rectangleJoueur.X = (int) Canvas.GetLeft(imgPerso);
            rectangleJoueur.Y = (int)Canvas.GetTop(imgPerso);
            rectangleJoueur.Height = (int) imgPerso.Height;
            rectangleJoueur.Width = (int)imgPerso.Width;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += canvasJeu_KeyDown;
            Application.Current.MainWindow.KeyUp += canvasJeu_KeyUp;
        }

        private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                imgPerso.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                ScaleTransform fliptrans = new ScaleTransform();
                fliptrans.ScaleX = 1;
                imgPerso.RenderTransform = fliptrans;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + 2);
                rectangleJoueur.X = rectangleJoueur.X + 2;
                
            }


            if (e.Key == Key.Left || e.Key == Key.Q)
            {
                imgPerso.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                ScaleTransform fliptrans = new ScaleTransform();
                fliptrans.ScaleX = -1;
                imgPerso.RenderTransform = fliptrans;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) - 2);
                rectangleJoueur.X = rectangleJoueur.X - 2;
            }


        }

        public void AffichageEntite(Ennemies entite)
        {
            Image ennemieImg = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(entite.style);
            bitmapImage.EndInit();
            ennemieImg.Source = bitmapImage;
            canvasJeu.Children.Add(ennemieImg);
            Canvas.SetLeft(ennemieImg, entite.posLeft);
            Canvas.SetTop(ennemieImg, entite.posTop);

        }
    }
}


