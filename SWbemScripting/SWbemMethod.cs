using System.Management;

namespace SWbemScripting {
    public class SWbemMethod {
        public string Name { get; set; }

        public SWbemMethod(MethodData data) {
            Name = data.Name;
        }
    }
}
