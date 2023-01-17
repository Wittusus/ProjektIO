using ProjektIO.Entities;

namespace ProjektIO.Models
{
    public class HedgeFundHistoriesGetViewModel
    {
        public IEnumerable<HedgefundHistory> HedgefundHistories { get; set; }
        public IEnumerable<LineChartData> ChartDatas { get; set; }  
    }
    public class LineChartData
    {
        public DateTime xValue;
        public double yValue;
        public double yValue1;
    }

}
