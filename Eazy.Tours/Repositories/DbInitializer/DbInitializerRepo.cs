using Microsoft.AspNetCore.Identity;

namespace Eazy.Tours.Repositories.DbInitializer
{
    public class DbInitializerRepo : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LoginDbContext _context;

        public DbInitializerRepo(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, LoginDbContext context)
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

            if (!_roleManager.RoleExistsAsync(WebsiteRole.Role_Administrator).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Role_Administrator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Role_Employee)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "123123123"
                }, "Admin@123").GetAwaiter().GetResult();
                ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, WebsiteRole.Role_Administrator).GetAwaiter().GetResult();

            }
            return;
        }
    }
}
