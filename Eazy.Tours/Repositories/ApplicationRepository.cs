
namespace Eazy.Tours.Repositories
{
    public class ApplicationRepository : DbRepository<ApplicationUser>, IApplicationUser
    {
        private LoginDbContext _context;


        public ApplicationRepository(LoginDbContext context) : base(context)
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