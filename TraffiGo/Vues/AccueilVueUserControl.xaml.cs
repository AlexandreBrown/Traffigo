using System.Windows.Controls;
using TraffiGo.Vues;
using TraffiGo.VueModeles;

namespace TraffiGo.Vues
{
    /// <summary>
    /// Logique d'interaction pour AccueilVueUserControl.xaml
    /// </summary>
    public partial class AccueilVueUserControl : UserControl
   {
      public AccueilVueUserControl()
      {
         InitializeComponent();
         FocusHelper.Focus(btnCreerCompte);
      }
   }
}
