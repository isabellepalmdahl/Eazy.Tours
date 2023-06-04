namespace Eazy.Tours.Repositories
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int Id, string bookingStatus, string? paymentStatus = null);
        void PaymentStatus(int Id, string sessionId, string paymentIntentId);
    }
}
