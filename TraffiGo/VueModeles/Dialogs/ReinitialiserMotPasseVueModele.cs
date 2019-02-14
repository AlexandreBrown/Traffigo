using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;
using TraffiGo.Modeles;
using TraffiGo.Modeles.ClasseSql;

namespace TraffiGo.VueModeles.Dialogs
{
    public class ReinitialiserMotPasseVueModele : VueModeleBase
    {

        public string TitreDialog { get; set; }

        public string CancelButtonText { get; set; }

        public string CourrielToReset { get; set; }

        public int ZIndex { get; set; }

        public ICommand CmdReinitialiserMotPasse
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (Validator.IsValidEmailAddress(CourrielToReset) && CourrielToReset != "traffigocanada@gmail.com")
                        {
                            if (MySqlUtilisateurs.CourrielExistant(CourrielToReset))
                            {
                                Courriel.SendEmail(CourrielToReset);
                            }
                            DialogManager.Close(0);
                            DialogManager.ShowSuccessDialog("Réinitialisation", "Si " + CourrielToReset + " correspond au courriel relié à votre compte, un courriel vous a été envoyé", "OK");
                        }
                        else
                        {
                            DialogManager.ShowErrorDialog("Erreur", '"'+CourrielToReset +'"'+ " ne correspond pas à un courriel valide!", "OK");
                        }
                    }catch(Exception)
                    {
                        DialogManager.ShowErrorDialog("Erreur", "Envoi du courriel impossible!Vérifiez votre connexion internet et réessayez à nouveau.", "OK");
                    }
                },() => { return string.IsNullOrWhiteSpace(CourrielToReset) == false; });
            }
        }

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
