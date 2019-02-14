using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TraffiGo.Modeles.Data
{
    public class Vehicule
    {
        public List<Object> Trajet { get; private set; }
        public TypeVehicule Type { get; private set; }
        public int Acceleration { get; private set; }
        public int Deceleration { get; private set; }
        public int LongueurX { get; private set; }
        public int LongueurY { get; private set; }
        public Point PositionDepart { get; private set; }

        /// <summary>
        /// Constructeur paramétré pour les chargements de BD
        /// </summary>
        /// <param name="type">string du type de véhicule</param>
        /// <param name="a">accélération</param>
        /// <param name="d">décélération</param>
        /// <param name="p">position de départ de la voiture</param>
        public Vehicule(string type, int a, int d, Point p)
        {
            Type = DefinirTypeVehicule(type);
            Acceleration = a;
            Deceleration = d;
            PositionDepart = p;
        }

        /// <summary>
        /// parsing pour le type de vehicule
        /// </summary>
        /// <param name="type">le nom  du type de véhicule</param>
        /// <returns>la valeur de l'enum pour le bon type</returns>
        private TypeVehicule DefinirTypeVehicule(string type)
        {
            switch (type)
            {
                case "Voiture":
                    return TypeVehicule.VOITURE;

                case "Camion":
                    return TypeVehicule.CAMION;

                case "Moto":
                    return TypeVehicule.MOTO;

                default:
                    return TypeVehicule.VOITURE;
            }
        }

        /// <summary>
        /// parsing pour écrire en BD le type de véhicule
        /// </summary>
        /// <returns>la string qui équivaut au type de véhicule</returns>
        public string ReturnTypeVehicule()
        {
            switch (Type)
            {
                case TypeVehicule.VOITURE:
                    return "Voiture";

                case TypeVehicule.CAMION:
                    return "Camion";

                case TypeVehicule.MOTO:
                    return "Moto";

                default:
                    return "Voiture";
            }
        }

        /// <summary>
        /// Valide si l'objet passé est du bon type et s'il n'est pas déjà dans le trajet avant de l'ajouter au trajet
        /// </summary>
        /// <param name="o">L'objet qui doit être testé </param>
        public void AjouterCaseTrajet(Object o)
        {
            if (o is Chemin || o is Intersection)
            {
                if(Trajet.Find((Object obj) => 
                {
                    if(obj is Chemin)
                    {
                        Chemin iteratorChemin = obj as Chemin;
                        if (!(o is Chemin))
                        {
                            return false;
                        }

                        Chemin chemin = o as Chemin;
                        return iteratorChemin.Position.X == chemin.Position.X && iteratorChemin.Position.Y == chemin.Position.Y;
                    }
                    else
                    {
                        Intersection iteratorIntersection = obj as Intersection;
                        if (!(o is Intersection))
                        {
                            return false;
                        }

                        Intersection intersection = o as Intersection;
                        return iteratorIntersection.Position.X == intersection.Position.X && iteratorIntersection.Position.Y == intersection.Position.Y;

                    }
                }) == null)
                {
                    Trajet.Add(o);
                }
                else
                {
                    throw new Exception("L'objet reçu est déjà dans le trajet. ");
                }
            }
            else
            {
                throw new Exception("L'objet reçu en paramètre n'est pas du bon type. Les types acceptés sont Chemin ou Intersection. ");
            }
        }
    }
}
