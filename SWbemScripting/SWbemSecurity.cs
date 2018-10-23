using System.Management;

namespace SWbemScripting {
    public class SWbemSecurity {
        public AuthenticationLevel AuthenticationLevel { get; set; }
        public ImpersonationLevel ImpersonationLevel { get; set; }

        //Todo: Privileges?
    }
}
