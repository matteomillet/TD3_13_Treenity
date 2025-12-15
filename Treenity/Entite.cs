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
    public class Entite
    {
        private const double GRAVITE = 3;
        public Canvas canvasJeu;   // Canvas sur lequel dessiné

        public double vitesseX = 0;
        public int DirectionRegard = 1;
        public int pvMax;   // PV Max de l'entité
        public int pv;      // PV actuel de l'entité
        public int degats;  // Dégats de l'entité
        public int vitesse; // Vitesse de l'entité
        public double vitesseY; // Force de gravité
        public int directionRegard;
        public int rayonAttaque;

        public double posTop;   // Position haute de l'entité
        public double posLeft;  // Position gauche de l'entité

        public Image entiteImg;  // Image de l'entité

        public Rect hitboxLogi;  // Hitbox logique de l'entité
        public System.Windows.Shapes.Rectangle hitboxVisu;  // Hitbox visuelle de l'entité

        public Entite(Canvas canvas, int pvInit, int degatsInit, int vitesseInit)   // Constructeur de la classe parent Entite
        {
            canvasJeu = canvas;
            pv = pvInit;
            degats = degatsInit;
            vitesse = vitesseInit;

            entiteImg = new Image();
        }

        public void DeplacerEntite(Canvas canvasJeux)
            /*
            méthode permettant de faire déplacer lateralement une entite. distance peut être négative, l'entite va donc reculer
            */
        {
            if (hitboxLogi.Y + vitesse >= 0 && hitboxLogi.Y + vitesse <= canvasJeu.ActualWidth) //ajouter la condition que l'entite ne sera pas bloquer par un obstacle
                hitboxLogi.Y += vitesse * directionRegard;
        }

        //Méthode a appeler a chaque tick pour chaque entite 
        public void UpdateVisu()    // Méthode pour mettre à jour la position de l'entité en fonction du Rect hiboxLogi
        {
            Canvas.SetLeft(entiteImg, hitboxLogi.X); // Mise à jour de la position de l'image en abscisse
            Canvas.SetTop(entiteImg, hitboxLogi.Y);   // Mise à jour de la position de l'image en ordonnée

            //Mise à jour de la hitbox visuelle

            Canvas.SetLeft(hitboxVisu, posLeft);
            Canvas.SetTop(hitboxVisu, posTop);   
        }

        //Méthode a appeler a chaque tick pour chaque entite 
        public void AppliquerGravite()
        {
            vitesseY += GRAVITE;
            hitboxLogi.Y += vitesseY;

            if (MethodeColision.EntiteToucheSol(hitboxLogi))
            {
                hitboxLogi.Y = 900 - hitboxLogi.Height;
                vitesseY = 0;
            }

        }

        public virtual void RecevoirDegats(int degats)
        {
            pv -= degats;
            if (pv < 0) pv = 0;
        }

        public void Attaque(Entite cible)
        {
            int centreEntiteX = (int)(hitboxLogi.X + (hitboxLogi.Width / 2));
            int centreEntiteY = (int)(hitboxLogi.Y + (hitboxLogi.Height / 2));

            double centreCibleX = cible.posLeft + (cible.entiteImg.Width / 2);
            double centreCibleY = cible.posTop + (cible.entiteImg.Height / 2);

            double distanceX = centreEntiteX - centreCibleX;
            double distanceY = centreEntiteY - centreCibleY;

            double distanceCarre = (distanceX * distanceX) + (distanceY * distanceY);

            bool estDevant = false;

            if (this.directionRegard == 1)
            {
                if (centreCibleX > centreEntiteX) estDevant = true;
            }
            else
                if (centreCibleX < centreEntiteX) estDevant = true;

            if (distanceCarre <= (rayonAttaque * rayonAttaque) && estDevant)
            {
                cible.RecevoirDegats(this.degats);
                Console.WriteLine("Ennemi touché dans le rayon !");
            }
        }
    }
}
