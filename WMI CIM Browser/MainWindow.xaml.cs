using System.Collections.Generic;
using System.Management;
using System.Windows;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Data;
using WbemLibrary;
using WMI_CIM_Browser.Controls;
using WMI_CIM_Browser.ViewModels;
using WMI_CIM_Browser.Windows;

namespace WMI_CIM_Browser {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private WbemLocator locator;
        private WbemService service;

        private SearchClassResultWindow searchClassResultWindow;

        public MainWindow() {
            InitializeComponent();

            locator = new WbemLocator();
            WindowState = WindowState.Maximized;

            PopulateClassNavigator("root");
        }

        /// <summary>
        /// Get the classes from a namespace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxNamespace_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key != System.Windows.Input.Key.Enter) { return; }
            PopulateClassNavigator(TextBoxNamespace.Text);
        }

        /// <summary>
        /// This method populates the ClassNavigator treeview with all the WMI classes belonging to the specified namespace.
        /// </summary>
        /// <param name="nameSpace">The namespace to retrieve the classes from.</param>
        private void PopulateClassNavigator(string nameSpace) {
            try {
                service = locator.ConnectServer(".", nameSpace);
                IList<WbemObject> objects = service.GetAllObjects();
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
            PopulateClassDetails(ClassNavigator.SelectedItem);

        }

        private void PopulateClassDetails(WbemTreeViewItem wbemTreeViewItem) {

            PropertyList.ItemsSource = null;

            WbemObject mObject = wbemTreeViewItem.DataContext;

            TextBlockCurrentClass.Text = mObject.Path.ClassName;
            TextBlockCurrentClass.DataContext = mObject;

            IList<DataGridPropertyView> properties = mObject.GetAllProperties().Values.Select(s => new DataGridPropertyView(s)).ToList();
            IList<DataGridMethodView> methods = mObject.Methods.Values.Select(s => new DataGridMethodView(s)).ToList();

            PropertyList.ItemsSource = properties;
            MethodList.ItemsSource = methods;
        }

        private void TextBoxClass_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key != System.Windows.Input.Key.Enter) { return; }
            IList<WbemTreeViewItem> classes = ClassNavigator.Search(TextBoxClass.Text);
            searchClassResultWindow = new SearchClassResultWindow(classes);
            searchClassResultWindow.ClassNameList.MouseDoubleClick += ClassNameList_MouseDoubleClick;
            searchClassResultWindow.Show();
        }

        private void ClassNameList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ListBox listBox = (ListBox)sender;
            ListBoxItem listBoxItem = (ListBoxItem)listBox.SelectedItem;
            WbemTreeViewItem item = (WbemTreeViewItem)listBoxItem.DataContext;
            PopulateClassDetails(item);
            searchClassResultWindow.Close();
            searchClassResultWindow = null;


        }

        private void PropertyList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            // individual attribute qualifiers

        }

        private void TextBlockCurrentClass_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem showQualifiers = new MenuItem();
            showQualifiers.Header = "Show Class Qualifiers";
            showQualifiers.Click += ShowClassQualifiers;
            contextMenu.Items.Add(showQualifiers);

            MenuItem showInstances = new MenuItem();
            showInstances.Header = "Show Class Instances";
            showInstances.Click += ShowClassInstances;
            contextMenu.Items.Add(showInstances);

            contextMenu.IsOpen = true;
        }

        private void ShowClassInstances(object sender, RoutedEventArgs e) {
            WbemObject mObject = ClassNavigator.SelectedItem.DataContext;
            IList<WbemObject> instances = service.InstancesOf(mObject.Path.ClassName);
            ExtraDetails.ItemsSource = instances;
        }

        private void ShowClassQualifiers(object sender, RoutedEventArgs e) {
            WbemObject mObject = ClassNavigator.SelectedItem.DataContext;
            IList<DataGridClassQualifierView> qualifiers = mObject.Qualifiers.Values.Select(s => new DataGridClassQualifierView(s)).ToList();
            ExtraDetails.ItemsSource = qualifiers;
        }


    }
}