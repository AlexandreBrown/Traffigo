using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for EditionIntersectionDocUserControl.xaml
    /// </summary>
    public partial class EditionIntersectionDocUserControl : UserControl
    {
        public EditionIntersectionDocUserControl()
        {
            InitializeComponent();
            DataContext = new EditionIntersectionDocVueModele();
        }
    }
}
