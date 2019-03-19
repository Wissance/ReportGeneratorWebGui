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
            AutodiscoveryConfig = new ReportsAutoDiscoveryConfigModel();
            AvailableDataSources = new Dictionary<DbEngine, string>();
        }

        public ReportsModel(IList<ReportParametersInfoModel> parametersFiles, IList<string> templateFiles, 
                            ReportsAutoDiscoveryConfigModel config, IDictionary<DbEngine, string> availableDataSources)
        {
            ParametersFiles = parametersFiles;
            TemplatesFiles = templateFiles;
            AutodiscoveryConfig = config;
            AvailableDataSources = availableDataSources;
        }

        public ReportsAutoDiscoveryConfigModel AutodiscoveryConfig { get; private set; }
        public IList<ReportParametersInfoModel> ParametersFiles { get; private set; }
        public IList<string> TemplatesFiles { get; private set; }
        public IDictionary<DbEngine, string> AvailableDataSources { get; private set; }
    }
}
