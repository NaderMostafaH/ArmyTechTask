using Account.PresentationModels.Dtos.Invoice.Detail;
using Account.PresentationModels.Dtos.Invoice.Header;
using System.ComponentModel.DataAnnotations;

namespace Account.PresentationModels.Dtos.Invoice
{
    public class InvoiceForUserDto
    {
        [Display(Name = "Invoice Number")]
        public long Id { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = null!;
        public DateTime Invoicedate { get; set; }
        [Display(Name = "Cashier")]
        public string CashierName { get; set; }
        [Display(Name = "Branch")]
        public string Branch { get; set; }
        public double price { get; set; }
        public virtual ICollection<DetailForUser> InvoiceDetails { get; set; }
    }
}
