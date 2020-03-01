namespace App.Repository
{
    public interface IRepository<T>
    {
        void Add(T item);

        T GetById(int id);
    }
}