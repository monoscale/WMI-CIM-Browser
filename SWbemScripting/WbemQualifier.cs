using System.Management;

namespace WbemLibrary {
    public class WbemQualifier {
        public string Name { get; private set; }
        public string CimType { get; private set; }
        public string Value { get; private set; }
        public bool PropagatesToInstance { get; private set; }
        public bool PropagatesToSubclass { get; private set; }
        public bool IsOverridable { get; private set; }
        public bool IsAmended { get; private set; }
        public bool IsLocal { get; private set; }

        public WbemQualifier(QualifierData data) {
            Name = data.Name;
            // CimType = (data.Value.GetType().ToString().Split('.'))[1];
            Value = data.Value.ToString();
            PropagatesToInstance = data.PropagatesToInstance;
            PropagatesToSubclass = data.PropagatesToSubclass;
            IsOverridable = data.IsOverridable;
            IsAmended = data.IsAmended;
            IsLocal = data.IsLocal;
        }
    }
}
