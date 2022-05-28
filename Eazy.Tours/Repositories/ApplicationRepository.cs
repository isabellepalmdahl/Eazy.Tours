
namespace Eazy.Tours.Repositories
{
    public class ApplicationRepository : Repository<ApplicationUser>, IApplicationUser
    {
        private AppDbContext _context;


        public ApplicationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        //public void Update(ApplicationUser applicationUser)
        //{
        //    //var productDb = _context.Products.FirstOrDefault(x => x.Id == applicationUser.Id);
        //    //if (productDb != null)
        //    //{
        //    //    productDb.Name = product.Name;
        //    //    productDb.Description = product.Description;
        //    //    productDb.Price = product.Price;
        //    //    if (product.ImageUrl != null)
        //    //    {
        //    //        productDb.ImageUrl = product.ImageUrl;
        //    //    }
        //    //}

        //}
    }
}