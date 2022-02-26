using Account.DataAccess.IRepository;
using Account.DomainModels.Models;

namespace Account.DataAccess.Repository
{
    public class CityRepository : RepositoryAsync<City>, ICityRepository
    {
        private readonly ArmyTechTaskContext _db;
        public CityRepository(ArmyTechTaskContext db) : base(db)
        {
            _db = db;
        }

        public void update(City entity)
        {
            _db.Cities.Update(entity);
        }
    }
}
