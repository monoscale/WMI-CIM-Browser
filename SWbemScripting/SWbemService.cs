using System.Collections.Generic;
using System.Management;
using System.Linq;

namespace SWbemLibrary {
    public class SWbemService {

        private ManagementScope scope;

        public SWbemService(ManagementScope scope) {
            this.scope = scope;
        }

        public IList<SWbemObject> ExecQuery(string strQuery, string strQueryLanguage = "WQL", int iFlags = 16, SWbemNamedValueSet objWbemNamedValueSet = null) {
            SelectQuery query = new SelectQuery(strQuery);
            IList<SWbemObject> result = new List<SWbemObject>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query)) {
                result = (from ManagementObject mObject
                          in searcher.Get()
                          select mObject)
                          .Select(m => new SWbemObject(new ManagementClass(m.ClassPath)))
                          .ToList();
            }

            return result;
        }
    }
}
