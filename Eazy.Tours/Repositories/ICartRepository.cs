namespace Eazy.Tours.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        int IncrementCartItem(Cart cart, int count);
        int DecrementCartItem(Cart cart, int count);
    }
}
