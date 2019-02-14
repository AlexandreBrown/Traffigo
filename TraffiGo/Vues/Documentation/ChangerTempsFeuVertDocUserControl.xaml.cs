using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ChangerTempsFeuVertDocUserControl.xaml
    /// </summary>
    public partial class ChangerTempsFeuVertDocUserControl : UserControl
    {
        public ChangerTempsFeuVertDocUserControl()
        {
            InitializeComponent();
            DataContext = new ChangerTempsFeuVertDocVueModele();
        }
    }
}
