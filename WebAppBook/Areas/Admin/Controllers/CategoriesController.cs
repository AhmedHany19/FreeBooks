using Domain.Entity;
using infrastructure.ViewModel;
using Infrastructure.IRepository;
using Infrastructure.IRepository.ServicesRepository;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAppBook.Areas.Admin.Controllers
{
    [Area(Helper.Admin)]
    public class CategoriesController : Controller
    {
        private readonly IServicesRepository<Category> _servicesCategory;
        private readonly IServicesRepositoryLog<LogCategory> _servicesLogCategory;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoriesController(IServicesRepository<Category> servicesCategory,
            IServicesRepositoryLog<LogCategory> servicesLogCategory,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _servicesLogCategory = servicesLogCategory;
            _servicesCategory = servicesCategory;
        }




        public IActionResult Categories()
        {

            return View(new CategoryViewModel
            {
                Categories = _servicesCategory.GetAll(),
                LogCategories = _servicesLogCategory.GetAll(),
                NewCategory = new Category()
            });
        }

        public IActionResult Delete(Guid Id)
        {
            var userId = _userManager.GetUserId(User);
            if (_servicesCategory.Delete(Id) && _servicesLogCategory.Delete(Id, Guid.Parse(userId)))
                return RedirectToAction(nameof(Categories));

            return RedirectToAction(nameof(Categories));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (model.NewCategory.Id==Guid.Parse(Guid.Empty.ToString()))
                {
                    // Create
                    if (_servicesCategory.FindBy(model.NewCategory.Name) != null) 
                       SessionMsg(Helper.Error,Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgDuplicateNameCategory);
                    else
                    {
                        if (_servicesCategory.Save(model.NewCategory)&&_servicesLogCategory.Save(model.NewCategory.Id,Guid.Parse(userId)))
                           SessionMsg(Helper.Success, Resource.ResourceWeb.lbSave, Resource.ResourceWeb.lbMsgSaveCategory);
                        else
                            SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgNotSavedCategory);
                    }

                }
                else 

                {
                    //Update
                    if (_servicesCategory.Save(model.NewCategory) && _servicesLogCategory.Update(model.NewCategory.Id, Guid.Parse(userId)))
                        SessionMsg(Helper.Success, Resource.ResourceWeb.lbUpdate, Resource.ResourceWeb.lbMsgUpdateCategory);
                    else
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotUpdate, Resource.ResourceWeb.lbMsgNotUpdatedCategory);
                }
            }

            return RedirectToAction(nameof(Categories));

        }

        public IActionResult DeleteLog(Guid Id)
        {
            if (_servicesLogCategory.DeleteLog(Id))
                return RedirectToAction(nameof(Categories));
            return RedirectToAction(nameof(Categories));
        }
        private void SessionMsg(string MsgType, string Title, string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, MsgType);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Msg, Msg);
        }
    }
}
