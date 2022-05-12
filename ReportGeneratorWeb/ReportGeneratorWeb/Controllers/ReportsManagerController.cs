using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using DbTools.Core;
using Microsoft.AspNetCore.Mvc;
using ReportGeneratorWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Config;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Core.ReportsGenerator;


namespace ReportGeneratorWeb.Controllers
{
    /// <summary>
    ///     Reports manager controller:
    ///         - displays in Left Panel: List of Params and Templates ...
    ///         - in center: Preview of Params Files
    ///         - under it link / button 4 report generation
    /// </summary>
    [Controller]
    [Route("[controller]")]
    public class ReportsManagerController : Controller
    {
        public ReportsManagerController(ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            _loggerFactory = loggerFactory;
            _environment = environment;
        }

        [HttpGet("")]
        [HttpGet("Index/")]
        public async Task<IActionResult> IndexAsync()
        {
            ReportsModel model = CreateReportsModel();
            return View(model);
        }

        [HttpGet("SetParameters/")]
        public async Task<IActionResult> SetParametersAsync([FromQuery] string parametersFile)
        {
            ReportsAutoDiscoveryConfigModel pathSearchConfig = GetAutoDiscoveryConfig();
            ExecutionConfig config = ExecutionConfigManager.Read(Path.Combine(pathSearchConfig.ParametersFilesDirectory, parametersFile));
            ParametersModel model = new ParametersModel();
            model.ParametersFile = parametersFile;
            if (config.DataSource == ReportDataSource.StoredProcedure)
            {
                IList<ParameterValueModel> data = config.StoredProcedureParameters.Select(p =>
                    new ParameterValueModel(ParameterType.StoredProcedure, p.ParameterName, p.ParameterValue.ToString())).ToList();
                model.Parameters = data;
                model.IsStoredProcedure = true;
            }
            else
            {
                IList<ParameterValueModel> whereParams = config.ViewParameters.WhereParameters.Select(p =>
                    new ParameterValueModel(ParameterType.Where, p.ParameterName, p.ParameterValue)).ToList();
                IList<ParameterValueModel> orderParams = config.ViewParameters.OrderByParameters.Select(p =>
                    new ParameterValueModel(ParameterType.Order, p.ParameterName, p.ParameterValue)).ToList();
                IList<ParameterValueModel> groupParams = config.ViewParameters.GroupByParameters.Select(p =>
                    new ParameterValueModel(ParameterType.Group, p.ParameterName, p.ParameterValue)).ToList();
                List<ParameterValueModel> parameters = new List<ParameterValueModel>();
                parameters.AddRange(whereParams);
                parameters.AddRange(orderParams);
                parameters.AddRange(groupParams);
                //result = result.Concat(whereParams).Concat(orderParams).Concat(groupParams).ToDictionary(item => item.Key, item => item.Value);
                model.Parameters = parameters;
                model.IsStoredProcedure = false;
            }
            return PartialView("Modals/SetParametersModal", model);
        }

        [HttpGet("GetParamsFile/")]
        public async Task<FileContentResult> GetParamsFileAsync([FromQuery] string parametersFileName)
        {
            ReportsAutoDiscoveryConfigModel config = GetAutoDiscoveryConfig();
            FileContentResult result = await GetFileAsync(parametersFileName, config.ParametersFilesDirectory);
            if (result == null)
                return null; // but this is not Ok, 404 should be
            return result;
        }

        [HttpGet("GetTemplateFile/")]
        public async Task<FileContentResult> GetTemplateFileAsync([FromQuery] string templateFileName)
        {
            ReportsAutoDiscoveryConfigModel config = GetAutoDiscoveryConfig();
            FileContentResult result = await GetFileAsync(templateFileName, config.TemplatesFilesDirectory);
            if (result == null)
                return null; // but this is not Ok, 404 should be
            return result;
        }

        [HttpPost("Generate/")]
        public async Task<IActionResult> GenerateAsync([FromBody] GenerationModel generation)
        {
            ReportsAutoDiscoveryConfigModel pathSearchConfig = GetAutoDiscoveryConfig();
            KeyValuePair<DbEngine, string> dataSourceDbEngine = _availableDataSources.First(item => string.Equals(item.Value.Trim().ToLower(), 
                                                                                                                                          generation.DataSourceType.Trim().ToLower()));
            IReportGeneratorManager manager = CreateReportGenerationManager(dataSourceDbEngine.Key, generation.DataSourceConnStr, generation.OutputType);
            string reportFile = GetReportFilePath("Report", Guid.NewGuid(), generation.OutputType);
            string parametersFile = Path.Combine(pathSearchConfig.ParametersFilesDirectory, generation.ParametersFile);
            ExecutionConfig config = CreateExecutionConfig(parametersFile, generation.Parameters);

            int result = await manager.GenerateAsync(Path.Combine(pathSearchConfig.TemplatesFilesDirectory, generation.TemplateFile), config, reportFile,
                                                      CreateOutputGenerationParameters(generation.OutputType, generation.OutputFileOptions));
            if (result > 0)
            {
                byte[] bytes = await System.IO.File.ReadAllBytesAsync(reportFile);
                string reportExtension = _reportTypes[generation.OutputType];
                ContentDisposition content = new ContentDisposition()
                {
                    FileName = reportFile,
                    Inline = false
                };
                Response.Headers.Add("Content-Disposition", content.ToString());
                return File(bytes, _expectedMimeTypes[reportExtension], Path.GetFileName(reportFile));
            }
            return Ok();
        }

        private async Task<FileContentResult> GetFileAsync(string fileName, string searchPath)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(searchPath))
                return null;  // file not found
            KeyValuePair<string, string> mimeType = _expectedMimeTypes.FirstOrDefault(mt => fileName.EndsWith(mt.Key));
            if (mimeType.Equals(default(KeyValuePair<string, string>)))
                return null;  // unsupported mime type
            IList<FileInfo> searchingFiles = GetFiles(searchPath, fileName); // actually we except one
            if (searchingFiles.Count == 0)
                return null;
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(searchingFiles.First().FullName);
            return File(bytes, mimeType.Value, searchingFiles.First().Name);
        }

        private string GetReportFilePath(string prefixName, Guid fileId, OutputReportType reportType)
        {
            string reportFileExtension = _reportTypes[reportType];
            return Path.Combine(".", $"{prefixName}_{fileId}{reportFileExtension}");
        }

        private ReportsModel CreateReportsModel()
        {
            ReportsAutoDiscoveryConfigModel autoDiscoveryConfig = GetAutoDiscoveryConfig();
            List<string> templatesFiles = new List<string>();
            IList<FileInfo> excelTemplates = GetFiles(autoDiscoveryConfig.TemplatesFilesDirectory, "*" + MsExcelExtension);
            IList<FileInfo> csvTemplates = GetFiles(autoDiscoveryConfig.TemplatesFilesDirectory, "*" + CsvExtension);
            templatesFiles.AddRange(excelTemplates.Select(fi => fi.Name));
            templatesFiles.AddRange(csvTemplates.Select(fi => fi.Name));
            IList<FileInfo> parametersFiles = GetFiles(autoDiscoveryConfig.ParametersFilesDirectory, "*" + XmlExtension);
            IList<ReportParametersInfoModel> parameters = parametersFiles.Select(CreateParametersModel).ToList();
            return new ReportsModel(parameters, templatesFiles, autoDiscoveryConfig, _availableDataSources);
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

        private ReportParametersInfoModel CreateParametersModel(FileInfo file)
        {
            ExecutionConfig config = ExecutionConfigManager.Read(file.FullName);
            string[] fileContent = System.IO.File.ReadAllLines(file.FullName);
            ReportParametersInfoModel parametersInfo = new ReportParametersInfoModel(file.Name, config.DisplayName, config.Description, fileContent);
            return parametersInfo;
        }

        private IReportGeneratorManager CreateReportGenerationManager(DbEngine dbEngine, string connStr, OutputReportType reportType)
        {
            if (reportType == OutputReportType.Excel)
                return new ExcelReportGeneratorManager(_loggerFactory, dbEngine, connStr);
            return new CsvReportGeneratorManager(_loggerFactory, dbEngine, connStr, ",");
        }

        private ExecutionConfig CreateExecutionConfig(string parametersFile, ParameterValueModel[] parameters)
        {
            ExecutionConfig config = ExecutionConfigManager.Read(parametersFile);
            if (parameters != null && parameters.Length > 0)
            {
                if (config.DataSource == ReportDataSource.StoredProcedure)
                {
                    foreach (ParameterValueModel parameter in parameters)
                    {
                        StoredProcedureParameter existingStoreProcParam = config.StoredProcedureParameters.FirstOrDefault(p => string.Equals(p.ParameterName.ToLower(), parameter.Name.ToLower()));
                        if (existingStoreProcParam != null)
                            existingStoreProcParam.ParameterValue = parameter.Value;
                    }
                }
                else
                {
                    foreach (ParameterValueModel parameter in parameters)
                    {
                        DbQueryParameter sqlStatementParameter = null;
                        if (parameter.Type == ParameterType.Where)
                            sqlStatementParameter = config.ViewParameters.WhereParameters.FirstOrDefault(p => string.Equals(p.ParameterName.ToLower(), parameter.Name.ToLower()));
                        if (parameter.Type == ParameterType.Order)
                            sqlStatementParameter = config.ViewParameters.OrderByParameters.FirstOrDefault(p => string.Equals(p.ParameterName.ToLower(), parameter.Name.ToLower()));
                        if (parameter.Type == ParameterType.Group)
                            sqlStatementParameter = config.ViewParameters.GroupByParameters.FirstOrDefault(p => string.Equals(p.ParameterName.ToLower(), parameter.Name.ToLower()));
                        if (sqlStatementParameter != null)
                            sqlStatementParameter.ParameterValue = parameter.Value.ToString();
                    }
                }
            }

            return config;
        }

        private object[] CreateOutputGenerationParameters(OutputReportType reportType, OutputFileGenerationOptionsModel outputOptions)
        {
            if (reportType == OutputReportType.Csv)
                return new object[] { };
            return ExcelReportGeneratorHelper.CreateParameters(outputOptions.Worksheet, outputOptions.Row, outputOptions.Column);
        }

        private const string ParametersSubDirectory = @"files/params";
        private const string TemplatesSubDirectory = @"files/templates";
        private const string ReportsOutSubDirectory = "files/gen";

        private const string MsExcelExtension = ".xlsx"; // for 2010+ version of Office
        private const string XmlExtension = ".xml";
        private const string CsvExtension = ".csv";

        private readonly IHostingEnvironment _environment;
        private readonly ILoggerFactory _loggerFactory;

        private readonly IDictionary<OutputReportType, string> _reportTypes = new Dictionary<OutputReportType, string>()
        {
            { OutputReportType.Excel, MsExcelExtension },
            { OutputReportType.Csv, CsvExtension }
        };

        private readonly IDictionary<string, string> _expectedMimeTypes = new Dictionary<string, string>()
        {
            { MsExcelExtension, "application/vnd.ms-excel"},
            { XmlExtension, "application/xml" },
            { CsvExtension, "text/csv"},
        };

        private readonly IDictionary<DbEngine, string> _availableDataSources = new Dictionary<DbEngine, string>()
        {
            { DbEngine.SqlServer, "MS SQL"},
            { DbEngine.SqLite, "SQLite"},
            { DbEngine.MySql, "MySQL"},
            { DbEngine.PostgresSql, "Postgres SQL"}
        };
    }
}