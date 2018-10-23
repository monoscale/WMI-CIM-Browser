
using System.Collections.Generic;
using System.Management;
using System.Linq;

namespace WbemLibrary {
    public class WbemObject {

        public WbemObjectPath Path { get; set; }
        /// <summary>
        /// Contains all the properties of the WMI object. Access a property by its name.
        /// </summary>
        public IDictionary<string, WbemProperty> Properties { get; set; }
        /// <summary>
        /// Contains all the system properties of the WMI object. Access a property by its name.
        /// </summary>
        public IDictionary<string, WbemProperty> SystemProperties { get; set; }
        /// <summary>
        /// Contains all the qualifiers of the WMI object. Access a qualifier by its name.
        /// </summary>
        public IDictionary<string, WbemQualifier> Qualifiers { get; set; }
        /// <summary>
        /// Contains all the methods of the WMI object. Access a method by its name.
        /// </summary>
        public IDictionary<string, WbemMethod> Methods { get; set; }
        
        public WbemObject(ManagementClass mObject) {
            Path = new WbemObjectPath(mObject.Path);


            IList<PropertyData> properties       = (from PropertyData  property  in mObject.Properties       select property ).ToList();
            IList<PropertyData> systemProperties = (from PropertyData  property  in mObject.SystemProperties select property ).ToList();
            IList<QualifierData> qualifiers      = (from QualifierData qualifier in mObject.Qualifiers       select qualifier).ToList();
            IList<MethodData> methods            = (from MethodData    method    in mObject.Methods          select method   ).ToList();


            SystemProperties = new Dictionary<string, WbemProperty>();
            foreach(PropertyData data in systemProperties) {
                SystemProperties[data.Name] = new WbemProperty(data);
            }
                

            Properties = new Dictionary<string, WbemProperty>();
            foreach (PropertyData data in properties) {
                Properties[data.Name] = new WbemProperty(data);
            }

            Qualifiers = new Dictionary<string, WbemQualifier>();
            foreach(QualifierData data in qualifiers) {
                Qualifiers[data.Name] = new WbemQualifier(data);
            }

            Methods = new Dictionary<string, WbemMethod>();
            foreach(MethodData data in methods) {
                Methods[data.Name] = new WbemMethod(data);
            
            }
        }

        public IDictionary<string, WbemProperty> GetAllProperties() {
            IDictionary<string, WbemProperty> result = Properties.ToDictionary(entry => entry.Key, entry => entry.Value); // COPY the dictionary
            foreach(KeyValuePair<string, WbemProperty> pair in SystemProperties) {
                result.Add(pair.Key, pair.Value);
            }
            return result;
        }
    }
}
