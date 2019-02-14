using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TraffiGo.Vues.DialogMessage;
using TraffiGo.Modeles.Messages.ChangementVue;
using TraffiGo.Modeles.Data;

namespace TraffiGo.VueModeles
{
    public class VueModeleBase : INotifyPropertyChanged
    {
        protected static Utilisateur Utilisateur { get; set; }
        public DialogListener DialogManager { get; set; } = new DialogListener();
        protected const string PackUrl = "pack://application:,,,";
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public virtual ICommand CmdQuitterProgramme
        {
            get
            {
                return new RelayCommand<object>((o) => ((Window)o).Close(), (o) => true);
            }
        }

        public virtual ICommand CmdRetourAccueil
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new AccueilVueModele()});
                });
            }
        }

        public virtual ICommand CmdRetourMenuPrincipal
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new MenuPrincipalVueModele()});
                });
            }
        }

        public void SetCursorToWait()
        {
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void SetCursorToHand()
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        public void SetCursorToNull()
        {
            Mouse.OverrideCursor = null;
        }
    }
}
