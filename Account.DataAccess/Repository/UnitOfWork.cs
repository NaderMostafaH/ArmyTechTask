using Account.DataAccess.IRepository;
using Account.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ArmyTechTaskContext _db;
        public UnitOfWork(ArmyTechTaskContext db)
        {
            _db = db;
            Branches = new BranchRepository(_db);
            Cities = new CityRepository(_db);
            Cashiers = new CashierRepository(_db);
            InvoiceHeaders = new InvoiceHeaderRepository(_db);
            InvoiceDetails = new InvoiceDetailRepository(_db);

        }

        public IBranchRepository Branches { get; private set; }
        public ICityRepository Cities { get; private set; }
        public ICashierRepository Cashiers { get; private set; }
        public IInvoiceHeaderRepository InvoiceHeaders { get; private set; }
        public IInvoiceDetailRepository InvoiceDetails { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
