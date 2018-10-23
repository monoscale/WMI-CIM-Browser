using System.Management;

namespace WbemLibrary {
    public class WbemSecurity {
        public AuthenticationLevel AuthenticationLevel { get; set; }
        public ImpersonationLevel ImpersonationLevel { get; set; }

        //Todo: Privileges?
    }
}
