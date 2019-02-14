using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TraffiGo.Modeles.Data
{
    [Serializable]
    public class Feu : INotifyPropertyChanged
    {
        public CouleurFeu Couleur { get; set; }
        public int TempsVert { get; set; }
        public int TempsJaune { get; set; }
        public int Cycle { get; set; }
        public Orientation OrientationControlee { get; private set; }
        public List<Direction> DirectionsControlees { get; set; }

        public Feu(CouleurFeu couleur, int tempsVert, int tempsJaune, int cycle, Orientation orientationControler, List<Direction> directionControler)
        {
            Couleur = couleur;
            TempsVert = tempsVert;
            TempsJaune = tempsJaune;
            Cycle = cycle;
            OrientationControlee = orientationControler;
            DirectionsControlees = directionControler;
        }

        public Feu(CouleurFeu couleur, int tempsVert, int tempsJaune, int cycle, Orientation orientationControler)
        {
            Couleur = couleur;
            TempsVert = tempsVert;
            TempsJaune = tempsJaune;
            Cycle = cycle;
            OrientationControlee = orientationControler;
            DirectionsControlees = new List<Direction>();
        }

        public Feu(CouleurFeu couleur, int tempsVert, int cycle, Orientation orientationControler)
        {
            Couleur = couleur;
            TempsVert = tempsVert;
            Cycle = cycle;
            OrientationControlee = orientationControler;
        }

        public Feu(Orientation o,int cycle)
        {
            OrientationControlee = o;
            Couleur = CouleurFeu.ROUGE;
            TempsVert = 15;
            TempsJaune = 3;
            Cycle = cycle;
            DirectionsControlees = new List<Direction>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AjouterDirection(Direction d)
        {
            if (ContientDirection(d) == false)
            {
                DirectionsControlees.Add(d);
            }
        }

        private bool ContientDirection(Direction d)
        {
            foreach(Direction dir in DirectionsControlees)
            {
                if (dir == d)
                    return true;
            }
            return false;
        }
    }
}
