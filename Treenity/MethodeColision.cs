using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Treenity
{
    internal class MethodeColision
    {
        private static readonly string RECUL = "40";
        public static string ColisionAvecEnnemies(List<Ennemies> entites, Rect joueur)
        {
            /*
             Méthode permettant de savoir si il y a une colision entre le joueur et un ennemie.
            Si il y a une colision le joueur va se prendre des dégât et reculer.

             */


            

            //Console.WriteLine("Detection des colision avec les ennemies : start");
            for (int i = 0; i < entites.Count; i++)
            {
                //Console.WriteLine("Detection de la colision avec l'ennemie numero : " + i);
                if (joueur.IntersectsWith(entites[i].hitboxLogi))
                {
                    Console.WriteLine("Colision avec l'ennemie numero " + i);

                    return DirectionColision(entites[i].hitboxLogi, joueur);
                    
                }
            }

            return "pas colision";

            
        }

        public static string DirectionColision(Rect entite, Rect joueur)
        {

            string directionColision = "";
            Rect rectIntersect = Rect.Intersect(entite, joueur);


            if (rectIntersect.Height >= rectIntersect.Width)
            {
                if (joueur.X > entite.X)
                    directionColision = "X+"; //droite
                else
                    directionColision = "X-"; //gauche
            }
            else
            {
                if (joueur.Y > entite.Y)
                    directionColision = "Y+"; // bas
                else
                    directionColision = "Y-"; // haut
            }

            directionColision += RECUL;
            //Console.WriteLine($"Position de la hitbox ( rectangle) de l'ennemie {entite.X}, {entite.Y}");
            //Console.WriteLine($"Position de la hitbox du joueur (rectangle joueur) : {joueur.X}, {joueur.Y}");
            //Console.WriteLine($"direction colision: {directionColision}");
            return directionColision;
        }

        public static string ColisionAvecObstacles(Rect[]obstacles, Rect joueur, Canvas canvasJeu)
        {
            if (joueur.X <= 0)
                return "gauche";

            if (joueur.X >= canvasJeu.ActualWidth - joueur.Width)
                return "droite";

            for (int i = 0; i < obstacles.Length; i++)
            {
                if (joueur.IntersectsWith(obstacles[i]))
                {
                    return DirectionColision(obstacles[i], joueur);
                }
            }

            return "pas colision";
        }

        public static bool EntiteToucheSol(Rect entite)
        {
            /*
             Methode permettant de savoir si l'entite mis en argument touche le sol.
             * */

                
            int solY = 1010; //valeur a mettre en global
            //Console.WriteLine(entite.Y);
            if (entite.Y + entite.Height >= solY)
            {
                return  true;
            }
            else
            {
                return false;
            }
        }
    }


}
