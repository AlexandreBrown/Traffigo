using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace TraffiGo.Modeles.Data
{
    [Serializable]
    public class Intersection : Element
    {
        public ObservableCollection<Chemin> LstChemins { get; set; }
        public ObservableCollection<Feu> LstFeux { get; set; }

        public Intersection(Point p, bool isEmpty = false) : base((int)p.X,(int)p.Y)
        {
            LstChemins = new ObservableCollection<Chemin>();
            LstFeux = new ObservableCollection<Feu>();

            if (isEmpty)
                return;

            Chemin c;
            Route r;
            Feu f;
            Voie v = new Voie();
            v.AjouterDirection(Direction.DROITE);
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.TOUTDROIT);

            //CHEMIN POSITION NORD 
            c = new Chemin(p);
            c.Emplacement = Orientation.NORD;

            r = new Route();
            r.Orientation = Orientation.SUD;

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            r = new Route();
            r.Orientation = Orientation.NORD;
            r.AjouterVoie(Direction.TOUTDROIT);

            c.AjouterRoute(r);
            LstChemins.Add(c);

            //CHEMIN POSITION EST
            c = new Chemin(p);
            c.Emplacement = Orientation.EST;

            r = new Route();
            r.Orientation = Orientation.OUEST;

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            r = new Route();
            r.Orientation = Orientation.EST;
            r.AjouterVoie(Direction.TOUTDROIT);

            c.AjouterRoute(r);
            LstChemins.Add(c);

            //CHEMIN POSITION SUD
            c = new Chemin(p);
            c.Emplacement = Orientation.SUD;

            r = new Route();
            r.Orientation = Orientation.NORD;

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            r = new Route();
            r.Orientation = Orientation.SUD;
            r.AjouterVoie(Direction.TOUTDROIT);

            c.AjouterRoute(r);
            LstChemins.Add(c);

            //CHEMIN POSITION OUEST
            c = new Chemin(p);
            c.Emplacement = Orientation.OUEST;

            r = new Route();
            r.Orientation = Orientation.EST;

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            r = new Route();
            r.Orientation = Orientation.OUEST;
            r.AjouterVoie(Direction.TOUTDROIT);

            c.AjouterRoute(r);
            LstChemins.Add(c);

            GenererFeux();

            Position = p;
        }

        public Intersection(Point p,ObservableCollection<Chemin> lstC, ObservableCollection<Feu> lstF) : base((int)p.X,(int)p.Y)
        {
            LstChemins = lstC;
            LstFeux = lstF;
        }

        private bool VoieEstSortie(Orientation orientationRoute,Orientation emplacementChemin)
        {
            return orientationRoute == emplacementChemin;
        }

        public void AjouterChemin(Orientation emplacement)
        {
            foreach(Chemin item in LstChemins)
            {
                if (item.Emplacement == emplacement)
                    throw new Exception("Cet emplacement est déjà occupé. ");
            }

            Chemin c = new Chemin(this.Position, emplacement);

            //ajout de la route qui sort
            Route r = new Route(emplacement);
            Voie v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(r.OrientationOppose);
            v = new Voie();
            v.AjouterDirection(Direction.DROITE);
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.TOUTDROIT);
            
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);
        }

        public void AjouterChemin(Chemin c)
        {
            foreach (Chemin item in LstChemins)
                if (item.Emplacement == c.Emplacement)
                    throw new Exception("Cet emplacement est déjà occupé. ");

            LstChemins.Add(c);
        }

        public void AjouterFeu(Feu f)
        {
            foreach(Feu item in LstFeux)
            {
                if (item.OrientationControlee == f.OrientationControlee)
                    throw new Exception("Cette orientation est déjà contrôlée par un autre feu. ");
            }

            LstFeux.Add(f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o">L'orientation à trouver</param>
        /// <param name="chercheEntree">vrai si entrée, faux sinon</param>
        /// <returns>vrai si possible, faux sinon</returns>
        public bool ConnexionPossible(Orientation o, bool chercheEntree)
        {
            if (chercheEntree)
            {
                Orientation oppose;
                switch (o)
                {
                    case Orientation.NORD:
                        oppose = Orientation.SUD;
                        break;
                    case Orientation.SUD:
                        oppose = Orientation.NORD;
                        break;
                    case Orientation.EST:
                        oppose = Orientation.OUEST;
                        break;
                    case Orientation.OUEST:
                        oppose = Orientation.EST;
                        break;
                    default:
                        oppose = Orientation.NORD;
                        break;
                }

                foreach(Chemin c in LstChemins)
                {
                    if(c.Emplacement != oppose)
                        continue;

                    foreach(Route r in c.LstRoutes)
                        if (r.Orientation == o)
                            return true;

                }
            }
            else
            {
                foreach(Chemin c in LstChemins)
                {
                    if (c.Emplacement != o)
                        continue;

                    foreach(Route r in c.LstRoutes)
                        if(r.Orientation == o)
                            return true;
                }
            }
            return false;
        }

        public void ChargerT2()
        {
            Chemin c;
            Route r;
            Voie v;
            
            //Chemin EST
            c = new Chemin(this.Position, Orientation.EST);

            //ajout de la route qui sort
            r = new Route(Orientation.EST);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.OUEST);
            v = new Voie();
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.TOUTDROIT);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            //Chemin SUD
            c = new Chemin(this.Position, Orientation.SUD);

            //ajout de la route qui sort
            r = new Route(Orientation.SUD);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.NORD);
            v = new Voie();
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.DROITE);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            //Chemin OUEST
            c = new Chemin(this.Position, Orientation.OUEST);

            //ajout de la route qui sort
            r = new Route(Orientation.OUEST);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.EST);
            v = new Voie();
            v.AjouterDirection(Direction.DROITE);
            v.AjouterDirection(Direction.TOUTDROIT);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            GenererFeux();
        }

        public void ChargerT3()
        {
            Chemin c;
            Route r;
            Voie v;

            //Chemin NORD
            c = new Chemin(this.Position, Orientation.NORD);

            //ajout de la route qui sort
            r = new Route(Orientation.NORD);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.SUD);
            v = new Voie();
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.DROITE);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            //Chemin EST
            c = new Chemin(this.Position, Orientation.EST);

            //ajout de la route qui sort
            r = new Route(Orientation.EST);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.OUEST);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            v.AjouterDirection(Direction.DROITE);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            //Chemin OUEST
            c = new Chemin(this.Position, Orientation.OUEST);

            //ajout de la route qui sort
            r = new Route(Orientation.OUEST);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.EST);
            v = new Voie();
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.TOUTDROIT);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            GenererFeux();
        }

        public void ChargerT4()
        {
            Chemin c;
            Route r;
            Voie v;

            //Chemin NORD
            c = new Chemin(this.Position, Orientation.NORD);

            //ajout de la route qui sort
            r = new Route(Orientation.NORD);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.SUD);
            v = new Voie();
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.TOUTDROIT);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            //Chemin EST
            c = new Chemin(this.Position, Orientation.EST);

            //ajout de la route qui sort
            r = new Route(Orientation.EST);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.OUEST);
            v = new Voie();
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.DROITE);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            //Chemin SUD
            c = new Chemin(this.Position, Orientation.SUD);

            //ajout de la route qui sort
            r = new Route(Orientation.SUD);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.NORD);
            v = new Voie();
            v.AjouterDirection(Direction.DROITE);
            v.AjouterDirection(Direction.TOUTDROIT);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            GenererFeux();
        }

        public void ChargerT5()
        {
            Chemin c;
            Route r;
            Voie v;

            //Chemin NORD
            c = new Chemin(this.Position, Orientation.NORD);

            //ajout de la route qui sort
            r = new Route(Orientation.NORD);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.SUD);
            v = new Voie();
            v.AjouterDirection(Direction.DROITE);
            v.AjouterDirection(Direction.TOUTDROIT);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            //Chemin SUD
            c = new Chemin(this.Position, Orientation.SUD);

            //ajout de la route qui sort
            r = new Route(Orientation.SUD);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.NORD);
            v = new Voie();
            v.AjouterDirection(Direction.GAUCHE);
            v.AjouterDirection(Direction.TOUTDROIT);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            //Chemin OUEST
            c = new Chemin(this.Position, Orientation.OUEST);

            //ajout de la route qui sort
            r = new Route(Orientation.OUEST);
            v = new Voie();
            v.AjouterDirection(Direction.TOUTDROIT);
            r.AjouterVoie(v);
            c.AjouterRoute(r);

            //ajout de la route qui entre
            r = new Route(Orientation.EST);
            v = new Voie();
            v.AjouterDirection(Direction.DROITE);
            v.AjouterDirection(Direction.GAUCHE);

            r.AjouterVoie(v);
            c.AjouterRoute(r);

            LstChemins.Add(c);

            GenererFeux();
        }

        private void GenererFeux()
        {
            LstFeux = new ObservableCollection<Feu>();

            int i = 0;
            foreach(Chemin c in LstChemins)
            {
                Feu f = new Feu(c.Emplacement.Value,i);
                i++;

                foreach(Route r in c.LstRoutes)
                {

                    if (r.Orientation == c.Emplacement.Value)
                        continue;

                    foreach (Voie v in r.LstVoies)
                    {
                        foreach (Direction d in v.LstDirections)
                        {
                            f.AjouterDirection(d);
                        }
                    }
                }
                LstFeux.Add(f);
            }
        }
    }
}
