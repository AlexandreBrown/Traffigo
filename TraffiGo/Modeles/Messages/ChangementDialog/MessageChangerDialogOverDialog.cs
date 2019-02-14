using System.Windows.Controls;

namespace TraffiGo.Modeles.Messages.ChangementDialog
{
    public class MessageChangerDialogOverDialog
    {
        public UserControl Dialog { get; set; }

        public MessageChangerDialogOverDialog(UserControl newDialog)
        {
            Dialog = newDialog;
        }
    }
}
