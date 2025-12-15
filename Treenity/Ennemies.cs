using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Treenity
{
    public class Ennemies : Entite
    {
        private static Random rand = new Random();  // Générateur de nombre aléatoire

        public System.Windows.Shapes.Rectangle barrePV; // Barre de PV de l'ennemi
        public System.Windows.Shapes.Rectangle barrePVMax;  // Barre de PV Max de l'ennemi

        public Ennemies(Canvas canvas, int pv, int degats, int vitesse, BitmapImage image)
            : base(canvas, pv, degats, vitesse) // Appel du constructeur parent
        {

            entiteImg.Source = image;   // Image source de l'ennemi
            entiteImg.Width = image.PixelWidth; // Largeur exact de l'image en pixel
            entiteImg.Height = image.PixelHeight;   // Hauteur exact de l'image en pixel

            posLeft = rand.Next(0, (int)(canvasJeu.ActualWidth - entiteImg.Width)); // Position aléatoire de l'ennemi en abscisse
            posTop = rand.Next(0, (int)(canvasJeu.ActualHeight - entiteImg.Height));    // Position aléatoire de l'ennemi en ordonnée

            hitboxLogi = new Rect(posLeft, posTop, entiteImg.Width, entiteImg.Height);   // Création de la hitbox logique de l'ennemie

            Canvas.SetLeft(entiteImg, posLeft);     // Initialisation de l'image à la position gauche de l'ennemi
            Canvas.SetTop(entiteImg, posTop);       // Initialisation de l'image à la position heute de l'ennemi
            canvasJeu.Children.Add(entiteImg);      // Affichage de l'image sur le canvas

            // Affichage de la hitbox visuelle de l'ennemi
            hitboxVisu = new System.Windows.Shapes.Rectangle
            {
                Width = entiteImg.Width,
                Height = entiteImg.Height,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            Canvas.SetLeft(hitboxVisu, posLeft);
            Canvas.SetTop(hitboxVisu, posTop);

            canvasJeu.Children.Add(hitboxVisu);
            // Fin affichage de la hitbox visuelle

            CreerBarreDeVie();  // Appel de la fonction pour creer la barre de vie
        }

        private void CreerBarreDeVie()  // Méthode de création de la barre de vie de l'ennemi
        {
            // Fond noir
            barrePVMax = new System.Windows.Shapes.Rectangle() { Width = 80, Height = 10, Fill = Brushes.Black };
            Canvas.SetLeft(barrePVMax, posLeft + (entiteImg.Width - barrePVMax.Width) / 2);
            Canvas.SetTop(barrePVMax, posTop + entiteImg.Height + 5);
            canvasJeu.Children.Add(barrePVMax);

            // Barre verte
            barrePV = new System.Windows.Shapes.Rectangle() { Width = 80, Height = 10, Fill = Brushes.Green };
            Canvas.SetLeft(barrePV, posLeft + (entiteImg.Width - 80) / 2);
            Canvas.SetTop(barrePV, posTop + entiteImg.Height + 5);
            canvasJeu.Children.Add(barrePV);
        }

        public void UpdateVisu()    // Méthode pour mettre à jour la position de l'entité en fonction du Rect hiboxLogi
        {
            Canvas.SetLeft(entiteImg, hitboxLogi.X); // Mise à jour de la position de l'image en abscisse
            Canvas.SetTop(entiteImg, hitboxLogi.Y);   // Mise à jour de la position de l'image en ordonnée

            //Mise à jour de la hitbox visuelle

            Canvas.SetLeft(hitboxVisu, hitboxLogi.X);
            Canvas.SetTop(hitboxVisu, hitboxLogi.Y);

            //Mise à jour de la barre de PV
            Canvas.SetLeft(barrePVMax, hitboxLogi.X + (entiteImg.Width - barrePVMax.Width) / 2);
            Canvas.SetTop(barrePVMax, hitboxLogi.Y + entiteImg.Height + 5);

            Canvas.SetLeft(barrePV, hitboxLogi.X + (entiteImg.Width - 80) / 2);
            Canvas.SetTop(barrePV, hitboxLogi.Y + entiteImg.Height + 5);

        }

        //MoveEnnemie qui peut être changer et genre elle appele une methode dans entite qui fait déplacer l'entite
        public void MoveEnnemie(Joueur joueur)  // Méthode de déplacement de l'ennemi
        {
            if (hitboxLogi.Right < joueur.hitboxLogi.Left)
                directionRegard = 1;
            else if (hitboxLogi.Left > joueur.hitboxLogi.Right)
                directionRegard = -1;

            // DÉPLACEMENT LOGIQUE
            hitboxLogi.X += vitesse * directionRegard;

            // Mise à jour visuelle
            
            // Barre de vie
            double decalage = (entiteImg.Width - 80) / 2;
            Canvas.SetLeft(barrePVMax, hitboxLogi.X + decalage);
            Canvas.SetLeft(barrePV, hitboxLogi.X + decalage);
        }

        //Methode en commun a Ennemie et Joueur donc a mettre dans entite
        public void Mourir()    // Méthode de mort de l'ennemi
        {
            canvasJeu.Children.Remove(entiteImg);
            canvasJeu.Children.Remove(barrePV);
            canvasJeu.Children.Remove(barrePVMax);
            canvasJeu.Children.Remove(hitboxVisu);
        }

        //Methode en commun a Ennemie et Joueur donc a mettre dans entite
        public override void RecevoirDegats(int degat)   // Méthode de dégats sur l'ennemi
        {
            base.RecevoirDegats(degats);    // Appel de la méthode RecevoirDegats dans la classe mère

            if (pv < 0)
            {
                pv = 0;
                Mourir();
            }

            // Mise à jour de la barre de vie
            double pourcentage = (double)pv / pvMax;

            
            
            barrePV.Width = 80 * pourcentage;

            if (pourcentage > 0.5)
                barrePV.Fill = Brushes.Green;
            else if (pourcentage > 0.25)
                barrePV.Fill = Brushes.Orange;
            else
                barrePV.Fill = Brushes.Red;
            
        }
    }
}
