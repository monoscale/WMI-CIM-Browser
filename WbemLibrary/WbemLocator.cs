using System.Management;

namespace WbemLibrary {
    public class WbemLocator {

        public WbemSecurity SWbemSecurity { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="strServer">Computer name to which you are connecting.</param>
        /// <param name="strNamespace">String that specifies the namespace to which you log on.</param>
        /// <param name="strUser">User name to use to connect.</param>
        /// <param name="strPassword">String that specifies the password to use when attempting to connect.</param>
        /// <param name="strLocale"></param>
        /// <param name="strAuthority"></param>
        /// <param name="iSecurityFlags"></param>
        /// <param name="objwbemNamedValueSet"></param>
        /// <returns></returns>
        public WbemService ConnectServer(string strServer = "."
            , string strNamespace = ""
            , string strUser = ""
            , string strPassword = ""
            , string strLocale = ""
            , string strAuthority = ""
            , int iSecurityFlags = 0
            , WbemNamedValueSet objwbemNamedValueSet = null) {

            ManagementPath path = new ManagementPath {
                Server = strServer,
                NamespacePath = strNamespace
            };
            ManagementScope scope = new ManagementScope(path);
            return new WbemService(scope);

        }
    }
}
