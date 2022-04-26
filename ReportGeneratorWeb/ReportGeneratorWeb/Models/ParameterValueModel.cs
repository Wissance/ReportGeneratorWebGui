using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportGeneratorWeb.Models
{
    public enum ParameterType
    {
        Where,
        Order,
        Group,
        StoredProcedure
    }

    public class ParameterValueModel
    {
        public ParameterValueModel()
        {
        }

        public ParameterValueModel(ParameterType type, string name, object value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public ParameterType Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
