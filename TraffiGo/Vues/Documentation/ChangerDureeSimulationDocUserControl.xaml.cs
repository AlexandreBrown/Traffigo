using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ChangerDureeSimulationDocUserControl.xaml
    /// </summary>
    public partial class ChangerDureeSimulationDocUserControl : UserControl
    {
        public ChangerDureeSimulationDocUserControl()
        {
            InitializeComponent();
            DataContext = new ChangerDureeSimulationDocVueModele();
        }
    }
}
