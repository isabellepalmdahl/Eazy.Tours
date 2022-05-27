using Microsoft.AspNetCore.Identity;

namespace Eazy.Tours.Repositories.DbInitializer
{
    public class DbInitializerRepo : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public DbInitializerRepo(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        //this can be used to seed roles but it will not be used in this project
        public void Initializer()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            //if (!_roleManager.RoleExistsAsync(WebsiteRole.Role_Administrator).GetAwaiter)
            //{

            //}
        }
    }
}
