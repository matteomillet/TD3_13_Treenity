using System.Reflection;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
        }

        private void AfficheDemarrage()
        {
            UCDemarrage uc = new UCDemarrage();

            ZoneJeu.Content = uc;

            uc.rulesButton.Click += AfficherRegles;
            uc.testFinish.Click += AfficherFinish;
        }

        private void AfficheDemarrage(object sender, RoutedEventArgs e)
        {
            UCDemarrage uc = new UCDemarrage();

            ZoneJeu.Content = uc;

            uc.rulesButton.Click += AfficherRegles;
            uc.testFinish.Click += AfficherFinish;
        }

        private void AfficherRegles(object sender, RoutedEventArgs e)
        {
            UCReglesJeu uc = new UCReglesJeu();

            ZoneJeu.Content = uc;

            uc.boutonRetourRegle.Click += AfficheDemarrage;
        }

        private void AfficherFinish(object sender, RoutedEventArgs e)
        {
            UCFinish uc = new UCFinish();

            ZoneJeu.Content = uc;

            uc.backMenuButton.Click += AfficheDemarrage;
        }
    }
}