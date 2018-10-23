using System.Management;

namespace WbemLibrary {
    public class WbemMethod {
        public string Name { get; set; }

        public WbemMethod(MethodData data) {
            Name = data.Name;
        }
    }
}
