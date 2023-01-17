namespace ProjektIO.Entities
{
    public class Hedgefund
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal RequiredSalary { get; set; }
        public DateTime CreationDate { get; set; }
        public List<HedgefundHistory> History { get; set; }
        public DateTime ReturnTime { get; set; }
    }
}
