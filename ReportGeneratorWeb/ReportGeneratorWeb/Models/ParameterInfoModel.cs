using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportGeneratorWeb.Models
{
    public class ParameterInfoModel
    {
        public ParameterType Type { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
