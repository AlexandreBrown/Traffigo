using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace TraffiGo.Modeles
{
    public class VoieGraphique : Voie , INotifyPropertyChanged
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Thickness Margin { get; set; }
        public Brush Stroke { get; set; }
        public Brush Fill { get; set; }
        public int StrokeThickness { get; set; }

        // Valeures par défauts
        public static Brush DefaultStroke { get; private set; } = Brushes.Black;
        public static Brush SelectedStroke { get; private set; } = Brushes.SkyBlue;
        public static Brush HoverStroke { get; private set; } = Brushes.Green;
        public static Brush DefaultFill { get; private set; } = (Brush)(new BrushConverter().ConvertFrom("#343a38"));
        public static Brush HoverFill { get; private set; } = (Brush)(new BrushConverter().ConvertFrom("#4c4c4c"));
        public static int DefaultStrokeThickness { get; private set; } = 2;

        public VoieGraphique(Voie v,int height,int width,Thickness margin)
        {
            ID = v.ID;
            LstDirections = v.LstDirections;
            Height = height;
            Width = width;
            Stroke = DefaultStroke;
            StrokeThickness = DefaultStrokeThickness;
            Fill = DefaultFill;
            Margin = margin;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
