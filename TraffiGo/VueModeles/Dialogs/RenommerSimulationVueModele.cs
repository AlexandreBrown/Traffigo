using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TraffiGo.Modeles.ClasseSql;
using TraffiGo.Vues;

namespace TraffiGo.VueModeles.Dialogs
{
    public class RenommerSimulationVueModele : VueModeleBase
    {
        public string NouveauNomSimulation { get; set; }
        public string AncienNomSimulation { get; set; }
        public string SrcImgDispoNomSimulation { get; set; }
        private const string BaseUrl = PackUrl + "/Resources/Images/SauvegarderSimulation/";
        private bool NomSimulationValide { get; set; }
        public Action<string> onClickAction { get; set; }
        public int ZIndex { get; set; }

        public RenommerSimulationVueModele()
        {
            NomSimulationValide = false;
        }

        public ICommand CmdRenommerSimulation
        {
            get
            {
                return new RelayCommand(()=>
                {
                    NomSimulationValide = false;
                    onClickAction?.Invoke(NouveauNomSimulation);
                    DialogManager.Close(ZIndex);
                }, () => { return NomSimulationValide; });
            }
        }

        public ICommand CmdVerifierSiNomSimValide
        {
            get
            {
                return new RelayCommand(()=>
                {
                    if(string.IsNullOrWhiteSpace(NouveauNomSimulation) == false)
                    {
                        if(MySqlSimulations.VerifNomSimulation(NouveauNomSimulation, Utilisateur.NomUtilisateur, MySqlSimulations.EstPublique(NouveauNomSimulation,Utilisateur.NomUtilisateur)).Count == 0)
                        {
                            NomSimulationValide = true;
                            SrcImgDispoNomSimulation = BaseUrl + "Available.png";
                        }
                        else
                        {
                            NomSimulationValide = false;
                            SrcImgDispoNomSimulation = BaseUrl + "NotAvailable.png";
                        }
                    }
                    else
                    {
                        SrcImgDispoNomSimulation = "";
                        NomSimulationValide = false;
                    }
                });
            }
        }

        public ICommand CmdFermerDialog
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DialogManager.Close(ZIndex);
                });
            }
        }
    }
}
