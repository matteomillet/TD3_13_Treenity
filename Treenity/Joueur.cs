using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Treenity
{
    public class Joueur : Entite
    {
        private int indexAnimMouvement = 0;      // Image à afficher
        private int timerAnimMouvement = 0;      // Compteur de temps
        private int vitesseAnimMouvement = 20;   // Vitesse de l'animation

        private int indexAnimAttaque = 0;      
        private int timerAnimAttaque = 0;      
        private int vitesseAnimAttaque = 8;

        public bool attaque = false;
        BitmapImage[] spriteBucheronAttaqueGauche = new BitmapImage[4];
        BitmapImage[] spriteBucheronAttaqueDroite = new BitmapImage[4];
        BitmapImage[] spriteBucheronMouvementGauche = new BitmapImage[3];
        BitmapImage[] spriteBucheronMouvementDroite = new BitmapImage[3];

        public Joueur(Canvas canvas, int pv, int degats, int vitesse, BitmapImage image)
            : base(canvas, pv, degats, vitesse) // Appel du constructeur parent
        {

            for (int i = 0; i < spriteBucheronMouvementGauche.Length; i++)
            {
                spriteBucheronMouvementGauche[i] = new BitmapImage(new Uri($"pack://application:,,,/Ressources/Images/bucheron/mouvement/bucheron{i + 1}gauche.png"));
            }
            for (int i = 0; i < spriteBucheronMouvementDroite.Length; i++)
            {
                spriteBucheronMouvementDroite[i] = new BitmapImage(new Uri($"pack://application:,,,/Ressources/Images/bucheron/mouvement/bucheron{i + 1}droite.png"));
            }

            for (int i = 0; i < spriteBucheronAttaqueGauche.Length; i++)
            {
                spriteBucheronAttaqueGauche[i] = new BitmapImage(new Uri($"pack://application:,,,/Ressources/Images/bucheron/attaque/bucheron{i + 1}gauche.png"));
            }
            for (int i = 0; i < spriteBucheronAttaqueDroite.Length; i++)
            {
                spriteBucheronAttaqueDroite[i] = new BitmapImage(new Uri($"pack://application:,,,/Ressources/Images/bucheron/attaque/bucheron{i + 1}droite.png"));
            }

            rayonAttaque = 200;

            entiteImg.Source = image;   // Image source du joueur
            entiteImg.Width = image.PixelWidth; // Largeur exact de l'image en pixel
            entiteImg.Height = image.PixelHeight;   // Hauteur exact de l'image en pixel

            // Position fixe du joueur
            posLeft = 200;
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

        public void AnimerMouvement()
        {
            timerAnimMouvement++;

            // Si le compteur atteint la limite (ex: tous les 10 ticks)
            if (timerAnimMouvement >= vitesseAnimMouvement)
            {
                timerAnimMouvement = 0; // On reset le timer
                indexAnimMouvement++;   // On passe à l'image suivante

                // Si on dépasse la fin du tableau (4 images), on revient à 0
                if (indexAnimMouvement >= spriteBucheronMouvementGauche.Length)
                {
                    indexAnimMouvement = 0;

                }

                // MISE A JOUR DE L'IMAGE
                if (directionRegard == 1) // Regarde à Droite
                {
                    entiteImg.Source = spriteBucheronMouvementDroite[indexAnimMouvement];
                }
                else // Regarde à Gauche (-1)
                {
                    entiteImg.Source = spriteBucheronMouvementGauche[indexAnimMouvement];
                }
            }
        }

        public void AnimerAttaque()
        {
            if (attaque)
            {
                timerAnimAttaque++;

                // Si le compteur atteint la limite (ex: tous les 10 ticks)
                if (timerAnimAttaque >= vitesseAnimAttaque)
                {
                    timerAnimAttaque = 0; // On reset le timer
                    indexAnimAttaque++;   // On passe à l'image suivante

                    // Si on dépasse la fin du tableau (4 images), on revient à 0
                    if (indexAnimAttaque >= spriteBucheronAttaqueGauche.Length)
                    {
                        attaque = false;
                        indexAnimAttaque = 0;
                    }

                    // MISE A JOUR DE L'IMAGE
                    if (directionRegard == 1) // Regarde à Droite
                    {
                        entiteImg.Source = spriteBucheronAttaqueDroite[indexAnimAttaque];
                    }
                    else // Regarde à Gauche (-1)
                    {
                        entiteImg.Source = spriteBucheronAttaqueGauche[indexAnimAttaque];
                    }
                }
            }
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
