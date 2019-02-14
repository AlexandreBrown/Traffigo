using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TraffiGo.Modeles;
using TraffiGo.Modeles.ClasseSql;
using TraffiGo.Modeles.Data;
using TraffiGo.Modeles.Messages.ChangementVue;
using TraffiGo.Vues;

namespace TraffiGo.VueModeles
{
    public class CreationCompteVueModele : VueModeleBase
    {
        public string TxtPrenom { get; set; }
        public string TxtNom { get; set; }
        public string TxtCourriel { get; set; }
        public string TxtNomUtilisateur { get; set; }
        public string PwbMotPasse { get; set; }
        public string PwbConfirmMotPasse { get; set; }
        public Action AfficherMenuPrincipal { get; set; }
        private bool CanCreateAccount { get; set; }
        private AbortableBackgroundworker worker;


        public CreationCompteVueModele()
        {
            CanCreateAccount = true;
            PwbMotPasse = "";
            PwbConfirmMotPasse = "";
            AfficherMenuPrincipal += OnAfficherMenuPrincipal;
        }

        private void NewThreadStart()
        {
            worker = new AbortableBackgroundworker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void OnAfficherMenuPrincipal()
        {
            Messenger.Default.Send(new MessageChangerVue { vueModele = new MenuPrincipalVueModele() });
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Validator.IsValidEmailAddress(TxtCourriel))
                {
                    if (PasswordsAreEquals() == false)
                    {
                        throw new Exception("Les mots de passe ne sont pas identiques!");
                    }
                    Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToWait);
                    CreerUtilisateur();
                    Application.Current.Dispatcher.BeginInvoke(AfficherMenuPrincipal);
                }
                else
                {
                    DialogManager.ShowErrorDialog("Erreur", '"' +TxtCourriel + '"' +" ne correspond pas à un courriel valide!", "OK");
                    CanCreateAccount = true;
                }

            }
            catch (ThreadAbortException)
            {
                e.Cancel = true;
                return;
            }
            catch (Exception exep)
            {
                DialogManager.ShowErrorDialog("Erreur", exep.Message, "OK");
                CanCreateAccount = true;
            }
            finally
            {
                Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToNull);
            }
        }

        public ICommand CmdCreerCompte
        {
            get
            {
                return new RelayCommand(() =>
               {
                   CanCreateAccount = false;
                   NewThreadStart();
               }, () => { return ChampsRemplit() && CanCreateAccount; });
            }
        }

        public override ICommand CmdRetourAccueil
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (worker != null && worker.IsBusy)
                    {
                        worker.Abort();
                        worker.Dispose();
                        Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToNull);
                    }
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new AccueilVueModele() });
                });
            }
        }

        public void OnPasswordChanged(object sender,RoutedEventArgs e)
        {
            PwbMotPasse = ((PasswordBox)sender).Password;
        }

        public void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            PwbConfirmMotPasse = ((PasswordBox)sender).Password;
        }

        public bool PasswordsAreEquals()
        {
            return PwbMotPasse == PwbConfirmMotPasse && !String.IsNullOrWhiteSpace(PwbMotPasse);
        }

        private bool ChampsRemplit()
        {
            return (string.IsNullOrWhiteSpace(TxtPrenom) || string.IsNullOrWhiteSpace(TxtNom) || string.IsNullOrWhiteSpace(TxtNomUtilisateur) || string.IsNullOrWhiteSpace(TxtCourriel) || string.IsNullOrWhiteSpace(PwbMotPasse) || string.IsNullOrWhiteSpace(PwbConfirmMotPasse)) == false;
        }

        public void CreerUtilisateur()
        {
            Utilisateur utilisateur;
            utilisateur = new Utilisateur(TxtPrenom, TxtNom, TxtNomUtilisateur, PwbMotPasse, TxtCourriel);
            MySqlUtilisateurs.Insert(utilisateur);
            Utilisateur = utilisateur;
        }
    }
}
