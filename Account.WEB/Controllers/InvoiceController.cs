using Account.DataAccess.IRepository;
using Account.DomainModels.Models;
using Account.PresentationModels.Dtos.Cashier;
using Account.PresentationModels.Dtos.Invoice;
using Account.PresentationModels.Dtos.Invoice.Detail;
using Account.PresentationModels.Dtos.Invoice.Header;
using Account.PresentationModels.ViewModels.Cashier;
using Account.PresentationModels.ViewModels.Invoice;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Account.WEB.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public InvoiceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> Upsert(long? Id)
        {
            ViewBag.BranchList = new SelectList( await _unitOfWork.Branches.GetAllAsync(), "Id" , "BranchName");
            HeaderForUser model = new HeaderForUser();
            //InvoiceForUpsertVM InvoiceVM = new InvoiceForUpsertVM()
            //{
            //    InvoiceHeader = new HeaderForUser(),
            //    BranchList = branches.Select(i => new SelectListItem
            //    {
            //        Text = i.BranchName,
            //        Value = i.Id.ToString()
            //    }),

            //};

            if (Id is null)
                return View(model);

            //Edit Cashier
            model = _mapper.Map<HeaderForUser>(await _unitOfWork.InvoiceHeaders.GetAsync(Id.GetValueOrDefault()));
            var cashiers = await _unitOfWork.Cashiers.GetAllAsync(b => b.BranchId == model.BranchId);
            ViewBag.CashierList = cashiers.Select(i => new SelectListItem
            {
                Text = i.CashierName,
                Value = i.Id.ToString()
            });

            if (model is null)
                return NotFound();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(HeaderForUser model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.branches = new SelectList(await _unitOfWork.Branches.GetAllAsync(), "Id", "BranchName");
                ViewBag.Cashier = new SelectList(await _unitOfWork.Cashiers.GetAllAsync(), "Id", "CashierName");

                return View(model);
            }
            if (model.Id != 0 && model.Id != null)
            {
                //Update Invoice Header
                InvoiceHeader objFromDb = await _unitOfWork.InvoiceHeaders.GetAsync(model.Id);
                objFromDb.CustomerName = model.CustomerName;
                objFromDb.BranchId = model.BranchId;
                objFromDb.CashierId = model.CashierId;
                objFromDb.Invoicedate = model.Invoicedate;

                _unitOfWork.InvoiceHeaders.update(objFromDb);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                await _unitOfWork.InvoiceHeaders.AddAsync(_mapper.Map<InvoiceHeader>(model));
                _unitOfWork.Save();
                return Json(new { success = true, Message = "Invoice Added Successfully" });
            }
       
        }





        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allObj = _mapper.Map<List<InvoiceForUserDto>>(await _unitOfWork.InvoiceHeaders.GetAllAsync(includeProperties: "Branch,Cashier,InvoiceDetails"));
                return Json(new { data = allObj });
            }
            catch (Exception ex )
            {

                throw ex;
            }
           
            
        }
        [HttpPost]
        public async Task<IActionResult> GetAllItems(long id)
        {
            try
            {
                var allObj = await _unitOfWork.InvoiceDetails.GetAllAsync(d => d.InvoiceHeaderId == id);
                
                return Json(new { data = allObj });
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        [HttpGet]
        public IActionResult GetBranch()
        {
            try
            {
                var allObj = _unitOfWork.Branches.GetAllAsync().Result.Select(c => new { c.Id, c.BranchName });
                return Json(allObj);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [HttpGet]
        public IActionResult GetCashier(int id)
        {
            try
            {
                var allObj =  _unitOfWork.Cashiers.GetAllAsync(c => c.BranchId == id).Result.Select(c => new {c.Id , c.CashierName});
                return Json( allObj );
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [HttpDelete]
        public  IActionResult Delete(long id)
        {

            var objFromDb = _unitOfWork.InvoiceHeaders.GetAsync(id);
            if (objFromDb.Result == null)
                return Json(new { success = false, Message = "Error While Deleting" });

            var detailFromDb = _unitOfWork.InvoiceDetails.GetAllAsync(d => d.InvoiceHeaderId == id);
            if (detailFromDb.Result.Count() > 0)
                _unitOfWork.InvoiceDetails.RemoveRangeAsync(detailFromDb.Result);

            _unitOfWork.InvoiceHeaders.RemoveAsync(objFromDb.Result);
            _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successful" });
        }

        #endregion
    }
}
