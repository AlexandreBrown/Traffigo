using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace TraffiGo.VueModeles.Dialogs
{
    public class MessageSuccesVueModele : VueModeleBase
    {
        public string TitreDialog { get; set; }

        public string MessageSucces { get; set; }

        public string OkButtonText { get; set; }

        public int ZIndex { get; set; }

        public ICommand CmdFermerDialog
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DialogManager.Close(ZIndex);
                });
            }
        }
    }
}
