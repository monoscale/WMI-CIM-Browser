using System.Collections.Generic;
using System.Management;
using System.Linq;

namespace WbemLibrary {
    public class WbemService {

        private ManagementScope scope;

        public WbemService(ManagementScope scope) {
            this.scope = scope;
        }

        /// <summary>
        /// Get all the WMI class objects.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WbemObject> GetAllObjects() {
            return ExecQuery("select * from meta_class");
        }

        /// <summary>
        /// The ExecQuery method of the SWbemServices object executes a query to retrieve objects. These objects are available through the returned SWbemObjectSet collection.
        /// </summary>
        /// <param name="strQuery">String that contains the text of the query. This parameter cannot be null.</param>
        /// <returns></returns>
        public IEnumerable<WbemObject> ExecQuery(string strQuery) {
            SelectQuery query = new SelectQuery(strQuery);
            IList<WbemObject> result;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query)) {
                result = (from ManagementObject mObject
                          in searcher.Get()
                          select mObject)
                          .Select(m => new WbemObject(new ManagementClass(m.ClassPath)))
                          .ToList();
            }
            return result;
        }

        /// <summary>
        /// The InstancesOf method returns a list of the instances of a specified class according to the user-specified selection criteria.
        /// This method implements a simple query. More complex queries may require the use of <seealso cref="ExecQuery"/>
        /// </summary>
        /// <param name="strClass">String that contains the name of the class for which instances are desired. This parameter cannot be null.</param>
        /// <returns></returns>
        public IEnumerable<WbemObject> InstancesOf(string strClass) {
            SelectQuery query = new SelectQuery($"select * from {strClass}");
            IList<WbemObject> result;
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
