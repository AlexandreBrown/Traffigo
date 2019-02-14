using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TraffiGo.Modeles;
using TraffiGo.Modeles.Data;
using TraffiGo.Modeles.Messages;
using TraffiGo.Modeles.Messages.ChangementVue;
using TraffiGo.VueModeles;
using TraffiGo.VueModeles.EditionIntersection;
using TraffiGo.Vues.EditionIntersection;

namespace TraffiGo.EditionIntersection.VueModeles
{
    public class EditionIntersectionVueModele : VueModeleBase
    {
        public Intersection IntersectioEnModif { get; set; }
        public UserControl IntersectionUC { get; set; }
        private Simulation SimulationEnCoursDeModification { get; set; }
        private bool EditionIntersection = false;
        public ObservableCollection<FeuGraphique> LstOrdreFeuxCirculation { get; set; }
        private Point? CaseSelectionnee { get; set; }
        public int IntersectionHeight { get; set; }
        public int IntersectionWidth { get; set; }
        public bool ReduireCycleBtnEnabled { get; set; }
        public bool AugmenterCycleBtnEnabled { get; set; }
        private const int HeightBtnOrdreFeu = 30;
        public FeuGraphique FeuSelectionne { get; set; }

        public bool TempsFeuVertEnabled
        {
            get
            {
                return FeuSelectionne != null;
            }
        }

        public bool TempsFeuJauneEnabled
        {
            get
            {
                return FeuSelectionne != null;
            }
        }

        public ObservableCollection<DirectionVoieGraphique> LstDirectionsVoie { get; set; }

        public EditionIntersectionVueModele(Simulation simulation,Point? position)
        {
            SimulationEnCoursDeModification = simulation;
            CaseSelectionnee = position;
            IntersectionHeight = 550;
            IntersectionWidth = 900;
            IntersectioEnModif = ((Intersection)(SimulationEnCoursDeModification.LstElem[(int)position.Value.X][(int)position.Value.Y])).DeepClone();
            var modeleIntersectionUC = new IntersectionVueModele(IntersectioEnModif, IntersectionHeight,IntersectionWidth);
            var vueIntersection = new IntersectionUserControl();
            vueIntersection.DataContext = modeleIntersectionUC;
            IntersectionUC = vueIntersection;

            LstDirectionsVoie = new ObservableCollection<DirectionVoieGraphique>();
            FeuSelectionne = null;
            ReduireCycleBtnEnabled = false;
            AugmenterCycleBtnEnabled = false;

            InitialiserLstOrdreFeuxCirculation();

            Messenger.Default.Register<MessageUpdateLstDirectionsVoie>(this, (messageLstDirectionsVoie) =>
            {
                UpdateLstDirectionsVoie(messageLstDirectionsVoie.LstDirectionsPossibles,messageLstDirectionsVoie.LstDirectionsActuelles);
            });
            Messenger.Default.Register<MessageUpdateFeuSelectionne>(this, (messageFeuSelectionne) =>
            {
                if(messageFeuSelectionne.Feu != null)
                {
                    UpdateFeuSelectionne(messageFeuSelectionne.Feu);
                    if(FeuSelectionne.Cycle > 0) { ReduireCycleBtnEnabled = true; } else { ReduireCycleBtnEnabled = false; }
                    if(FeuSelectionne.Cycle < LstOrdreFeuxCirculation.Count - 1) { AugmenterCycleBtnEnabled = true; } else { AugmenterCycleBtnEnabled = false; }
                }
            });
            Messenger.Default.Register<MessageUpdateLstFeuxVoies>(this, (messageLstFeux) =>
            {
                UpdateOrdreFeux(messageLstFeux.NouvelleListFeux);
            });
        }

        private void UpdateOrdreFeux(ObservableCollection<FeuGraphique> nouvelleListe)
        {
            if(LstOrdreFeuxCirculation == null)
            {
                LstOrdreFeuxCirculation = new ObservableCollection<FeuGraphique>();
            }
            LstOrdreFeuxCirculation.Clear();
            for (int i = 0; i < nouvelleListe.Count; i++)
            {
                LstOrdreFeuxCirculation.Add(new FeuGraphique(nouvelleListe[i], HeightBtnOrdreFeu, "Feu " + nouvelleListe[i].OrientationControlee.ToString()));
            }
            TrierLstFeux();
        }

        private void UpdateFeuSelectionne(FeuGraphique feu)
        {
            if (FeuSelectionne != null)
            {
                RemoveSelectedEffect(FeuSelectionne);
            }
            FeuSelectionne = feu;
            AddSelectedEffect(feu);
        }

        private void RemoveSelectedEffect(FeuGraphique feu)
        {
            for (int i = 0; i < LstOrdreFeuxCirculation.Count; i++)
            {
                if(feu.OrientationControlee == LstOrdreFeuxCirculation[i].OrientationControlee)
                {
                    LstOrdreFeuxCirculation[i].Contour = FeuGraphique.DefaultContour;
                }
            }
        }

        private void AddSelectedEffect(FeuGraphique feu)
        {
            for (int i = 0; i < LstOrdreFeuxCirculation.Count; i++)
            {
                if (feu.OrientationControlee == LstOrdreFeuxCirculation[i].OrientationControlee)
                {
                    LstOrdreFeuxCirculation[i].Contour = FeuGraphique.SelectedContour;
                }
            }
        }

        private void InitialiserLstOrdreFeuxCirculation()
        {
            
            LstOrdreFeuxCirculation = new ObservableCollection<FeuGraphique>();
            for (int i =0;i<IntersectioEnModif.LstFeux.Count;i++)
            {
                LstOrdreFeuxCirculation.Add(new FeuGraphique(IntersectioEnModif.LstFeux[i], HeightBtnOrdreFeu,"Feu "+IntersectioEnModif.LstFeux[i].OrientationControlee.ToString()));
            }
            TrierLstFeux();
        }

        private void TrierLstDirectionsVoie()
        {
            for (int k = 0; k < LstDirectionsVoie.Count; k++)
            {
                for (int i = 0; i < LstDirectionsVoie.Count-1; i++)
                {
                    for (int j = i+1; j < LstDirectionsVoie.Count; j++)
                    {
                        if(LstDirectionsVoie[j].LstDirections.Count > LstDirectionsVoie[i].LstDirections.Count)
                        {
                            DirectionVoieGraphique temp = LstDirectionsVoie[j].DeepClone();

                            LstDirectionsVoie[j] = LstDirectionsVoie[i];
                            LstDirectionsVoie[i] = temp;
                        }
                    }
                }
            }
        }

        private void TrierLstFeux()
        {
            ObservableCollection<FeuGraphique> premierElem = new ObservableCollection<FeuGraphique>();
            ObservableCollection<FeuGraphique> deuxiemeElem = new ObservableCollection<FeuGraphique>();
            ObservableCollection<FeuGraphique> troisiemeElem = new ObservableCollection<FeuGraphique>();
            ObservableCollection<FeuGraphique> quatriemeElem = new ObservableCollection<FeuGraphique>();
            for (int i = 0; i < LstOrdreFeuxCirculation.Count; i++)
            {
                switch (LstOrdreFeuxCirculation[i].Cycle)
                {
                    case 0:
                        premierElem.Add(LstOrdreFeuxCirculation[i]);
                        break;
                    case 1:
                        deuxiemeElem.Add(LstOrdreFeuxCirculation[i]);
                        break;
                    case 2:
                        troisiemeElem.Add(LstOrdreFeuxCirculation[i]);
                        break;
                    case 3:
                        quatriemeElem.Add(LstOrdreFeuxCirculation[i]);
                        break;
                }
            }

            LstOrdreFeuxCirculation.Clear();
            foreach (var item in premierElem)
            {
                LstOrdreFeuxCirculation.Add(item);
            }
            foreach (var item in deuxiemeElem)
            {
                LstOrdreFeuxCirculation.Add(item);
            }
            foreach (var item in troisiemeElem)
            {
                LstOrdreFeuxCirculation.Add(item);
            }
            foreach (var item in quatriemeElem)
            {
                LstOrdreFeuxCirculation.Add(item);
            }
        }

        private void UpdateLstDirectionsVoie(List<Direction> directionsPossibles,List<Direction> directionsActuelles)
        {
            if(LstDirectionsVoie != null && directionsPossibles != null)
            {
                LstDirectionsVoie.Clear();
                List<string> lstSources = DirectionVoieGraphique.GetSourcesFromTousDirectionsPossibles(directionsPossibles);
                foreach (string source in lstSources)
                {
                    if(source != DirectionVoieGraphique.GetSourceFromLstDirections(directionsActuelles))
                    {
                        LstDirectionsVoie.Add(new DirectionVoieGraphique(source, DirectionVoieGraphique.GetListDirectionsFromSource(source)));
                    }
                }
            }
            TrierLstDirectionsVoie();
        }

        private void onRetourAction(bool clickedYes)
        {
            if (clickedYes)
            {
                Messenger.Default.Send(new MessageChangerVue { vueModele = new EditionSimulationVueModele(SimulationEnCoursDeModification, CaseSelectionnee) });
            }
        }

        private void UpdateTempsFeuVertFromFeuSelectionne()
        {
            Messenger.Default.Send(new MessageUpdateTempsFeuVert { Feu = FeuSelectionne });
        }

        private void UpdateTempsFeuJauneFromFeuSelectionne()
        {
            Messenger.Default.Send(new MessageUpdateTempsFeuJaune { Feu = FeuSelectionne });
        }

        public ICommand CmdDirectionMouseDown
        {
            get
            {
                return new RelayCommand<DirectionVoieGraphique>((directions) =>
                {
                    EditionIntersection = true;
                    Messenger.Default.Send(new MessageModifyDirections { nouvellesDirections = directions.LstDirections });
                });
            }
        }

        public ICommand CmdRetourSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (EditionIntersection)
                    {
                        DialogManager.ShowYesNoCancelDialog("Quitter?", "Voulez-vous vraiment quitter sans enregistrer les modifications?", onRetourAction);
                    }
                    else
                    {
                        Messenger.Default.Send(new MessageChangerVue { vueModele = new EditionSimulationVueModele(SimulationEnCoursDeModification, CaseSelectionnee) });
                    }
                });
            }
        }

        public ICommand CmdTempsFeuJauneValueChanged
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EditionIntersection = true;
                    UpdateTempsFeuJauneFromFeuSelectionne();
                });
            }
        }

        public ICommand CmdTempsFeuVertValueChanged
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EditionIntersection = true;
                    UpdateTempsFeuVertFromFeuSelectionne();
                });
            }
        }

        public ICommand CmdAugmenterCycleFeu
        {
            get
            {
                return new RelayCommand(()=>
                {
                    EditionIntersection = true;
                    Messenger.Default.Send(new MessageUpdateCycleFeuSelectionne { ReduireCycle = false});
                });
            }
        }

        public ICommand CmdAppliquerChangements
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EditionIntersection = false;
                    Messenger.Default.Send(new MessageSaveChanges { Simulation = SimulationEnCoursDeModification,CaseSelectionnee = CaseSelectionnee.Value});
                });
            }
        }

        public ICommand CmdReduireCycleFeu
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EditionIntersection = true;
                    Messenger.Default.Send(new MessageUpdateCycleFeuSelectionne { ReduireCycle = true });
                });
            }
        }
    }
}
