using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportGeneratorWeb.Models
{
    public class ParameterInfoModel
    {
        public ParameterInfoModel()
        {
        }

        public ParameterInfoModel(ParameterType type, string key, object value)
        {
            Type = type;
            Key = key;
            Value = value;
        }

        public ParameterType Type { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
