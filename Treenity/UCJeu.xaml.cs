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
        public int vitessePerso = 2;
        Ennemies[] ennemies = new Ennemies[10];
        public UCJeu()
        {
            InitializeComponent();

            for(int i = 0; i < ennemies.Length; i++)
            {
                ennemies[i] = new Ennemies();
                AffichageEntite(ennemies[i]);
            }
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
            imgPerso.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform fliptrans = new ScaleTransform();
            imgPerso.RenderTransform = fliptrans;

            if (e.Key == Key.Right || e.Key == Key.D)
            {  
                fliptrans.ScaleX = 1;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + vitessePerso);
            }


            if (e.Key == Key.Left || e.Key == Key.Q)
            {
                fliptrans.ScaleX = -1;
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) - vitessePerso);
            }
        }

        public void AffichageEntite(Ennemies entite)
        {
            Image ennemieImg = new Image();
            ennemieImg.Source = entite.imageEnnemie;
            canvasJeu.Children.Add(ennemieImg);
            Canvas.SetLeft(ennemieImg, entite.posLeft);
            Canvas.SetTop(ennemieImg, entite.posTop);
        }
    }
}


