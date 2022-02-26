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
    public class InvoiceDetailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public InvoiceDetailController(IUnitOfWork unitOfWork , IMapper mapper)
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
            ViewBag.branches = new SelectList(await _unitOfWork.Branches.GetAllAsync(), "Id", "BranchName");
            ViewBag.Cashier = new SelectList(await _unitOfWork.Cashiers.GetAllAsync(), "Id", "CashierName");

            if (Id is null)
                return View();

            var objFromDb = await _unitOfWork.InvoiceDetails.GetAsync(Id.GetValueOrDefault());
            if(objFromDb is null)
                return NotFound();


           
             return Json(new {data = objFromDb , success = true });   
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DetailForUser model)
        {
            ViewBag.branches = new SelectList(await _unitOfWork.Branches.GetAllAsync(), "Id", "BranchName");
            ViewBag.Cashier = new SelectList(await _unitOfWork.Cashiers.GetAllAsync(), "Id", "CashierName");

            if (model == null)
                return Json(new {success = false});

                if(!ModelState.IsValid)
                return Json(new { success = false });

                if(model.Id == 0 || model.Id == null)
                {
                    //Add Item 
                    await _unitOfWork.InvoiceDetails.AddAsync(_mapper.Map<InvoiceDetail>(model));
                    _unitOfWork.Save();
                     return Json(new { success = true , Message = "Item Adding Successsfully" });
                }
                else
                {
                        //Update Item
                        var objFromDb = await _unitOfWork.InvoiceDetails.GetAsync(model.Id);

                        if (objFromDb is null)
                            return Json(new { success = false, Message = "NotFound" });

                        //objFromDb = _mapper.Map<InvoiceDetail>(model);
                        objFromDb.ItemName = model.ItemName;
                        objFromDb.ItemPrice = model.ItemPrice;
                        objFromDb.ItemCount = model.ItemCount;

                          _unitOfWork.InvoiceDetails.update(objFromDb);
                          _unitOfWork.Save();
                        
                        return Json(new { success = true, Message = "Update Successful" });
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
      

        [HttpDelete]
        public IActionResult Delete(long id)
        {

            var objFromDb = _unitOfWork.InvoiceDetails.GetAsync(id);
            if (objFromDb == null)
                return Json(new { success = false, Message = "Error While Deleting" });

            _unitOfWork.InvoiceDetails.RemoveAsync(objFromDb.Result);
            _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successful" });
        }

        #endregion
    }
}
