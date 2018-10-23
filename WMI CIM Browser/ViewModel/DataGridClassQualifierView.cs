using WbemLibrary;

namespace WMI_CIM_Browser.ViewModel {
    public class DataGridClassQualifierView {
        public string Name { get; private set; }
        public string CimType { get; private set; }
        public string Value { get; private set; }
        public bool PropagatesToInstance { get; private set; }
        public bool PropagatesToSubclass { get; private set; }
        public bool IsOverrideable { get; private set; }
        public bool IsAmended { get; private set; }
        public bool IsLocal { get; private set; }

        public DataGridClassQualifierView(WbemQualifier qualifier) {
            Name = qualifier.Name; 
            CimType = (qualifier.Value.GetType().ToString().Split('.'))[1];
            Value = qualifier.Value.ToString();
            PropagatesToInstance = qualifier.PropagatesToInstance;
            PropagatesToSubclass = qualifier.PropagatesToSubclass;
            IsOverrideable = qualifier.IsOverridable;
            IsAmended = qualifier.IsAmended;
            IsLocal = qualifier.IsLocal;
        }
    }
}
