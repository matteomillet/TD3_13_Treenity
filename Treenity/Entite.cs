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

        public void DeplacerEntite(int distance, Canvas canvasJeux)
            /*
            méthode permettant de faire déplacer lateralement une entite. distance peut être négative, l'entite va donc reculer
            */
        {
            if (hitboxLogi.Y + distance >= 0 && hitboxLogi.Y + distance <= canvasJeu.ActualWidth) //ajouter la condition que l'entite ne sera pas bloquer par un obstacle
                hitboxLogi.Y += distance;
        }

        //Méthode a appeler a chaque tick pour chaque entite 
        public void UpdateVisu()    // Méthode pour mettre à jour la position de l'entité
        {
            Canvas.SetLeft(entiteImg, posLeft); // Mise à jour de la position de l'image en abscisse
            Canvas.SetTop(entiteImg, posTop);   // Mise à jour de la position de l'image en ordonnée

            // Mise à jour de la hitbox logique

            hitboxLogi.X = posLeft;
            hitboxLogi.Y = posTop;

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

            Canvas.SetTop(entiteImg, hitboxLogi.Y);
        }

        
    }
}
