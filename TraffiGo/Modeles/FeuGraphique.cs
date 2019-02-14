using System.Windows;
using System.Windows.Media;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles
{
    public class FeuGraphique : Feu
    {
        public int Height { get; set; }
        public Brush Contour { get; set; }
        public static Brush DefaultContour { get; private set; } = (SolidColorBrush)(new BrushConverter().ConvertFrom("#343a38"));
        public static Brush SelectedContour { get; private set; } = Brushes.SkyBlue;
        public static Brush HoverContour { get; private set; } = Brushes.Green;
        public Thickness Margin { get; set; }
        public string Description { get; set; }

        public FeuGraphique(Feu f,int height,Thickness margin) : this(f,height)
        {
            Margin = margin;
        }

        public FeuGraphique(Feu f, int height, string content) : this(f, height)
        {
            Description = content;
        }

        public FeuGraphique(Feu f, int height) : base(f.Couleur, f.TempsVert, f.TempsJaune, f.Cycle, f.OrientationControlee, f.DirectionsControlees)
        {
            Height = height;
            Contour = DefaultContour;
        }
    }
}
