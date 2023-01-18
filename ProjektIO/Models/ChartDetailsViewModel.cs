using ProjektIO.Entities;

namespace ProjektIO.Models
{
    public class ChartDetailsViewModel
    {
        public List<LineChartData> HedgefundOneChartData { get; set; }
        public List<LineChartData> HedgefundTwoChartData { get; set; }
        public string HedgefundOneName { get; set; }
        public string HedgefundTwoName { get; set; }
    }
}
