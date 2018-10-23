using SWbemLibrary.Constants;
using System;
using System.Management;

namespace SWbemLibrary {
    public class SWbemProperty {


        public WbemCimTypeEnum Cimtype {
            get {
                throw new NotImplementedException("Still need to figure out how to do Cimtype");
            }
            set {
                throw new NotImplementedException("Still need to figure out how to do Cimtype");
            }
        }
        public bool IsArray { get; set; }
        public bool IsLocal { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        //qualifiers
        public object Value { get; set; }

        public SWbemProperty(PropertyData propertyData) {

            IsArray = propertyData.IsArray;
            Name = propertyData.Name;
            IsLocal = propertyData.IsLocal;
            Origin = propertyData.Origin;
            Value = propertyData.Value;
        }
    }
}