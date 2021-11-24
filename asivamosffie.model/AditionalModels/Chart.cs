using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.AditionalModels
{
    public class Chart
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public double DevicePixelRatio { get; set; }
        public string Format { get; set; }
        public string BackgroundColor { get; set; }
        public string Key { get; set; }
        public string Version { get; set; }
        public string Config { get; set; }

        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public Chart()
        {
            Width = 500;
            Height = 300;
            DevicePixelRatio = 1.0;
            Format = "png";
            BackgroundColor = "transparent";

            Scheme = "https";
            Host = "quickchart.io";
            Port = 443;
        }
    }

    public class QuickChartShortUrlResponse
    {
        #pragma warning disable IDE1006 // Naming Styles
        public bool status { get; set; }
        public string url { get; set; }
        #pragma warning restore IDE1006 // Naming Styles
    }
}
