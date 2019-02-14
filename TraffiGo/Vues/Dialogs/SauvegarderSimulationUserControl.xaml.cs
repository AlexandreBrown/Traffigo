using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TraffiGo.VueModeles.Dialogs;

namespace TraffiGo.Vues.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour SauvegarderSimulationUserControl.xaml
    /// </summary>
    public partial class SauvegarderSimulationUserControl : UserControl
    {
        public SauvegarderSimulationUserControl()
        {
            InitializeComponent();
            DataContext = new SauvegarderSimulationVueModele();
            FocusHelper.Focus(btnCancel);
        }

        private void Prive_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((SauvegarderSimulationVueModele)DataContext).SetCursorToHand();
        }

        private void Prive_MouseLeave(object sender, MouseEventArgs e)
        {
            ((SauvegarderSimulationVueModele)DataContext).SetCursorToNull();
        }

        private void Publique_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((SauvegarderSimulationVueModele)DataContext).SetCursorToHand();
        }

        private void Publique_MouseLeave(object sender, MouseEventArgs e)
        {
            ((SauvegarderSimulationVueModele)DataContext).SetCursorToNull();
        }

        private void NomSimulation_KeyUp(object sender, KeyEventArgs e)
        {
            ((SauvegarderSimulationVueModele)DataContext).VerifierSiNomValide();
        }

        private void NomSimulation_KeyDown(object sender, KeyEventArgs e)
        {
            ((SauvegarderSimulationVueModele)DataContext).VerifierSiNomValide();
        }


        private void Prive_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((SauvegarderSimulationVueModele)DataContext).VerifierSiNomValide();
        }

        private void Publique_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((SauvegarderSimulationVueModele)DataContext).VerifierSiNomValide();
        }

    }
}
