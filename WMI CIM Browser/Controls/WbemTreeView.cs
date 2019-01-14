using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WbemLibrary;

namespace WMI_CIM_Browser.Controls {
    /// <summary>
    /// Subclass of TreeView which automatically assumes its Items will be WbemTreeViewItem instances. 
    /// </summary>
    public class WbemTreeView : TreeView {

        /// <summary>
        /// Returns the current selected WbemTreeViewItem
        /// </summary>
        public new WbemTreeViewItem SelectedItem => (WbemTreeViewItem)base.SelectedItem;

        public void PopulateTreeView(IList<WbemObject> objects) {



            Items.Clear();

            // Dictionary with the key a WbemObject en the value the corresponding wbemTreeViewItem for this wbemobject
            Dictionary<WbemObject, WbemTreeViewItem> subClassesForClass = new Dictionary<WbemObject, WbemTreeViewItem>();
            foreach (WbemObject mObject in objects) {
                string superclass = (string)mObject.SystemProperties[SystemProperties.__SUPERCLASS].Value;
                if (superclass == null) { // current mObject is a root class
                    WbemTreeViewItem wbemTreeViewItem = new WbemTreeViewItem {
                        Header = mObject.Path.ClassName,
                        DataContext = mObject

                    };
                    subClassesForClass.Add(mObject, wbemTreeViewItem);
                    Items.Add(wbemTreeViewItem);
                } else {
                    // find the direct superclass of the current managementobject
                    foreach (WbemObject innermObject in subClassesForClass.Keys.ToList()) {
                        if ((string)mObject.SystemProperties[SystemProperties.__SUPERCLASS].Value == innermObject.Path.ClassName) {
                            // we found the superclass, add mObject to the wbemTreeViewItem of innermObject
                            WbemTreeViewItem innerwbemTreeViewItem = new WbemTreeViewItem {
                                Header = mObject.Path.ClassName,
                                DataContext = mObject
                            };
                            WbemTreeViewItem outerwbemTreeViewItem = subClassesForClass[innermObject];
                            outerwbemTreeViewItem.Items.Add(innerwbemTreeViewItem);
                            subClassesForClass.Add(mObject, innerwbemTreeViewItem); // The current object can also have children
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Searches the treeview for classes that matches the given string.
        /// </summary>
        /// <param name="pattern">The string to match.</param>
        /// <returns>A list containing all the WbemTreeViewInstances that represent a class that matches the pattern.</returns>
        public IList<WbemTreeViewItem> Search(string pattern) {

            // The order of traversal does not matter since we have to scan every node anyway

            List<WbemTreeViewItem> resultClasses = new List<WbemTreeViewItem>();
            Stack<WbemTreeViewItem> treeViewItemStack = new Stack<WbemTreeViewItem>();
            foreach (WbemTreeViewItem t in Items) { // initialize
                treeViewItemStack.Push(t);
            }

            while (treeViewItemStack.Count != 0) {
                WbemTreeViewItem it = treeViewItemStack.Peek();
                treeViewItemStack.Pop();
                foreach (WbemTreeViewItem inner in it.Items) {
                    treeViewItemStack.Push(inner);
                }

                WbemObject wbemObject = it.DataContext;
                if (Regex.IsMatch(wbemObject.Path.ClassName.ToLower(), pattern.ToLower())) {
                    resultClasses.Add(it);
                }
            }

            resultClasses.Sort((x, y) => string.Compare(x.DataContext.Path.ClassName, y.DataContext.Path.ClassName, StringComparison.InvariantCulture));
            return resultClasses;
        }

        public void ExpandTo(WbemTreeViewItem wbemTreeViewItem) {
            DependencyObject parent = wbemTreeViewItem.Parent;
            while (parent is WbemTreeViewItem item) {
                item.IsExpanded = true;
                parent = item.Parent;
            }
        }
    }
}
