using System.Management;

namespace SWbemLibrary {
    public class SWbemSecurity {
        public AuthenticationLevel AuthenticationLevel { get; set; }
        public ImpersonationLevel ImpersonationLevel { get; set; }

        //Todo: Privileges?
    }
}
