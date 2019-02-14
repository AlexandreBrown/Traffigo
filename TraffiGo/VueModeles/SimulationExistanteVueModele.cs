using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TraffiGo.Modeles.Data;
using TraffiGo.Modeles.ClasseSql;
using TraffiGo.Modeles.Messages.ChangementVue;
using GalaSoft.MvvmLight.Messaging;
using System;
using TraffiGo.Modeles;
using System.Windows;
using System.ComponentModel;

namespace TraffiGo.VueModeles
{
    public class SimulationExistanteVueModele : VueModeleBase
    {
        public ObservableCollection<TabItem> TabsInfoSimulation { get; set; }

        public ObservableCollection<Simulation> SimulationsPrivees { get; set; }

        public ObservableCollection<Simulation> SimulationsPubliques { get; set; }

        AbortableBackgroundworker worker = new AbortableBackgroundworker();

        public Simulation SelectedItem { get; set; }

        private Simulation loadedItem;
        public Simulation LoadedItem
        {
            get
            {
                return loadedItem;
            }
            set
            {
                loadedItem = value;
                Application.Current.Dispatcher.Invoke((Action)Initialize);
            }
        }

        private int headerSelected;

        #region Grille Simulation
        public int GrilleSimulationHeightChargement { get; set; } = 300;
        public int GrilleSimulationWidthChargement { get; set; } = 725;
        public int CaseHeight { get; set; }
        public int CaseWidth { get; set; }
        public ObservableCollection<ObservableCollection<Case>> LstCases { get; set; }
        #endregion

        public int HeaderSelected
        {
            get
            {
                return headerSelected;
            }
            set
            {
                SelectedItem = null;
                headerSelected = value;
            }
        }

        public SimulationExistanteVueModele()
        {
            SimulationsPrivees = new ObservableCollection<Simulation>();
            SimulationsPubliques = new ObservableCollection<Simulation>();
            worker.WorkerSupportsCancellation = true;

            HeaderSelected = (int)Header.Personnelles;

            ChargerSimulations();
        }

        #region Chargement
        private void ChargerSimulations()
        {
            worker.DoWork += delegate { ChargerPriver(); };

            worker.DoWork += delegate { ChargerPublic(); };

            worker.RunWorkerAsync();
            
        }

        private void ChargerPriver()
        {
            SimulationsPrivees = new ObservableCollection<Simulation>(MySqlSimulations.RetrievePartiellePrive(Utilisateur.NomUtilisateur));
        }

        private void ChargerPublic()
        {
            SimulationsPubliques = new ObservableCollection<Simulation>(MySqlSimulations.RetrievePartiellePublic());
        }

        #endregion

        private void Initialize()
        {
            if(LoadedItem != null)
            {
                // Grille simulation
                CaseHeight = GridMath.TrouverSizeOptimaleCaseSelonDim(Simulation.nbCols, Simulation.nbRows, GrilleSimulationHeightChargement, GrilleSimulationWidthChargement);
                CaseWidth = GridMath.TrouverSizeOptimaleCaseSelonDim(Simulation.nbCols, Simulation.nbRows, GrilleSimulationHeightChargement, GrilleSimulationWidthChargement);
                InitializeLstCases();
                ResizeGrilleSimulation();
            }
        }

        private void InitializeLstCases()
        {
            LstCases = new ObservableCollection<ObservableCollection<Case>>();
            for (int i = 0; i < LoadedItem.LstElem.Count; i++)
            {
                LstCases.Add(new ObservableCollection<Case>());
                for (int j = 0; j < LoadedItem.LstElem[i].Count; j++)
                {
                    Case uneCase = new Case(CaseHeight, CaseWidth, (int)LoadedItem.LstElem[i][j].Position.X, (int)LoadedItem.LstElem[i][j].Position.Y);
                    LstCases[i].Add(uneCase);
                    LstCases[i][j].SetFill(LoadedItem.LstElem[i][j]);
                }
            }
        }

        private void ResizeGrilleSimulation()
        {
            GrilleSimulationHeightChargement = CaseHeight * Simulation.nbRows;
            GrilleSimulationWidthChargement = CaseWidth * Simulation.nbCols;
        }

        public ICommand CmdSupprimerSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SetCursorToWait();
                    worker = new AbortableBackgroundworker();
                    worker.WorkerSupportsCancellation = true;
                    worker.DoWork += delegate
                    {
                        DialogManager.ShowYesNoCancelDialog(
                            "Supprimer?",
                            "Voulez-vous vraiment supprimer la simulation \" " + SelectedItem.Nom + " \" ?",
                            new Action<bool>(SupprimerSimulation));
                    };

                    worker.RunWorkerCompleted += delegate { SetCursorToNull(); };
                    worker.RunWorkerAsync();

                    
                }, ()=> { return HeaderSelected == (int)Header.Personnelles && SelectedItem != null && LoadedItem != null; } );
            }
        }

        public ICommand CmdRenommerSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SetCursorToWait();
                    worker = new AbortableBackgroundworker();
                    worker.WorkerSupportsCancellation = true;
                    worker.DoWork += delegate 
                    {
                        DialogManager.ShowRenameDialog(SelectedItem.Nom,new Action<string>(Renommer));
                    };
                    worker.RunWorkerCompleted += delegate { SetCursorToNull(); };

                    worker.RunWorkerAsync();
                }, () => { return HeaderSelected == (int)Header.Personnelles && SelectedItem != null && LoadedItem != null; });
            }
        }

        public override ICommand CmdRetourMenuPrincipal
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
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new MenuPrincipalVueModele() });
                });
            }
        }

        public ICommand CmdLoadSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SetCursorToWait();

                    worker = new AbortableBackgroundworker();
                    worker.WorkerSupportsCancellation = true;
                    worker.DoWork += delegate
                    {
                        LoadedItem = MySqlSimulations.Retrieve(SelectedItem.Nom);
                    };

                    worker.RunWorkerCompleted += delegate { SetCursorToNull(); };

                    worker.RunWorkerAsync();

                }, () => { return SelectedItem != null; });
            }
        }


        public ICommand CmdChargerSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LoadedItem.ChangerAuteur(Utilisateur.NomUtilisateur);
                    LoadedItem.RenommerSimulation($"{LoadedItem.Nom} - Copie");
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new EditionSimulationVueModele(LoadedItem) });
                }, () => { return SelectedItem != null && LoadedItem != null; });
            }
        }

        private void Renommer(string nouveauNom)
        {
            MySqlSimulations.Rename(SelectedItem.Nom, nouveauNom);
            SelectedItem.RenommerSimulation(nouveauNom);
        }

        private void SupprimerSimulation(bool result)
        {
            if (result)
            {
                MySqlSimulations.Delete(SelectedItem);
                SimulationsPrivees.Remove(SelectedItem);
                SelectedItem = null;
            }

        }

        private enum Header
        {
            Personnelles,
            Publiques
        }

    }
        public sealed class TabItem
        {
            public string Header { get; set; }
            public string Content { get; set; }
        }

}
