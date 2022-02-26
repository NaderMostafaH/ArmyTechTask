using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Account.PresentationModels.Dtos.Cashier
{
    public class CashierListForUserDto
    {
        public int Id { get; set; }
        [Display(Name = "Cashier Name")]
        public string CashierName { get; set; } = null!;
        public string Branch { get; set; }
        public int OrdersCount { get; set; }
    }


    public class CashierListForUserDtoValidator : AbstractValidator<CashierListForUserDto>
    {
        public CashierListForUserDtoValidator()
        {
            RuleFor(x => x.CashierName).NotEmpty().WithMessage("Cashier Name Is Required").MaximumLength(200);
            RuleFor(x => x.Branch).NotEmpty().WithMessage("Cashier Must be in Branch");
        }
    }
}
