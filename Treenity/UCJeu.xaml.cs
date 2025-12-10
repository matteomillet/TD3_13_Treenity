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
        Ennemies ennemie1 = new Ennemies();
        public UCJeu()
        {
            InitializeComponent();
            AffichageEntite(ennemie1);
            
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
                imgPerso.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform fliptrans = new ScaleTransform();
                fliptrans.ScaleX = 1;
                imgPerso.RenderTransform = fliptrans;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + 2);
            }


            if (e.Key == Key.Left || e.Key == Key.Q)
            {
                imgPerso.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform fliptrans = new ScaleTransform();
                fliptrans.ScaleX = -1;
                imgPerso.RenderTransform = fliptrans;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) - 2);

            }


        }

        public void AffichageEntite(Ennemies entite)
        {
            Image ennemieImg = new Image();
            ennemieImg.Width = 200;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(entite.style);
            bitmapImage.DecodePixelWidth = 200;
            bitmapImage.EndInit();
            ennemieImg.Source = bitmapImage;
            canvasJeu.Children.Add(ennemieImg);
            Canvas.SetLeft(ennemieImg, entite.posLeft);
            Canvas.SetTop(ennemieImg, entite.posTop);

        }
    }
}


