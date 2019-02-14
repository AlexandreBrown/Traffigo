using System.Windows.Controls;
using TraffiGo.VueModeles.Dialogs;

namespace TraffiGo.Vues.Dialogs
{
    /// <summary>
    /// Interaction logic for MessageSuccesUserControl.xaml
    /// </summary>
    public partial class MessageSuccesUserControl : UserControl
    {
        public MessageSuccesUserControl()
        {
            InitializeComponent();
            DataContext = new MessageSuccesVueModele();
            FocusHelper.Focus(btnOk);
        }
    }
}
