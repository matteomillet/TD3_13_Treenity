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
        public const double FORCE_SAUT = -30;
        public int DirectionRegard = 1; // 1 = Droite, -1 = Gauche

        public Joueur(Canvas canvas, int pv, int degats, int vitesse, BitmapImage image)
            : base(canvas, pv, degats, vitesse) // Appel du constructeur parent
        {
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

        public void Deplacer(string direction)  // Méthode de déplacement du joueur
        {
            if (direction == "Gauche")
            {
                posLeft -= vitesse;
                DirectionRegard = -1;
            }
            else if (direction == "Droite")
            {
                posLeft += vitesse;
                DirectionRegard = 1;
            }
            else if (direction == "Saut")
            {
                vitesseY = FORCE_SAUT;
            }

            UpdateVisu();
        }

        public void Mourir()    // Méthode de mort du joueur
        {
            canvasJeu.Children.Remove(entiteImg);
            canvasJeu.Children.Remove(hitboxVisu);
        }

        public void RecevoirDegats(int degat)   // Méthode de dégats sur le joueur
        {
            pv -= degat;
            if (pv < 0)
            {
                pv = 0;
                Mourir();
            }
        }
    }
}
