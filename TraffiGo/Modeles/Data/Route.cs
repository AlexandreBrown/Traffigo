using System;
using System.Collections.Generic;
using TraffiGo.Modeles.Data;

namespace TraffiGo
{
    [Serializable]
    public class Route
    {
        public Orientation Orientation { get; set; }
        public Orientation OrientationOppose
        {
            get
            {
                Orientation o = Orientation;
                switch (o)
                {
                    case Orientation.NORD:
                        o = Orientation.SUD;
                        break;
                    case Orientation.EST:
                        o = Orientation.OUEST;
                        break;
                    case Orientation.SUD:
                        o = Orientation.NORD;
                        break;
                    case Orientation.OUEST:
                        o = Orientation.EST;
                        break;
                }
                return o;
            }
        }
        public List<Voie> LstVoies { get; set; }

        /// <summary>
        /// Constructeur par défault de la classe Route, l'orientation de base est NORD
        /// </summary>
        public Route()
        {
            Orientation = Orientation.NORD;
            LstVoies = new List<Voie>();
        }

        /// <summary>
        /// Constructeur paramétré à 2 paramètres. La liste est initialisée comme nouvelle liste.
        /// </summary>
        /// <param name="o">L'orientation</param>
        public Route(Orientation o)
        {
            Orientation = o;
            LstVoies = new List<Voie>();
        }

        /// <summary>
        /// Constructeur paramétré à 3 paramètres. Une certaine validation est faite pour que la 
        /// liste passée en paramètre soit validée.
        /// </summary>
        /// <param name="o">l'orientation</param>
        /// <param name="lst"> la liste de voies à ajouter.</param>
        public Route(Orientation o, List<Voie> lst) : this(o)
        {
            if (lst.Count > 4)
                LstVoies = new List<Voie>();
            else
                LstVoies = lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private Voie CreerVoie(Direction d)
        {
            return new Voie(d);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void AjouterVoie()
        {
            if (LstVoies.Count > 3)
                throw new Exception("La route ne peut pas contenir plus de 4 voies. ");

            Voie v = CreerVoie(Direction.TOUTDROIT);
            LstVoies.Add(v);

        }

        public void AjouterVoie(Voie v)
        {
            if (LstVoies.Count > 3)
                throw new Exception("La route ne peut pas contenir plus de 4 voies. ");

            LstVoies.Add(v);
        }

        public void AjouterVoie(Direction d)
        {
            if (LstVoies.Count > 3)
                throw new Exception("La liste de voies est pleine!");

            Voie v = CreerVoie(d);
            LstVoies.Add(v);

        }

        public bool ContientDirection(Direction d)
        {
            foreach(Voie v in LstVoies)
                if (v.ContientDirection(d))
                    return true;

            return false;
        }

        public Orientation NouvelleOrientation()
        {
            Orientation o = Orientation;
            if (ContientDirection(Direction.DROITE))
            {
                if (o == Orientation.OUEST)
                    return Orientation.NORD;

                return ++o;
            } else if (ContientDirection(Direction.GAUCHE))
            {
                if (o == Orientation.NORD)
                 return Orientation.OUEST;

                return --o;
            }
            return o;
        }

        public Orientation NouvelleOrientationOpposee()
        {
            Orientation o = NouvelleOrientation();
            switch (o)
            {
                case Orientation.NORD:
                    return Orientation.SUD;
                case Orientation.EST:
                    return Orientation.OUEST;
                case Orientation.SUD:
                    return Orientation.NORD;
                case Orientation.OUEST:
                    return Orientation.EST;
                default:
                    return o;
            }
        }

        /// <summary>
        /// Parcours toutes les routes et regarde si une route qui contient seulement la direction TOUTDROIT est de la même orientation que celle reçue.
        /// </summary>
        /// <param name="c">le chemin qui doit être vérifé avec celui </param>
        /// <returns></returns>
        public bool ConnexionPossible(Chemin c, Orientation o, bool estSortie)
        {
            if (c.EstVirage())
            {
                if(estSortie)
                {
                    foreach (Route r in c.LstRoutes)
                    {
                        if (r.ContientDirection(Direction.DROITE) || r.ContientDirection(Direction.GAUCHE))
                            continue;

                        if (r.Orientation == o)
                            return true;
                    }
                }
                else // on cherche une entrée, C-A-D une route
                {    
                    foreach (Route r in c.LstRoutes)
                    {
                        if (r.ContientDirection(Direction.TOUTDROIT))
                            continue;

                        if (r.Orientation == o)
                            return true;
                    }
                }

            }
            else
            {
                foreach (Route r in c.LstRoutes)
                {
                    if (r.Orientation == o)
                        return true;
                }
            }

            return false;
        }

        public bool ConnexionPossible(Intersection i, bool estProchain)
        {
            Orientation o;
            if (estProchain)
                o = NouvelleOrientationOpposee();
            else
                o = Orientation;

            foreach (Chemin c in i.LstChemins)
            {
                if (c.Emplacement == o)
                {
                    foreach(Route r in c.LstRoutes)
                    {
                        if (r.Orientation == o)
                            return true;
                    }
                }
            }
            return false;
        }
        
    }

}
