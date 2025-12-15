using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Treenity
{
    public class Joueur : Entite
    {
        public Joueur(Canvas canvas, int pv, int degats, int vitesse, BitmapImage image)
            : base(canvas, pv, degats, vitesse) // Appel du constructeur parent
        {
            rayonAttaque = 200;

            entiteImg.Source = image;   // Image source du joueur
            entiteImg.Width = image.PixelWidth; // Largeur exact de l'image en pixel
            entiteImg.Height = image.PixelHeight;   // Hauteur exact de l'image en pixel

            // Position fixe du joueur
            posLeft = 100;
            posTop = 600;

            hitboxLogi = new System.Windows.Rect(posLeft, posTop, entiteImg.Width, entiteImg.Height);   // Création de la hitbox logique du joueur

            Canvas.SetLeft(entiteImg, posLeft);     // Initialisation de l'image à la position gauche du joueur
            Canvas.SetTop(entiteImg, posTop);       // Initialisation de l'image à la position heute du joueur
            canvasJeu.Children.Add(entiteImg);      // Affichage de l'image sur le canvas

            // Affichage de la hitbox visuelle de l'ennemi
            hitboxVisu = new System.Windows.Shapes.Rectangle
            {
                Width = entiteImg.Width,
                Height = entiteImg.Height,
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };

            Canvas.SetLeft(hitboxVisu, posLeft);
            Canvas.SetTop(hitboxVisu, posTop);

            canvasJeu.Children.Add(hitboxVisu);
            // Fin affichage de la hitbox visuelle
        }


        public void UpdateVisu()    // Méthode pour mettre à jour la position de l'entité en fonction du Rect hiboxLogi
        {
            Canvas.SetLeft(entiteImg, hitboxLogi.X); // Mise à jour de la position de l'image en abscisse
            Canvas.SetTop(entiteImg, hitboxLogi.Y);   // Mise à jour de la position de l'image en ordonnée

            //Mise à jour de la hitbox visuelle

            Canvas.SetLeft(hitboxVisu, hitboxLogi.X);
            Canvas.SetTop(hitboxVisu, hitboxLogi.Y);
        }


        //Methode en commun a Ennemie et Joueur donc a mettre dans entite
        public void Mourir()    // Méthode de mort du joueur
        {
            canvasJeu.Children.Remove(entiteImg);
            canvasJeu.Children.Remove(hitboxVisu);
        }

        //Methode en commun a Ennemie et Joueur donc a mettre dans entite
        public override void RecevoirDegats(int degat)   // Méthode de dégats sur le joueur
        {
            base.RecevoirDegats(degat); // Appel de la méthode RecevoirDegats dans la classe mère

            Console.WriteLine($"AÏE ! Le joueur a pris {degat} dégâts. PV restants : {pv}");

            if (pv <= 0)
            {
                pv = 0;
                Mourir();
            }
        }

        //Methode en commun a Ennemie et Joueur donc a mettre dans entite si l'ennemie peut recevoir du recul
        /*
        public void RecevoirRecul(string direction)
        {
            string abssice = direction.Substring(0, 1);
            int recul = int.Parse(direction.Substring(1, direction.Length - 1));

            if (abssice == "X" && hitboxLogi.X + recul > 0 && hitboxLogi.X + recul < canvasJeu.ActualWidth)
            {
                hitboxLogi.X = hitboxLogi.X + recul;
                Canvas.SetLeft(entiteImg, Canvas.GetLeft(entiteImg) + recul);
                Canvas.SetLeft(hitboxVisu, Canvas.GetLeft(hitboxVisu) + recul);
            }
            else
            {
                hitboxLogi.Y = hitboxLogi.Y + recul;
                Canvas.SetTop(entiteImg, Canvas.GetTop(entiteImg) + recul);
                Canvas.SetTop(hitboxVisu, Canvas.GetTop(hitboxVisu) + recul);
            }
        }
        */
    }
}
