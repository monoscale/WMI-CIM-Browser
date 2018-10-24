using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WbemLibrary;

namespace WMI_CIM_Browser.Controls {
    /// <summary>
    /// Subclass of TreeView which automatically assumes its Items will be WbemTreeViewItem instances. 
    /// </summary>
    public class WbemTreeView : TreeView{

        /// <summary>
        /// Returns the current selected WbemTreeViewItem
        /// </summary>
        public new WbemTreeViewItem SelectedItem {
            get {
                return (WbemTreeViewItem) base.SelectedItem;
            }
        }



        public void PopulateTreeView(IList<WbemObject> objects) {

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
                    this.Items.Add(wbemTreeViewItem);
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

        
    }
}
