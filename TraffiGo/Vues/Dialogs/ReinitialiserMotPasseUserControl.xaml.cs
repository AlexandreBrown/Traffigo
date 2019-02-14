using System.Windows.Controls;
using TraffiGo.VueModeles.Dialogs;

namespace TraffiGo.Vues.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour ReinitialiserMotPasseUserControl.xaml
    /// </summary>
    public partial class ReinitialiserMotPasseUserControl : UserControl
    {
        public ReinitialiserMotPasseUserControl()
        {
            InitializeComponent();
            DataContext = new ReinitialiserMotPasseVueModele();
            FocusHelper.Focus(btnCancel);
        }
    }
}
