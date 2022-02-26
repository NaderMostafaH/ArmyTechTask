using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.DataAccess.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBranchRepository Branches { get; }
        ICityRepository Cities { get; }
        ICashierRepository Cashiers { get; }
        IInvoiceHeaderRepository InvoiceHeaders { get; }
        IInvoiceDetailRepository InvoiceDetails { get; }

        void Save();
    }
}
