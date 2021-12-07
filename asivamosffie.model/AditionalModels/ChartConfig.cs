using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.AditionalModels
{
    public class ChartConfig
    {
        public string type { get; set; }
        public Data data { get; set; }
        public Options options { get; set; }
    }
    public class Data
    {
        public List<string> labels { get; set; }
        public List<Dataset> datasets { get; set; }
    }
    public class Dataset
    {
        public string label { get; set; }
        public bool fill { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public List<decimal> data { get; set; }
    }

    public class Options
    {
        public Title title { get; set; }
        public Scales scales { get; set; }
    }
    public class Title
    {
        public bool display { get; set; }
        public string text { get; set; }
    }
    public class Scales
    {
        public List<XAx> xAxes { get; set; }
        public List<YAx> yAxes { get; set; }
    }
    public class XAx
    {
        public bool stacked { get; set; }
    }

    public class YAx
    {
        public bool stacked { get; set; }
    }
}
