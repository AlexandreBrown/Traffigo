using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for RenommerDocUserControl.xaml
    /// </summary>
    public partial class RenommerSimulationDocUserControl : UserControl
    {
        public RenommerSimulationDocUserControl()
        {
            InitializeComponent();
            DataContext = new RenommerSimulationDocVueModele();
        }
    }
}
