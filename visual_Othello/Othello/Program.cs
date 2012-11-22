using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

//Les cases valant 0 sont vides
//Les cases valant 1 sont blanches
//Les cases valant 2 sont noires
//Les cases valant 4 sont les bords du plateau


namespace Othello
{
    class Program
    {
        //grille qui représente le plateau de jeu 8 lignes, 8 colonnes
        static int[,] grille = new int[10, 10];

        //tableau à 2 dimension qui sert à découpler l'abcisse x et l'ordonnée y d'un nombre donné sous la forme xy
        static int[,] numberReference = new int[89, 2];

        //représente un tableau avec la succession des coups lors de la partie
        static int[] partyHistory = new int[60];

        static void Main(string[] args)
        {
           
            InitializeNummberReference();
            InitializeGrille();
            AfficheGrille(grille);
            Console.Write("\n");
            Console.Read();

            int startupSearchPosition = 11;
            int pariteJoueur = 1;

            for (int i = 0; i < 60; i++)
            {
               partyHistory[i] = NextPossibility(ref grille, startupSearchPosition, pariteJoueur);

                pariteJoueur = pariteJoueur == 2 ? 1 : 2;


               Console.WriteLine("\n");
                AfficheGrille(grille);
                Console.Write("Position du pion sur la grille : " + partyHistory[i]);
                
                 Thread.Sleep(1000);
            }
            
            Thread.Sleep(10000);
                
                Console.ReadLine();
             


        }

        static void AfficheGrille(int[,] grilleParam)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(grilleParam[i, j]);
                }
                Console.Write("\n");
            }
        }

        //Initialise le plateau 2 pions noirs, 2 pions blancs placés en croix
        static void InitializeGrille()
        {

            grille[3, 4] = 2;
            grille[4, 4] = 2;
            grille[5, 5] = 1;
            grille[4, 5] = 2;
            grille[5, 4] = 2;

            for (int i = 0; i < 10; i++)
            {
                grille[0, i] = 4;
                grille[9, i] = 4;
                grille[i, 0] = 4;
                grille[i, 9] = 4;
            }

        }

        //initialize le tableau sous la forme 
        //  55  5   5
        //  56  5   6
        //  57  5   7
        static void InitializeNummberReference()
        {
            for (int i = 88; i >= 11; i--)
            {
                string iString = i.ToString();
                numberReference[i, 0] = Convert.ToInt32(iString[0].ToString());
                numberReference[i, 1] = Convert.ToInt32(iString[1].ToString());
            }
        }

        static int NextPossibility(ref int[,] grilleParam, int playerCoulor)
        {
            return NextPossibility(ref grilleParam, 11, playerCoulor);
        }

        static int NextPossibility(ref int[,] grilleParam, int position, int playerCoulor)
        {
            int adverseColour = playerCoulor == 1 ? 2 : 1;
            int i = numberReference[position, 0];
            int j = numberReference[position, 1];


            //variables définies ici pour éviter les réallocations dans les boucles for
            //couple d'ordonnées
            int u;
            int v;
            //prochaine position trouvée
            int nextPositionFound = 0;

            //Le but ici est de ne commencer les calcus qu'à partir de la position donnée 

            #region parcoursLigneCourante
            //Boucle for sur la ligne courante de la position de départ
            for (int m = j; m < 8; m++)
            {
                //si la case n'est pas vide, on passe à la case suivante
                if (grilleParam[i, m] != 0)
                    continue;
                else
                {
                    #region axeHautGauche
                    if (grilleParam[i - 1, m - 1] == adverseColour)
                    {
                        //on prend la case située dans la diagonale supérieure gauche 
                        v = m - 1;
                        for (u = i - 2; u > 0; u--)
                        {
                            //on se décale en diagonale haut/gauche et on vérifie que l'on n'est pas sorti du plateau de jeu
                            v--;
                            //on doit vérifier qu'en remontant en diagonale on atteigne pas le bord de la grille
                            if (v < 1)
                                break;

                            //si on rencontre une case vide, on arrête l'axe de recherche
                            if (grilleParam[u, v] == 0)
                                break;

                            //si on rencontre un nouveau pion de sa couleur alors il faut changer la couleur de tout les pions intermédiaires
                            if (grilleParam[u, v] == playerCoulor)
                            {
                                //tant que l'on est pas revenu à la position du pion joué, on change la couleur des pions
                                while (u != i && v != m)
                                {
                                    u++;
                                    v++;
                                    grilleParam[u, v] = playerCoulor;
                                }

                                //on enregistre la position de pion qui a pu être joué
                                nextPositionFound = i * 10 + m;
                                break;
                            }
                        }
                    }
                    #endregion

                    #region axeHaut
                    if (grilleParam[i - 1, m] == adverseColour)
                    {
                        for (u = i - 2; u > 0; u--)
                        {
                            //si on rencontre une case vide, on arrête l'axe de recherche
                            if (grilleParam[u, m] == 0)
                                break;

                            if (grilleParam[u, m] == playerCoulor)
                            {
                                while (u != i)
                                {
                                    u++;
                                    grilleParam[u, m] = playerCoulor;
                                }

                                nextPositionFound = i * 10 + m;
                                break;
                            }
                        }
                    }
                    #endregion

                    #region axeHautDroit
                    if (grilleParam[i - 1, m + 1] == adverseColour)
                    {
                        v = m + 1;
                        for (u = i - 2; u > 0; u--)
                        {
                            v++;
                            if (v > 8)
                                break;

                            //si on rencontre une case vide, on arrête l'axe de recherche
                            if (grilleParam[u, v] == 0)
                                break;

                            if (grilleParam[u, v] == playerCoulor)
                            {
                                while (u != i && v != m)
                                {
                                    u++;
                                    v--;
                                    grilleParam[u, v] = playerCoulor;
                                }

                                nextPositionFound = i * 10 + m;
                                break;
                            }
                        }
                    }
                    #endregion


                    #region axeGauche
                    if (grilleParam[i, m - 1] == adverseColour)
                    {
                        for (v = m - 2; v > 0; v--)
                        {
                            if (grilleParam[i, v] == 0)
                                break;

                            if (grilleParam[i, v] == playerCoulor)
                            {
                                while (v != m)
                                {
                                    v++;
                                    grilleParam[i, v] = playerCoulor;
                                }

                                nextPositionFound = i * 10 + m;
                                break;
                            }
                        }
                    }
                    #endregion

                    #region axeDroit
                    if (grilleParam[i, m + 1] == adverseColour)
                    {
                        for (v = m + 2; v < 9; v++)
                        {
                            if (grilleParam[i, v] == 0)
                                break;

                            if (grilleParam[i, v] == playerCoulor)
                            {
                                while (v != m)
                                {
                                    v--;
                                    grilleParam[i, v] = playerCoulor;
                                }

                                nextPositionFound = i * 10 + m;
                                break;
                            }
                        }
                    }
                    #endregion


                    #region axeBasGauche
                    if (grilleParam[i + 1, m - 1] == adverseColour)
                    {
                        v = m - 1;
                        for (u = i + 2; u < 9; u++)
                        {
                            v--;
                            if (v < 1)
                                break;

                            //si on rencontre une case vide, on arrête l'axe de recherche
                            if (grilleParam[u, v] == 0)
                                break;

                            //si on rencontre un nouveau pion de sa couleur alors il faut changer la couleur de tout les pions intermédiaires
                            if (grilleParam[u, v] == playerCoulor)
                            {
                                //tant que l'on est pas revenu à la position du pion joué, on change la couleur des pions
                                while (u != i && v != m)
                                {
                                    u--;
                                    v++;
                                    grilleParam[u, v] = playerCoulor;
                                }

                                //on enregistre la position de pion qui a pu être joué
                                nextPositionFound = i * 10 + m;
                                break;
                            }
                        }
                    }
                    #endregion

                    #region axeBas
                    if (grilleParam[i + 1, m] == adverseColour)
                    {
                        for (u = i + 2; u < 9; u++)
                        {
                            //si on rencontre une case vide, on arrête l'axe de recherche
                            if (grilleParam[u, m] == 0)
                                break;

                            if (grilleParam[u, m] == playerCoulor)
                            {
                                while (u != i)
                                {
                                    u--;
                                    grilleParam[u, m] = playerCoulor;
                                }

                                nextPositionFound = i * 10 + m;
                                break;
                            }
                        }
                    }
                    #endregion

                    #region axeBasDroit
                    if (grilleParam[i + 1, m + 1] == adverseColour)
                    {
                        v = m + 1;
                        for (u = i + 2; u < 9; u++)
                        {
                            v++;
                            if (v > 8)
                                break;

                            //si on rencontre une case vide, on arrête l'axe de recherche
                            if (grilleParam[u, v] == 0)
                                break;

                            if (grilleParam[u, v] == playerCoulor)
                            {
                                while (u != i && v != m)
                                {
                                    u--;
                                    v--;
                                    grilleParam[u, v] = playerCoulor;
                                }

                                nextPositionFound = i * 10 + m;
                                break;
                            }
                        }
                    }
                    #endregion

                    if (nextPositionFound != 0)
                        return nextPositionFound;
                }

            }
            #endregion

            #region parcoursProchainesLignes
            //Suite de la boucle for sur les prochaines lignes de la grille jusqu'à la fin
            for (int n = i + 1; n < 8; n++)
                for (int m = 0; m < 8; m++)
                {
                    //si la case n'est pas vide, on passe à la case suivante
                    if (grilleParam[n, m] != 0)
                        continue;
                    else
                    {
                        #region axeHautGauche
                        if (grilleParam[n - 1, m - 1] == adverseColour)
                        {
                            //on prend la case située dans la diagonale supérieure gauche 
                            v = m - 1;
                            for (u = n - 2; u > 0; u--)
                            {
                                //on se décale en diagonale haut/gauche et on vérifie que l'on n'est pas sorti du plateau de jeu
                                v--;
                                //on doit vérifier qu'en remontant en diagonale on atteigne pas le bord de la grille
                                if (v < 1)
                                    break;

                                //si on rencontre une case vide, on arrête l'axe de recherche
                                if (grilleParam[u, v] == 0)
                                    break;

                                //si on rencontre un nouveau pion de sa couleur alors il faut changer la couleur de tout les pions intermédiaires
                                if (grilleParam[u, v] == playerCoulor)
                                {
                                    //tant que l'on est pas revenu à la position du pion joué, on change la couleur des pions
                                    while (u != n && v != m)
                                    {
                                        u++;
                                        v++;
                                        grilleParam[u, v] = playerCoulor;
                                    }

                                    //on enregistre la position de pion qui a pu être joué
                                    nextPositionFound = n * 10 + m;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region axeHaut
                        if (grilleParam[n - 1, m] == adverseColour)
                        {
                            for (u = n - 2; u > 0; u--)
                            {
                                //si on rencontre une case vide, on arrête l'axe de recherche
                                if (grilleParam[u, m] == 0)
                                    break;

                                if (grilleParam[u, m] == playerCoulor)
                                {
                                    while (u != n)
                                    {
                                        u++;
                                        grilleParam[u, m] = playerCoulor;
                                    }

                                    nextPositionFound = n * 10 + m;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region axeHautDroit
                        if (grilleParam[n - 1, m + 1] == adverseColour)
                        {
                            v = m + 1;
                            for (u = n - 2; u > 0; u--)
                            {
                                v++;
                                if (v > 8)
                                    break;

                                //si on rencontre une case vide, on arrête l'axe de recherche
                                if (grilleParam[u, v] == 0)
                                    break;

                                if (grilleParam[u, v] == playerCoulor)
                                {
                                    while (u != n && v != m)
                                    {
                                        u++;
                                        v--;
                                        grilleParam[u, v] = playerCoulor;
                                    }

                                    nextPositionFound = n * 10 + m;
                                    break;
                                }
                            }
                        }
                        #endregion


                        #region axeGauche
                        if (grilleParam[n, m - 1] == adverseColour)
                        {
                            for (v = m - 2; v > 0; v--)
                            {
                                if (grilleParam[n, v] == 0)
                                    break;

                                if (grilleParam[n, v] == playerCoulor)
                                {
                                    while (v != m)
                                    {
                                        v++;
                                        grilleParam[n, v] = playerCoulor;
                                    }

                                    nextPositionFound = n * 10 + m;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region axeDroit
                        if (grilleParam[n, m + 1] == adverseColour)
                        {
                            for (v = m + 2; v < 9; v++)
                            {
                                if (grilleParam[n, v] == 0)
                                    break;

                                if (grilleParam[n, v] == playerCoulor)
                                {
                                    while (v != m)
                                    {
                                        v--;
                                        grilleParam[n, v] = playerCoulor;
                                    }

                                    nextPositionFound = n * 10 + m;
                                    break;
                                }
                            }
                        }
                        #endregion


                        #region axeBasGauche
                        if (grilleParam[n + 1, m - 1] == adverseColour)
                        {
                            v = m - 1;
                            for (u = n + 2; u < 9; u++)
                            {
                                v--;
                                if (v < 1)
                                    break;

                                //si on rencontre une case vide, on arrête l'axe de recherche
                                if (grilleParam[u, v] == 0)
                                    break;

                                //si on rencontre un nouveau pion de sa couleur alors il faut changer la couleur de tout les pions intermédiaires
                                if (grilleParam[u, v] == playerCoulor)
                                {
                                    //tant que l'on est pas revenu à la position du pion joué, on change la couleur des pions
                                    while (u != n && v != m)
                                    {
                                        u--;
                                        v++;
                                        grilleParam[u, v] = playerCoulor;
                                    }

                                    //on enregistre la position de pion qui a pu être joué
                                    nextPositionFound = n * 10 + m;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region axeBas
                        if (grilleParam[n + 1, m] == adverseColour)
                        {
                            for (u = n + 2; u < 9; u++)
                            {
                                //si on rencontre une case vide, on arrête l'axe de recherche
                                if (grilleParam[u, m] == 0)
                                    break;

                                if (grilleParam[u, m] == playerCoulor)
                                {
                                    while (u != n)
                                    {
                                        u--;
                                        grilleParam[u, m] = playerCoulor;
                                    }

                                    nextPositionFound = n * 10 + m;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region axeBasDroit
                        if (grilleParam[n + 1, m + 1] == adverseColour)
                       {
                            v = m + 1;
                            for (u = n + 2; u < 9; u++)
                            {
                                v++;
                                if (v > 8)
                                    break;

                                //si on rencontre une case vide, on arrête l'axe de recherche
                                if (grilleParam[u, v] == 0)
                                    break;

                                if (grilleParam[u, v] == playerCoulor)
                                {
                                    while (u != n && v != m)
                                    {
                                        u--;
                                        v--;
                                        grilleParam[u, v] = playerCoulor;
                                    }

                                    nextPositionFound = n * 10 + m;
                                    break;
                                }
                            }
                        }
                        #endregion

                        if (nextPositionFound != 0)
                            return nextPositionFound;
                    }
                }
            #endregion

            //si on a pas trouvé de prochaine position on renvoi le code 0
            return nextPositionFound;

        }


    }
}
