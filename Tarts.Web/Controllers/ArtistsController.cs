using System.Web.Mvc;
using Tarts.Events;
using Tarts.Persistance;

namespace Tarts.Web.Controllers
{
    public class ArtistsController : Controller
    {
        private GenericRepo Repo;
        public ArtistsController(GenericRepo repo)
        {
            Repo = repo;
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ViewResult Bio(string id)
        {
            var o = Repo.GetByFieldName<Artist>("Slug", id);
            return View(o);
        }

        

        
    }
}
