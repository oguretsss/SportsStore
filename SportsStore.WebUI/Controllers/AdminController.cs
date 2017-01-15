using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Web.Mvc;
using System.Linq;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.Products.
                FirstOrDefault(
                    x => x.ProductID == productId);
            return View(product);
        }
        
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if(ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = string.Format("ERROR: Couldn't save changes to {0}.", product.Name);
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productID)
        {
            Product deleted = repository.DeleteProduct(productID);
            if(deleted != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deleted.Name);
            }
            return RedirectToAction("Index");
        }
    }
}