using Account.DomainModels.Models;
using Account.PresentationModels.Dtos.Cashier;
using Account.PresentationModels.Dtos.Invoice;
using Account.PresentationModels.Dtos.Invoice.Detail;
using Account.PresentationModels.Dtos.Invoice.Header;
using AutoMapper;

namespace Account.WEB.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cashier, CashierListForUserDto>()
            .ForMember(dest => dest.Branch, opt =>
            {
                opt.MapFrom(src => src.Branch.BranchName);
            })
            .ForMember(dest => dest.OrdersCount, opt =>
            {
                opt.MapFrom(src => src.InvoiceHeaders.Where(i => i.CashierId == src.Id).Count());
            });

            CreateMap<Cashier, CashierForUpsertDto>().ReverseMap();


            CreateMap<InvoiceHeader, HeaderForUser>().ReverseMap();
            

            CreateMap<InvoiceDetail, DetailForUser>()

                .ReverseMap();

            CreateMap<InvoiceHeader, InvoiceForUserDto>()
            .ForMember(dest => dest.Branch, opt =>
             {
                 opt.MapFrom(src => src.Branch.BranchName);
             })
            .ForMember(dest => dest.CashierName, opt =>
             {
                 opt.MapFrom(src => src.Cashier.CashierName);
             })
            .ForMember(dest => dest.price, opt =>
             {
                 opt.MapFrom(src => src.InvoiceDetails.Sum(x => x.ItemCount * x.ItemPrice));
             });
        }
    }
}
