using System.IO;

namespace ReportGeneratorWeb.Models
{
    /// <summary>
    ///    General setting for searching, all Dirs are on a server machine:
    ///        Reports Parameters files (.xml)
    ///        Excel Templates files (.xlsx)
    ///        Report Itself (.xlsx)
    /// </summary>
    public class ReportsAutoDiscoveryConfigModel
    {
        public ReportsAutoDiscoveryConfigModel()
        {
            InitImpl(DefaultTemplatesPath, DefaultParamsPath, DefaultOutPath);
        }

        public ReportsAutoDiscoveryConfigModel(string templatesDirectory, string parametersDirectory, string reportsOutDirectory)
        {
            InitImpl(templatesDirectory, parametersDirectory, reportsOutDirectory);
        }

        private void InitImpl(string templatesDirectory, string parametersDirectory, string reportsOutDirectory)
        {
            TemplatesFilesDirectory = Path.GetFullPath(templatesDirectory);
            ParametersFilesDirectory = Path.GetFullPath(parametersDirectory);
            ReportsFilesOutDirectory = Path.GetFullPath(reportsOutDirectory);
        }

        public string TemplatesFilesDirectory { get; set; }
        public string ParametersFilesDirectory { get; set; }
        public string ReportsFilesOutDirectory { get; set; }

        private const string DefaultTemplatesPath = @".\wwwroot\files\templates";
        private const string DefaultParamsPath = @".\wwwroot\files\params";
        private const string DefaultOutPath = @".\out";
    }
}
