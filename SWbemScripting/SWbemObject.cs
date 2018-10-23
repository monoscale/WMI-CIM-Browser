
using System.Collections.Generic;
using System.Management;
using System.Linq;

namespace SWbemScripting {
    public class SWbemObject {

        public SWbemObjectPath Path { get; set; }
        /// <summary>
        /// Contains all the properties of the WMI object. Access a property by its name.
        /// </summary>
        public IDictionary<string, SWbemProperty> Properties { get; set; }
        /// <summary>
        /// Contains all the system properties of the WMI object. Access a property by its name.
        /// </summary>
        public IDictionary<string, SWbemProperty> SystemProperties { get; set; }
        /// <summary>
        /// Contains all the qualifiers of the WMI object. Access a qualifier by its name.
        /// </summary>
        public IDictionary<string, SWbemQualifier> Qualifiers { get; set; }
        /// <summary>
        /// Contains all the methods of the WMI object. Access a method by its name.
        /// </summary>
        public IDictionary<string, SWbemMethod> Methods { get; set; }
        
        public SWbemObject(ManagementClass mObject) {
            Path = new SWbemObjectPath(mObject.Path);


            IList<PropertyData> properties       = (from PropertyData  property  in mObject.Properties       select property ).ToList();
            IList<PropertyData> systemProperties = (from PropertyData  property  in mObject.SystemProperties select property ).ToList();
            IList<QualifierData> qualifiers      = (from QualifierData qualifier in mObject.Qualifiers       select qualifier).ToList();
            IList<MethodData> methods            = (from MethodData    method    in mObject.Methods          select method   ).ToList();


            SystemProperties = new Dictionary<string, SWbemProperty>();
            foreach(PropertyData data in systemProperties) {
                SystemProperties[data.Name] = new SWbemProperty(data);
            }
                

            Properties = new Dictionary<string, SWbemProperty>();
            foreach (PropertyData data in properties) {
                Properties[data.Name] = new SWbemProperty(data);
            }

            Qualifiers = new Dictionary<string, SWbemQualifier>();
            foreach(QualifierData data in qualifiers) {
                Qualifiers[data.Name] = new SWbemQualifier(data);
            }

            Methods = new Dictionary<string, SWbemMethod>();
            foreach(MethodData data in methods) {
                Methods[data.Name] = new SWbemMethod(data);
            
            }
        }

        public IDictionary<string, SWbemProperty> GetAllProperties() {
            IDictionary<string, SWbemProperty> result = Properties.ToDictionary(entry => entry.Key, entry => entry.Value); // COPY the dictionary
            foreach(KeyValuePair<string, SWbemProperty> pair in SystemProperties) {
                result.Add(pair.Key, pair.Value);
            }
            return result;
        }
    }
}
