using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for SauvegarderSimulationDocUserControl.xaml
    /// </summary>
    public partial class SauvegarderSimulationDocUserControl : UserControl
    {
        public SauvegarderSimulationDocUserControl()
        {
            InitializeComponent();
            DataContext = new SauvegarderSimulationDocVueModele();
        }
    }
}
