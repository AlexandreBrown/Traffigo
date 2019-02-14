using System;
using System.ComponentModel;
using System.Windows;

namespace TraffiGo.Modeles
{
    [Serializable]
    public class Element : INotifyPropertyChanged
    {
        protected const string DraggableItemsURL = "/Resources/Images/Simulation/DraggableItems/";
        protected const string PackUrl = "pack://application:,,,";

        public event PropertyChangedEventHandler PropertyChanged;

        public Point Position { get; set; }

        public Element(int x,int y)
        {
            Position = new Point(x, y);
        }
    }
}
