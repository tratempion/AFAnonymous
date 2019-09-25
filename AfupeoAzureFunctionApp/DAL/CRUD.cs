namespace DAL
{
    public interface CRUD
    {
        void Create(string data);
        string Read();
        void Update(string data);
        void Delete(string data);
    }
}