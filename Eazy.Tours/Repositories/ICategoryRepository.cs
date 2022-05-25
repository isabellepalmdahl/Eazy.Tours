namespace Eazy.Tours.Repositories
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category category);
    }
}
