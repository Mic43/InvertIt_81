namespace Infrastructure.Storage
{
    public interface IFiniteTypeStorer<TKey, TValue>
    {
        void Write(TKey key, TValue value);
        TValue Read(TKey key);
    }
}