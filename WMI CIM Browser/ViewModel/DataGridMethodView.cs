using WbemLibrary;

namespace WMI_CIM_Browser.ViewModel {
    public class DataGridMethodView {


        public string Name { get; set; }



        public DataGridMethodView(WbemMethod data) {
            Name = data.Name;

        }
    }
}
