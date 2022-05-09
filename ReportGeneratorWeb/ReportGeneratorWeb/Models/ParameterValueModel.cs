using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportGeneratorWeb.Models
{
    public enum ParameterType
    {
        Where = 1,
        Order = 2,
        Group = 3,
        StoredProcedure = 4
    }

    public class ParameterValueModel
    {
        public ParameterValueModel()
        {
        }

        public ParameterValueModel(ParameterType type, string name, string value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public ParameterType Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
