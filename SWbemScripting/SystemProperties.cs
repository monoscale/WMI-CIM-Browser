namespace SWbemScripting {
    public class SystemProperties { // not an enum because I want to be able to type SystemProperties.__CLASS  and get the string
        public static string __CLASS { get { return "__CLASS"; } }
        public static string __DERIVATION { get { return "__DERIVATION"; } }
        public static string __DYNASTY { get { return "__DYNASTY"; } }
        public static string __GENUS { get { return "__GENUS"; } }
        public static string __NAMESPACE { get { return "__NAMESPACE"; } }
        public static string __PATH { get { return "__PATH"; } }
        public static string __PROPERTY_COUNT { get { return "__PROPERTY_COUNT"; } }
        public static string __RELPATH { get { return "__RELPATH"; } }
        public static string __SERVER { get { return "__SERVER"; } }
        public static string __SUPERCLASS { get { return "__SUPERCLASS"; } }
    }
}
