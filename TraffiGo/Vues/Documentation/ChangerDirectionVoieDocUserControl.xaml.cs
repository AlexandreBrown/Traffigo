using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ChangerDirectionVoieDocUserControl.xaml
    /// </summary>
    public partial class ChangerDirectionVoieDocUserControl : UserControl
    {
        public ChangerDirectionVoieDocUserControl()
        {
            InitializeComponent();
            DataContext = new ChangerDirectionVoieDocVueModele();
        }
    }
}
