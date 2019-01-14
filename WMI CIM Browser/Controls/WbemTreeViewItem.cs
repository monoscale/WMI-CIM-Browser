using System.Windows.Controls;
using WbemLibrary;

namespace WMI_CIM_Browser.Controls {

    /// <summary>
    /// Subclass of TreeViewItem which automatically assumes its DataContext will be an instance of WbemObject.
    /// </summary>
    public class WbemTreeViewItem : TreeViewItem {


        /// <summary>
        /// Gets or sets the DataContext. 
        /// The DataContext attribute can only hold values of type WbemObject.
        /// </summary>
        public new WbemObject DataContext {
            get => (WbemObject)base.DataContext;
            set => base.DataContext = value;
        }
    }
}
