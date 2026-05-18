namespace EF_Interceptors.Entities.Contracts
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public DateTime? TimeDeleted { get; set; }

        public abstract void Delete();
        public abstract void UndoDelete();
    }
}
