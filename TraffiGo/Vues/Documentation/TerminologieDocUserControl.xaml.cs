using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for TerminologieDocUserControl.xaml
    /// </summary>
    public partial class TerminologieDocUserControl : UserControl
    {
        public TerminologieDocUserControl()
        {
            InitializeComponent();
            DataContext = new TerminologieDocVueModele();
        }
    }
}
