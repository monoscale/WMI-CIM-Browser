using System.Collections.Generic;
using System.Management;
using System.Windows;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Data;
using WMI_CIM_Browser.ViewModel;
using WbemLibrary;
using WMI_CIM_Browser.Controls;

namespace WMI_CIM_Browser {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        WbemLocator locator;
        WbemService service;

        public MainWindow() {
            InitializeComponent();

            locator = new WbemLocator();
            WindowState = WindowState.Maximized;

            PopulateClassNavigator("root");
        }

        private WbemObject GetSWbemObjectForwbemTreeViewItem(WbemTreeViewItem wbemTreeViewItem) {
            return wbemTreeViewItem.DataContext;
        }


        /// <summary>
        /// Get the classes from a namespace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxNamespace_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key != System.Windows.Input.Key.Enter) {
                return;
            }

            PopulateClassNavigator(TextBoxNamespace.Text);
        }

        /// <summary>
        /// This method populates the ClassNavigator treeview with all the WMI classes belonging to the specified namespace.
        /// </summary>
        /// <param name="nameSpace">The namespace to retrieve the classes from.</param>
        private void PopulateClassNavigator(string nameSpace) {

            ClassNavigator.Items.Clear();
            try {
                service = locator.ConnectServer(".", nameSpace);
                IList<WbemObject> objects = service.ExecQuery("select * from meta_class");
                ClassNavigator.PopulateTreeView(objects);
            } catch (ManagementException me) {
                TextBlockErrors.Text = me.Message;
            } catch (ArgumentException ae) {
                TextBlockErrors.Text = ae.Message;
            }
        }


        /// <summary>
        /// Give extra detail for the selected WMI Class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassNavigator_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            if (ClassNavigator.SelectedItem == null) {
                //it just means the user went out of focus
                return;
            }

            PropertyList.ItemsSource = null;

            WbemTreeViewItem wbemTreeViewItem = ClassNavigator.SelectedItem;
            WbemObject mObject = wbemTreeViewItem.DataContext;

            TextBlockCurrentClass.Text = mObject.Path.ClassName;
            TextBlockCurrentClass.DataContext = mObject;

            IList<DataGridPropertyView> properties = (from WbemProperty property
                                                      in mObject.GetAllProperties().Values
                                                      select property).ToList().Select(s => new DataGridPropertyView(s)).ToList();
            IList<WbemMethod> methods = (from WbemMethod method
                                          in mObject.Methods.Values
                                         select method).ToList();

            PropertyList.ItemsSource = properties;
            MethodList.ItemsSource = methods;
  
        }


        /// <summary>
        /// When clicking 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlockCurrentClass_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            WbemObject mObject = GetSWbemObjectForwbemTreeViewItem(ClassNavigator.SelectedItem);

            IList<DataGridClassQualifierView> qualifiers = (from WbemQualifier property
                                                            in mObject.Qualifiers.Values
                                                            select property).ToList().Select(s => new DataGridClassQualifierView(s)).ToList();
            ExtraDetails.ItemsSource = qualifiers;
        }

        private void TextBoxClass_TextChanged(object sender, TextChangedEventArgs e) {

        }

        private void PropertyList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
           

        }
    }
}