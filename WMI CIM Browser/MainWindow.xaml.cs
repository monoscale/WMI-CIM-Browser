using System.Collections.Generic;
using System.Management;
using System.Windows;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Data;
using WMI_CIM_Browser.ViewModel;
using SWbemLibrary;

namespace WMI_CIM_Browser {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        SWbemLocator locator;
        SWbemService service;

        public void Initialize() {
            locator = new SWbemLocator();
            WindowState = WindowState.Maximized;
        }

        public MainWindow() {
            InitializeComponent();
            Initialize();
            GetClassesForNamespace("root");
        }

        private SWbemObject GetSWbemObjectForTreeViewItem(TreeViewItem treeViewItem) {
            return (SWbemObject)treeViewItem.DataContext;
        }


        /// <summary>
        /// Get the classes from a namespace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxNamespace_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key != System.Windows.Input.Key.Enter) {
                return; // if key is not ENTER
            }
            GetClassesForNamespace(TextBoxNamespace.Text);
        }

        private void GetClassesForNamespace(string nameSpace) {

            ClassNavigator.Items.Clear();
            try {
                service = locator.ConnectServer(".", nameSpace);
                IList<SWbemObject> objects = service.ExecQuery("select * from meta_class");

                // dictionary bijhouden waarbij de key een ManagementObject is, en de daarbij horende TreeViewItem
                Dictionary<SWbemObject, TreeViewItem> subClassesForClass = new Dictionary<SWbemObject, TreeViewItem>();
                foreach (SWbemObject mObject in objects) {
                    string superclass = (string)mObject.SystemProperties[SystemProperties.__SUPERCLASS].Value;
                    if (superclass == null) { // current mObject is a root class
                        TreeViewItem treeViewItem = new TreeViewItem {
                            Header = mObject.Path.ClassName,
                            DataContext = mObject

                        };
                        subClassesForClass.Add(mObject, treeViewItem);
                        ClassNavigator.Items.Add(treeViewItem);
                    } else {
                        // find the direct superclass of the current managementobject
                        foreach (SWbemObject innermObject in subClassesForClass.Keys.ToList()) {
                            if ((string)mObject.SystemProperties[SystemProperties.__SUPERCLASS].Value == innermObject.Path.ClassName) {
                                // we found the superclass, add mObject to the treeviewitem of innermObject
                                TreeViewItem innerTreeViewItem = new TreeViewItem {
                                    Header = mObject.Path.ClassName,
                                    DataContext = mObject
                                };
                                TreeViewItem outerTreeViewItem = subClassesForClass[innermObject];
                                outerTreeViewItem.Items.Add(innerTreeViewItem);
                                subClassesForClass.Add(mObject, innerTreeViewItem); // The current object can also have children
                            }
                        }
                    }
                }

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

            TreeViewItem treeViewItem = (TreeViewItem)ClassNavigator.SelectedItem;
            SWbemObject mObject = (SWbemObject)treeViewItem.DataContext;

            TextBlockCurrentClass.Text = mObject.Path.ClassName;
            TextBlockCurrentClass.DataContext = mObject;

            IList<DataGridPropertyView> properties = (from SWbemProperty property
                                                      in mObject.GetAllProperties().Values
                                                      select property).ToList().Select(s => new DataGridPropertyView(s)).ToList();
            IList<SWbemMethod> methods = (from SWbemMethod method
                                          in mObject.Methods.Values
                                          select method).ToList();

            PropertyList.ItemsSource = properties;
            MethodList.ItemsSource = methods;

        }

        private void TextBlockCurrentClass_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            SWbemObject mObject = GetSWbemObjectForTreeViewItem((TreeViewItem)ClassNavigator.SelectedItem);

            IList<DataGridClassQualifierView> qualifiers = (from SWbemQualifier property
                                                            in mObject.Qualifiers.Values
                                                            select property).ToList().Select(s => new DataGridClassQualifierView(s)).ToList();
            ExtraDetails.ItemsSource = qualifiers;
        }

        private void TextBoxClass_TextChanged(object sender, TextChangedEventArgs e) {

        }
    }
}