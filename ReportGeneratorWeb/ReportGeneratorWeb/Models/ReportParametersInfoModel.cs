using System;

namespace ReportGeneratorWeb.Models
{
    public class ReportParametersInfoModel
    {
        public ReportParametersInfoModel()
        {

        }

        public ReportParametersInfoModel(string parametersFileName, string displayName, string description, string[] fileContent)
        {
            ParametersFileName = parametersFileName;
            DisplayName = displayName;
            Description = description;
            FileContent = fileContent;
        }

        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public string ParametersFileName { get; private set; }
        public string[] FileContent { get; private set; }
    }
}