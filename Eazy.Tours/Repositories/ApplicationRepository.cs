
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
        //    //var applicationUserDb = _context.Products.FirstOrDefault(x => x.Id == applicationUser.Id);
        //    //if (applicationUserDb != null)
        //    //{
        //    //    applicationUserDb.FirstName = applicationUser.FirstName;
        //    //    applicationUserDb.LastName = applicationUser.LastName;
        //    //    applicationUserDb.Email = applicationUser.Email;

        //    //}

        //}
    }
}