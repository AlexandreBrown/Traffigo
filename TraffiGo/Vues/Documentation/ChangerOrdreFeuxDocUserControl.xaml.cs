using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ChangerOrdreFeuxDocUserControl.xaml
    /// </summary>
    public partial class ChangerOrdreFeuxDocUserControl : UserControl
    {
        public ChangerOrdreFeuxDocUserControl()
        {
            InitializeComponent();
            DataContext = new ChangerOrdreFeuxDocVueModele();
        }
    }
}
