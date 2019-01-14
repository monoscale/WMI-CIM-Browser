namespace WbemLibrary {
    public class SystemProperties { // not an enum because I want to be able to type SystemProperties.__CLASS  and get the string
        public static string __CLASS => "__CLASS";
        public static string __DERIVATION => "__DERIVATION";
        public static string __DYNASTY => "__DYNASTY";
        public static string __GENUS => "__GENUS";
        public static string __NAMESPACE => "__NAMESPACE";
        public static string __PATH => "__PATH";
        public static string __PROPERTY_COUNT => "__PROPERTY_COUNT";
        public static string __RELPATH => "__RELPATH";
        public static string __SERVER => "__SERVER";
        public static string __SUPERCLASS => "__SUPERCLASS";
    }
}
