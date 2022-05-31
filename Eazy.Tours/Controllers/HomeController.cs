using Eazy.Tours.Models;
using Eazy.Tours.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Eazy.Tours.Controllers
{
    //[Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        //private readonly IDbRepository _repo;
        private IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _logger = logger;
            //_repo = repo;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IActionResult Index(string SearchString)
        {
            //checking if user is showing - delete later
            //var user = _repo.GetUserById(1);

            //IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");

            //return View(products);

            ViewData["CurrentFilter"] = SearchString;
            var products = from p in _context.Products
                           select p;
            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(p => p.Name.Contains(SearchString));
            }
            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int? productId)
        {
            CartVM cart = new CartVM()
            {
                Product = _unitOfWork.Product.GetT(x => x.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = (int)productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Cart cart)
        {               
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cart.ApplicationUserId = claims.Value;

                var cartItem = _unitOfWork.Cart.GetT(x => x.ProductId == cart.ProductId && x.ApplicationUserId == claims.Value);

                if (cartItem == null)
                {
                    _unitOfWork.Cart.Add(cart);
                    _unitOfWork.Save();
                    HttpContext.Session.SetInt32("SessionCart", _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value).ToList().Count);
                }
                else
                {
                    _unitOfWork.Cart.IncrementCartItem(cartItem, cart.Count);
                    _unitOfWork.Save();
                }
            }
            return RedirectToAction("Index");
        }


        public IActionResult AboutUs()
        {
            return View();
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