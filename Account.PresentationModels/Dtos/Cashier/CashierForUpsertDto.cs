using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Account.PresentationModels.Dtos.Cashier
{
    public class CashierForUpsertDto
    {
        public int Id { get; set; }
        [Display(Name="Cashier Name")]
        public string CashierName { get; set; } = null!;

        [Display(Name = "Branch")]
        public int BranchId { get; set; }
    }


    public class CashierForUpsertDtoValidator : AbstractValidator<CashierForUpsertDto>
    {
        public CashierForUpsertDtoValidator()
        {
            RuleFor(x => x.CashierName).NotEmpty().WithMessage("Cashier Name Is Required").MaximumLength(200);
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Cashier Must be in Branch");
        }
    }
}
