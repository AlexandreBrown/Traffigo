using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for CreerSimulationDocUserControl.xaml
    /// </summary>
    public partial class CreerSimulationDocUserControl : UserControl
    {
        public CreerSimulationDocUserControl()
        {
            InitializeComponent();
            DataContext = new CreerSimulationDocVueModele();
        }
    }
}
