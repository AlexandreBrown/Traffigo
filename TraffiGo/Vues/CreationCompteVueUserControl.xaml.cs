using System.Windows;
using System.Windows.Controls;
using TraffiGo.Vues;
using TraffiGo.VueModeles;

namespace TraffiGo.Vues
{
    /// <summary>
    /// Interaction logic for CreationCompteVueUserControl.xaml
    /// </summary>
    public partial class CreationCompteVueUserControl : UserControl
    {
        public CreationCompteVueUserControl()
        {
            InitializeComponent();
            FocusHelper.Focus(txbPrenom);
        }
    }
}
