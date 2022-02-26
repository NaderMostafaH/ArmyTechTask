using Account.DataAccess.IRepository;
using Account.DomainModels.Models;
using Account.PresentationModels.Dtos.Cashier;
using Account.PresentationModels.ViewModels.Cashier;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Account.WEB.Controllers
{
    public class CashierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CashierController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> Upsert(int? Id)
        {
            IEnumerable<Branch> branches = await _unitOfWork.Branches.GetAllAsync();
            CashierForUpsertVM CashierVM = new CashierForUpsertVM()
            {
                Cashier = new CashierForUpsertDto(),
                BranchList = branches.Select(i => new SelectListItem
                {
                    Text = i.BranchName ,
                    Value = i.Id.ToString()
                })
            };

            if (Id is null)
                return View(CashierVM);

            //Edit Cashier
            var cashier = await _unitOfWork.Cashiers.GetAsync(Id.GetValueOrDefault());
                    CashierVM.Cashier.CashierName = cashier.CashierName;
            CashierVM.Cashier.BranchId = cashier.BranchId;
            CashierVM.Cashier.Id = cashier.Id;

            if (CashierVM is null)
                    return NotFound();

             return View(CashierVM);   
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CashierForUpsertVM cashierVM)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<Branch> branches = await _unitOfWork.Branches.GetAllAsync();
                cashierVM.BranchList = branches.Select(i => new SelectListItem
                {
                    Text = i.BranchName,
                    Value = i.Id.ToString()
                });
                return View(cashierVM);
            }
            if (cashierVM.Cashier.Id != 0)
            {
                //Update Cashier
                Cashier objFromDb = await _unitOfWork.Cashiers.GetAsync(cashierVM.Cashier.Id);
                objFromDb.BranchId = cashierVM.Cashier.BranchId;
                objFromDb.CashierName = cashierVM.Cashier.CashierName;

                _unitOfWork.Cashiers.update(objFromDb);
            }
               
            if (cashierVM.Cashier.Id == 0)
                  await  _unitOfWork.Cashiers.AddAsync(_mapper.Map<Cashier>(cashierVM.Cashier));
 
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
                var allObj = _mapper.Map<List<CashierListForUserDto>>(await _unitOfWork.Cashiers.GetAllAsync(includeProperties: "Branch,InvoiceHeaders"));
                return Json(new { data = allObj });
 
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            var objFromDb = await _unitOfWork.Cashiers.GetAsync(id);
            if (objFromDb == null)
                return Json(new { success = false, Message = "Error While Deleting" });

            var cashierOrders = await _unitOfWork.InvoiceHeaders.GetAllAsync(i => i.CashierId == id);
            List<InvoiceDetail> detailList = new List<InvoiceDetail>();
            foreach (var order in cashierOrders)
                detailList.AddRange(_unitOfWork.InvoiceDetails.GetAllAsync(d => d.InvoiceHeaderId == order.Id).Result);
            
            await _unitOfWork.InvoiceDetails.RemoveRangeAsync(detailList);
            await _unitOfWork.InvoiceHeaders.RemoveRangeAsync(cashierOrders);
            await _unitOfWork.Cashiers.RemoveAsync(objFromDb);
                  _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successful" });
        }

        #endregion
    }
}
