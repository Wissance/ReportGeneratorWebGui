using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportGeneratorWeb.Models
{
    public class GenerationModel
    {
        public GenerationModel()
        {
        }

        public GenerationModel(string dataSourceType, string dataSourceConnStr, OutputReportType outputType, string parametersFile,
                               string templateFile, ParameterInfoModel[] parameters, OutputFileGenerationOptions outputFileOptions)
        {
            DataSourceType = dataSourceType;
            DataSourceConnStr = dataSourceConnStr;
            OutputType = outputType;
            ParametersFile = parametersFile;
            TemplateFile = templateFile;
            Parameters = parameters;
            OutputFileOptions = outputFileOptions;
        }

        public string DataSourceType { get; set; }
        public string DataSourceConnStr { get; set; }
        public OutputReportType OutputType { get; set; }
        public string ParametersFile { get; set; }
        public string TemplateFile { get; set; }
        public ParameterInfoModel[] Parameters { get; set; }
        public OutputFileGenerationOptions OutputFileOptions { get; set; }
    }
}
