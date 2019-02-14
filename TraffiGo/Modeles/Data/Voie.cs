using System;
using System.Collections.Generic;

namespace TraffiGo
{
    [Serializable]
    public class Voie
    {
        public List<Direction> LstDirections { get; set; }
        public int ID;

        public Voie()
        {
            LstDirections = new List<Direction>();
        }

        public Voie(List<Direction> lstD,int id)
        {
            LstDirections = lstD;
            ID = id;
        }

        public Voie(List<Direction> lstD)
        {
            LstDirections = lstD;
        }

        public Voie(Direction d) : this()
        {
            AjouterDirection(d);
        }

        public void AjouterDirection(Direction d)
        {
            if (LstDirections.Count > 2)
                throw new Exception("La voie ne peut pas contenir plus de 3 direction. ");

            LstDirections.Add(d);
        }

        public void ChangerDirections(List<Direction> d)
        {
            if (d.Count > 3)
                throw new Exception("La liste reçue est trop grande. ");

            LstDirections.Clear();
            LstDirections = d;
        }

        public bool ContientDirection(Direction d)
        {
            foreach(Direction i in LstDirections)
                if (i == d)
                    return true;

            return false;
        }

        public static bool ContientGaucheToutdroitDroite(List<Direction> lst)
        {
            return ContientGauche(lst) && ContientToutdroit(lst) && ContientDroite(lst);
        }

        public static bool ContientGaucheToutdroit(List<Direction> lst)
        {
            return ContientGauche(lst) && ContientToutdroit(lst);
        }

        public static bool ContientGaucheDroite(List<Direction> lst)
        {
            return ContientGauche(lst) && ContientDroite(lst);
        }

        public static bool ContientToutdroitDroite(List<Direction> lst)
        {
            return ContientToutdroit(lst) && ContientDroite(lst);
        }

        public static bool ContientGauche(List<Direction> lst)
        {
            return lst.Contains(Direction.GAUCHE);
        }

        public static bool ContientToutdroit(List<Direction> lst)
        {
            return lst.Contains(Direction.TOUTDROIT);
        }

        public static bool ContientDroite(List<Direction> lst)
        {
            return lst.Contains(Direction.DROITE);
        }

    }
}
