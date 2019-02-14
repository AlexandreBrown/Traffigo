using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for AjouterIntersectionDocUserControl.xaml
    /// </summary>
    public partial class AjouterIntersectionDocUserControl : UserControl
    {
        public AjouterIntersectionDocUserControl()
        {
            InitializeComponent();
            DataContext = new AjouterIntersectionDocVueModele();
        }
    }
}
