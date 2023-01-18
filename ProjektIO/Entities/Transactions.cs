using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProjektIO.Entities
{
    public enum TypeOfTransaction
    {
        Payment, 
        Invest,
        Income,
        Return,
        ReturnToUser,
        BlockCash,
    }
    public class Transactions
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreationUserId { get; set; }
        public IdentityUser CreationUser { get; set; }
        public string TargetUserId { get; set; }
        public IdentityUser TargetUser { get; set; }
        public TypeOfTransaction TransactionType { get; set; }
        public decimal AmountOfTransaction { get; set; }
        public int? HedgefundId { get; set; }
        public Hedgefund? Hedgefund { get; set; }
        public DateTime? ReturnTimeOfInvestment { get; set; }
    }
}
