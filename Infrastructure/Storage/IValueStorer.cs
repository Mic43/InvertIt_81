namespace Infrastructure.Storage
{
    public interface IValueStorer<TKey,TValue>
    {
        void Write(TKey id, TValue value);
        TValue Read(TKey id, TValue defaultValue);
    }
}