using System.Windows.Controls;
using TraffiGo.VueModeles.Dialogs;

namespace TraffiGo.Vues.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour MessageYesNoCancelUserControl.xaml
    /// </summary>
    public partial class MessageYesNoCancelUserControl : UserControl
    {
        public MessageYesNoCancelUserControl()
        {
            InitializeComponent();
            DataContext = new MessageYesNoCancelVueModele();
            FocusHelper.Focus(btnCancel);
        }
    }
}
