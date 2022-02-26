using Account.DomainModels.Models;
using System.ComponentModel.DataAnnotations;

namespace Account.PresentationModels.Dtos.Invoice.Header
{
    public class HeaderForUser
    {
        public long? Id { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        public DateTime Invoicedate { get; set; }
        [Display(Name = "Cashier")]
        public int? CashierId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
    }
}
