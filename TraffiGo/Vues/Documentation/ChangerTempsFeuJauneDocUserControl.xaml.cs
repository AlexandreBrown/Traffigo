using System.Windows.Controls;
using TraffiGo.VueModeles.Documentation;

namespace TraffiGo.Vues.Documentation
{
    /// <summary>
    /// Interaction logic for ChangerTempsFeuJauneDocUserControl.xaml
    /// </summary>
    public partial class ChangerTempsFeuJauneDocUserControl : UserControl
    {
        public ChangerTempsFeuJauneDocUserControl()
        {
            InitializeComponent();
            DataContext = new ChangerTempsFeuJauneDocVueModele();
        }
    }
}
