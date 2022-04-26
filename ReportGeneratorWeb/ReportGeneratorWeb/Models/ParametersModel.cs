using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ReportGeneratorWeb.Models
{

    public class ParametersModel
    {
        public ParametersModel()
        {
            Parameters = new List<ParameterValueModel>();
        }

        public ParametersModel(string parametersFile, IList<ParameterValueModel> parameters, bool isStoredProcedure)
        {
            ParametersFile = parametersFile;
            Parameters = parameters;
            IsStoredProcedure = isStoredProcedure;
        }

        public string ParametersFile { get; set; }
        public IList<ParameterValueModel> Parameters { get; set; }
        public bool IsStoredProcedure { get; set; }
    }
}
