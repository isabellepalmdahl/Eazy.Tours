namespace Eazy.Tours.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private AppDbContext _context;
        public OrderHeaderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void PaymentStatus(int Id, string sessionId, string paymentIntentId)
        {
            var orderHeader = _context.OrderHeaders.FirstOrDefault(x => x.Id == Id);
            orderHeader.DateOfPayment = DateTime.Now;
            orderHeader.PaymentIntentId = paymentIntentId;
            orderHeader.SessionId = sessionId;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);

            //var categoryDB = _context.Categories.FirstOrDefault(x => x.Id == Id);
            //if (categoryDB != null)
            //{
            //    categoryDB.Name = category.Name;
            //    categoryDB.DisplayOrder = category.DisplayOrder;
            //}
        }

        public void UpdateStatus(int Id, string bookingStatus, string? paymentStatus)
        {
            var order = _context.OrderHeaders.FirstOrDefault(x =>x.Id == Id);

            if (order != null)
            {
                order.BookingStatus = bookingStatus;
            }
            if (paymentStatus != null)
            {
                order.PaymentStatus = paymentStatus;
            }
        }
    }
}
