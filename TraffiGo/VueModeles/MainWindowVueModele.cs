using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;
using TraffiGo.EditionIntersection.VueModeles;
using TraffiGo.Modeles.Messages.ChangementDialog;
using TraffiGo.Modeles.Messages.ChangementVue;
using TraffiGo.Vues;
using TraffiGo.Vues.EditionIntersection;

namespace TraffiGo.VueModeles
{
    public class MainWindowVueModele : VueModeleBase
    {
        public UserControl VueActuelle { get; set; }
        public UserControl Dialog { get; set; }
        public UserControl DialogOverDialog { get; set; }
        public bool VueIsEnabled { get; set; }
        public bool DialogIsEnabled { get; set; }
        public bool DialogEnCours { get; set; }
        public bool DialogOverDialogFlag { get; set; }
        public MainWindowVueModele()
        {
            VueIsEnabled = true;
            DialogIsEnabled = false;
            DialogEnCours = false;
            DialogOverDialogFlag = false;
            ChangerVue(new AccueilVueModele());
            Messenger.Default.Register<MessageChangerVue>(this, (messageChangerVue) =>
            {
                ChangerVue(messageChangerVue.vueModele);
            });
            Messenger.Default.Register<MessageChangerDialog>(this, (messageChangerDialog) =>
            {
                Dialog = messageChangerDialog.Dialog;
                if(VueIsEnabled)
                {
                    VueIsEnabled = false;
                    DialogIsEnabled = true;
                    DialogEnCours = DialogIsEnabled;
                }
                else
                {
                    VueIsEnabled = true;
                    DialogIsEnabled = false;
                    DialogEnCours = DialogIsEnabled;
                }
            });
            Messenger.Default.Register<MessageChangerDialogOverDialog>(this, (messageChangerDialogOverDialog) =>
            {
                DialogOverDialog = messageChangerDialogOverDialog.Dialog;
                if(DialogOverDialogFlag == false)
                {
                    DialogOverDialogFlag = true;
                }
                else
                {
                    DialogOverDialogFlag = false;
                }

            // Si la vue est actif et qu'on veux afficher un dialog ZIndex 1 et qu'il n'y avait pas déjà un dialog
            if (VueIsEnabled && DialogOverDialogFlag && DialogEnCours == false)
            {
                // On désactive la vue
                VueIsEnabled = false;
            }
            // Si il y a présentement un dialog ZIndex 0 et qu'on veux afficher un dialog ZIndex 1
            else if (DialogEnCours && DialogOverDialogFlag)
            {
                // On désactive le dialog ZIndex 0
                DialogIsEnabled = false;
            }
            // Si on veux fermer le dialog ZIndex 1 et qu'il y avait un dialog en cours
            else if (DialogOverDialogFlag == false && DialogEnCours)
            {
                DialogIsEnabled = true;
            }
            // Si on veux fermer le dialog ZIndex 1 et qu'il n'y avait pas de dialog en cours
            else if(DialogOverDialogFlag == false && DialogEnCours == false)
            {
                VueIsEnabled = true;
            }
            });
        }

        public void ChangerVue(VueModeleBase vueModele)
        {
            if(vueModele is AccueilVueModele)
            {
                VueActuelle = new AccueilVueUserControl();
            }
            else if(vueModele is CreationCompteVueModele)
            {
                VueActuelle = new CreationCompteVueUserControl();
            }
            else if (vueModele is ConnexionVueModele)
            {
                VueActuelle = new ConnexionVueUserControl();
            }
            else if (vueModele is MenuPrincipalVueModele)
            {
                VueActuelle = new MenuPrincipalVueUserControl();
            }
            else if(vueModele is DocumentationVueModele)
            {
                VueActuelle = new DocumentationVueUserControl();
            }
            else if(vueModele is EditionSimulationVueModele)
            {
                VueActuelle = new EditionSimulationVueUserControl();
            }
            else if(vueModele is SimulationExistanteVueModele)
            {
                VueActuelle = new SimulationExistanteVueUserControl();
            }
            else if (vueModele is EditionIntersectionVueModele)
            {
                VueActuelle = new EditionIntersectionVueUserControl();
            }
            VueActuelle.DataContext = vueModele;
        }
    }
}
