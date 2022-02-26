using Account.DataAccess.IRepository;
using Account.DomainModels.Models;

namespace Account.DataAccess.Repository
{
    public class InvoiceHeaderRepository : RepositoryAsync<InvoiceHeader>, IInvoiceHeaderRepository
    {
        private readonly ArmyTechTaskContext _db;
        public InvoiceHeaderRepository(ArmyTechTaskContext db) : base(db)
        {
            _db = db;
        }

        public void update(InvoiceHeader entity)
        {
            _db.InvoiceHeaders.Update(entity);
        }
    }
}
