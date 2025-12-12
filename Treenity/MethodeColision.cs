using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Treenity
{
    internal class MethodeColision
    {
        public static bool ColisionAvecEnnemies(Ennemies[] entites, Rect joueur)
        {
            /*
             Méthode permettant de savoir si il y a une colision entre le joueur et un ennemie.
            Si il y a une colision le joueur va se prendre des dégât et reculer.

             */


            bool colision = false;

            //Console.WriteLine("Detection des colision avec les ennemies : start");
            for (int i = 0; i < entites.Length; i++)
            {
                //Console.WriteLine("Detection de la colision avec l'ennemie numero : " + i);
                if (joueur.IntersectsWith(entites[i].rectangle))
                {
                    //Console.WriteLine("Colision avec l'ennemie numero " + i);

                    DirectionColision(entites[i].rectangle, joueur);
                    return true;
                }
            }

            return colision;
        }

        public static string DirectionColision(Rect entite, Rect joueur)
        {

            string directionColision = "";
            double distanceRect = Math.Sqrt(Math.Pow((joueur.X - entite.X), 2) + Math.Pow((joueur.Y - entite.Y), 2));
            Rect rectIntersect = Rect.Intersect(entite, joueur);

            if (rectIntersect.Height > rectIntersect.Width)
            {
                if (joueur.X > entite.X)
                    directionColision = "droite";
                else
                    directionColision = "gauche";
            }
            else
            {
                if (joueur.Y > entite.Y)
                    directionColision = "bas";
                else
                    directionColision = "haut";
            }

            Console.WriteLine($"Position de la hitbox ( rectangle) de l'ennemie {entite.X}, {entite.Y}");
            Console.WriteLine($"Position de la hitbox du joueur (rectangle joueur) : {joueur.X}, {joueur.Y}");
            Console.WriteLine($"direction colision: {directionColision}");
            return directionColision;
        }

        public static string ColisionAvecObstacles(Rect[]obstacles, Rect joueur )
        {

            for (int i = 0; i < obstacles.Length; i++)
            {
                if (joueur.IntersectsWith(obstacles[i]))
                {
                    return DirectionColision(obstacles[i], joueur);
                }
            }

            return "pas colision";
        }
    }


}
