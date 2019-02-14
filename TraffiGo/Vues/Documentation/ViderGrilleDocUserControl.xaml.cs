using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ViderGrilleDocUserControl.xaml
    /// </summary>
    public partial class ViderGrilleDocUserControl : UserControl
    {
        public ViderGrilleDocUserControl()
        {
            InitializeComponent();
            DataContext = new ViderGrilleDocVueModele();
        }
    }
}
