using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TraffiGo.Modeles;
using TraffiGo.Modeles.Data;
using TraffiGo.Modeles.Messages;
using TraffiGo.Modeles.Messages.ChangementVue;

namespace TraffiGo.VueModeles.EditionIntersection
{
    public class IntersectionVueModele : VueModeleBase
    {
        public Intersection IntersectionEnModif { get; set; }

        public ObservableCollection<VoieGraphique> LstVoiesNord { get; set; }
        public ObservableCollection<VoieGraphique> LstVoiesEst { get; set; }
        public ObservableCollection<VoieGraphique> LstVoiesSud { get; set; }
        public ObservableCollection<VoieGraphique> LstVoiesOuest { get; set; }

        public ObservableCollection<DirectionVoieGraphique> LstDirectionsVoiesNord { get; set; }
        public ObservableCollection<DirectionVoieGraphique> LstDirectionsVoiesEst { get; set; }
        public ObservableCollection<DirectionVoieGraphique> LstDirectionsVoiesSud { get; set; }
        public ObservableCollection<DirectionVoieGraphique> LstDirectionsVoiesOuest { get; set; }

        private const string BaseURL = "/Resources/Images/EditionIntersection/";

        private VoieGraphique voieSelectionnee;
        public VoieGraphique VoieSelectionnee
        {
            get
            {
                return voieSelectionnee;
            }
            set
            {
                voieSelectionnee = value;

                List<Direction> lstDirectionsPossibles = new List<Direction>();
                lstDirectionsPossibles = TrouverDirectionsPossiblesFromVoie(VoieSelectionnee, TrouverEmplacementFromVoieGraphique(VoieSelectionnee));


                string sourceDirectionsActuelles = DirectionVoieGraphique.GetSourceFromLstDirections(voieSelectionnee.LstDirections);
                List<Direction> lstDirectionsActuelles = new List<Direction>();
                lstDirectionsActuelles = DirectionVoieGraphique.GetListDirectionsFromSource(sourceDirectionsActuelles);

                Messenger.Default.Send(new MessageUpdateLstDirectionsVoie { LstDirectionsPossibles = lstDirectionsPossibles, LstDirectionsActuelles = lstDirectionsActuelles });
            }
        }

        private FeuGraphique feuSelectionne;
        public FeuGraphique FeuSelectionne
        {
            get
            {
                return feuSelectionne;
            }
            set
            {
                feuSelectionne = value;
                Messenger.Default.Send(new MessageUpdateFeuSelectionne { Feu = feuSelectionne });
            }
        }

        private int IntersectionHeight { get; set; }
        private int IntersectionWidth { get; set; }

        public int MillieuIntersectionTop { get; set; }
        public int MillieuIntersectionLeft { get; set; }
        public int MillieuIntersectionHeight { get; set; }
        public int MillieuIntersectionWidth { get; set; }

        public int VoiesNordTop { get; set; }
        public int VoiesNordLeft { get; set; }

        public int VoiesEstTop { get; set; }
        public int VoiesEstLeft { get; set; }

        public int VoiesSudTop { get; set; }
        public int VoiesSudLeft { get; set; }

        public int VoiesOuestTop { get; set; }
        public int VoiesOuestLeft { get; set; }

        private List<int> LstIdVoiesEstSortie { get; set; }

        public ObservableCollection<FeuGraphique> LstFeuxVoiesNord { get; set; }
        public ObservableCollection<FeuGraphique> LstFeuxVoiesEst { get; set; }
        public ObservableCollection<FeuGraphique> LstFeuxVoiesSud { get; set; }
        public ObservableCollection<FeuGraphique> LstFeuxVoiesOuest { get; set; }

        public IntersectionVueModele(Intersection intersectionSource,int height,int width)
        {
            // Setting default values
            VoieSelectionnee = null;
            FeuSelectionne = null;

            // Setting Intersection
            IntersectionEnModif = intersectionSource;
            IntersectionHeight = height;
            IntersectionWidth = width;

            // Initialisation des listes de voies
            LstVoiesNord = new ObservableCollection<VoieGraphique>();
            LstVoiesEst = new ObservableCollection<VoieGraphique>();
            LstVoiesSud = new ObservableCollection<VoieGraphique>();
            LstVoiesOuest = new ObservableCollection<VoieGraphique>();

            // Initialisation des listes de directions
            LstDirectionsVoiesNord = new ObservableCollection<DirectionVoieGraphique>();
            LstDirectionsVoiesEst = new ObservableCollection<DirectionVoieGraphique>();
            LstDirectionsVoiesSud = new ObservableCollection<DirectionVoieGraphique>();
            LstDirectionsVoiesOuest = new ObservableCollection<DirectionVoieGraphique>();

            // Initialisation des feux
            LstFeuxVoiesNord = new ObservableCollection<FeuGraphique>();
            LstFeuxVoiesEst = new ObservableCollection<FeuGraphique>();
            LstFeuxVoiesSud = new ObservableCollection<FeuGraphique>();
            LstFeuxVoiesOuest = new ObservableCollection<FeuGraphique>();

            // Setting MillieuIntersection
            MillieuIntersectionHeight = 250;
            MillieuIntersectionWidth  = 250;
            MillieuIntersectionTop  = (IntersectionHeight / 2)-(MillieuIntersectionHeight/2);
            MillieuIntersectionLeft = (IntersectionWidth / 2) - (MillieuIntersectionWidth / 2);

            // Positionnement
            PositionnerListes();

            // Trie
            TrierChemins();

            // Voies
            InitialiserLstsVoies();

            // LstSorties
            BuildListeSortie();

            // Directions
            InitialiserLstsDirections();

            // Feux
            InitialiserLstFeux();

            Messenger.Default.Register<MessageModifyDirections>(this, (message) =>
            {
                if(VoieSelectionnee != null)
                {
                    ModifyDirectionVoieSelectionnee(message.nouvellesDirections);

                    List<Direction> lstDirectionsPossibles = new List<Direction>();
                    lstDirectionsPossibles = TrouverDirectionsPossiblesFromVoie(VoieSelectionnee, TrouverEmplacementFromVoieGraphique(VoieSelectionnee));


                    string sourceDirectionsActuelles = DirectionVoieGraphique.GetSourceFromLstDirections(voieSelectionnee.LstDirections);
                    List<Direction> lstDirectionsActuelles = new List<Direction>();
                    lstDirectionsActuelles = DirectionVoieGraphique.GetListDirectionsFromSource(sourceDirectionsActuelles);

                    Messenger.Default.Send(new MessageUpdateLstDirectionsVoie { LstDirectionsPossibles = lstDirectionsPossibles, LstDirectionsActuelles = lstDirectionsActuelles });
                }
            });

            Messenger.Default.Register<MessageUpdateCycleFeuSelectionne>(this, (messageCycleFeuSelectionne) =>
            {
                if(FeuSelectionne != null)
                {
                    if (messageCycleFeuSelectionne.ReduireCycle)
                    {
                        FeuSelectionne.Cycle--;
                    }
                    else
                    {
                        FeuSelectionne.Cycle++;
                    }

                    UpdateCycleFeuFromFeuGraphique(FeuSelectionne);

                    ObservableCollection<FeuGraphique> lstFeux = new ObservableCollection<FeuGraphique>();
                    foreach (FeuGraphique item in LstFeuxVoiesNord)
                    {
                        lstFeux.Add(item);
                    }
                    foreach (FeuGraphique item in LstFeuxVoiesEst)
                    {
                        lstFeux.Add(item);
                    }
                    foreach (FeuGraphique item in LstFeuxVoiesSud)
                    {
                        lstFeux.Add(item);
                    }
                    foreach (FeuGraphique item in LstFeuxVoiesOuest)
                    {
                        lstFeux.Add(item);
                    }

                    for (int i = 0; i < lstFeux.Count; i++)
                    {
                        if (lstFeux[i].OrientationControlee != FeuSelectionne.OrientationControlee && lstFeux[i].Cycle == FeuSelectionne.Cycle)
                        {
                            if (messageCycleFeuSelectionne.ReduireCycle) // Si notre intention était de réduire le feu sélectionné alors on doit augmenté celui de feu en conflit
                            {
                                lstFeux[i].Cycle++;
                            }
                            else
                            {
                                lstFeux[i].Cycle--;
                            }
                            break;
                        }
                    }

                    Messenger.Default.Send(new MessageUpdateLstFeuxVoies { NouvelleListFeux = lstFeux });
                    Messenger.Default.Send(new MessageUpdateFeuSelectionne { Feu = feuSelectionne });
                }
            });

            Messenger.Default.Register<MessageUpdateTempsFeuVert>(this, (message) =>
            {
                for (int i = 0; i < IntersectionEnModif.LstFeux.Count; i++)
                {
                    if(message.Feu.OrientationControlee == IntersectionEnModif.LstFeux[i].OrientationControlee)
                    {
                        IntersectionEnModif.LstFeux[i].TempsVert = message.Feu.TempsVert;
                    }
                }
            });

            Messenger.Default.Register<MessageUpdateTempsFeuJaune>(this, (message) =>
            {
                for (int i = 0; i < IntersectionEnModif.LstFeux.Count; i++)
                {
                    if (message.Feu.OrientationControlee == IntersectionEnModif.LstFeux[i].OrientationControlee)
                    {
                        IntersectionEnModif.LstFeux[i].TempsJaune = message.Feu.TempsJaune;
                    }
                }
            });

            Messenger.Default.Register<MessageSaveChanges>(this, (message) =>
            {
                Intersection IntersectionAvecModif = new Intersection(new Point(message.CaseSelectionnee.X, message.CaseSelectionnee.Y), true);

                IntersectionAvecModif.LstChemins = IntersectionEnModif.LstChemins;
                IntersectionAvecModif.LstFeux = IntersectionEnModif.LstFeux;

                Simulation sim = new Simulation(message.Simulation);
                sim.LstElem[(int)message.CaseSelectionnee.X][(int)message.CaseSelectionnee.Y] = IntersectionAvecModif;
                Messenger.Default.Send(new MessageChangerVue { vueModele = new EditionSimulationVueModele(sim, message.CaseSelectionnee) });
            });
        }

        private void UpdateCycleFeuFromFeuGraphique(FeuGraphique fg)
        {
            for (int i = 0; i < IntersectionEnModif.LstFeux.Count; i++)
            {
                if(IntersectionEnModif.LstFeux[i].OrientationControlee == fg.OrientationControlee)
                {
                    IntersectionEnModif.LstFeux[i].Cycle = fg.Cycle;
                }
            }
        }

        private void ModifierCycleAncien(Orientation orientationSource,int cycleSource,bool reduireCycleFeuEnConflit)
        {
            if(orientationSource != Orientation.NORD)
            {
                for (int i =0;i<LstFeuxVoiesNord.Count;i++)
                {
                    if(LstFeuxVoiesNord[i].Cycle == cycleSource)
                    {
                        if (reduireCycleFeuEnConflit)
                        {
                            --LstFeuxVoiesNord[i].Cycle;
                        }
                        else
                        {
                            ++LstFeuxVoiesNord[i].Cycle;
                        }
                    }
                }
            }
            if(orientationSource != Orientation.EST)
            {
                for (int i = 0; i < LstFeuxVoiesEst.Count; i++)
                {
                    if (LstFeuxVoiesEst[i].Cycle == cycleSource)
                    {
                        if (reduireCycleFeuEnConflit)
                        {
                            --LstFeuxVoiesEst[i].Cycle;
                        }
                        else
                        {
                            ++LstFeuxVoiesEst[i].Cycle;
                        }
                    }
                }
            }
            if(orientationSource != Orientation.SUD)
            {
                for (int i = 0; i < LstFeuxVoiesSud.Count; i++)
                {
                    if (LstFeuxVoiesSud[i].Cycle == cycleSource)
                    {
                        if (reduireCycleFeuEnConflit)
                        {
                            --LstFeuxVoiesSud[i].Cycle;
                        }
                        else
                        {
                            ++LstFeuxVoiesSud[i].Cycle;
                        }
                    }
                }
            }
            if(orientationSource != Orientation.OUEST)
            {
                for (int i = 0; i < LstFeuxVoiesOuest.Count; i++)
                {
                    if (LstFeuxVoiesOuest[i].Cycle == cycleSource)
                    {
                        if (reduireCycleFeuEnConflit)
                        {
                            --LstFeuxVoiesOuest[i].Cycle;
                        }
                        else
                        {
                            ++LstFeuxVoiesOuest[i].Cycle;
                        }
                    }
                }
            }
        }

        private void InitialiserLstFeux()
        {
            InitialiserFeuxNord();
            InitialiserFeuxEst();
            InitialiserFeuxSud();
            InitialiserFeuxOuest();
        }

        private void InitialiserFeuxNord()
        {
            foreach (Feu feu in IntersectionEnModif.LstFeux)
            {
                switch (feu.OrientationControlee)
                {
                    case Orientation.NORD:
                        foreach (VoieGraphique voieGraph in LstVoiesNord)
                        {
                            if (EstSortie(voieGraph.ID) == false)
                            {
                                int heightFeu = voieGraph.Width / 2;
                                LstFeuxVoiesNord.Add(new FeuGraphique(feu, heightFeu, new Thickness(MillieuIntersectionLeft + voieGraph.Margin.Left + voieGraph.Width / 2 + 15, voieGraph.Margin.Top + voieGraph.Height + heightFeu + 4, 0, 0)));
                            }
                        }
                        return;
                }
            }
        }

        private void InitialiserFeuxEst()
        {
            foreach (Feu feu in IntersectionEnModif.LstFeux)
            {
                switch (feu.OrientationControlee)
                {
                    case Orientation.EST:
                        foreach (VoieGraphique voieGraph in LstVoiesEst)
                        {
                            if (EstSortie(voieGraph.ID) == false)
                            {
                                int heightFeu = voieGraph.Height / 2;
                                LstFeuxVoiesEst.Add(new FeuGraphique(feu, heightFeu, new Thickness(MillieuIntersectionLeft + MillieuIntersectionWidth - ( heightFeu + 4),MillieuIntersectionTop + voieGraph.Height/2 + 15, 0, 0)));
                            }
                        }
                        return;
                }
            }
        }

        private void InitialiserFeuxSud()
        {
            foreach (Feu feu in IntersectionEnModif.LstFeux)
            {
                switch (feu.OrientationControlee)
                {
                    case Orientation.SUD:
                        foreach (VoieGraphique voieGraph in LstVoiesSud)
                        {
                            if (EstSortie(voieGraph.ID) == false)
                            {
                                int heightFeu = voieGraph.Width / 2;
                                LstFeuxVoiesSud.Add(new FeuGraphique(feu, heightFeu, new Thickness(MillieuIntersectionLeft  + voieGraph.Margin.Left + voieGraph.Width/4 + 15, MillieuIntersectionTop + MillieuIntersectionHeight - (heightFeu+4), 0, 0)));
                            }
                        }
                        return;
                }
            }
        }

        private void InitialiserFeuxOuest()
        {
            foreach (Feu feu in IntersectionEnModif.LstFeux)
            {
                switch (feu.OrientationControlee)
                {
                    case Orientation.OUEST:
                        foreach (VoieGraphique voieGraph in LstVoiesOuest)
                        {
                            if (EstSortie(voieGraph.ID) == false)
                            {
                                int heightFeu = voieGraph.Height / 2;
                                LstFeuxVoiesOuest.Add(new FeuGraphique(feu, heightFeu, new Thickness(MillieuIntersectionLeft + (heightFeu+4), MillieuIntersectionTop + voieGraph.Margin.Top + voieGraph.Height/4 + 15, 0, 0)));
                            }
                        }
                        return;
                }
            }
        }

        private void ModifyDirectionVoieSelectionnee(List<Direction> nouvellesDirections)
        {
            if(VoieSelectionnee != null)
            {
                UpdateVoieFromVoieGraphique(VoieSelectionnee, nouvellesDirections);
                VoieSelectionnee.LstDirections = nouvellesDirections;
                UpdateSourceDirectionsVoieGraphique(VoieSelectionnee);
            }
        }

        private void UpdateSourceDirectionsVoieGraphique(VoieGraphique vg)
        {
            for (int i = 0; i < IntersectionEnModif.LstChemins.Count; i++)
            {
                for (int j = 0; j < IntersectionEnModif.LstChemins[i].LstRoutes.Count; j++)
                {
                    for (int k = 0; k < IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies.Count; k++)
                    {
                        if (IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].ID == vg.ID)
                        {
                            switch (IntersectionEnModif.LstChemins[i].Emplacement)
                            {
                                case Orientation.NORD:
                                    LstDirectionsVoiesNord[0].Source = DirectionVoieGraphique.GetSourceFromLstDirections(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections);
                                    break;
                                case Orientation.EST:
                                    LstDirectionsVoiesEst[0].Source = DirectionVoieGraphique.GetSourceFromLstDirections(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections);
                                    break;
                                case Orientation.SUD:
                                    LstDirectionsVoiesSud[0].Source = DirectionVoieGraphique.GetSourceFromLstDirections(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections);
                                    break;
                                case Orientation.OUEST:
                                    LstDirectionsVoiesOuest[0].Source = DirectionVoieGraphique.GetSourceFromLstDirections(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void UpdateVoieFromVoieGraphique(VoieGraphique vg,List<Direction> nouvellesDirections)
        {
            for (int i = 0; i < IntersectionEnModif.LstChemins.Count; i++)
            {
                for (int j = 0; j < IntersectionEnModif.LstChemins[i].LstRoutes.Count; j++)
                {
                    for (int k = 0; k < IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies.Count; k++)
                    {
                        if(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].ID == vg.ID)
                        {
                            IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections = nouvellesDirections;
                            switch (IntersectionEnModif.LstChemins[i].Emplacement)
                            {
                                case Orientation.NORD:
                                    for (int l = 0; l < IntersectionEnModif.LstFeux.Count; l++)
                                    {
                                        if(IntersectionEnModif.LstFeux[l].OrientationControlee == Orientation.NORD)
                                        {
                                            IntersectionEnModif.LstFeux[l].DirectionsControlees = IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections;
                                        }
                                    }
                                    break;
                                case Orientation.EST:
                                    for (int l = 0; l < IntersectionEnModif.LstFeux.Count; l++)
                                    {
                                        if (IntersectionEnModif.LstFeux[l].OrientationControlee == Orientation.EST)
                                        {
                                            IntersectionEnModif.LstFeux[l].DirectionsControlees = IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections;
                                        }
                                    }
                                    break;
                                case Orientation.SUD:
                                    for (int l = 0; l < IntersectionEnModif.LstFeux.Count; l++)
                                    {
                                        if (IntersectionEnModif.LstFeux[l].OrientationControlee == Orientation.SUD)
                                        {
                                            IntersectionEnModif.LstFeux[l].DirectionsControlees = IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections;
                                        }
                                    }
                                    break;
                                case Orientation.OUEST:
                                    for (int l = 0; l < IntersectionEnModif.LstFeux.Count; l++)
                                    {
                                        if (IntersectionEnModif.LstFeux[l].OrientationControlee == Orientation.OUEST)
                                        {
                                            IntersectionEnModif.LstFeux[l].DirectionsControlees = IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private Orientation TrouverEmplacementFromVoieGraphique(VoieGraphique v)
        {
            foreach (Chemin c in IntersectionEnModif.LstChemins)
            {
                switch (c.Emplacement)
                {
                    case Orientation.NORD:
                        foreach (VoieGraphique voieGraphique in LstVoiesNord)
                        {
                            if (voieGraphique.ID == v.ID)
                            {
                                return (Orientation)c.Emplacement;
                            }
                        }
                        break;
                    case Orientation.EST:
                        foreach (VoieGraphique voieGraphique in LstVoiesEst)
                        {
                            if (voieGraphique.ID == v.ID)
                            {
                                return (Orientation)c.Emplacement;
                            }
                        }
                        break;
                    case Orientation.SUD:
                        foreach (VoieGraphique voieGraphique in LstVoiesSud)
                        {
                            if (voieGraphique.ID == v.ID)
                            {
                                return (Orientation)c.Emplacement;
                            }
                        }
                        break;
                    case Orientation.OUEST:
                        foreach (VoieGraphique voieGraphique in LstVoiesOuest)
                        {
                            if (voieGraphique.ID == v.ID)
                            {
                                return (Orientation)c.Emplacement;
                            }
                        }
                        break;
                }
            }
            throw new Exception("Voie introuvable dans l'intersection!");
        }

        private List<Direction> TrouverDirectionsPossiblesFromVoie(VoieGraphique voie,Orientation emplacementSource)
        {
            List<Direction> lstDirectionsPermises = new List<Direction>();

            foreach (Chemin c in IntersectionEnModif.LstChemins)
            {
                if (c.Emplacement != emplacementSource)
                {
                    switch (c.Emplacement)
                    {
                        case Orientation.NORD:
                            foreach (VoieGraphique voieGraphique in LstVoiesNord)
                            {
                                if (EstSortie(voieGraphique.ID))
                                {
                                    lstDirectionsPermises.Add(CalculerDirectionFromOrientations(emplacementSource, (Orientation)c.Emplacement));
                                }
                            }
                            break;
                        case Orientation.EST:
                            foreach (VoieGraphique voieGraphique in LstVoiesEst)
                            {
                                if (EstSortie(voieGraphique.ID))
                                {
                                    lstDirectionsPermises.Add(CalculerDirectionFromOrientations(emplacementSource, (Orientation)c.Emplacement));
                                }
                            }
                            break;
                        case Orientation.SUD:
                            foreach (VoieGraphique voieGraphique in LstVoiesSud)
                            {
                                if (EstSortie(voieGraphique.ID))
                                {
                                    lstDirectionsPermises.Add(CalculerDirectionFromOrientations(emplacementSource, (Orientation)c.Emplacement));
                                }
                            }
                            break;
                        case Orientation.OUEST:
                            foreach (VoieGraphique voieGraphique in LstVoiesOuest)
                            {
                                if (EstSortie(voieGraphique.ID))
                                {
                                    lstDirectionsPermises.Add(CalculerDirectionFromOrientations(emplacementSource, (Orientation)c.Emplacement));
                                }
                            }
                            break;
                    }
                }
            }
            return lstDirectionsPermises;
        }

        private Direction CalculerDirectionFromOrientations(Orientation source,Orientation destination)
        {
            switch (source)
            {
                case Orientation.NORD:
                    return CalculerDirectionFromOrientationNord(destination);
                case Orientation.EST:
                    return CalculerDirectionFromOrientationEst(destination);
                case Orientation.SUD:
                    return CalculerDirectionFromOrientationSud(destination);
                case Orientation.OUEST:
                    return CalculerDirectionFromOrientationOuest(destination);
            }
            throw new Exception("Orientation source invalide lors du calcul des directions");
        }

        private Direction CalculerDirectionFromOrientationNord(Orientation destination)
        {
            switch (destination)
            {
                case Orientation.EST:
                    return Direction.GAUCHE;
                case Orientation.SUD:
                    return Direction.TOUTDROIT;
                case Orientation.OUEST:
                    return Direction.DROITE;
            }
            throw new Exception("Direction possible invalide!");
        }

        private Direction CalculerDirectionFromOrientationEst(Orientation destination)
        {
            switch (destination)
            {
                case Orientation.SUD:
                    return Direction.GAUCHE;
                case Orientation.OUEST:
                    return Direction.TOUTDROIT;
                case Orientation.NORD:
                    return Direction.DROITE;
            }
            throw new Exception("Direction possible invalide!");
        }

        private Direction CalculerDirectionFromOrientationSud(Orientation destination)
        {
            switch (destination)
            {
                case Orientation.OUEST:
                    return Direction.GAUCHE;
                case Orientation.NORD:
                    return Direction.TOUTDROIT;
                case Orientation.EST:
                    return Direction.DROITE;
            }
            throw new Exception("Direction possible invalide!");
        }

        private Direction CalculerDirectionFromOrientationOuest(Orientation destination)
        {
            switch (destination)
            {
                case Orientation.NORD:
                    return Direction.GAUCHE;
                case Orientation.EST:
                    return Direction.TOUTDROIT;
                case Orientation.SUD:
                    return Direction.DROITE;
            }
            throw new Exception("Direction possible invalide!");
        }

        private void TrierChemins()
        {
            ObservableCollection<Route> lstRoutesTriee = new ObservableCollection<Route>(); ;
            ObservableCollection<Route> lstRoutesSorties = new ObservableCollection<Route>();
            ObservableCollection<Route> lstRoutesEntrees = new ObservableCollection<Route>();
            if (IntersectionEnModif != null)
            {
                // Chemins
                for (int i = 0; i < IntersectionEnModif.LstChemins.Count; i++)
                {
                        lstRoutesEntrees = new ObservableCollection<Route>();
                        lstRoutesSorties = new ObservableCollection<Route>();
                    // Routes
                    for (int j = 0; j < IntersectionEnModif.LstChemins[i].LstRoutes.Count; j++)
                    {
                        if (RouteCheminEstSortie(IntersectionEnModif.LstChemins[i], IntersectionEnModif.LstChemins[i].LstRoutes[j]))
                        {
                            lstRoutesSorties.Add(IntersectionEnModif.LstChemins[i].LstRoutes[j]);
                        }
                        else
                        {
                            lstRoutesEntrees.Add(IntersectionEnModif.LstChemins[i].LstRoutes[j]);
                        }
                    }
                    // Après avoir parcouru les routes du chemin
                    switch (IntersectionEnModif.LstChemins[i].Emplacement)
                    {
                        case Orientation.NORD:
                            lstRoutesTriee = new ObservableCollection<Route>();
                            foreach (Route route in lstRoutesEntrees.Union(lstRoutesSorties))
                                lstRoutesTriee.Add(route);
                            break;
                        case Orientation.EST:
                            lstRoutesTriee = new ObservableCollection<Route>();
                            foreach (Route route in lstRoutesEntrees.Union(lstRoutesSorties))
                                lstRoutesTriee.Add(route);
                            break;
                        case Orientation.SUD:
                            lstRoutesTriee = new ObservableCollection<Route>();
                            foreach (Route route in lstRoutesSorties.Union(lstRoutesEntrees))
                                lstRoutesTriee.Add(route);
                            break;
                        case Orientation.OUEST:
                            lstRoutesTriee = new ObservableCollection<Route>();
                            foreach (Route route in lstRoutesSorties.Union(lstRoutesEntrees))
                                lstRoutesTriee.Add(route);
                            break;
                    }
                    IntersectionEnModif.LstChemins[i].LstRoutes = lstRoutesTriee;
                }
            }
        }

        private bool RouteCheminEstSortie(Chemin c,Route r)
        {
            return c.Emplacement == r.Orientation;
        }

        private void InitialiserLstsDirections()
        {
            const int heightWidthIMG = 50;
            InitialiserDirectionsVoiesNord(heightWidthIMG);
            InitialiserDirectionsVoiesEst(heightWidthIMG);
            InitialiserDirectionsVoiesSud(heightWidthIMG);
            InitialiserDirectionsVoiesOuest(heightWidthIMG);
        }

        private void InitialiserDirectionsVoiesNord(int heightWidthIMG)
        {
            foreach (VoieGraphique voie in LstVoiesNord)
            {
                if(EstSortie(voie.ID) == false)
                {
                    LstDirectionsVoiesNord.Add(new DirectionVoieGraphique(heightWidthIMG, heightWidthIMG, new Thickness(
                                                                                                        (VoiesNordLeft + voie.Margin.Left + voie.Width) - voie.Width/4 -4,
                                                                                                        voie.Height - heightWidthIMG/2,
                                                                                                        voie.Margin.Right, 
                                                                                                        voie.Margin.Bottom), 
                                                                                                                            DirectionVoieGraphique.GetSourceFromLstDirections(voie.LstDirections),
                                                                                                                            DirectionVoieGraphique.GetListDirectionsFromSource(DirectionVoieGraphique.GetSourceFromLstDirections(voie.LstDirections))));
                }
            }
        }

        private void InitialiserDirectionsVoiesEst(int heightWidthIMG)
        {
            foreach (VoieGraphique voie in LstVoiesEst)
            {
                if (EstSortie(voie.ID) == false)
                {
                    LstDirectionsVoiesEst.Add(new DirectionVoieGraphique(heightWidthIMG, heightWidthIMG, new Thickness(
                                                                                                        (VoiesEstLeft + heightWidthIMG/2),
                                                                                                        (VoiesEstTop + voie.Margin.Top + voie.Height) - voie.Height/4 -4,
                                                                                                        voie.Margin.Right,
                                                                                                        voie.Margin.Bottom),
                                                                                                                            DirectionVoieGraphique.GetSourceFromLstDirections(voie.LstDirections),
                                                                                                                            DirectionVoieGraphique.GetListDirectionsFromSource(DirectionVoieGraphique.GetSourceFromLstDirections(voie.LstDirections))));
                }
            }
        }

        private void InitialiserDirectionsVoiesSud(int heightWidthIMG)
        {
            foreach (VoieGraphique voie in LstVoiesSud)
            {
                if (EstSortie(voie.ID) == false)
                {
                    LstDirectionsVoiesSud.Add(new DirectionVoieGraphique(heightWidthIMG, heightWidthIMG, new Thickness(
                                                                                                        (VoiesSudLeft + voie.Margin.Left + voie.Width/4) + 4,
                                                                                                        (VoiesSudTop +  heightWidthIMG/2),
                                                                                                        voie.Margin.Right,
                                                                                                        voie.Margin.Bottom),
                                                                                                                            DirectionVoieGraphique.GetSourceFromLstDirections(voie.LstDirections),
                                                                                                                            DirectionVoieGraphique.GetListDirectionsFromSource(DirectionVoieGraphique.GetSourceFromLstDirections(voie.LstDirections))));
                }
            }
        }

        private void InitialiserDirectionsVoiesOuest(int heightWidthIMG)
        {
            foreach (VoieGraphique voie in LstVoiesOuest)
            {
                if (EstSortie(voie.ID) == false)
                {
                    LstDirectionsVoiesOuest.Add(new DirectionVoieGraphique(heightWidthIMG, heightWidthIMG, new Thickness(
                                                                                                        (voie.Width - heightWidthIMG/2 ) + 4,
                                                                                                        (VoiesOuestTop + voie.Margin.Top + voie.Height/4),
                                                                                                        voie.Margin.Right,
                                                                                                        voie.Margin.Bottom),
                                                                                                                            DirectionVoieGraphique.GetSourceFromLstDirections(voie.LstDirections),
                                                                                                                            DirectionVoieGraphique.GetListDirectionsFromSource(DirectionVoieGraphique.GetSourceFromLstDirections(voie.LstDirections))));
                }
            }
        }



        private void UpdateVoieSelectionne(VoieGraphique v)
        {
            if(VoieSelectionnee != null)
            {
                RemoveSelectedEffect(VoieSelectionnee);
            }
            VoieSelectionnee = v;
            AddSelectedEffect(VoieSelectionnee);
        }

        private void UpdateFeuSelectionne(FeuGraphique f)
        {
            if (FeuSelectionne != null)
            {
                RemoveSelectedEffect(FeuSelectionne);
            }
            FeuSelectionne = f;
            AddSelectedEffect(f);
        }


        private void BuildListeSortie()
        {
            LstIdVoiesEstSortie = new List<int>();
            List<int> lstIds = new List<int>();
            int compteurVoieParcouruDansChemin;

            foreach (Chemin chemin in IntersectionEnModif.LstChemins)
            {
                switch (chemin.Emplacement)
                {
                    case Orientation.NORD:
                        lstIds = GetIdsFromEmplacement(Orientation.NORD);
                        compteurVoieParcouruDansChemin = 0;
                        foreach (Route route in chemin.LstRoutes)
                        {
                            if (RouteCheminEstSortie(chemin,route))
                            {
                                for (int i=0;i<route.LstVoies.Count;i++)
                                {
                                    LstIdVoiesEstSortie.Add(lstIds[compteurVoieParcouruDansChemin + i]);
                                }
                            }
                            compteurVoieParcouruDansChemin += route.LstVoies.Count;
                        }
                        break;
                    case Orientation.EST:
                        lstIds = GetIdsFromEmplacement(Orientation.EST);
                        compteurVoieParcouruDansChemin = 0;
                        foreach (Route route in chemin.LstRoutes)
                        {
                            if (RouteCheminEstSortie(chemin, route))
                            {
                                for (int i = 0; i < route.LstVoies.Count; i++)
                                {
                                    LstIdVoiesEstSortie.Add(lstIds[compteurVoieParcouruDansChemin + i]);
                                }
                            }
                            compteurVoieParcouruDansChemin += route.LstVoies.Count;
                        }
                        break;
                    case Orientation.SUD:
                        lstIds = GetIdsFromEmplacement(Orientation.SUD);
                        compteurVoieParcouruDansChemin = 0;
                        foreach (Route route in chemin.LstRoutes)
                        {
                            if (RouteCheminEstSortie(chemin, route))
                            {
                                for (int i = 0; i < route.LstVoies.Count; i++)
                                {
                                    LstIdVoiesEstSortie.Add(lstIds[compteurVoieParcouruDansChemin + i]);
                                }
                            }
                            compteurVoieParcouruDansChemin += route.LstVoies.Count;
                        }
                        break;
                    case Orientation.OUEST:
                        lstIds = GetIdsFromEmplacement(Orientation.OUEST);
                        compteurVoieParcouruDansChemin = 0;
                        foreach (Route route in chemin.LstRoutes)
                        {
                            if (RouteCheminEstSortie(chemin, route))
                            {
                                for (int i=0;i<route.LstVoies.Count;i++)
                                {
                                    LstIdVoiesEstSortie.Add(lstIds[compteurVoieParcouruDansChemin + i]);
                                }
                            }
                            compteurVoieParcouruDansChemin += route.LstVoies.Count;
                        }
                        break;
                }
            }
        }

        private List<int> GetIdsFromEmplacement(Orientation emplacement)
        {
            List<int> lstId = new List<int>();

            foreach (VoieGraphique voie in (emplacement == Orientation.NORD ? LstVoiesNord : (emplacement == Orientation.EST ? LstVoiesEst : (emplacement == Orientation.SUD ? LstVoiesSud : LstVoiesOuest))))
            {
                lstId.Add(voie.ID);
            }
            return lstId;
        }

        private void RemoveSelectedEffect(VoieGraphique v)
        {
            v.Stroke = VoieGraphique.DefaultStroke;
            v.StrokeThickness = VoieGraphique.DefaultStrokeThickness;
        }

        private void RemoveSelectedEffect(FeuGraphique f)
        {
            f.Contour = FeuGraphique.DefaultContour;
        }

        private void AddSelectedEffect(VoieGraphique v)
        {
            v.Stroke = VoieGraphique.SelectedStroke;
            v.StrokeThickness = 4;
        }

        private void AddSelectedEffect(FeuGraphique f)
        {
            f.Contour = FeuGraphique.SelectedContour;
        }

        private void AddHoverEffect(VoieGraphique v)
        {
            if(v != VoieSelectionnee)
            {
                v.Stroke = VoieGraphique.HoverStroke;
                v.StrokeThickness = 4;
            }
            v.Fill = VoieGraphique.HoverFill;
            SetCursorToHand();
        }

        private void AddHoverEffect(FeuGraphique f)
        {
            if (f != FeuSelectionne)
            {
                f.Contour = FeuGraphique.HoverContour;
            }
            SetCursorToHand();
        }

        private void RemoveHoverEffect(VoieGraphique v)
        {
            if(v != VoieSelectionnee)
            {
                v.Stroke = VoieGraphique.DefaultStroke;
                v.StrokeThickness = VoieGraphique.DefaultStrokeThickness;
            }
            v.Fill = VoieGraphique.DefaultFill;
            SetCursorToNull();
        }
        private void RemoveHoverEffect(FeuGraphique f)
        {
            if (f != FeuSelectionne)
            {
                f.Contour = FeuGraphique.DefaultContour;
            }
            SetCursorToNull();
        }


        private void PositionnerListes()
        {
            PositionnerNord();
            PositionnerEst();
            PositionnerSud();
            PositionnerOuest();
        }

        private void PositionnerNord()
        {
            VoiesNordLeft = MillieuIntersectionLeft;
            VoiesNordTop  = 0;
        }

        private void PositionnerEst()
        {
            VoiesEstLeft = (IntersectionWidth / 2) + MillieuIntersectionWidth / 2;
            VoiesEstTop  = MillieuIntersectionTop;
        }

        private void PositionnerSud()
        {
            VoiesSudLeft = MillieuIntersectionLeft;
            VoiesSudTop  = (IntersectionHeight / 2) + MillieuIntersectionHeight/2;
        }

        private void PositionnerOuest()
        {
            VoiesOuestLeft = 0;
            VoiesOuestTop = MillieuIntersectionTop;
        }



        private void InitialiserLstsVoies()
        {
            int id = 0;
            for (int i = 0; i < IntersectionEnModif.LstChemins.Count; i++)
            {
                int compteurVoieChemin = 0;
                for (int j = 0; j < IntersectionEnModif.LstChemins[i].LstRoutes.Count; j++)
                {
                    int height = CalculerHeightVoie(IntersectionEnModif.LstChemins[i].Emplacement, IntersectionEnModif.LstChemins[i].CalculerNbVoies());
                    int width = CalculerWidthVoie(IntersectionEnModif.LstChemins[i].Emplacement, IntersectionEnModif.LstChemins[i].CalculerNbVoies());

                    for (int k = 0; k < IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies.Count; k++)
                    {
                        IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k] =  new Voie(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k].LstDirections,id);
                        switch (IntersectionEnModif.LstChemins[i].Emplacement)
                        {
                            case Orientation.NORD:
                                LstVoiesNord.Add(new VoieGraphique(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k], height, width, new Thickness(compteurVoieChemin * width, 0, 0, 0)));
                                break;
                            case Orientation.EST:
                                LstVoiesEst.Add(new VoieGraphique(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k], height, width, new Thickness(0, compteurVoieChemin * height, 0, 0)));
                                break;
                            case Orientation.SUD:
                                LstVoiesSud.Add(new VoieGraphique(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k], height, width, new Thickness(compteurVoieChemin * width, 0, 0, 0)));
                                break;
                            case Orientation.OUEST:
                                LstVoiesOuest.Add(new VoieGraphique(IntersectionEnModif.LstChemins[i].LstRoutes[j].LstVoies[k], height, width, new Thickness(0, compteurVoieChemin * height, 0, 0)));
                                break;
                        }
                        id++;
                        compteurVoieChemin++;
                    }
                }
            }
        }

        private bool EstSortie(int idVoie)
        {
            return LstIdVoiesEstSortie.Contains(idVoie);
        }

        private int CalculerHeightVoie(Orientation? o,int nbVoies)
        {
            int height = 0;

            switch (o)
            {
                case Orientation.NORD:
                    height = (IntersectionHeight - MillieuIntersectionHeight) / 2;
                    break;
                case Orientation.EST:
                    height = MillieuIntersectionHeight / nbVoies;
                    break;
                case Orientation.SUD:
                    height = (IntersectionHeight - MillieuIntersectionHeight) / 2;
                    break;
                case Orientation.OUEST:
                    height = MillieuIntersectionHeight / nbVoies;
                    break;
            }

            return height;
        }
        
        private int CalculerWidthVoie(Orientation? o, int nbVoies)
        {
            int width = 0;

            switch (o)
            {
                case Orientation.NORD:
                    width = MillieuIntersectionWidth / nbVoies;
                    break;
                case Orientation.EST:
                    width = (IntersectionWidth - MillieuIntersectionWidth) / 2;
                    break;
                case Orientation.SUD:
                    width = MillieuIntersectionWidth / nbVoies;
                    break;
                case Orientation.OUEST:
                    width = (IntersectionWidth - MillieuIntersectionWidth) / 2;
                    break;
            }

            return width;
        }

        public ICommand CmdVoieMouseDown
        {
            get
            {
                return new RelayCommand<VoieGraphique>((v) =>
                {
                    if (EstSortie(v.ID) == false)
                    {
                        UpdateVoieSelectionne(v);
                    }
                });
            }
        }

        public ICommand CmdVoieMouseEnter
        {
            get
            {
                return new RelayCommand<VoieGraphique>((v) =>
                {
                    if (EstSortie(v.ID) == false)
                    {
                        AddHoverEffect(v);
                    }
                });
            }
        }

        public ICommand CmdVoieMouseLeave
        {
            get
            {
                return new RelayCommand<VoieGraphique>((v) =>
                {
                    if (EstSortie(v.ID) == false)
                    {
                        RemoveHoverEffect(v);
                    }
                });
            }
        }

        public ICommand CmdFeuMouseDown
        {
            get
            {
                return new RelayCommand<FeuGraphique>((f) =>
                {
                    UpdateFeuSelectionne(f);
                });
            }
        }

        public ICommand CmdFeuMouseEnter
        {
            get
            {
                return new RelayCommand<FeuGraphique>((f) =>
                {
                    AddHoverEffect(f);
                });
            }
        }

        public ICommand CmdFeuMouseLeave
        {
            get
            {
                return new RelayCommand<FeuGraphique>((f) =>
                {
                    RemoveHoverEffect(f);
                });
            }
        }
    }
}
