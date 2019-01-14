using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WMI_CIM_Browser.Controls;

namespace WMI_CIM_Browser.Windows {
    /// <summary>
    /// Shows the list of found classes and fires an event when a class is clicked.
    /// </summary>
    public partial class SearchClassResultWindow : Window {


        public event EventHandler<WbemTreeViewItem> ClassNameSelected;

        public SearchClassResultWindow() {
            InitializeComponent();
            Title = "Search results";
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

        private void ClassNameList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListBox listBox = (ListBox)sender;
            ListBoxItem listBoxItem = (ListBoxItem)listBox.SelectedItem;
            WbemTreeViewItem item = (WbemTreeViewItem)listBoxItem.DataContext;
            ClassNameSelected(ClassNameList, item);
            
        }
    }
}
