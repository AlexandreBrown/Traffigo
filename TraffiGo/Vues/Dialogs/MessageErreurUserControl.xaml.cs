using System.Windows.Controls;
using System.Windows.Input;
using TraffiGo.VueModeles.Dialogs;

namespace TraffiGo.Vues.Dialogs
{
    /// <summary>
    /// Interaction logic for MessageErreurUserControl.xaml
    /// </summary>
    public partial class MessageErreurUserControl : UserControl
    {
        public MessageErreurUserControl()
        {
            InitializeComponent();
            DataContext = new MessageErreurVueModele();
        }
    }
}
