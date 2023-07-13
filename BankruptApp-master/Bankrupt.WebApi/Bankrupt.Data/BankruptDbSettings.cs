namespace Bankrupt.Data
{
    public class BankruptDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string DocumentsCollectionName { get; set; } = null!;
    }
}
