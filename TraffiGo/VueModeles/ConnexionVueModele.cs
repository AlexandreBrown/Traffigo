using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TraffiGo.Modeles;
using TraffiGo.Modeles.Data;
using TraffiGo.Modeles.Messages.ChangementVue;

namespace TraffiGo.VueModeles
{
    public class ConnexionVueModele : VueModeleBase
    {
        public string TxtNomUtilisateur { get; set; }
        public string PwbMotPasse { get; set; }
        public Action AfficherMenuPrincipal { get; set; }
        private bool CanToLogIn { get; set; } 
        private AbortableBackgroundworker worker;

        public ConnexionVueModele()
        {
            CanToLogIn = true;
            PwbMotPasse = "";
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

        public ICommand CmdConnecterCompte
        {
            get
            {
                return new RelayCommand(() =>
                {
                   CanToLogIn = false;
                   NewThreadStart();
                }, () => { return ChampsRemplit() && CanToLogIn; });
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToWait);
                Utilisateur u = new Utilisateur(TxtNomUtilisateur, PwbMotPasse);
                u.Connexion();
                Utilisateur = u;
                Application.Current.Dispatcher.BeginInvoke(AfficherMenuPrincipal);
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true;
                return;
            }
            catch (Exception exep)
            {
                DialogManager.ShowErrorDialog("Erreur de connexion", exep.Message, "OK");
                CanToLogIn = true;
            }
            finally
            {
                Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToNull);
            }
        }

        public override ICommand CmdRetourAccueil
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if(worker != null && worker.IsBusy)
                    {
                        worker.Abort();
                        worker.Dispose();
                        Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToNull);
                    }
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new AccueilVueModele()});
                });
            }
        }


        public bool ChampsRemplit()
            {
                return (String.IsNullOrWhiteSpace(TxtNomUtilisateur) || String.IsNullOrWhiteSpace(PwbMotPasse)) == false;
            }

            public void OnPasswordChanged(object sender, RoutedEventArgs e)
            {
                PwbMotPasse = ((PasswordBox)sender).Password;
            }
        }
}
