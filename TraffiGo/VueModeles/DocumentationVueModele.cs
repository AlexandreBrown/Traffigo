using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TraffiGo.Vues.Documentation;

namespace TraffiGo.VueModeles
{
    public class Procedure
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string TitreProcedure { get; set; }
        public ICommand CommandToExecute { get; set; }

    }

    public class DocumentationVueModele : VueModeleBase
    {
        public UserControl DocActuelle { get; set; }
        public string CurrentDocTitle { get; set; }
        public string TxtRechercheDoc { get; set; }
        public Brush SearchBarBorderColor { get; set; }
        private const int DefaultWidth = 300;
        private const int DefaultHeight = 50;

        public ObservableCollection<Procedure> Procedures { get; set; }
        public ObservableCollection<Procedure> ProceduresToShow { get; set; }

        public ICommand CmdTerminologieDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new TerminologieDocUserControl();
                });
            }
        }

        public ICommand CmdCreerSimulationDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new CreerSimulationDocUserControl();
                });
            }
        }

        
        public ICommand CmdChargerSimulationDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ChargerSimulationDocUserControl();
                });
            }
        }
        
        public ICommand CmdSauvegarderSimulationDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new SauvegarderSimulationDocUserControl();
                });
            }
        }
   
        public ICommand CmdRenommerSimulationDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new RenommerSimulationDocUserControl();
                });
            }
        }

        public ICommand CmdSupprimerSimulationDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new SupprimerSimulationDocUserControl();
                });
            }
        }
        
        public ICommand CmdAjouterIntersectionDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new AjouterIntersectionDocUserControl();
                });
            }
        }
        
        public ICommand CmdAjouterRouteDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new AjouterRouteDocUserControl();
                });
            }
        }
       
        public ICommand CmdSupprimerElementDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new SupprimerElemSimulationDocUserControl();
                });
            }
        }
         
        public ICommand CmdViderGrilleDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ViderGrilleDocUserControl();
                });
            }
        }
        
        public ICommand CmdChangerNiveauTraficDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ChangerNiveauTraficDocUserControl();
                });
            }
        }
        
        public ICommand CmdChangerTypesVehiculesDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ChangerTypesVehiculesDocUserControl();
                });
            }
        }
        
        public ICommand CmdChangerDureeSimulationDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ChangerDureeSimulationDocUserControl();
                });
            }
        }
        
        
        public ICommand CmdEditionIntersectionDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new EditionIntersectionDocUserControl();
                });
            }
        }
        
        public ICommand CmdChangerOrdreFeuxDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ChangerOrdreFeuxDocUserControl();
                });
            }
        }
        
        public ICommand CmdChangerDirectionVoieDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ChangerDirectionVoieDocUserControl();
                });
            }
        }
        
        public ICommand CmdChangerTempsFeuVertDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ChangerTempsFeuVertDocUserControl();
                });
            }
        }

        public ICommand CmdChangerTempsFeuJauneDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new ChangerTempsFeuJauneDocUserControl();
                });
            }
        }

        public ICommand CmdLancerSimulationDoc
        {
            get
            {
                return new RelayCommand<string>((titreProcedure) =>
                {
                    CurrentDocTitle = titreProcedure;
                    DocActuelle = new LancerSimulationDocUserControl();
                });
            }
        }

        public ICommand CmdOnKeyDownSearchBar
        {
            get
            {
                return new RelayCommand(() => 
                {
                    UpdateProceduresSelonTexte(TxtRechercheDoc);
                });
            }  
        }

        public ICommand CmdOnKeyUpSearchBar
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (String.IsNullOrWhiteSpace(TxtRechercheDoc))
                    {
                        ProceduresToShow = new ObservableCollection<Procedure>();
                        ProceduresToShow = Procedures;
                        SearchBarBorderColor = Brushes.Black;
                    }
                    else
                    {
                        UpdateProceduresSelonTexte(TxtRechercheDoc);
                    }
                });
            }
        }

        private void UpdateProceduresSelonTexte(string texte)
        {
            ObservableCollection<Procedure> matchingProcedures = new ObservableCollection<Procedure>();
            foreach (Procedure p in Procedures)
            {
                if (p.TitreProcedure.ToLower().Contains(texte.ToLower()))
                {
                    matchingProcedures.Add(new Procedure
                    {
                        Width = p.Width,
                        Height = p.Height,
                        TitreProcedure = p.TitreProcedure,
                        CommandToExecute = p.CommandToExecute
                    });
                }
            }
            if(matchingProcedures.Count == 0)
            {
                ProceduresToShow = new ObservableCollection<Procedure>();
                SearchBarBorderColor = Brushes.Red;
            }
            else
            {
                ProceduresToShow = new ObservableCollection<Procedure>();
                ProceduresToShow = matchingProcedures;
                SearchBarBorderColor = Brushes.Black;
            }
        }

        private void InitialiserProcedures()
        {
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Terminologie" ,                          CommandToExecute = CmdTerminologieDoc});
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Créer une simulation",                   CommandToExecute = CmdCreerSimulationDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Charger une simulation",                 CommandToExecute = CmdChargerSimulationDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Sauvegarder une simulation",             CommandToExecute = CmdSauvegarderSimulationDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Renommer une simulation",                CommandToExecute = CmdRenommerSimulationDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Supprimer une simulation",               CommandToExecute = CmdSupprimerSimulationDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Ajouter une intersection",               CommandToExecute = CmdAjouterIntersectionDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Ajouter une route",                      CommandToExecute = CmdAjouterRouteDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Supprimer un element de la simulation" , CommandToExecute = CmdSupprimerElementDoc});
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Vider la grille de simulation",          CommandToExecute = CmdViderGrilleDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Changer le niveau de trafic",            CommandToExecute = CmdChangerNiveauTraficDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Changer les types de vehicules",         CommandToExecute = CmdChangerTypesVehiculesDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Changer la durée d'une simulation",      CommandToExecute = CmdChangerDureeSimulationDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Editer une intersection",                CommandToExecute = CmdEditionIntersectionDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Changer l'ordre des feux de circulation",CommandToExecute = CmdChangerOrdreFeuxDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Changer la direction d'une voie",        CommandToExecute = CmdChangerDirectionVoieDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Changer le temps d'un feu vert",         CommandToExecute = CmdChangerTempsFeuVertDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Changer le temps d'un feu jaune",        CommandToExecute = CmdChangerTempsFeuJauneDoc });
            Procedures.Add(new Procedure { Width = DefaultWidth, Height = DefaultHeight, TitreProcedure = "Lancer une simulation",                  CommandToExecute = CmdLancerSimulationDoc });
        }

        public DocumentationVueModele()
        {
            Procedures = new ObservableCollection<Procedure>();
            ProceduresToShow = new ObservableCollection<Procedure>();
            TxtRechercheDoc = "";
            SearchBarBorderColor = Brushes.Black;
            InitialiserProcedures();
            ProceduresToShow = Procedures;
        }
    }
}
