namespace Eazy.Tours.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private AppDbContext _context;

        public OrderDetailRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
           
            //var categoryDb = _context.Categories.FirstOrDefault(x => x.Id == Id);
            //if (categoryDb != null)
            //{
            //    categoryDb.Name = category.Name;
            //    categoryDb.DisplayOrder = category.DisplayOrder;
            //}
        }
    }
}
