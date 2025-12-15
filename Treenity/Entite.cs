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

        public void DeplacerEntite(Canvas canvasJeu)
            /*
            méthode permettant de faire déplacer lateralement une entite. distance peut être négative, l'entite va donc reculer
            */
        {
            if (hitboxLogi.X += vitesseX <= 0 || hitboxLogi.X += vitesseX >= canvasJeu.ActualWidth)
            hitboxLogi.X += vitesseX;
        }

        //Méthode a appeler a chaque tick pour chaque entite 
        public void UpdateVisu()    // Méthode pour mettre à jour la position de l'entité en fonction du Rect hiboxLogi
        {
            Canvas.SetLeft(entiteImg, hitboxLogi.X); // Mise à jour de la position de l'image en abscisse
            Canvas.SetTop(entiteImg, hitboxLogi.Y);   // Mise à jour de la position de l'image en ordonnée

            //Mise à jour de la hitbox visuelle

            Canvas.SetLeft(hitboxVisu, hitboxLogi.X);
            Canvas.SetTop(hitboxVisu, hitboxLogi.Y);

            
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

        
    }
}
