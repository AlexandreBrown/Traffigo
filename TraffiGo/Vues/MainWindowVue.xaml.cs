using System.Windows;
using TraffiGo.VueModeles;

namespace TraffiGo.Vues
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindowVue : Window
   {
      public MainWindowVue()
      {
         InitializeComponent();
         DataContext = new MainWindowVueModele();
      }
   }
}
