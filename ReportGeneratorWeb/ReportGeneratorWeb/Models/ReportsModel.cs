using System.Collections.Generic;
using DbTools.Core;

namespace ReportGeneratorWeb.Models
{
    public class ReportsModel
    {
        public ReportsModel()
        {
            ParametersFiles = new List<ReportParametersInfoModel>();
            TemplatesFiles = new List<string>();
            AutoDiscoveryConfig = new ReportsAutoDiscoveryConfigModel();
            AvailableDataSources = new Dictionary<DbEngine, string>();
        }

        public ReportsModel(IList<ReportParametersInfoModel> parametersFiles, IList<string> templateFiles, 
                            ReportsAutoDiscoveryConfigModel config, IDictionary<DbEngine, string> availableDataSources)
        {
            ParametersFiles = parametersFiles;
            TemplatesFiles = templateFiles;
            AutoDiscoveryConfig = config;
            AvailableDataSources = availableDataSources;
        }

        public ReportsAutoDiscoveryConfigModel AutoDiscoveryConfig { get; }
        public IList<ReportParametersInfoModel> ParametersFiles { get; }
        public IList<string> TemplatesFiles { get; }
        public IDictionary<DbEngine, string> AvailableDataSources { get; }
    }
}
