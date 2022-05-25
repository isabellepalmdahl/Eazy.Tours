namespace Eazy.Tours.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        //public ICartRepository Cart { get; private set; }
        //public IApplicationUser ApplicationUser { get; private set; }
        //public IOrderHeaderRepository OrderHeader { get; private set; }
        //public IOrderDetailRepository OrderDetail { get; private set; }
        public IUserRepository User { get; }
        public IRoleRepository Role { get; }

        public UnitOfWork(IUserRepository user, IRoleRepository role, AppDbContext context)
        {
          _context = context;
          Category = new CategoryRepository(context);
          Product = new ProductRepository(context);
          //Cart = new CartRepository(context);
          //ApplicationUser = new ApplicationRepository(context);
          //OrderHeader = new OrderHeaderRepository(context);
          //OrderDetail = new OrderDetailRepository(context);
          User = user;
          Role = role;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
