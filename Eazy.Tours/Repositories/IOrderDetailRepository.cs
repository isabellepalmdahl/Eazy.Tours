namespace Eazy.Tours.Repositories
{
    public interface IOrderDetailRepository: IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
