using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TraffiGo.VueModeles.Dialogs
{
    public class ModifierMotPasseVueModele : VueModeleBase
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public string TitreDialog { get; set; }

        public string OkButtonText { get; set; }

        public string CancelButtonText { get; set; }

        public int ZIndex { get; set; }

        public void OnOldPasswordChanged(object sender, RoutedEventArgs e)
        {
            OldPassword = ((PasswordBox)sender).Password;
        }

        public void OnNewPasswordChanged(object sender, RoutedEventArgs e)
        {
            NewPassword = ((PasswordBox)sender).Password;
        }

        public void OnConfirmNewPasswordChanged(object sender, RoutedEventArgs e)
        {
            ConfirmNewPassword = ((PasswordBox)sender).Password;
        }
        
        private bool MotPassesValides()
        {
            if(NewPassword != ConfirmNewPassword)
            {
                throw new Exception("Les mots de passes ne correspondent pas!");
            }
            return true;
        }

        public ICommand CmdModifierMotPasse
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (Utilisateur.CheckPassword(OldPassword))
                        {
                            if (MotPassesValides())
                            {
                                Utilisateur.ModifierMotPasse(NewPassword);
                                DialogManager.Close(0);
                                return;
                            }
                            throw new Exception("Les mots de passes ne correspondent pas");
                        }
                        throw new Exception("Le mot de passe actuel est incorrect");
                    }catch(Exception e)
                    {
                        DialogManager.ShowErrorDialog("Erreur", e.Message, "OK");
                    }

                }, ()=> { return !string.IsNullOrWhiteSpace(OldPassword) && !string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(ConfirmNewPassword); });
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
