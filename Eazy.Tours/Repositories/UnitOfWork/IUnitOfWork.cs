namespace Eazy.Tours.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }

        IRoleRepository Role { get; }

        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        //ICartRepository Cart { get; }
        //IApplicationUser ApplicationUser { get; }
        //IOrderHeaderRepository OrderHeader { get; }
        //IOrderDetailRepository OrderDetail { get; }

        void Save();
    }
}
