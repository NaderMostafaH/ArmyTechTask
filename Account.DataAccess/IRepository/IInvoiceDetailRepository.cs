using Account.DomainModels.Models;

namespace Account.DataAccess.IRepository
{
    public interface IInvoiceDetailRepository : IRepositoryAsync<InvoiceDetail>
    {
        void update(InvoiceDetail entity);
    }
}
