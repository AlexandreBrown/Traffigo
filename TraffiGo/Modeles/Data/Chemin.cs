using System;
using System.Collections.ObjectModel;
using System.Windows;
using TraffiGo.Modeles;

namespace TraffiGo.Modeles.Data
{
    [Serializable]
    public class Chemin : Element
    {
        public ObservableCollection<Route> LstRoutes { get; set; }
        public Orientation? Emplacement { get; set; }

        public Chemin(Point position) : base((int)position.X, (int)position.Y)
        {
            LstRoutes = new ObservableCollection<Route>();
            Emplacement = null;
        }

        public Chemin(Point position, Orientation emplacement) : this(position)
        {
            Emplacement = emplacement;
        }

        public Chemin(Point position,ObservableCollection<Route> lstRoute, Orientation? emplacement) :base((int)position.X,(int)position.Y)
        {
            LstRoutes = lstRoute;
            Emplacement = emplacement;
        }

        public bool EstVirage()
        {
            foreach(Route r in LstRoutes)
            {
                foreach(Voie v in r.LstVoies)
                {
                    foreach(Direction d in v.LstDirections)
                    {
                        if(d != Direction.TOUTDROIT)
                            return true;
                    }
                }
            }
            return false;
        }

        public int CalculerNbVoies()
        {
            int i = 0;
            foreach (Route r in this.LstRoutes)
            {
                foreach (Voie v in r.LstVoies)
                {
                    i++;
                }
            }
            return i;
        }

        public void AjouterRoute(Route r)
        {
            LstRoutes.Add(r);
        }

        public bool ContientDirection(Direction d)
        {
            foreach( Route r in LstRoutes)
                if (r.ContientDirection(d))
                    return true;

            return false;
        }


        public bool ContientOrientation(Orientation o)
        {
            foreach (Route r in LstRoutes)
                if (r.Orientation == o)
                    return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o">L'orientation à trouver</param>
        /// <param name="chercheEntree">vrai si on cherche une entrée, faux sinon</param>
        /// <returns>vrai si possible, faux sinon</returns>
        public bool ConnexionPossible(Orientation o, bool chercheEntree)
        {
            if (chercheEntree)
            {
                if (EstVirage())
                {
                    foreach(Route r in LstRoutes)
                        if (r.Orientation == o)
                            if (!r.ContientDirection(Direction.TOUTDROIT))
                                return true;
                }
                else
                {
                    foreach (Route r in LstRoutes)
                        if (r.Orientation == o)
                            return true;
                }
            }
            else
            {
                if (EstVirage())
                {
                    foreach (Route r in LstRoutes)
                        if (r.Orientation == o)
                            if (r.ContientDirection(Direction.TOUTDROIT))
                                return true;
                }
                else
                {
                    foreach (Route r in LstRoutes)
                        if (r.Orientation == o)
                            return true;
                }
            }
            return false;
        }



    }
}
