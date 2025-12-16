using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Treenity
{
    public class GestionSons
    {
        public static double VolumeGlobal = 0.5;
        public static void Jouer(string nomFichier)
        {
            MediaPlayer player = new MediaPlayer();

            // Chargement du son
            player.Open(new Uri($"Ressources/Sons/{nomFichier}", UriKind.Relative));

            // Gestion du volume
            player.Volume = VolumeGlobal;

            player.Play();
        }
    }
}
