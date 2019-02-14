using System.Windows.Controls;
using TraffiGo.Vues;
using TraffiGo.VueModeles;

namespace TraffiGo.Vues
{
    /// <summary>
    /// Logique d'interaction pour ConnexionVueUserControl.xaml
    /// </summary>
    public partial class ConnexionVueUserControl : UserControl
    {
        public ConnexionVueUserControl()
        {
            InitializeComponent();
            FocusHelper.Focus(txbUsername);
        }
    }
}
