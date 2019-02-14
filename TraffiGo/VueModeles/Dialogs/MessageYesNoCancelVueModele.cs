using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;

namespace TraffiGo.VueModeles.Dialogs
{
    public class MessageYesNoCancelVueModele : VueModeleBase
    {
        public bool Result { get; set; }
        public string TitreDialog { get; set; }
        public string Message { get; set; }
        public int ZIndex { get; set; }
        public Action<bool> OnClickAction { get; set; }

        public ICommand CmdYes
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Result = true;
                    OnClickAction?.Invoke(Result);
                    DialogManager.Close(ZIndex);
                });
            }
        }

        public ICommand CmdNo
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Result = false;
                    OnClickAction?.Invoke(Result);
                    DialogManager.Close(ZIndex);
                });
            }
        }

        public ICommand CmdCancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Result = false;
                    OnClickAction?.Invoke(Result);
                    DialogManager.Close(ZIndex);
                });
            }
        }
    }
}
