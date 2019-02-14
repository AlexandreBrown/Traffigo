using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ChargerSimulationDocUserControl.xaml
    /// </summary>
    public partial class ChargerSimulationDocUserControl : UserControl
    {
        public ChargerSimulationDocUserControl()
        {
            InitializeComponent();
            DataContext = new ChargerSimulationDocVueModele();
        }
    }
}
