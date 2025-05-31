namespace Company.CRUD.MVC.PL.Services
{
    public interface ISingletonService
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}
