using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using TraffiGo.Modeles.Messages;
using GalaSoft.MvvmLight.Messaging;
using TraffiGo.Modeles.Data;
using TraffiGo.Modeles.ClasseSql;
using TraffiGo.Modeles;
using System.Collections.Generic;

namespace TraffiGo.VueModeles.Dialogs
{
    public class SauvegarderSimulationVueModele : VueModeleBase
    {
        private AbortableBackgroundworker worker;
        public string TitreDialog { get; set; }
        public string SubTitle1 { get; set; }
        public string Rdo1Text { get; set; }
        public string Rdo2Text { get; set; }
        public bool PriveIsChecked { get; set; }
        public bool PubliqueIsChecked { get; set; }
        public Brush NomSimulationBGColor { get; set; }
        public bool NomSimulationIsEnabled { get; set; }
        public string SubTitle2 { get; set; }
        public string NomSimulation { get; set; }
        public string OkButtonText { get; set; }
        public string CancelButtonText { get; set; }
        private bool NomSimulationValide { get; set; }
        public Simulation Sim { get; set; }
        public Brush NomSimulationBorderColor { get; set; }
        public string SrcImgDispoNomSimulation { get; set; }
        private const string BaseUrl = PackUrl + "/Resources/Images/SauvegarderSimulation/";
        public int ZIndex { get; set; }
        public bool Ecrasement { get; set; } = false;
        public Simulation SimTemp { get; set; }


        public SauvegarderSimulationVueModele()
        {
            SrcImgDispoNomSimulation = "";
            NomSimulationBGColor = Brushes.White;
            NomSimulationIsEnabled = true;
            NomSimulationBorderColor = Brushes.Black;
            NomSimulationValide = false;
            SubTitle1 = "Accessibilité de la simulation";
            Rdo1Text = "Privé";
            PriveIsChecked = true;
            Rdo2Text = "Publique";
            PubliqueIsChecked = false;
            SubTitle2 = "Nom de la simulation";
            NomSimulation = "";
            SetCursorToNull();
        }

        private void NewThreadStart()
        {
            worker = new AbortableBackgroundworker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogManager.Close(ZIndex);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                NomSimulationValide = false;
                Sim.RenommerSimulation(NomSimulation);
                Application.Current.Dispatcher.Invoke((Action)SetCursorToWait);
                if (PubliqueIsChecked)
                {
                    MySqlSimulations.InsertComplet(Sim, true);
                }
                else
                {
                    MySqlSimulations.InsertComplet(Sim, false);
                }
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true;
                return;
            }
            catch (Exception)
            {
                e.Cancel = true;
                return;
            }
            finally
            {
                Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToNull);
            }
        }

        public ICommand CmdSauvegarderSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if(Ecrasement)
                    {
                        DialogManager.ShowYesNoCancelDialog(
                            "Écraser?", 
                            $"La simulation '{NomSimulation}' Existe déjà et vous apartient. Voulez-vous vraiment l'écraser?", 
                            (b) => 
                            {
                                if (b)
                                {
                                    CreateCopy(NomSimulation);
                                    MySqlSimulations.Delete(NomSimulation);
                                    Sauvegarder();
                                }
                            }
                        );
                    }
                    else
                    {
                        Sauvegarder();
                    }


                }, () => { return ChampsRemplit() && NomSimulationValide; });
            }
        }

        private void Sauvegarder()
        {
            NomSimulationIsEnabled = false;
            NomSimulationBGColor = Brushes.DarkGray;
            NewThreadStart();
            Messenger.Default.Send(new MessageSimHasChanged { Value = false });
        }

        private void CreateCopy(string nom)
        {
            SimTemp = MySqlSimulations.Retrieve(nom);
        }

        private bool ChampsRemplit()
        {
            return (String.IsNullOrWhiteSpace(NomSimulation) == false);
        }

        public ICommand CmdFermerDialog
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (worker != null && worker.IsBusy)
                    {
                        worker.Abort();
                        worker.Dispose();
                        worker = new AbortableBackgroundworker();


                        AbortableBackgroundworker workerDeleteSim = new AbortableBackgroundworker();
                        workerDeleteSim.DoWork += WorkerDeleteSim_DoWork;
                        if (Ecrasement)
                        {
                            workerDeleteSim.RunWorkerCompleted += WorkerDeleteSim_WorkerCompleted;
                        }
                        workerDeleteSim.RunWorkerAsync();
                        Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToNull);

                    }
                    DialogManager.Close(ZIndex);
                });
            }
        }

        private void WorkerDeleteSim_DoWork(object sender, DoWorkEventArgs e)
        {
            //Application.Current.Dispatcher.BeginInvoke((Action)(() => { MySqlSimulations.Delete(NomSimulation); }));
            MySqlSimulations.Delete(NomSimulation);
        }

        private void WorkerDeleteSim_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Sim = SimTemp;

            Sauvegarder();
        }

        public void VerifierSiNomValide()
        {
            if (String.IsNullOrWhiteSpace(NomSimulation))
            {
                SrcImgDispoNomSimulation = "";
                NomSimulationBorderColor = Brushes.Black;
                return;
            }
            
            List<Simulation> lst = new List<Simulation>();
            lst = MySqlSimulations.VerifNomSimulation(NomSimulation, Utilisateur.NomUtilisateur, PubliqueIsChecked);

            NomSimulationValide = true;
            Ecrasement = false;

            if (lst.Count > 0 )
            {
                if (!PubliqueIsChecked)
                    Ecrasement = true;
                    
                else
                {
                    foreach (Simulation s in lst)
                    {
                        if (s.Auteur == Utilisateur.NomUtilisateur)
                        {
                            Ecrasement = true;
                        }
                        else
                        {
                            Ecrasement = false;
                            NomSimulationValide = false;
                            break;
                        }
                    }
                }
            }

            if (NomSimulationValide)
            {
                SrcImgDispoNomSimulation = BaseUrl + "Available.png";
                if (!Ecrasement)
                {
                    NomSimulationBorderColor = Brushes.Green;
                }
                else
                {
                    NomSimulationBorderColor = Brushes.Yellow;
                }
            }
            else
            {
                NomSimulationBorderColor = Brushes.Red;
                SrcImgDispoNomSimulation = BaseUrl + "NotAvailable.png";
            }
        }
    }
}
