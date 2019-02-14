using System.Windows.Controls;
using TraffiGo.VueModeles.Dialogs;

namespace TraffiGo.Vues.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour ModifierMotPasseUserControl.xaml
    /// </summary>
    public partial class ModifierMotPasseUserControl : UserControl
    {
        public ModifierMotPasseUserControl()
        {
            InitializeComponent();
            DataContext = new ModifierMotPasseVueModele();
            FocusHelper.Focus(btnCancel);
        }
    }
}
