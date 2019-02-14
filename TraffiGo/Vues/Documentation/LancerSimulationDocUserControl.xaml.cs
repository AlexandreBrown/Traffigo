using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for LancerSimulationDocUserControl.xaml
    /// </summary>
    public partial class LancerSimulationDocUserControl : UserControl
    {
        public LancerSimulationDocUserControl()
        {
            InitializeComponent();
            DataContext = new LancerSimulationDocVueModele();
        }
    }
}
