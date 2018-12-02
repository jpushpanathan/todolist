namespace TodoList.Core.Interfaces
{
    public interface IMemoryCacheEx
    {
        bool Add<T>(string key, T input, int expirationDurationInSeconds);
        bool Add<T>(string key, T input);
        bool Exists(string key);
        T Get<T>(string key);
        bool Remove(string key);
        bool Update<T>(string key, T input, int expirationDurationInSeconds);
        bool Update<T>(string key, T input);
    }
}