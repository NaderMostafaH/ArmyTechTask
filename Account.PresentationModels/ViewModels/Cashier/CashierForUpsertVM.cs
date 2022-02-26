using Account.PresentationModels.Dtos.Cashier;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Account.PresentationModels.ViewModels.Cashier
{
    public class CashierForUpsertVM
    {
        public CashierForUpsertDto Cashier { get; set; }
        public IEnumerable<SelectListItem>? BranchList { get; set; }
    }
}
