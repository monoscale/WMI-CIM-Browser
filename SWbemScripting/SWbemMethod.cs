using System.Management;

namespace SWbemLibrary {
    public class SWbemMethod {
        public string Name { get; set; }

        public SWbemMethod(MethodData data) {
            Name = data.Name;
        }
    }
}
