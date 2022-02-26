using Account.DataAccess.IRepository;
using Account.DomainModels.Models;

namespace Account.DataAccess.Repository
{
    public class BranchRepository : RepositoryAsync<Branch>, IBranchRepository
    {
        private readonly ArmyTechTaskContext _db;
        public BranchRepository(ArmyTechTaskContext db) : base(db)
        {
            _db = db;
        }

        public void update(Branch entity)
        {
            _db.Branches.Update(entity);
        }
    }
}
