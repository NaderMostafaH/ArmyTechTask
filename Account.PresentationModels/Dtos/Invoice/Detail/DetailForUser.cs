using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Account.PresentationModels.Dtos.Invoice.Detail
{
    public class DetailForUser
    {
        public long? Id { get; set; }
        [Display(Name = "Invoice Number")]
        public long InvoiceHeaderId { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; } = null!;
        [Display(Name = "Item Count")]
        public double ItemCount { get; set; }
        [Display(Name = "Item Price")]
        public double ItemPrice { get; set; }
    }
}
