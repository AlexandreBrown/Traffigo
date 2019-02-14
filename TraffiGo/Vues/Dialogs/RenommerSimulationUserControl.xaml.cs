using System.Windows.Controls;
using TraffiGo.VueModeles.Dialogs;

namespace TraffiGo.Vues.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour RenommerSimulationUserControl.xaml
    /// </summary>
    public partial class RenommerSimulationUserControl : UserControl
    {
        public RenommerSimulationUserControl()
        {
            InitializeComponent();
            DataContext = new RenommerSimulationVueModele();
            FocusHelper.Focus(btnCancel);
        }
    }
}
