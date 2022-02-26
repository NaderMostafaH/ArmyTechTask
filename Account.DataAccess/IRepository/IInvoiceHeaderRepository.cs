using Account.DomainModels.Models;

namespace Account.DataAccess.IRepository
{
    public interface IInvoiceHeaderRepository : IRepositoryAsync<InvoiceHeader>
    {
        void update(InvoiceHeader entity);
    }
}
