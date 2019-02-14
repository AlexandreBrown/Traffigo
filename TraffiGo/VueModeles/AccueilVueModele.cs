using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using TraffiGo.Modeles.Messages.ChangementVue;
using TraffiGo.Modeles.Data;

namespace TraffiGo.VueModeles
{
    public class AccueilVueModele : VueModeleBase
    {
        public ICommand CmdOuvrirVueCreationCompte
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new CreationCompteVueModele() });
                });
            }
        }

        public ICommand CmdOuvrirVueConnexion
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new ConnexionVueModele() });
                });
            }
        }

        public ICommand CmdOuvrirVueMotPasseOublie
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DialogManager.ShowReinitialiserMotPasseDialog();
                });
            }
        }

    }
}
