namespace Eazy.Tours.Repositories
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product product);
    }
}
