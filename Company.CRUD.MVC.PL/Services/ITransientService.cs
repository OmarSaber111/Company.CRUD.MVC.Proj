namespace Company.CRUD.MVC.PL.Services
{
    public interface ITransientService
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}
