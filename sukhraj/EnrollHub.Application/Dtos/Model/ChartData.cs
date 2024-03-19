using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollHub.Application.Dtos.Model
{
    public class ChartData
    {
        public ChartData()
        {
            labels = new List<string>();
            datasets = new List<Dataset>();
        }
        public List<string> labels { get; set; }
        public List<Dataset> datasets { get; set; }
    }

    public class Dataset
    {
        public Dataset()
        {
            data = new List<string>();

        }
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public List<string> data { get; set; }

    }
}
