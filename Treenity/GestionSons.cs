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
        public static void Jouer(string nomFichier)
        {
            // Création d'un nouveau lecteur à chaque fois pour pouvoir superposer les sons (ex: 2 ennemis meurent en même temps)
            MediaPlayer player = new MediaPlayer();

            // Chargement du son
            player.Open(new Uri($"Ressources/Sons/{nomFichier}", UriKind.Relative));

            player.Play();
        }
    }
}
