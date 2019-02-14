using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for SupprimerElemSimulationDocUserControl.xaml
    /// </summary>
    public partial class SupprimerElemSimulationDocUserControl : UserControl
    {
        public SupprimerElemSimulationDocUserControl()
        {
            InitializeComponent();
            DataContext = new SupprimerElemSimulationDocVueModele();
        }
    }
}
