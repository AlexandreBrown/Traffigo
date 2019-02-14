using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TraffiGo.Vues;
using TraffiGo.VueModeles;

namespace TraffiGo.Vues
{
    /// <summary>
    /// Interaction logic for DocumentationVueUserControl.xaml
    /// </summary>
    public partial class DocumentationVueUserControl : UserControl
    {
        
        public DocumentationVueUserControl()
        {
            InitializeComponent();
            FocusHelper.Focus(txbSearchBar);
        }
    }
}
