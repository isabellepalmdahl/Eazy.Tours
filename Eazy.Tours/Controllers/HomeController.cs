using Eazy.Tours.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Eazy.Tours.Controllers
{
    //[Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDbRepository _repo;
        private IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IDbRepository repo, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //checking if user is showing - delete later
            //var user = _repo.GetUserById(1);

            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int? productId)
        {
            Cart cart = new Cart()
            {
                Product = _unitOfWork.Product.GetT(x => x.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = (int)productId
            };
            return View(cart);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}