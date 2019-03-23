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

    public class ParametersModel
    {
        public ParametersModel()
        {
            Parameters = new Dictionary<Tuple<ParameterType, string>, object>();
        }

        public ParametersModel(string parametersFile, IDictionary<Tuple<ParameterType, string>, object> parameters)
        {
            ParametersFile = parametersFile;
            Parameters = parameters;
        }

        public string ParametersFile { get; set; }
        public IDictionary<Tuple<ParameterType, string>, object> Parameters { get; set; }
        public bool IsStoredProcedure { get; set; }
    }
}
