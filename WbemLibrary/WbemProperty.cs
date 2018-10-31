using System;
using System.Management;
using WbemLibrary.Constants;

namespace WbemLibrary {
    public class WbemProperty {


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

        public WbemProperty(PropertyData propertyData) {

            IsArray = propertyData.IsArray;
            Name = propertyData.Name;
            IsLocal = propertyData.IsLocal;
            Origin = propertyData.Origin;
            Value = propertyData.Value;
        }
    }
}