using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace TraffiGo.Vues
{
    public static class FocusHelper
    {
        public static void Focus(UIElement element)
        {
            if (!element.Focus())
            {
                element.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(delegate ()
                {
                    element.Focus();
                }));
            }
        }
    }
}
