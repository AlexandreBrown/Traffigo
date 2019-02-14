using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for AjouterRouteDocUserControl.xaml
    /// </summary>
    public partial class AjouterRouteDocUserControl : UserControl
    {
        public AjouterRouteDocUserControl()
        {
            InitializeComponent();
            DataContext = new AjouterRouteDocVueModele();
        }
    }
}
