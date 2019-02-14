using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Input;
using TraffiGo.Modeles;
using TraffiGo.Modeles.Messages.ChangementVue;

namespace TraffiGo.VueModeles
{
    public class MenuPrincipalVueModele : VueModeleBase
    {
        private AbortableBackgroundworker workerOuvrirVueSim;

        private void NewThreadOuvrirSimStart()
        {
            workerOuvrirVueSim = new AbortableBackgroundworker();
            workerOuvrirVueSim.WorkerSupportsCancellation = true;
            workerOuvrirVueSim.DoWork += WorkerOuvrirVueSim_DoWork;
            workerOuvrirVueSim.RunWorkerAsync();
        }

        private void WorkerOuvrirVueSim_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(()=> { Messenger.Default.Send(new MessageChangerVue { vueModele = new EditionSimulationVueModele() }); }));
        }


        public ICommand CmdDeconnecterUtilisateur
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (workerOuvrirVueSim != null && workerOuvrirVueSim.IsBusy)
                    {
                        workerOuvrirVueSim.Abort();
                        workerOuvrirVueSim.Dispose();
                    }
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new AccueilVueModele()});
                });
            }
        }

        public ICommand CmdOuvrirVueDocumentation
        {
            get
            {
                return new RelayCommand(() => {
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new DocumentationVueModele() });
                });
            }
        }

        public ICommand CmdOuvrirVueSimulation
        {
            get
            {
                return new RelayCommand(() => {
                    NewThreadOuvrirSimStart();
                });
            }
        }

        public ICommand CmdOuvrirVueSimulationExistante
        {
            get
            {
                return new RelayCommand(() =>{
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new SimulationExistanteVueModele() });
                });
            }
        }

        public ICommand CmdModifierMotDePasse
        {
            get
            {
                return new RelayCommand(()=>
                {
                    DialogManager.ShowModifierMotPasseDialog();
                });
            }
        }
    }
}
