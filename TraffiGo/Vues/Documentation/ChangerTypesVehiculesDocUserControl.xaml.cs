using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ChangerTypesVehiculesDocUserControl.xaml
    /// </summary>
    public partial class ChangerTypesVehiculesDocUserControl : UserControl
    {
        public ChangerTypesVehiculesDocUserControl()
        {
            InitializeComponent();
            DataContext = new ChangerTypesVehiculesDocVueModele();
        }
    }
}
