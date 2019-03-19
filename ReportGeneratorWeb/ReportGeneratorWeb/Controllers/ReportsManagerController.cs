using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DbTools.Core;
using Microsoft.AspNetCore.Mvc;
using ReportGeneratorWeb.Models;
using Microsoft.AspNetCore.Hosting;
using ReportGenerator.Core.Config;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Core.ReportsGenerator;


namespace ReportGeneratorWeb.Controllers
{
    /// <summary>
    ///     Reports manager controller:
    ///         - displays in Left Panel: List of Params and Tenplates ...
    ///         - in center: Preview of Params Files
    ///         - under it link / button 4 report generation
    /// </summary>
    public class ReportsManagerController : Controller
    {
        public ReportsManagerController(/*IFileStoreService fileStoreService,*/ IHostingEnvironment environment)
        {
            //_fileStoreService = fileStoreService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ReportsModel model = CreateModel();
            return View(model);
        }

        [HttpGet]
        public IActionResult GetParamsFile([FromQuery] string parameteresFileName)
        {
            ReportsAutoDiscoveryConfigModel config = GetAutoDiscoveryConfig();
            FileContentResult result = GetFile(parameteresFileName, config.ParametersFilesDirectory);
            if (result == null)
                return Ok(); // but this is not Ok, 404 should be
            return result;
        }

        [HttpGet]
        public IActionResult GetTemplateFile([FromQuery] string templateFileName)
        {
            ReportsAutoDiscoveryConfigModel config = GetAutoDiscoveryConfig();
            FileContentResult result = GetFile(templateFileName, config.TemplatesFilesDirectory);
            if (result == null)
                return Ok(); // but this is not Ok, 404 should be
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateReport([FromQuery] string parametersFile, [FromQuery] string templateFile,
                                                        [FromQuery] int worksheet, [FromQuery] int row, [FromQuery] int column)
        {
            ReportsAutoDiscoveryConfigModel pathSearchConfig = GetAutoDiscoveryConfig();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(null, DbEngine.SqlServer, "");
            string reportFile = GetExcelFilePath("Report", Guid.NewGuid());
            ExecutionConfig config = ExecutionConfigManager.Read(Path.Combine(pathSearchConfig.ParametersFilesDirectory, parametersFile));
            bool result = await manager.GenerateAsync(Path.Combine(pathSearchConfig.TemplatesFilesDirectory, templateFile), config, reportFile,
                ExcelReportGeneratorHelper.CreateParameters(worksheet, row, column));
            if (result)
            {
                byte[] bytes = System.IO.File.ReadAllBytes(reportFile);
                return File(bytes, _expectedMimeTypes[MsExcelExtension], "Report.xlsx");
            }
            return null;
        }

        private FileContentResult GetFile(string fileName, string searchPath)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(searchPath))
                return null;  // file not found
            KeyValuePair<string, string> mimeType = _expectedMimeTypes.FirstOrDefault(mt => fileName.EndsWith(mt.Key));
            if (mimeType.Equals(default(KeyValuePair<string, string>)))
                return null;  // unsupported mime type
            IList<FileInfo> searchingFiles = GetFiles(searchPath, fileName); // actually we except one
            if (searchingFiles.Count == 0)
                return null;
            byte[] bytes = System.IO.File.ReadAllBytes(searchingFiles.First().FullName);
            return File(bytes, mimeType.Value, searchingFiles.First().Name);
        }

        private string GetExcelFilePath(string prefixName, Guid fileId)
        {
            return Path.Combine(/*_fileStoreService.GetTempFileStorePath()*/ ".", $"{prefixName}_{fileId}.xlsx");
        }

        private ReportsModel CreateModel()
        {
            ReportsAutoDiscoveryConfigModel autoDiscoveryConfig = GetAutoDiscoveryConfig();
            IList<string> templatesFiles = GetFiles(autoDiscoveryConfig.TemplatesFilesDirectory, "*" + MsExcelExtension).Select(fi => fi.Name).ToList();
            IList<FileInfo> parametersFiles = GetFiles(autoDiscoveryConfig.ParametersFilesDirectory, "*" + XmlExtension);
            IList<ReportParametersInfoModel> parameters = parametersFiles.Select(Create).ToList();
            return new ReportsModel(parameters, templatesFiles, autoDiscoveryConfig);
        }

        private ReportsAutoDiscoveryConfigModel GetAutoDiscoveryConfig()
        {
            return new ReportsAutoDiscoveryConfigModel(Path.Combine(_environment.WebRootPath, TemplatesSubDirectory),
                                                       Path.Combine(_environment.WebRootPath, ParametersSubDirectory),
                                                       Path.Combine(_environment.WebRootPath, ReportsOutSubDirectory));
        }

        private IList<FileInfo> GetFiles(string searchPath, string namePattern)
        {
            IList<FileInfo> discoveredFiles = new List<FileInfo>();
            if (!string.IsNullOrEmpty(searchPath) && Directory.Exists(searchPath))
            {
                string[] files = Directory.GetFiles(searchPath, namePattern);
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        if (fileInfo.Exists)
                            discoveredFiles.Add(fileInfo);
                    }
                }
            }
            return discoveredFiles;
        }

        private ReportParametersInfoModel Create(FileInfo file)
        {
            ExecutionConfig config = ExecutionConfigManager.Read(file.FullName);
            string[] fileContent = System.IO.File.ReadAllLines(file.FullName);
            ReportParametersInfoModel parametersInfo = new ReportParametersInfoModel(file.Name, config.DisplayName, config.Description, fileContent);
            return parametersInfo;
        }

        private const string ParametersSubDirectory = @"files/params";
        private const string TemplatesSubDirectory = @"files/templates";
        private const string ReportsOutSubDirectory = "files/gen";

        private const string MsExcelExtension = ".xlsx"; // for 2010+ version of Office
        private const string XmlExtension = ".xml";

        private readonly IHostingEnvironment _environment;

        private readonly IDictionary<string, string> _expectedMimeTypes = new Dictionary<string, string>()
        {
            { MsExcelExtension, "application/vnd.ms-excel"},
            { XmlExtension, "application/xml" }
        };
    }
}