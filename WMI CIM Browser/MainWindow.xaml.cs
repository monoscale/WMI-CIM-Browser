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


        #region EVENTS
        /// <summary>
        /// Get the classes from a namespace
        /// </summary>
        private void TextBoxNamespace_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key != System.Windows.Input.Key.Enter) { return; }
            PopulateClassNavigator(TextBoxNamespace.Text);
        }

        /// <summary>
        /// Give extra detail for the selected WMI Class
        /// </summary>
        private void ClassNavigator_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            if (ClassNavigator.SelectedItem == null) {
                //it just means the user went out of focus
                return;
            }
            PopulateClassDetails(ClassNavigator.SelectedItem);
        }


        /// <summary>
        /// Starts searching for classes when enter button is pressed. 
        /// Once the search is done, it will open a new SearchClassResultWindow Window with the results.
        /// </summary>
        private void TextBoxClass_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key != System.Windows.Input.Key.Enter) { return; }
            IList<WbemTreeViewItem> classes = ClassNavigator.Search(TextBoxClass.Text);
            searchClassResultWindow = new SearchClassResultWindow(classes);
            searchClassResultWindow.ClassNameSelected += SearchClassResultWindow_ClassNameSelected;
            searchClassResultWindow.Show();
        }

        /// <summary>
        /// When double clicking in the list of classnames from the search operation (in the SearchClassResultWindow),
        /// we retrieve the classname and load the details. The SearchClassResultWindow will also be closed.
        /// </summary>
        private void SearchClassResultWindow_ClassNameSelected(object sender, WbemTreeViewItem wbemTreeViewItem) {
            PopulateClassDetails(wbemTreeViewItem);
            wbemTreeViewItem.IsSelected = true;
            searchClassResultWindow.Close();
            searchClassResultWindow = null;
        }

        /// <summary>
        /// When right clicking on the current classname, give a menu to interact with that class.
        /// </summary>
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

        private void PropertyList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            // individual attribute qualifiers

        }

        /// <summary>
        /// Shows the instances of a class. This can only be called when a current class is selected.
        /// We also know which class this (The selected item in ClassNavigator).
        /// </summary>
        private void ShowClassInstances(object sender, RoutedEventArgs e) {
            WbemObject mObject = ClassNavigator.SelectedItem.DataContext;
            IList<WbemObject> instances = service.InstancesOf(mObject.Path.ClassName);
            ExtraDetails.ItemsSource = instances;
        }

        /// <summary>
        /// Shows the class qualifiers of a class. This can only be called when a current class is selected.
        /// We also know which class this (The selected item in ClassNavigator).
        /// </summary>
        private void ShowClassQualifiers(object sender, RoutedEventArgs e) {
            WbemObject mObject = ClassNavigator.SelectedItem.DataContext;
            IList<DataGridClassQualifierView> qualifiers = mObject.Qualifiers.Values.Select(s => new DataGridClassQualifierView(s)).ToList();
            ExtraDetails.ItemsSource = qualifiers;
        }
        #endregion


        #region HELPER METHODS

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
        /// Populates the properties and methods from a class that is represented by a wbemTreeViewItem.
        /// </summary>
        /// <param name="wbemTreeViewItem">The WbemTreeViewItem that represents the class</param>
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
        #endregion


    }
}