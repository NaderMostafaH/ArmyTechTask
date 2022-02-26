using Account.PresentationModels.Dtos.Cashier;
using Account.PresentationModels.Dtos.Invoice.Detail;
using Account.PresentationModels.Dtos.Invoice.Header;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Account.PresentationModels.ViewModels.Invoice
{
    public class InvoiceForUpsertVM
    {
        public HeaderForUser InvoiceHeader { get; set; }
        public IEnumerable<SelectListItem>? BranchList { get; set; } 
        public IEnumerable<SelectListItem>? CashierList { get; set; } = new List<SelectListItem>();
    }
}
