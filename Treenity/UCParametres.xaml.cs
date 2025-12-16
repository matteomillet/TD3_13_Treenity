using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour UCParametres.xaml
    /// </summary>
    public partial class UCParametres : UserControl
    {
        public UCParametres()
        {
            InitializeComponent();

            // Initialisation du slider à la valeur actuelle
            SliderVolume.Value = GestionSons.VolumeGlobal;

            // Mise à jour du texte de pourcentage
            UpdateTextePourcentage();

            if (App.ModeZQSD == true)
                RadioZQSD.IsChecked = true;
            else
                RadioFleches.IsChecked = true;
        }

        private void SliderChangementVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GestionSons.VolumeGlobal = SliderVolume.Value;

            // 3. Mise à jour visuelle du texte (ex: "0.5" devient "50%")
            UpdateTextePourcentage();
        }

        private void UpdateTextePourcentage()
        {
            TextPourcentage.Text = $"{(int)(GestionSons.VolumeGlobal * 100)}%";
        }

        private void ChangerControles(object sender, RoutedEventArgs e)
        {
            if (RadioZQSD.IsChecked == true)
                App.ModeZQSD = true;
            else
                App.ModeZQSD = false;
        }
    }
}
