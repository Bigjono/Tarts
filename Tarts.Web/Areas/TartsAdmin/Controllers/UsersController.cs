using System;
using System.Web.Mvc;
using Tarts.Admin;
using Tarts.Content;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Models.Admin;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class UsersController : Controller
    {

        private GenericRepo Repo;
        private User User;
        public UsersController(GenericRepo repo)
        {
            Repo = repo;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(Repo.GetAll<User>());
        }

        [Authorize]
        public ActionResult New (NewUserPostModel model = null)
        {
            if (model == null) model = new NewUserPostModel();
            return View(new User(){Email=model.Email});
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var model = Repo.GetById<User>(id);
            return View(model);
        }

        [Authorize]
        public ActionResult Create(NewUserPostModel model)
        {
            var usr = new User(model.Email, model.Password);
            var checkCreds = usr.Validate();
            if (checkCreds.Succeeded)
            {
                Repo.Save(usr);
                return RedirectToAction("Edit", new { id = usr.ID });
            }
            TempData.Add("ErrorMessage", checkCreds.Message);
            return RedirectToAction("New", model);
        }

        [Authorize]
        public ActionResult Update(UserPostModel model)
        {
            var usr = Repo.GetById<User>(model.ID);
            if (usr == null)
            {
                TempData.Add("ErrorMessage", "Failed to find user");
                return RedirectToAction("Index");
            }
            UpdateModel(usr, model);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ChangeEmail(ChangeEmailPostModel model)
        {
            var usr = Repo.GetById<User>(model.ID);
            if(usr == null)
            {
                TempData.Add("ErrorMessage", "Failed to find user");
                return RedirectToAction("Index");
            }
            var retVal = usr.ChangeEmail(model.Email);
            if (retVal.Succeeded)
                TempData.Add("Message", "Email Updated");
            else
                TempData.Add("ErrorMessage", retVal.Message);
            Repo.Save(usr);
            return RedirectToAction("Edit", new { id = usr.ID });
        }

        [Authorize]
        public ActionResult ChangePassword(ChangePasswordPostModel model)
        {
            var usr = Repo.GetById<User>(model.ID);
            if (usr == null)
            {
                TempData.Add("ErrorMessage", "Failed to find user");
                return RedirectToAction("Index");
            }
            var retVal = usr.ChangePassword(model.Password);
            if (retVal.Succeeded)
                TempData.Add("Message", "Password Updated");
            else
                TempData.Add("ErrorMessage", retVal.Message);
            Repo.Save(usr);
            return RedirectToAction("Edit", new { id = usr.ID });
        }

        [Authorize]
        public ActionResult Destroy(int id)
        {
            var usr = Repo.GetById<User>(id);
            Repo.Delete(usr);
            return RedirectToAction("Index");
        }

        private void UpdateModel(User user, UserPostModel model)
        {
            user.Update(model.FirstName,model.Surname);
            if ((user.Enabled != model.Enabled) && (model.Enabled)) user.EnabledUser();
            if ((user.Enabled != model.Enabled) && (!model.Enabled)) user.DisableUser();
            Repo.Save(user);
        }
    }
}
