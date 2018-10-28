using WbemLibrary;

namespace WMI_CIM_Browser.ViewModels {
    public class DataGridPropertyView { // class used to wrap a PropertyData class

        public string Name { get; private set; }
        public string CimType { get; private set; }
        public string Value { get; private set; }
        public string Description { get; private set; }

        public DataGridPropertyView(WbemProperty property) {
            Name = property.Name;

            if (property.IsArray) {
                CimType = "array of ";
            }
           // CimType += property.Type.ToString();

            if (property.Value != null) {
                if (property.IsArray) {
                    foreach(string s in (string[])property.Value) {
                        Value += $"{s}, ";
                    }
                } else {
                    Value = property.Value.ToString();
                }
            } else {
                Value = "<empty>";
            }


      //      if (property.Qualifiers["Description"] != null) {
       //         Description = (string)property.Qualifiers["Description"].Value;
      //      }
            

            
            
        }

    }
}
