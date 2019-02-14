using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TraffiGo.Modeles;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;
using TraffiGo.Modeles.Messages;
using TraffiGo.Modeles.Messages.ChangementVue;
using System.Web.Script.Serialization;
using TraffiGo.Modeles.Data;
using System.Collections.Generic;
using TraffiGo.EditionIntersection.VueModeles;

namespace TraffiGo.VueModeles
{
    public class ObjetDraggable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Source { get; set; }
        public Thickness Margin { get; set; }
    }

    public class EditionSimulationVueModele : VueModeleBase
    {
        #region Attributs

        private AbortableBackgroundworker workerLaunchSim;
        private AbortableBackgroundworker workerOuvrirDefinirInters;

        #region DraggableItems

        public int CnvDraggableItemsHeight { get; set; }
        public int CnvDraggableItemsWidth { get; set; }
        private const int DraggableItemsNbCols = 11;
        private const int DraggableItemsNbRows = 1;
        public ObservableCollection<ObservableCollection<ObjetDraggable>> DraggableItems { get; set; }
        public ObservableCollection<ObjetDraggable> LstImgBeinDragged { get; set; }

        #endregion

        #region GrilleSimulation

        public bool EditionEstActive { get; set; } = true;
        private Action<bool> actionViderGrille;
        public static int GrilleSimulationHeight { get; set; } = 380;
        public static int GrilleSimulationWidth  { get; set; } = 855;
        public int CaseHeight { get; set; }
        public int CaseWidth { get; set; }
        public Simulation simulation { get; set; }
        public ObservableCollection<ObservableCollection<Case>> LstCases { get; set; }
        public bool SimContientIntersChem { get; set; }
        public bool SimHasChanged { get; set; }
        public Point? CaseSelectionnee { get; set; }
        private const string DraggableItemsURL = "/Resources/Images/Simulation/DraggableItems/";
        public Brush IntegerUpDownBG { get; set; }
        private bool integerUpDownHitVisible;
        public bool IntegerUpDownHitVisible
        {
            get
            {
                if(integerUpDownHitVisible == true)
                {
                    IntegerUpDownBG = Brushes.White;
                }
                else
                {
                    IntegerUpDownBG = Brushes.Gray;
                }
                return integerUpDownHitVisible;
            }
            set
            {
                if(value != integerUpDownHitVisible)
                {
                    integerUpDownHitVisible = value;
                }
            }
        }

        public bool VoituresIsChecked { get; set; }
        public bool CamionsIsChecked { get; set; }
        public bool MotosIsChecked { get; set; }
        public bool AleatoireIsChecked { get; set; }

        public int[] NbCols { get; set; }
        public int[] NbRows { get; set; }

        public int ColWidth {
            get
            {
                return GrilleSimulationWidth / Simulation.nbCols;
            }
        }

        public int RowHeight
        {
            get
            {
                return GrilleSimulationHeight / Simulation.nbRows;
            }
        }
        private int duree;
        public int Duree { get { return duree; }
            set
            {
                duree = value;
                simulation.ChangerDuree(value);
            }

        }
        #endregion

        #endregion

        public EditionSimulationVueModele()
        {
            simulation = simulation = new Simulation(Utilisateur.NomUtilisateur, DateTime.Now, "");
            Initialize();
            AleatoireIsChecked = true;
            Duree = 30;
        }

        public EditionSimulationVueModele(Simulation sim)
        {
            simulation = sim;
            Initialize();
            AleatoireIsChecked = true;
            Duree = 30;
        }

        public EditionSimulationVueModele(Simulation sim,Point? caseSelectionnee) : this(sim)
        {
            CaseSelectionnee = caseSelectionnee;
            AddSelectedEffect(LstCases[(int)CaseSelectionnee.Value.X][(int)CaseSelectionnee.Value.Y]);
        }

        private void Initialize()
        {

            // DraggableItems 
            CnvDraggableItemsHeight = 120;
            CnvDraggableItemsWidth = 1270;
            DraggableItems = new ObservableCollection<ObservableCollection<ObjetDraggable>>();
            DefineDraggableItems();

            // Grille simulation
            CaseHeight = GridMath.TrouverSizeOptimaleCaseSelonDim(Simulation.nbCols, Simulation.nbRows, GrilleSimulationHeight, GrilleSimulationWidth);
            CaseWidth = GridMath.TrouverSizeOptimaleCaseSelonDim(Simulation.nbCols, Simulation.nbRows, GrilleSimulationHeight, GrilleSimulationWidth);
            InitializeLstCases();
            ResizeGrilleSimulation();
            CaseSelectionnee = null;
            IntegerUpDownHitVisible = true;
            SimContientIntersChem = simulation.ContientCheminOuIntersection();
            SimHasChanged = false;

            NbCols = new int[Simulation.nbCols];
            for (int i = 0; i < Simulation.nbCols; i++)
            {
                NbCols[i] = i + 1;
            }
            NbRows = new int[Simulation.nbRows];
            for (int i = 0; i < Simulation.nbRows; i++)
            {
                NbRows[i] = i + 1;
            }

            Messenger.Default.Register<MessageSimHasChanged>(this, (messageSimHasChanged) =>
            {
                SimHasChanged = messageSimHasChanged.Value;
            });
        }

        #region DraggableItems
        private void DefineDraggableItems()
        {
            int optimalSize = CnvDraggableItemsHeight;
            int spacer = 8;
            for (int i = 0; i < DraggableItemsNbCols; i++)
            {
                DraggableItems.Add(new ObservableCollection<ObjetDraggable>());
                for (int j = 0; j < DraggableItemsNbRows; j++)
                {
                    DraggableItems[i].Add(new ObjetDraggable { Height = optimalSize - spacer, Width = optimalSize - spacer, Margin = new Thickness(i*optimalSize + spacer, spacer/2,0,0), Source = TrouverSourceSelonIndex(i) });
                }
            }
            CnvDraggableItemsWidth = optimalSize * DraggableItems.Count;
        }

        private string TrouverSourceSelonIndex(int index)
        {
            BitmapImage imgSrc;
            string path = PackUrl;
            if(index < 6)
            {
                path += DraggableItemsURL + "Chemin" + index.ToString() + ".jpg";
            }
            else
            {
                path += DraggableItemsURL + "Intersection"+ (index - 5 ).ToString() +".jpg";
            }

            imgSrc = new BitmapImage(new Uri(path,UriKind.Absolute));

            return path;
        }

        #endregion

        private void InitializeLstCases()
        {
            LstCases = new ObservableCollection<ObservableCollection<Case>>();
            for (int i = 0; i < simulation.LstElem.Count; i++)
            {
                LstCases.Add(new ObservableCollection<Case>());
                for (int j = 0; j < simulation.LstElem[i].Count; j++)
                {
                    Case uneCase = new Case(CaseHeight, CaseWidth, (int)simulation.LstElem[i][j].Position.X, (int)simulation.LstElem[i][j].Position.Y);
                    LstCases[i].Add(uneCase);
                    LstCases[i][j].SetFill(simulation.LstElem[i][j]);
                }
            }
        }

        public void AfficherEditionIntersection()
        {
            if(CaseSelectionnee != null && simulation.LstElem[(int)CaseSelectionnee.Value.X][(int)CaseSelectionnee.Value.Y] is Intersection)
            {
                if (workerOuvrirDefinirInters != null && workerOuvrirDefinirInters.IsBusy)
                {
                    workerOuvrirDefinirInters.Abort();
                    workerOuvrirDefinirInters.Dispose();
                }
                workerOuvrirDefinirInters = new AbortableBackgroundworker();
                workerOuvrirDefinirInters.WorkerSupportsCancellation = true;
                workerOuvrirDefinirInters.DoWork += WorkerOuvrirDefinirInters_DoWork;
                workerOuvrirDefinirInters.RunWorkerAsync();
            }
        }

        private void ResizeGrilleSimulation()
        {
            GrilleSimulationHeight = CaseHeight * Simulation.nbRows;
            GrilleSimulationWidth  = CaseWidth * Simulation.nbCols;
        }

        private void AddHoverEffect(Case c)
        {
            c.Stroke = Brushes.Green;
            c.StrokeThickness = 4;
        }

        private void RemoveRectangleEffect(Case c)
        {
            c.Stroke = Brushes.Black;
            c.StrokeThickness = 1;
        }

        private void AddSelectedEffect(Case c)
        {
            c.Stroke = Brushes.SkyBlue;
            c.StrokeThickness = 4;
        }

        #region commandes

        public ICommand CmdMouseEnterCase
        {
            get
            {
                return new RelayCommand<Case>((c) =>
                {
                    // Si la case que nous avons Enter n'est pas la case sélectionnée
                    if (c.Position != CaseSelectionnee)
                    {
                        // On ajoute un effet de hover
                        AddHoverEffect(c);
                    }
                    // Si la case contient une intersection ou un chemin
                    if (simulation.EstOccupe(c.Position))
                    {
                        SetCursorToHand();
                    }
                    else
                    {
                        SetCursorToNull();
                    }
                });
            }
        }

        public ICommand CmdMouseLeaveCase
        {
            get
            {
                return new RelayCommand<Case>((c) =>
                {
                    // Si la case que nous avons Leave n'est pas la case sélectionnée
                    if (c.Position != CaseSelectionnee)
                    {
                        // On retire l'effet de hover
                        RemoveRectangleEffect(c);
                    }
                    SetCursorToNull();
                });
            }
        }

        public ICommand CmdMouseDownCase
        {
            get
            {
                return new RelayCommand<Case>((c) =>
                {
                    // On vérifier si il y avait une case sélectionnée avant notre click
                    if (CaseSelectionnee != null)
                    {
                        // On retire l'effet sur celle-ci
                        RemoveRectangleEffect(LstCases[(int)CaseSelectionnee.Value.X][(int)CaseSelectionnee.Value.Y]);
                    }

                    // Si la case que nous avons clické est occupée
                    if (simulation.EstOccupe(c.Position))
                    {
                        // On met à jour la cas sélectionnée pour la case actuelle
                        CaseSelectionnee = c.Position;

                        // On ajoute un effet de sélection à cette case
                        AddSelectedEffect(c);
                    }
                    else // Si le rectangle sélectionné est vide
                    {
                        // On met à jour la case sélectionnée pour null
                        CaseSelectionnee = null;
                    }
                });
            }
        }

        public ICommand CmdReduireNiveauTrafic
        {
            get
            {
                return new RelayCommand(() =>
                {
                    simulation.ReduireNiveauTrafic();
                });
            }
        }

        public override ICommand CmdRetourMenuPrincipal
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (workerLaunchSim != null && workerLaunchSim.IsBusy)
                    {
                        workerLaunchSim.Abort();
                        workerLaunchSim.Dispose();
                        Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToNull);
                    }
                    if (workerOuvrirDefinirInters != null && workerOuvrirDefinirInters.IsBusy)
                    {
                        workerOuvrirDefinirInters.Abort();
                        workerOuvrirDefinirInters.Dispose();
                    }
                    Messenger.Default.Send(new MessageChangerVue { vueModele = new MenuPrincipalVueModele() });
                });
            }
        }



        public ICommand CmdAugmenterNiveauTrafic
        {
            get
            {
                return new RelayCommand(() =>
                {
                    simulation.AugmenterNiveauTrafic();
                });
            }
        }

        public ICommand CmdDureeLimiteeToggle
        {
            get
            {
                return new RelayCommand<Case>((c) =>
                {
                    if(IntegerUpDownHitVisible == true)
                    {
                        IntegerUpDownHitVisible = false;
                        Duree = 0;
                    }
                    else
                    {
                        IntegerUpDownHitVisible = true;
                        Duree = 30;
                    }

                    simulation.ChangerDuree(Duree);

                });
            }
        }

        public ICommand CmdEditerIntersection
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AfficherEditionIntersection();
                }, CaseSelectionneeEstIntersection);
            }
        }

        private void WorkerOuvrirDefinirInters_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() => { Messenger.Default.Send(new MessageChangerVue { vueModele = new EditionIntersectionVueModele(simulation, CaseSelectionnee) }); }));
        }

        public ICommand CmdSupprimerElementGrilleSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    simulation.SupprimerCase(CaseSelectionnee);
                    LstCases[(int)CaseSelectionnee.Value.X][(int)CaseSelectionnee.Value.Y].SetFill(simulation.LstElem[(int)CaseSelectionnee.Value.X][(int)CaseSelectionnee.Value.Y]);
                    LstCases[(int)CaseSelectionnee.Value.X][(int)CaseSelectionnee.Value.Y].Stroke = Case.DefaultStroke;
                    LstCases[(int)CaseSelectionnee.Value.X][(int)CaseSelectionnee.Value.Y].StrokeThickness = Case.DefaultStrokeThickness;
                    CaseSelectionnee = null;
                    SimHasChanged = true;
                    SimContientIntersChem = simulation.ContientCheminOuIntersection();
                }, () => { return CaseSelectionnee != null; });
            }
        }

        public ICommand CmdViderGrilleSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DialogManager.ShowYesNoCancelDialog("Vider la grille?",
                                                        "Êtes-vous certain de vouloir vider la grille de simulation dans son entièreté? Cette action n'est pas réversible.",
                                                        actionViderGrille = new Action<bool>(onViderGrille));
                }, () => { return SimContientIntersChem; });
            }
        }

        public ICommand CmdAleatoireChecked
        {
            get
            {
                return new RelayCommand(() =>
                {
                    VoituresIsChecked = false;
                    MotosIsChecked = false;
                    CamionsIsChecked = false;
                    AleatoireIsChecked = true;
                    List<TypeVehicule> lst = new List<TypeVehicule>();
                    lst.Add(TypeVehicule.CAMION);
                    lst.Add(TypeVehicule.MOTO);
                    lst.Add(TypeVehicule.VOITURE);
                    
                    simulation.ModifierTypeVehicules(lst.ToArray());
                });
            }
        }

        public ICommand CmdVehiculeChecked
        {
            get
            {
                return new RelayCommand(() =>
                {

                    List<TypeVehicule> lst = new List<TypeVehicule>();

                    if(VoituresIsChecked && MotosIsChecked && CamionsIsChecked)
                    {
                        AleatoireIsChecked = true;
                        VoituresIsChecked = false;
                        MotosIsChecked = false;
                        CamionsIsChecked = false;

                        lst.Add(TypeVehicule.CAMION);
                        lst.Add(TypeVehicule.MOTO);
                        lst.Add(TypeVehicule.VOITURE);

                    }
                    else
                    {
                        AleatoireIsChecked = false;

                        if (VoituresIsChecked)
                            lst.Add(TypeVehicule.VOITURE);

                        if (CamionsIsChecked)
                            lst.Add(TypeVehicule.CAMION);

                        if (MotosIsChecked)
                            lst.Add(TypeVehicule.MOTO);


                    }

                    simulation.ModifierTypeVehicules(lst.ToArray());
                });
            }
        }

        private void onViderGrille(bool result)
        {
            if(result == true)
            {
                simulation.ViderGrille();
                for (int i = 0; i < simulation.LstElem.Count; i++)
                {
                    for (int j = 0; j < simulation.LstElem[i].Count; j++)
                    {
                        LstCases[i][j].SetFill(simulation.LstElem[i][j]);
                        LstCases[i][j].Stroke = Case.DefaultStroke;
                        LstCases[i][j].StrokeThickness = Case.DefaultStrokeThickness;
                    }
                }
                CaseSelectionnee = null;
                SimHasChanged = true;
                SimContientIntersChem = simulation.ContientCheminOuIntersection();
            }
        }

        public ICommand CmdSauvegarderSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DialogManager.ShowSaveSimulationDialog(simulation);
                },()=> { return SimContientIntersChem && SimHasChanged; });
            }
        }

        public ICommand CmdOuvrirSimulation
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {

                        if (simulation.GrilleEstValide())
                        {
                            workerLaunchSim = new AbortableBackgroundworker();
                            workerLaunchSim.WorkerSupportsCancellation = true;
                            workerLaunchSim.DoWork += WorkerLaunchSim_DoWork;
                            workerLaunchSim.RunWorkerAsync();
                        }
                    }
                    catch (AggregateException e)
                    {
                        DialogManager.ShowErrorDialog("Erreur", e.InnerException.Message, "OK");
                    }
                    catch (Exception e)
                    {
                        DialogManager.ShowErrorDialog("Erreur", e.Message, "OK");
                    }
                }, () => { return SimContientIntersChem && AtLeastOneChecked(); });
            }
        }

        private void WorkerLaunchSim_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            EditionEstActive = false;
            Application.Current.Dispatcher.BeginInvoke((Action)SetCursorToHand);
            string currentWorkingDIR = Directory.GetCurrentDirectory();

            WriteSimulationJSON(currentWorkingDIR);

            LaunchSimulation(currentWorkingDIR);
        }

        private bool AtLeastOneChecked()
        {
            return AleatoireIsChecked || VoituresIsChecked || CamionsIsChecked || MotosIsChecked;
        }
        private void LaunchSimulation(string currentWorkingDIR)
        {
            string path = currentWorkingDIR + "\\Resources\\Simulation\\Simulation.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = path;

            Process p = new Process();
            p.StartInfo = startInfo;
            p.EnableRaisingEvents = true;
            p.Start();
            p.Exited += P_Exited;
        }

        private void P_Exited(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => EditionEstActive = true));
        }

        private void WriteSimulationJSON(string currentWorkingDIR)
        {
            string content = new JavaScriptSerializer().Serialize(simulation);
            string fileDir = (currentWorkingDIR + "\\object.json");
            using (StreamWriter outputFile = new StreamWriter(fileDir))
            {
                outputFile.WriteLine(content);
            }
        }

        #endregion

        public void UpdateCursorBasedOnCaseSelected(Point p)
        {
            if (simulation.EstOccupe(p))
            {
                SetCursorToHand();
            }
        }

        private bool CaseSelectionneeEstIntersection()
        {
            if(CaseSelectionnee != null)
            {
                return simulation.LstElem[(int)CaseSelectionnee.Value.X][(int)CaseSelectionnee.Value.Y] is Intersection;
            }
            return false;
        }

        #region update affichage
        public void AjouterCaseSelonImg(Point positionCase, string sourceImage)
        {
            try
            {
                if (sourceImage.Contains("Chemin"))
                {
                    Chemin c = new Chemin(positionCase);
                    Route r;

                    char chemin = sourceImage[sourceImage.Length - 5];
                    // For testing pruposes
                    switch (chemin)
                    {
                        case '0':

                            r = new Route(Orientation.EST);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.OUEST);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);
                            break;
                        case '1':
                            r = new Route(Orientation.NORD);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.SUD);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);
                            break;
                        case '2':
                            r = new Route(Orientation.OUEST);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.SUD);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.NORD);
                            r.AjouterVoie(Direction.GAUCHE);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.EST);
                            r.AjouterVoie(Direction.DROITE);
                            c.AjouterRoute(r);
                            break;
                        case '3':

                            r = new Route(Orientation.OUEST);
                            r.AjouterVoie(Direction.GAUCHE);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.SUD);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.NORD);
                            r.AjouterVoie(Direction.DROITE);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.EST);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);
                            break;
                        case '4':

                            r = new Route(Orientation.OUEST);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.SUD);
                            r.AjouterVoie(Direction.DROITE);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.NORD);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.EST);
                            r.AjouterVoie(Direction.GAUCHE);
                            c.AjouterRoute(r);
                            break;
                        case '5':

                            r = new Route(Orientation.OUEST);
                            r.AjouterVoie(Direction.DROITE);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.SUD);
                            r.AjouterVoie(Direction.GAUCHE);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.NORD);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);

                            r = new Route(Orientation.EST);
                            r.AjouterVoie(Direction.TOUTDROIT);
                            c.AjouterRoute(r);
                            break;
                    }
                    simulation.AjouterChemin(c);
                    LstCases[(int)c.Position.X][(int)c.Position.Y].SetFill(c);
                }
                else // Intersection
                {
                    // For testing pruposes ( pour l'instant on a seulement 1 intersection ( 4 branches ))
                    Intersection i;
                    Chemin c;
                    Feu f;
                    char intersection = sourceImage[sourceImage.Length - 5];
                    switch (intersection)
                    {
                        case '1':
                            i = new Intersection(positionCase);
                            break;
                        case '2':
                            i = new Intersection(positionCase, true);

                            i.ChargerT2();
                            break;
                        case '3':
                            i = new Intersection(positionCase, true);
                            i.ChargerT3();

                            break;
                        case '4':
                            i = new Intersection(positionCase, true);
                            i.ChargerT4();

                            break;
                        case '5':
                            i = new Intersection(positionCase, true);
                            i.ChargerT5();

                            break;
                        default:
                            i = new Intersection(positionCase, false);
                            break;

                    }

                    simulation.AjouterIntersection(i);
                    LstCases[(int)i.Position.X][(int)i.Position.Y].SetFill(i);
                }
                SimHasChanged = true;
                SimContientIntersChem = simulation.ContientCheminOuIntersection();
            }
            catch (Exception e)
            {
                DialogManager.ShowErrorDialog("Erreur", e.Message, "OK");
            }
        }
        #endregion
    }
}