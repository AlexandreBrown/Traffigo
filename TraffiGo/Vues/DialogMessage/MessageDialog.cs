using System;

namespace TraffiGo.Vues.DialogMessage
{
    public enum DialogType
    {

        Informative,
        Warning,
        Error,
        Success,
        YesNoCancel,
        SaveSimulation,
        ModifyPassword,
        ResetPassword,
        RenameSimulation
    }

    public class MessageDialog
    {
        public string Titre { get; set; }
        public string Message { get; set; }
        public string OkButtonText { get; set; }
        public string CancelButtonText { get; set; }
        public DialogType TypeDialog { get; set; }
        public object Obj { get; set; }
        /// <summary>
        /// bool is the result of the click (yes == true, no == false)
        /// </summary>
        public Action<bool> OnClickAction_bool { get; set; }

        public Action<string> OnClickAction_string { get; set; }

        public MessageDialog(DialogType type, string titre, string message)
        {
            Titre = titre;
            Message = message;
            TypeDialog = type;
        }

        public MessageDialog(DialogType type,object o)
        {
            Obj = o;
            TypeDialog = type;
        }

        public MessageDialog(DialogType type, object o, Action<string> onClickAction) : this(type,o)
        {
            OnClickAction_string = onClickAction;
        }

        public MessageDialog(DialogType type, string titre, string message,Action<bool> onClickAction):this(type,titre,message)
        {
            OnClickAction_bool = onClickAction;
        }

        public MessageDialog(DialogType type,string titre, string message, string okButtonText) : this(type,titre,message)
        {
            OkButtonText = okButtonText;
        }


        public MessageDialog(DialogType type,string titre, string message, string okButtonText, string cancelButtonText) : this(type,titre, message,okButtonText)
        {
            CancelButtonText = cancelButtonText;
        }

        public MessageDialog(DialogType type, string titre, string message, string okButtonText, string cancelButtonText,object o) : this(type, titre, message, okButtonText, cancelButtonText)
        {
            Obj = o;
        }

        public MessageDialog(DialogType type) : this(type,"", "", "", ""){}
    }
}
