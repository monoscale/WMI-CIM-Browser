using System.Management;

namespace SWbemLibrary {
    public class SWbemObjectPath {

        // string authority
        public string ClassName { get; set; }
        public bool IsClass { get; set; }
        public bool IsSingleton { get; set; }
        public string Namespace { get; set; }
        public string Path { get; set; }
        public string RelPath { get; set; }
        public string Server { get; set; }

        public SWbemObjectPath(ManagementPath path) {
            ClassName = path.ClassName;
            IsClass = path.IsClass;
            IsSingleton = path.IsSingleton;
            Namespace = path.NamespacePath;
            Path = path.Path;
            RelPath = path.RelativePath;
            Server = path.Server; 
        }

        void SetAsSingleton() {
            // 
        }
    }
}
