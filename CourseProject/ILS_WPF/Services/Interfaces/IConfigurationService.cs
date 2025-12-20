namespace ILS_WPF.Services.Interfaces
{
    public interface IConfigurationService<T>
    {
        public T Configuration { get; }
        public Task SaveAsync();
        public Task LoadAsync();
        public void Reset();
    }
}
