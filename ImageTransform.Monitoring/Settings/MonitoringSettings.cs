using System;
using Microsoft.Extensions.Configuration;
using Vostok.Logging.File.Configuration;

namespace ImageTransform.Monitoring.Settings
{
    public class MonitoringSettings
    {
        public Uri ApiUrl { get; set; }
        public FileLogSettings FileLogSettings { get; set; }

        public string Test { get; set; }
    }
}