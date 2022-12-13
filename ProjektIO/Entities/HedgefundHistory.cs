namespace ProjektIO.Entities
{
    public class HedgefundHistory
    {
        public int Id { get; set; }
        public double ReturnRate { get; set; }
        public DateTime ChangeDate { get; set; }
        public int HedgefundId { get; set; }
    }
}
