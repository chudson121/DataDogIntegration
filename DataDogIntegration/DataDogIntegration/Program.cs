using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DataDogIntegration.CommandLine;
using DataDogIntegration.Logging.Interfaces;
using DataDogIntegration.Logging.Services;

namespace DataDogIntegration
{
    public class Program
    {
        private static readonly ILogger Logger = LoggingService.GetLoggingService();
        private static CommandLineProcessor _clp;

        public static void Main(string[] args)
        {
            Logger.Info("Program startup");
            ConfigureCommandLIne(args);

            var dashboardId = _clp.Options.DashboardId;
            var checkId = _clp.Options.PingdomId; // "123";
            var checkName = _clp.Options.PingdomName;

            var datadogConfiguration = LoadConfigFromAppSettings();

            //if (string.IsNullOrEmpty(_clp.Options.DashboardId))
            //{
            //    throw new ArgumentNullException("Dashboard Id must be specified");
            //}

            //GetDashBoard(datadogConfiguration, _clp.Options.DashboardId);
            GetDashBoard(datadogConfiguration, "118397");

            //if (string.IsNullOrEmpty(_clp.Options.InputFilePath))
            //{

            //    ProcessSingleWidget(datadogConfiguration, dashboardId, checkId, checkName);
            //}
            //else
            //{
            //    ProcessFileForWidgets(datadogConfiguration, _clp.Options.InputFilePath);
            //}

            if (!string.IsNullOrEmpty(_clp.Options.InputFilePath))
                ProcessFileForWidgets(datadogConfiguration, _clp.Options.InputFilePath);

            //Metric
            //api.Metric.send(metric='my.series', points=[(now, 15), (future_10s, 16)])
            SubmitMetricToDd(datadogConfiguration);

            Logger.Info("Program End");
        }

        private static void SubmitMetricToDd(DataDogConfig datadogConfiguration)
        {
            var dProcessor = new DataDogProcessor(datadogConfiguration);
            var ddms = new DataDogMetricSeries
            {
                series = new List<Series>
                {
                    new Series { metric = "TestApp.TestCounter",points = new List<List<long>> { new List<long> {DateTimeOffset.UtcNow.ToUnixTimeSeconds(),5} }}
                }
            };


            dProcessor.SubmitSingleMetric(ddms);
            Logger.Info("Metric Sent");
        }

        private static DataDogConfig LoadConfigFromAppSettings()
        {
            return new DataDogConfig()
            {
                BaseUrl = System.Configuration.ConfigurationManager.AppSettings["datadog:BaseUrl"],
                api_key = System.Configuration.ConfigurationManager.AppSettings["datadog:apikey"],
                app_key = System.Configuration.ConfigurationManager.AppSettings["datadog:appkey"],
            };
        }

        private static Screenboard GetDashBoard(DataDogConfig datadogConfiguration, string dashboardId)
        {
            var dProcessor = new DataDogProcessor(datadogConfiguration);
            var dashboard = dProcessor.GetDashboardById(dashboardId);
            return dashboard;
        }

        private static void ProcessFileForWidgets(DataDogConfig datadogConfiguration, string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File Not found {path}");
            }

            // Open the file to read from.
            var readText = File.ReadAllLines(path);
            foreach (var s in readText)
            {
                if(string.IsNullOrEmpty(s)) {continue;}

                Logger.Debug(s);
                var dashboardId = _clp.Options.DashboardId;
                var splitLine = s.Split('|');

                var checkId = splitLine[0]; //"";
                var checkName = splitLine[1]; // "";
                ProcessSingleWidget(datadogConfiguration, dashboardId, checkId, checkName);
            }
        }

        private static void ProcessSingleWidget(DataDogConfig datadogConfiguration, string dashboardId, string checkId, string checkName)
        {
            if(string.IsNullOrEmpty(dashboardId))
                throw new ArgumentNullException("DashboardId is missing");

            if (string.IsNullOrEmpty(checkId))
                throw new ArgumentNullException("checkId is missing");

            if (string.IsNullOrEmpty(checkName))
                throw new ArgumentNullException("checkName is missing");


            var dProcessor = new DataDogProcessor(datadogConfiguration);
            var dashboard = dProcessor.GetDashboardById(dashboardId);
            var title = checkName;

            var wigetsToAdd = new List<Widget>{Widget.CreatePingdomCheckWidget(title, checkName, checkId)};
            var updateddashboard = dProcessor.AddWidgetDashboard(dashboard, wigetsToAdd);
        }

        private static void ConfigureCommandLIne(string[] args)
        {
            Logger.Info("Configure Command Line Settings");
            _clp = new CommandLineProcessor(args, Logger);
            //Logger.Info("Debug Mode:{0}", _clp.Options);
        }
    }
}
