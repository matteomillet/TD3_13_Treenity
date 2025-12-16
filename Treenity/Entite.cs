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
        public double vitesseXRecul = 0;
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
            pvMax = pvInit;
            pv = pvInit;
            degats = degatsInit;
            vitesse = vitesseInit;

            entiteImg = new Image();
        }

        //Méthode a appeler a chaque tick pour chaque entite 
        public void AppliquerGravite(List<Rect> obstacles)
        {
            
            vitesseY += GRAVITE;

            double nouveauY = hitboxLogi.Y + vitesseY;

            bool collisionVerticale = false;

            foreach (Rect obstacle in obstacles)
            {
                // Chevauchement horizontal uniquement
                if (hitboxLogi.Right <= obstacle.Left ||
                    hitboxLogi.Left >= obstacle.Right)
                    continue;

                // Atterrissage
                if (vitesseY > 0 &&
                    hitboxLogi.Bottom <= obstacle.Top &&
                    nouveauY + hitboxLogi.Height >= obstacle.Top)
                {
                    nouveauY = obstacle.Top - hitboxLogi.Height;
                    collisionVerticale = true;
                    break;
                }

                // Tête contre plafond
                if (vitesseY < 0 &&
                    hitboxLogi.Top >= obstacle.Bottom &&
                    nouveauY <= obstacle.Bottom)
                {
                    nouveauY = obstacle.Bottom;
                    collisionVerticale = true;
                    break;
                }
            }

            //Application du mouvement
            hitboxLogi.Y = nouveauY;

            if (collisionVerticale)
                vitesseY = 0;

            //Sol global
            if (hitboxLogi.Bottom >= 1010)
            {
                hitboxLogi.Y = 1010 - hitboxLogi.Height;
                vitesseY = 0;
            }
        }




        public virtual void RecevoirDegats(int degats)
        {
            pv -= degats;
            if (pv < 0) pv = 0;
            GestionSons.Jouer("hitSound");
        }

        public void Attaque(Entite cible)
        {
            int centreEntiteX = (int)(hitboxLogi.X + (hitboxLogi.Width / 2));
            int centreEntiteY = (int)(hitboxLogi.Y + (hitboxLogi.Height / 2));

            double centreCibleX = cible.hitboxLogi.X + (cible.hitboxLogi.Width / 2);
            double centreCibleY = cible.hitboxLogi.Y + (cible.hitboxLogi.Height / 2);

            double distanceX = centreEntiteX - centreCibleX;
            double distanceY = centreEntiteY - centreCibleY;

            double distanceCarre = (distanceX * distanceX) + (distanceY * distanceY);

            bool estDevant = false;

            if (directionRegard == 1)
            {
                if (centreCibleX > centreEntiteX) estDevant = true;
            }
            else
                if (centreCibleX < centreEntiteX) estDevant = true;

            if (distanceCarre <= (rayonAttaque * rayonAttaque) && estDevant)
            {
                cible.RecevoirDegats(degats);
                Console.WriteLine("Ennemi touché dans le rayon !");
            }
        }

        public void RecevoirRecul(string direction)
        {
            string abssice = direction.Substring(0, 1);
            int recul = int.Parse(direction.Substring(1, direction.Length - 1));

            if (abssice == "X" && hitboxLogi.X + recul > 0 && hitboxLogi.X + recul < canvasJeu.ActualWidth)
            {
                vitesseY -= 30;
                vitesseXRecul += recul;

            }
            else
            {
                hitboxLogi.Y = hitboxLogi.Y + recul;
                
            }
        }
    }
}
