using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace TraffiGo.Modeles
{
    public class DirectionVoieGraphique :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Height { get; set; }
        public int Width { get; set; }
        public Thickness Margin { get; set; }
        public string Source { get; set; }
        public List<Direction> LstDirections { get; set; }

        public DirectionVoieGraphique(int height,int width,Thickness margin,string source,List<Direction> lstDirections) : this(source,lstDirections)
        {
            Height = height;
            Width = width;
            Margin = margin;
        }

        public DirectionVoieGraphique(string source, List<Direction> lstDirections)
        {
            LstDirections = lstDirections;
            Source = source;
        }

        public static List<Direction> GetListDirectionsFromSource(string source)
        {
            string baseUrl = "pack://application:,,,/Resources/Images/EditionIntersection/";
            string extension = ".png";
            string data = source.Substring(baseUrl.Length,(source.Length) - (baseUrl.Length + extension.Length));
            List<Direction> lst = new List<Direction>();

            // GAUCHE + TOUTDROIT + DROITE
            if (data.Contains("GAUCHE_TOUTDROIT_DROITE"))
            {
                lst.Add(Direction.GAUCHE);
                lst.Add(Direction.TOUTDROIT);
                lst.Add(Direction.DROITE);
                return lst;
            }
            // GAUCHE + TOUTDROIT
            else if (data.Contains("GAUCHE_TOUTDROIT"))
            {
                lst.Add(Direction.GAUCHE);
                lst.Add(Direction.TOUTDROIT);
                return lst;
            }
            // GAUCHE + DROITE
            else if (data.Contains("GAUCHE_DROITE"))
            {
                lst.Add(Direction.GAUCHE);
                lst.Add(Direction.DROITE);
                return lst;
            }
            // TOUTDROIT + DROITE
            else if (data.Contains("TOUTDROIT_DROITE"))
            {
                lst.Add(Direction.TOUTDROIT);
                lst.Add(Direction.DROITE);
                return lst;
            }
            // GAUCHE
            else if (data.Contains("GAUCHE"))
            {
                lst.Add(Direction.GAUCHE);
                return lst;
            }
            // TOUTDROIT
            else if (data.Contains("TOUTDROIT"))
            {
                lst.Add(Direction.TOUTDROIT);
                return lst;
            }
            // DROITE
            else
            {
                lst.Add(Direction.DROITE);
                return lst;
            }

            throw new Exception("Liste de directions introuvable à partir du lien");
        }

        public static string GetSourceFromLstDirections(List<Direction> lstDirections)
        {
            const string charSeparation = "_";
            const string extension = ".png";
            const string debutPath = "pack://application:,,,/Resources/Images/EditionIntersection/";

            // GAUCHE + TOUTDROIT + DROITE
            if (Voie.ContientGaucheToutdroitDroite(lstDirections))
            {
                return debutPath + Direction.GAUCHE.ToString() + charSeparation + Direction.TOUTDROIT.ToString() + charSeparation + Direction.DROITE.ToString() + extension;
            }
            // GAUCHE + TOUTDROIT
            else if (Voie.ContientGaucheToutdroit(lstDirections))
            {
                return debutPath + Direction.GAUCHE.ToString() + charSeparation + Direction.TOUTDROIT.ToString() + extension;
            }
            // GAUCHE + DROITE
            else if (Voie.ContientGaucheDroite(lstDirections))
            {
                return debutPath + Direction.GAUCHE.ToString() + charSeparation + Direction.DROITE.ToString() + extension;
            }
            // TOUTDROIT + DROITE
            else if (Voie.ContientToutdroitDroite(lstDirections))
            {
                return debutPath + Direction.TOUTDROIT.ToString() + charSeparation + Direction.DROITE.ToString() + extension;
            }
            // GAUCHE
            else if (Voie.ContientGauche(lstDirections))
            {
                return debutPath + Direction.GAUCHE.ToString() + extension;
            }
            // TOUTDROIT
            else if (Voie.ContientToutdroit(lstDirections))
            {
                return debutPath + Direction.TOUTDROIT.ToString() + extension;
            }
            // DROITE
            else
            {
                return debutPath + Direction.DROITE.ToString() + extension;
            }
        }

        private static bool ListeListeDirectionContientListeDirections(List<List<Direction>> lstDirectionsA,List<Direction> lstDirectionsB)
        {
            
            foreach (List<Direction> list in lstDirectionsA)
            {
                int compteurDirectionsTrouves = 0;
                if (list.Count == lstDirectionsB.Count)
                {
                    foreach (Direction directionA in list)
                    {
                        foreach (Direction directionB in lstDirectionsB)
                        {
                            if(directionA == directionB)
                            {
                                compteurDirectionsTrouves++;
                            }
                            if(compteurDirectionsTrouves == lstDirectionsB.Count)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static List<string> GetSourcesFromTousDirectionsPossibles(List<Direction> lstDirectionsPossibles)
        {
            const string charSeparation = "_";
            const string extension = ".png";
            const string debutPath = "pack://application:,,,/Resources/Images/EditionIntersection/";
            List<string> lstSources = new List<string>();
            List<List<Direction>> lstDirectionsAjoutees = new List<List<Direction>>();

            // GAUCHE + TOUTDROIT + DROITE
            if (Voie.ContientGaucheToutdroitDroite(lstDirectionsPossibles))
            {
                List<Direction> lst = new List<Direction>();
                lst.Add(Direction.GAUCHE);
                lst.Add(Direction.TOUTDROIT);
                lst.Add(Direction.DROITE);
                if (ListeListeDirectionContientListeDirections(lstDirectionsAjoutees, lst) == false)
                {
                    lstDirectionsAjoutees.Add(lst);
                    lstSources.Add(debutPath + Direction.GAUCHE.ToString() + charSeparation + Direction.TOUTDROIT.ToString() + charSeparation + Direction.DROITE.ToString() + extension);
                }
            }
            // GAUCHE + TOUTDROIT
            if (Voie.ContientGaucheToutdroit(lstDirectionsPossibles))
            {
                List<Direction> lst = new List<Direction>();
                lst.Add(Direction.GAUCHE);
                lst.Add(Direction.TOUTDROIT);
                if (ListeListeDirectionContientListeDirections(lstDirectionsAjoutees, lst) == false)
                {
                    lstDirectionsAjoutees.Add(lst);
                    lstSources.Add(debutPath + Direction.GAUCHE.ToString() + charSeparation + Direction.TOUTDROIT.ToString() + extension);
                }
            }
            // GAUCHE + DROITE
            if (Voie.ContientGaucheDroite(lstDirectionsPossibles))
            {
                List<Direction> lst = new List<Direction>();
                lst.Add(Direction.GAUCHE);
                lst.Add(Direction.DROITE);
                if (ListeListeDirectionContientListeDirections(lstDirectionsAjoutees, lst) == false)
                {
                    lstDirectionsAjoutees.Add(lst);
                    lstSources.Add(debutPath + Direction.GAUCHE.ToString() + charSeparation + Direction.DROITE.ToString() + extension);
                }
            }
            // TOUTDROIT + DROITE
            if (Voie.ContientToutdroitDroite(lstDirectionsPossibles))
            {
                List<Direction> lst = new List<Direction>();
                lst.Add(Direction.TOUTDROIT);
                lst.Add(Direction.DROITE);
                if (ListeListeDirectionContientListeDirections(lstDirectionsAjoutees, lst) == false)
                {
                    lstDirectionsAjoutees.Add(lst);
                    lstSources.Add(debutPath + Direction.TOUTDROIT.ToString() + charSeparation + Direction.DROITE.ToString() + extension);
                }
            }
            // GAUCHE
            if (Voie.ContientGauche(lstDirectionsPossibles))
            {
                List<Direction> lst = new List<Direction>();
                lst.Add(Direction.GAUCHE);
                if (ListeListeDirectionContientListeDirections(lstDirectionsAjoutees, lst) == false)
                {
                    lstDirectionsAjoutees.Add(lst);
                    lstSources.Add(debutPath + Direction.GAUCHE.ToString() + extension);
                }
            }
            // TOUTDROIT
            if (Voie.ContientToutdroit(lstDirectionsPossibles))
            {
                List<Direction> lst = new List<Direction>();
                lst.Add(Direction.TOUTDROIT);
                if (ListeListeDirectionContientListeDirections(lstDirectionsAjoutees, lst) == false)
                {
                    lstDirectionsAjoutees.Add(lst);
                    lstSources.Add(debutPath + Direction.TOUTDROIT.ToString() + extension);
                }
            }
            // DROITE
            if (Voie.ContientDroite(lstDirectionsPossibles))
            {
                List<Direction> lst = new List<Direction>();
                lst.Add(Direction.DROITE);
                if (ListeListeDirectionContientListeDirections(lstDirectionsAjoutees, lst) == false)
                {
                    lstDirectionsAjoutees.Add(lst);
                    lstSources.Add(debutPath + Direction.DROITE.ToString() + extension);
                }
            }
            return lstSources;
        }
    }
}
