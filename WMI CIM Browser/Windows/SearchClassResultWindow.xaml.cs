using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WMI_CIM_Browser.Controls;

namespace WMI_CIM_Browser.Windows {
    /// <summary>
    /// Interaction logic for SearchClassResult.xaml
    /// </summary>
    public partial class SearchClassResultWindow : Window {

        public SearchClassResultWindow() {
            InitializeComponent();
        }

        public SearchClassResultWindow(IList<WbemTreeViewItem> classes) : this() {
            foreach (WbemTreeViewItem classRepr in classes) {
                ListBoxItem listBoxItem = new ListBoxItem {
                    Content = classRepr.DataContext.Path.ClassName,
                    DataContext = classRepr
                };
                ClassNameList.Items.Add(listBoxItem);
            }
        }
    }
}
