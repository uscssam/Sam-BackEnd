namespace SAM.Repositories.Interfaces
{
    public interface IRepositoryDatabase<T> 
        where T: class
    {
        T Create(T model);
        T Read(int id);
        List<T> Search(T model);
        List<T> ReadAll();
        T Update(T model);
        void Delete(int id);
    }
}
