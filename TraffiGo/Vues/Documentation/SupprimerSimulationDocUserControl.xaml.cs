using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for SupprimerSimulationDocUserControl.xaml
    /// </summary>
    public partial class SupprimerSimulationDocUserControl : UserControl
    {
        public SupprimerSimulationDocUserControl()
        {
            InitializeComponent();
            DataContext = new SupprimerSimulationDocVueModele();
        }
    }
}
