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

        public static int COOLDOWN_ATTAQUE = 60;
        public int cooldownActuel;

        BitmapImage[] spriteEnnemiGauche = new BitmapImage[4];
        BitmapImage[] spriteEnnemiDroite = new BitmapImage[4];

        private int indexAnim = 0;      // Image à afficher
        private int timerAnim = 0;      // Compteur de temps
        private int vitesseAnim = 10;   // Vitesse de l'animation

        public Ennemies(Canvas canvas, int pv, int degats, int vitesse, string name)
            : base(canvas, pv, degats, vitesse) // Appel du constructeur parent
        {
            for (int i = 0; i < spriteEnnemiGauche.Length; i++)
            {
                spriteEnnemiGauche[i] = new BitmapImage(new Uri($"pack://application:,,,/Ressources/Images/{name}/{name}{i+1}gauche.png"));
            }
            for (int i = 0; i < spriteEnnemiDroite.Length; i++)
            {
                spriteEnnemiDroite[i] = new BitmapImage(new Uri($"pack://application:,,,/Ressources/Images/{name}/{name}{i + 1}droite.png"));
            }

            rayonAttaque = 150;

            entiteImg.Source = spriteEnnemiGauche[0];   // Image source de l'ennemi
            entiteImg.Width = spriteEnnemiGauche[0].PixelWidth; // Largeur exact de l'image en pixel
            entiteImg.Height = spriteEnnemiGauche[0].PixelHeight;   // Hauteur exact de l'image en pixel

            posLeft = rand.Next(500, (int)(canvasJeu.ActualWidth - entiteImg.Width)); // Position aléatoire de l'ennemi en abscisse
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

        public void Animer()
        {
            timerAnim++;

            // Si le compteur atteint la limite (ex: tous les 10 ticks)
            if (timerAnim >= vitesseAnim)
            {
                timerAnim = 0; // On reset le timer
                indexAnim++;   // On passe à l'image suivante

                // Si on dépasse la fin du tableau (4 images), on revient à 0
                if (indexAnim >= spriteEnnemiGauche.Length)
                {
                    indexAnim = 0;
                }

                // MISE A JOUR DE L'IMAGE
                if (directionRegard == 1) // Regarde à Droite
                {
                    entiteImg.Source = spriteEnnemiDroite[indexAnim];
                }
                else // Regarde à Gauche (-1)
                {
                    entiteImg.Source = spriteEnnemiGauche[indexAnim];
                }
            }
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
            // DÉPLACEMENT LOGIQUE
            hitboxLogi.X += vitesse * directionRegard;
            
            // Barre de vie
            double decalage = (entiteImg.Width - 80) / 2;
            Canvas.SetLeft(barrePVMax, hitboxLogi.X + decalage);
            Canvas.SetLeft(barrePV, hitboxLogi.X + decalage);
        }

        public void Decision(Joueur joueur)
        {

            double centreEnnemiX = hitboxLogi.X + (hitboxLogi.Width / 2);
            double centreJoueurX = joueur.hitboxLogi.X + (joueur.hitboxLogi.Width / 2);

            if (centreJoueurX > centreEnnemiX)
                directionRegard = 1;
            else
                directionRegard = -1;

            MoveEnnemie(joueur);
            Animer();

            if (cooldownActuel > 0)
            {
                // Si le cooldown n'est pas fini, on attend
                cooldownActuel--;
            }
            else
            {
                // Si le cooldown est à 0, on tente une attaque !

                // "this" (L'ennemi) attaque "leJoueur" (La cible)
                // La méthode Attaque() du parent va vérifier toute seule la distance et la direction.
                this.Attaque(joueur);

                // On recharge le timer, qu'on ait touché ou pas (pour simuler le temps de l'animation)
                cooldownActuel = COOLDOWN_ATTAQUE;
            }
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
            base.RecevoirDegats(degat);    // Appel de la méthode RecevoirDegats dans la classe mère

            if (pv <= 0)
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
