using System.Collections.Generic;
using System.Management;
using System.Linq;

namespace WbemLibrary {
    public class WbemService {

        private ManagementScope scope;

        public WbemService(ManagementScope scope) {
            this.scope = scope;
        }

        public IList<WbemObject> ExecQuery(string strQuery, string strQueryLanguage = "WQL", int iFlags = 16, WbemNamedValueSet objWbemNamedValueSet = null) {
            SelectQuery query = new SelectQuery(strQuery);
            IList<WbemObject> result = new List<WbemObject>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query)) {
                result = (from ManagementObject mObject
                          in searcher.Get()
                          select mObject)
                          .Select(m => new WbemObject(new ManagementClass(m.ClassPath)))
                          .ToList();
            }

            return result;
        }
    }
}
