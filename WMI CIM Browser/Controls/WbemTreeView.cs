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
        
        /// <summary>
        /// Populates a logical treeview, where each WbemObject represents a node, which can have other WbemObjects as children.
        /// </summary>
        /// <param name="objects">List of Wbemobjects</param>
        public void PopulateTreeView(IEnumerable<WbemObject> objects) {
            Items.Clear();

            Dictionary<string, WbemTreeViewItem> wbemTreeViewItemForClassName = new Dictionary<string, WbemTreeViewItem>();
            foreach (WbemObject wbemObject in objects) {
                WbemTreeViewItem newWbemTreeViewItem = new WbemTreeViewItem {
                    Header = wbemObject.Path.ClassName,
                    DataContext = wbemObject
                };

                string superclass = (string) wbemObject.SystemProperties[SystemProperties.__SUPERCLASS].Value;
                string classname = (string)wbemObject.SystemProperties[SystemProperties.__CLASS].Value;

                // current wbemObject is a root node.
                if (superclass == null) {
                    wbemTreeViewItemForClassName[classname] = newWbemTreeViewItem;
                    Items.Add(newWbemTreeViewItem);
                }
                // current wbemObject is a child node, the parent node already exists in memory
                else {
                    WbemTreeViewItem parent = wbemTreeViewItemForClassName[superclass];
                    parent.Items.Add(newWbemTreeViewItem);
                    wbemTreeViewItemForClassName[classname] = newWbemTreeViewItem;
                }
            }
            
          
        }

        /// <summary>
        /// Searches the treeview for classes that matches the given string.
        /// </summary>
        /// <param name="pattern">The string to match.</param>
        /// <returns>A list containing all the WbemTreeViewInstances that represent a class that matches the pattern.</returns>
        public IEnumerable<WbemTreeViewItem> Search(string pattern) {

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
