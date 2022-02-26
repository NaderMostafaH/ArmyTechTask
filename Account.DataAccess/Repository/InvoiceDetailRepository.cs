using Account.DataAccess.IRepository;
using Account.DomainModels.Models;

namespace Account.DataAccess.Repository
{
    public class InvoiceDetailRepository : RepositoryAsync<InvoiceDetail>, IInvoiceDetailRepository
    {
        private readonly ArmyTechTaskContext _db;
        public InvoiceDetailRepository(ArmyTechTaskContext db) : base(db)
        {
            _db = db;
        }

        public void update(InvoiceDetail entity)
        {
            _db.InvoiceDetails.Update(entity);
        }
    }
}
