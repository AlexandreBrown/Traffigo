using System.Windows.Controls;

namespace TraffiGo.Modeles.Messages.ChangementDialog
{
    public class MessageChangerDialog
    {
        public UserControl Dialog { get; set; }

        public MessageChangerDialog(UserControl dialog)
        {
            Dialog = dialog;
        }
    }
}
