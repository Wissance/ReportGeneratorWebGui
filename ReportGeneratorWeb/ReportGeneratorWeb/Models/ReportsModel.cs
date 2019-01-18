using System.Collections.Generic;

namespace ReportGeneratorWeb.Models
{
    public class ReportsModel
    {
        public ReportsModel()
        {
            ParametersFiles = new List<ReportParametersInfoModel>();
            TemplatesFiles = new List<string>();
            AutodiscoveryConfig = new ReportsAutoDiscoveryConfigModel();
        }

        public ReportsModel(IList<ReportParametersInfoModel> parametersFiles, IList<string> templateFiles, ReportsAutoDiscoveryConfigModel config)
        {
            ParametersFiles = parametersFiles;
            TemplatesFiles = templateFiles;
            AutodiscoveryConfig = config;
        }

        public ReportsAutoDiscoveryConfigModel AutodiscoveryConfig { get; private set; }
        public IList<ReportParametersInfoModel> ParametersFiles { get; private set; }
        public IList<string> TemplatesFiles { get; private set; }
    }
}
