using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Controls;
using TraffiGo.Modeles.Data;
using TraffiGo.Modeles.Messages.ChangementDialog;
using TraffiGo.VueModeles.Dialogs;
using TraffiGo.Vues.Dialogs;
namespace TraffiGo.Vues.DialogMessage
{
    public class DialogListener
    {
        private UserControl Dialog { get; set; }
        private UserControl newDialogToShow = new UserControl();
        public DialogListener()
        {
            Messenger.Default.Register<MessageDialog>(this, (dialogContent) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    newDialogToShow = new UserControl();
                    switch ((int)dialogContent.TypeDialog)
                    {
                        case (int)DialogType.Error:
                            newDialogToShow = new MessageErreurUserControl();
                            var modeleMsgErr = newDialogToShow.DataContext as MessageErreurVueModele;
                                if (modeleMsgErr != null)
                                {
                                    modeleMsgErr.TitreDialog = dialogContent.Titre;
                                    modeleMsgErr.MessageErreur = dialogContent.Message;
                                    modeleMsgErr.OkButtonText = dialogContent.OkButtonText;
                                    modeleMsgErr.ZIndex = 1;
                                }
                            break;
                        case (int)DialogType.Success:
                            newDialogToShow = new MessageSuccesUserControl();
                            var modeleMsgSucc = newDialogToShow.DataContext as MessageSuccesVueModele;
                            if (modeleMsgSucc != null)
                            {
                                modeleMsgSucc.TitreDialog = dialogContent.Titre;
                                modeleMsgSucc.MessageSucces = dialogContent.Message;
                                modeleMsgSucc.OkButtonText = dialogContent.OkButtonText;
                                modeleMsgSucc.ZIndex = 1;
                            }
                            break;
                        case (int)DialogType.YesNoCancel:
                            newDialogToShow = new MessageYesNoCancelUserControl();
                            var modeleYesNoCancel = newDialogToShow.DataContext as MessageYesNoCancelVueModele;
                            if (modeleYesNoCancel != null)
                            {
                                modeleYesNoCancel.TitreDialog = dialogContent.Titre;
                                modeleYesNoCancel.Message = dialogContent.Message;
                                modeleYesNoCancel.OnClickAction = dialogContent.OnClickAction_bool;
                                modeleYesNoCancel.ZIndex = 1;
                            }
                            break;
                        case (int)DialogType.SaveSimulation:
                            newDialogToShow = new SauvegarderSimulationUserControl();
                            var modeleSaveSim = newDialogToShow.DataContext as SauvegarderSimulationVueModele;
                            if (modeleSaveSim != null)
                            {
                                modeleSaveSim.TitreDialog = dialogContent.Titre;
                                modeleSaveSim.OkButtonText = dialogContent.OkButtonText;
                                modeleSaveSim.CancelButtonText = dialogContent.CancelButtonText;
                                modeleSaveSim.Sim = (Simulation)dialogContent.Obj;
                                modeleSaveSim.ZIndex = 0;
                            }
                            break;
                        case (int)DialogType.ModifyPassword:
                            newDialogToShow = new ModifierMotPasseUserControl();
                            var modeleModifPassword = newDialogToShow.DataContext as ModifierMotPasseVueModele;
                            if (modeleModifPassword != null)
                            {
                                modeleModifPassword.TitreDialog = dialogContent.Titre;
                                modeleModifPassword.OkButtonText = dialogContent.OkButtonText;
                                modeleModifPassword.CancelButtonText = dialogContent.CancelButtonText;
                                modeleModifPassword.ZIndex = 0;
                            }
                            break;
                        case (int)DialogType.ResetPassword:
                            newDialogToShow = new ReinitialiserMotPasseUserControl();
                            var modeleResetPassword = newDialogToShow.DataContext as ReinitialiserMotPasseVueModele;
                            if (modeleResetPassword != null)
                            {
                                modeleResetPassword.TitreDialog = dialogContent.Titre;
                                modeleResetPassword.CancelButtonText = dialogContent.OkButtonText;
                                modeleResetPassword.ZIndex = 0;
                            }
                            break;
                        case (int)DialogType.RenameSimulation:
                            newDialogToShow = new RenommerSimulationUserControl();
                            var modeleRenommerSim = newDialogToShow.DataContext as RenommerSimulationVueModele;
                            if (modeleRenommerSim != null)
                            {
                                modeleRenommerSim.AncienNomSimulation =(string)dialogContent.Obj;
                                modeleRenommerSim.onClickAction = dialogContent.OnClickAction_string;
                                modeleRenommerSim.ZIndex = 0;
                            }
                            break;
                    }
                        SetDialog(newDialogToShow);
                });
            });
        }

        private void SetDialog(UserControl newDialogToShow)
        {
            if (newDialogToShow != null)
            Dialog = newDialogToShow;
        }

        public void ShowErrorDialog(string titre,string message,string okButtonText)
        {
            Messenger.Default.Send(new MessageDialog(DialogType.Error,titre, message,okButtonText));
            Messenger.Default.Send(new MessageChangerDialogOverDialog(Dialog));
        }

        public void ShowYesNoCancelDialog(string titre,string message,Action<bool> onClickAction)
        {
            Messenger.Default.Send(new MessageDialog(DialogType.YesNoCancel, titre, message, onClickAction));
            Messenger.Default.Send(new MessageChangerDialogOverDialog(Dialog));
        }

        public void ShowSuccessDialog(string titre, string message, string okButtonText)
        {
            Messenger.Default.Send(new MessageDialog(DialogType.Success, titre, message, okButtonText));
            Messenger.Default.Send(new MessageChangerDialogOverDialog(Dialog));
        }

        public void ShowRenameDialog(string nomSimulationActuel,Action<string> onClickAction)
        {
            Messenger.Default.Send(new MessageDialog(DialogType.RenameSimulation, nomSimulationActuel, onClickAction));
            Messenger.Default.Send(new MessageChangerDialog(Dialog));
        }

        public void ShowSaveSimulationDialog(Simulation sim)
        {
            Messenger.Default.Send(new MessageDialog(DialogType.SaveSimulation, "Sauvegarder une simulation","", "OK","Annuler",sim));
            Messenger.Default.Send(new MessageChangerDialog(Dialog));
        }

        public void ShowModifierMotPasseDialog()
        {
            Messenger.Default.Send(new MessageDialog(DialogType.ModifyPassword,"Modification du mot de passe","","Appliquer", "Annuler"));
            Messenger.Default.Send(new MessageChangerDialog(Dialog));
        }

        public void ShowReinitialiserMotPasseDialog()
        {
            Messenger.Default.Send(new MessageDialog(DialogType.ResetPassword, "Réinitialisation du mot de passe", "", "Annuler"));
            Messenger.Default.Send(new MessageChangerDialog(Dialog));
        }

        public void Close(int zIndex)
        {
            Dialog = new UserControl();
            switch (zIndex)
            {
                case 0:
                    Messenger.Default.Send(new MessageChangerDialog(Dialog));
                    break;
                case 1:
                    Messenger.Default.Send(new MessageChangerDialogOverDialog(Dialog));
                    break;
            }
        }
    }
}
