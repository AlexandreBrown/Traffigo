using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ChangerNiveauTraficDocUserControl.xaml
    /// </summary>
    public partial class ChangerNiveauTraficDocUserControl : UserControl
    {
        public ChangerNiveauTraficDocUserControl()
        {
            InitializeComponent();
            DataContext = new ChangerNiveauTraficDocVueModele();
        }
    }
}
