using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TraffiGo.Modeles.Data
{

    public class Simulation : INotifyPropertyChanged
    {

        #region Attributs
        public const int nbCols = 9;
        public const int nbRows = 4;
        public string Nom { get; private set; }
        public ObservableCollection<ObservableCollection<Element>> LstElem { get; set; }
        public string Auteur { get; private set; }
        public DateTime DateCreation { get; private set; }
        public Trafic NiveauTrafic { get; set; } = Trafic.Faible;
        public event PropertyChangedEventHandler PropertyChanged; // Do Not Remove
        public TypeVehicule[] TypesVehicules { get; private set; }

        public int Duree { get; private set; }
        #endregion

        #region constructeurs
        /// <summary>
        /// constructeur qui crée LstCases
        /// </summary>
        private Simulation()
        {
            LstElem = new ObservableCollection<ObservableCollection<Element>>();
            for (int i = 0; i < nbCols; i++)
            {
                LstElem.Add(new ObservableCollection<Element>());
                for (int j = 0; j < nbRows; j++)
                {
                    Element elem = new Element(i,j);
                    LstElem[i].Add(elem);
                }
            }
            Duree = 0;
        }

        /// <summary>
        /// constructeur par recopie
        /// </summary>
        /// <param name="s">la simulation</param>
        public Simulation(Simulation s)
        {
            Nom = s.Nom;
            LstElem = new ObservableCollection<ObservableCollection<Element>>(s.LstElem);
            Auteur = s.Auteur;
            DateCreation = s.DateCreation;
        }

        /// <summary>
        /// constructeur paramétré
        /// </summary>
        /// <param name="auteur">l'auteur</param>
        /// <param name="date">la date de création</param>
        /// <param name="nom">le nom de la simulation</param>
        public Simulation(string auteur, DateTime date, string nom) : this()
        {
            this.Auteur = auteur;
            this.DateCreation = date;
            this.Nom = nom;
        }

        #endregion


        /// <summary>
        /// change le nom de la simulation
        /// </summary>
        /// <param name="nouveauNom">le nouveau nom de la simulation</param>
        public void RenommerSimulation(string nouveauNom)
        {
            if (nouveauNom.Length > 50)
            {
                throw new Exception("Le nom de la simulation entré est trop long. ");
            }

            Nom = nouveauNom;
        }

        /// <summary>
        /// retourne la case demandée
        /// </summary>
        /// <param name="p">la case à retourner</param>
        /// <returns></returns>
        public Element RetournerCase(Point p)
        {
            return LstElem[(int)p.X][(int)p.Y];
        }


        /// <summary>
        /// copie une simulation complète
        /// </summary>
        /// <returns>une simulation</returns>
        public Simulation Copier()
        {
            return new Simulation(this);
        }

        #region Suppression
        /// <summary>
        /// supprime ce qu'il y a dans la case ciblée
        /// </summary>
        /// <param name="p">la position de la case ciblée</param>
        public void SupprimerCase(Point? p)
        {
            Element elem = new Element((int)p.Value.X,(int)p.Value.Y);
            LstElem[(int)p?.X][(int)p?.Y] = elem;
        }

        /// <summary>
        /// vide complètement la grille
        /// </summary>
        public void ViderGrille()
        {
            for (int i = 0; i < LstElem.Count; i++)
            {
                for (int j = 0; j < LstElem[i].Count; j++)
                {
                    Element newElem = new Element(i,j);
                    LstElem[i][j] = newElem;
                }
            }
        }


        #endregion

        #region Intersections
        /// <summary>
        /// crée une nouvelle intersection
        /// </summary>
        /// <param name="position">la position de la nouvelle intersection</param>
        /// <returns>l'intersection créée</returns>
        private Intersection CreerIntersection(Point position)
        {
            if(!PositionValide(position))
                throw new Exception("Le chemin est à l'extérieur de la grille. ");

            Intersection nouvelleIntersection = new Intersection(position, false);
            return nouvelleIntersection;
        }

        /// <summary>
        /// ajoute une nouvelle intersection
        /// </summary>
        /// <param name="position">la position de la nouvelle intersection</param>
        public void AjouterIntersection(Point position)
        {

            if (EstOccupe(position))
                throw new Exception($"La case {position.X + 1}, {position.Y + 1} est déjà occupée. ");

            LstElem[(int)position.X][(int)position.Y] = CreerIntersection(position);
        }

        /// <summary>
        /// ajoute une intersection (pour la BD)
        /// </summary>
        /// <param name="i">l'intersection à ajouter</param>
        public void AjouterIntersection(Intersection i)
        {
            if (EstOccupe(i.Position))
                throw new Exception($"La case {i.Position.X + 1}, {i.Position.Y + 1} est déjà occupée. ");

            LstElem[(int)i.Position.X][(int)i.Position.Y] = i;
        }
        #endregion

        #region Chemins
        /// <summary>
        /// crée un nouveau chemin
        /// </summary>
        /// <param name="position">la position du chemin à ajouter</param>
        /// <returns>le chemin créé</returns>
        private Chemin CreerChemin(Point position)
        {
            if (!PositionValide(position))
                throw new Exception("Le chemin est à l'extérieur de la grille. ");

            Chemin nouveauChemin = new Chemin(position);
            return nouveauChemin;
        }

        /// <summary>
        /// ajoute un nouveau chemin à la position désirée
        /// </summary>
        /// <param name="position">la position où ajouter un chemin</param>
        public void AjouterChemin(Point position)
        {
            if (EstOccupe(position))
                throw new Exception($"La case {position.X + 1}, {position.Y + 1} est déjà occupée. ");

            LstElem[(int)position.X][(int)position.Y] = CreerChemin(position);
        }

        /// <summary>
        /// ajoute un chemin (pour la BD)
        /// </summary>
        /// <param name="c">le chemin à ajouter</param>
        public void AjouterChemin(Chemin c)
        {

            if (EstOccupe(c.Position))
            {
                throw new Exception($"La case {c.Position.X + 1}, {c.Position.Y + 1} est déjà occupée. ");
            }
            else
            {
                LstElem[(int)c.Position.X][(int)c.Position.Y] = c;
            }
        }
        #endregion

        #region modifications trafic
        /// <summary>
        /// modifie le niveau de trafic
        /// </summary>
        /// <param name="nivTrafic">le niveau de trafic choisis</param>
        public void ModifierTrafic(Trafic nivTrafic)
        {
            NiveauTrafic = nivTrafic;
        }

        /// <summary>
        /// change les types de véhicules choisis
        /// </summary>
        /// <param name="types">les nouveaus types choisis</param>
        public void ModifierTypeVehicules(TypeVehicule[] types)
        {
            if (types.Length > Enum.GetNames(typeof(TypeVehicule)).Length)
                throw new Exception("Chaque type de véhicule ne peut être choisi qu'une seule fois. ");

            TypesVehicules = types;
        }

        public void ChangerDuree(int i)
        {
            if (i < 0)
                throw new Exception("La durée ne peut pas être inférieure à 0(infini). ");

            if (i < 30)
                Duree = 0;
            else
                Duree = i;
        }

        #endregion

        #region Validation Drag & Drop
        /// <summary>
        /// vérifie si la position est occupée
        /// </summary>
        /// <param name="p">la position à vérifier</param>
        /// <returns>vrai si elle est occupée, faux sinon</returns>
        public bool EstOccupe(Point p)
        {
            if(!PositionValide(p))
                throw new Exception("Le point est à l'extérieur de la grille. ");


            return LstElem[(int)p.X][(int)p.Y] is Chemin || LstElem[(int)p.X][(int)p.Y] is Intersection;

        }

        #endregion


        public bool ContientCheminOuIntersection()
        {

            for (int i = 0; i < LstElem.Count; i++)
            {
                for (int j = 0; j < LstElem[i].Count; j++)
                {
                    Element currentElem = LstElem[i][j];
                    if (currentElem is Chemin || currentElem is Intersection)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //on a besions de plusieurs choses : 
        //1 : une fonction qui parcours le tableau à la recherche d'intersections/de chemins
        //2 : une fonction récursive qui va valider que tous les trajets à partir d'une case sont possibles
        //3 : cette fonction doit aussi "keep track" de ou on a commencé, pour être certain de ne pas retourner sur elle même.
        //Note : on va throw une erreur contenant la position de la case pour savoir ou est l'erreur. 

        public bool GrilleEstValide()
        {
            bool contientVirage = false;
            foreach (ObservableCollection<Element> lstO in LstElem)
            {
                foreach (Element e in lstO)
                {
                    bool temp = ThreadTrajetValide(e);
                    if (temp == true)
                    {
                        if (contientVirage == false)
                        {
                            contientVirage = true;
                        }
                    }
                }
            }

            if (contientVirage)
            {
                if (VirageValide())
                    return true;
                throw new Exception("La simulation contient une boucle qui ne mène à aucune entrée/sortie. Veuillez relier tous les éléments à une extrémité pour continuer. ");
            }
            return true;
        }

        public bool ThreadTrajetValide(Element elem)
        {
                if (elem is Chemin)
                {
                    Chemin chemin = elem as Chemin;
                    bool estVirage = chemin.EstVirage();

                    return TestPositionValide(chemin.Position);
                }
                else if (elem is Intersection)
                {
                    Intersection intersection = elem as Intersection;

                    return TestPositionValide(intersection.Position);
                }
            return false;
        }


        public bool TestPositionValide(Point p)
        {
            Point precedent = new Point();
            Point prochain = new Point();
            bool contientVirage = false;

            if (LstElem[(int)p.X][(int)p.Y] is Chemin)
            {
                Chemin c = LstElem[(int)p.X][(int)p.Y] as Chemin;

                contientVirage = c.EstVirage();

                foreach (Route r in c.LstRoutes)
                {
                    // une route tout droit dans un virage c'est simplement une sortie.
                    //on doit valider les entrées car à partir d'une entrée on peut trouver toutes les cases adjacentes

                    if (c.EstVirage())
                        if (r.ContientDirection(Direction.TOUTDROIT))
                            continue;

                    prochain = CalculerPointSuivant(c.Position, r.NouvelleOrientation());
                    precedent = CalculerPointPrecedent(c.Position, r.Orientation);

                    if (PositionValide(prochain))
                    {
                        Element o = LstElem[(int)prochain.X][(int)prochain.Y];

                        if(o is Chemin)
                        {
                            if (!((Chemin)o).ConnexionPossible(r.NouvelleOrientation(), true))
                                throw new Exception($"La case ({c.Position.X + 1}, {c.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");

                        }else if (o is Intersection)
                        {
                            if (!((Intersection)o).ConnexionPossible(r.NouvelleOrientation(), true))
                                throw new Exception($"La case ({c.Position.X + 1}, {c.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }
                        else
                        {
                            throw new Exception($"La case ({c.Position.X + 1}, {c.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }
                    }

                    if (PositionValide(precedent))
                    {
                        Element o = LstElem[(int)precedent.X][(int)precedent.Y];

                        if (o is Chemin)
                        {
                            if (!((Chemin)o).ConnexionPossible(r.Orientation, false))
                                throw new Exception($"La case ({c.Position.X + 1}, {c.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");

                        }
                        else if (o is Intersection)
                        {
                            if (!((Intersection)o).ConnexionPossible(r.Orientation, false))
                                throw new Exception($"La case ({c.Position.X + 1}, {c.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }
                        else
                        {
                            throw new Exception($"La case ({c.Position.X + 1}, {c.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }
                    }
                }
            }
            //intersection
            else
            {
                Intersection i = LstElem[(int)p.X][(int)p.Y] as Intersection;

                foreach(Chemin c in i.LstChemins)
                {
                    //on peut simplement regarder les sorties de chaque chemin
                    foreach(Route r in c.LstRoutes)
                    {
                        if (c.Emplacement != r.Orientation)
                        {                            
                            //si c'est pas égal, on va calculer le précédent avec son orientation
                            precedent = CalculerPointPrecedent(i.Position, r.Orientation);
                        }
                        else
                        {
                            //sinon, on va calculer le prochain avec son orientation
                            prochain = CalculerPointSuivant(i.Position, r.Orientation);
                        }
                    }

                    if (PositionValide(prochain))
                    {
                        Element o = LstElem[(int)prochain.X][(int)prochain.Y];

                        if(o is Chemin)
                        {
                            if(!((Chemin)o).ConnexionPossible(c.Emplacement.Value, true))
                                throw new Exception($"La case ({i.Position.X + 1}, {i.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }
                        else if(o is Intersection)
                        {
                            if(!((Intersection)o).ConnexionPossible(c.Emplacement.Value, true))
                                throw new Exception($"La case ({i.Position.X + 1}, {i.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }
                        else
                        {
                            throw new Exception($"La case ({i.Position.X + 1}, {i.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }

                    }

                    if (PositionValide(precedent))
                    {
                        Orientation oppose = OrientationOpposee(c.Emplacement.Value);

                        Element o = LstElem[(int)precedent.X][(int)precedent.Y];

                        if (o is Chemin)
                        {
                            if (!((Chemin)o).ConnexionPossible(oppose, false))
                                throw new Exception($"La case ({i.Position.X + 1}, {i.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");

                        }
                        else if (o is Intersection)
                        {
                            if (!((Intersection)o).ConnexionPossible(oppose, false))
                                throw new Exception($"La case ({i.Position.X + 1}, {i.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }
                        else
                        {
                            throw new Exception($"La case ({c.Position.X + 1}, {c.Position.Y + 1}) ne peut se rendre à une entrée ou une sortie, veuillez vérifier la grille. ");
                        }
                    }
                }
            }

            return contientVirage;
        }

        private bool VirageValide()
        {
            //trouver un virage
            foreach (ObservableCollection<Element> lstO in LstElem)
            {
                foreach (object o in lstO)
                {
                    if (o is Chemin)
                    {
                        Chemin c = o as Chemin;

                        if (!c.EstVirage())
                            continue;

                        foreach (Route r in c.LstRoutes)
                        {
                            if (TrajetValide(c.Position, r.Orientation, new List<Point>()))
                                break;
                            else
                                return false;
                        }
                    }
                }
            }
            return true;

        }

        public bool TrajetValide(Point aVerifier, Orientation aSuivre, List<Point> dejaPasse)
        {

            if (dejaPasse.Contains(aVerifier))
                return false;

            //on s'est rendu jusqu'à l'extérieur de la grille, le trajet est valide!
            if (!PositionValide(aVerifier))
                return true;

            Element o = LstElem[(int)aVerifier.X][(int)aVerifier.Y];

            dejaPasse.Add(aVerifier);

            if (o is Chemin)
            {
                Chemin c = o as Chemin;

                //on trouve simplement la route qu'on doit suivre
                foreach (Route r in c.LstRoutes)
                {
                    //quand on la trouve, on calcule la nouvelle orientation
                    if (r.Orientation == aSuivre)
                    { 
                        aSuivre = r.NouvelleOrientation();
                        break;
                    }
                }

                aVerifier = CalculerPointSuivant(c.Position, aSuivre);

                return TrajetValide(new Point(aVerifier.X, aVerifier.Y), aSuivre, new List<Point>(dejaPasse));


            }
            else
            {
                Intersection i = o as Intersection;

                Orientation oppose = OrientationOpposee(aSuivre);

                foreach (Chemin c in i.LstChemins)
                {
                    if (c.Emplacement != oppose)
                        continue;

                    //on a l'opposé, c'est-a-dire l'endroit par lequel on entre
                    foreach (Route r in c.LstRoutes)
                    {
                        //on a pas trouvé la bonne route d'entrée
                        if (r.Orientation != aSuivre)
                            continue;

                        //on a trouvé la bonne voie, c'est-a-dire la voie par laquelle on entre
                        foreach (Voie v in r.LstVoies)
                        {
                            bool valide = false;

                            //on calcule la prochaine orientation à suivre
                            foreach (Direction d in v.LstDirections)
                            {
                                Orientation temp = NouvelleOrientation(aSuivre, d);

                                aVerifier = CalculerPointSuivant(i.Position, temp);

                                bool trajetValide = TrajetValide(new Point(aVerifier.X, aVerifier.Y), temp, new List<Point>(dejaPasse));

                                if (trajetValide)
                                {
                                    return true;
                                }
                            }
                            return valide;
                        }

                    }
                }
                return false;
            }
        }

        public Point CalculerPointSuivant(Point p, Orientation o)
        {
            switch (o)
            {
                case Orientation.NORD:
                    p.Y--;
                    break;
                case Orientation.EST:
                    p.X++;
                    break;
                case Orientation.SUD:
                    p.Y++;
                    break;
                case Orientation.OUEST:
                    p.X--;
                    break;
                default:
                    break;
            }

            return p;
        }

        public Point CalculerPointPrecedent(Point p, Orientation o)
        {
            switch (o)
            {
                case Orientation.NORD:
                    p.Y++;
                    break;
                case Orientation.EST:
                    p.X--;
                    break;
                case Orientation.SUD:
                    p.Y--;
                    break;
                case Orientation.OUEST:
                    p.X++;
                    break;
                default:
                    break;
            }

            return p;
        }

        public void AugmenterNiveauTrafic()
        {
            int augmentation = 1; // Défini l'incrémentation (utile dans le cas ou les valeurs dans l'enum ne font pas des bons de 1)
            if ((int)NiveauTrafic < Enum.GetNames(typeof(Trafic)).Length - 1)
            {
                NiveauTrafic = (Trafic)((int)NiveauTrafic + augmentation);
            }
        }

        public void ReduireNiveauTrafic()
        {
            int reduction = 1;
            if ((int)NiveauTrafic > (int)Trafic.Faible)
            {
                NiveauTrafic = (Trafic)((int)NiveauTrafic - reduction);
            }
        }

        public bool PositionValide(Point p)
        {
            return !(p.X < 0 || p.Y < 0) && p.X < nbCols && p.Y < nbRows;
        }

        private Orientation NouvelleOrientation(Orientation o, Direction d)
        {
            if (d == Direction.DROITE)
            {
                if (o == Orientation.OUEST)
                    return Orientation.NORD;

                return ++o;
            }
            else if (d == Direction.GAUCHE)
            {
                if (o == Orientation.NORD)
                    return Orientation.OUEST;

                return --o;
            }
            return o;
        }

        public Orientation OrientationOpposee(Orientation o)
        {
            Orientation oppose = Orientation.NORD;

            switch (o)
            {
                case Orientation.NORD:
                    oppose = Orientation.SUD;
                    break;
                case Orientation.EST:
                    oppose = Orientation.OUEST;
                    break;
                case Orientation.SUD:
                    oppose = Orientation.NORD;
                    break;
                case Orientation.OUEST:
                    oppose = Orientation.EST;
                    break;
            }

            return oppose;
        }


        public void ChangerAuteur(string s)
        {
            Auteur = s;
        }

    }
}
