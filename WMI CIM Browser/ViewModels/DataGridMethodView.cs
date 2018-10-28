using WbemLibrary;

namespace WMI_CIM_Browser.ViewModels {
    public class DataGridMethodView {


        public string Name { get; set; }



        public DataGridMethodView(WbemMethod data) {
            Name = data.Name;

        }
    }
}
