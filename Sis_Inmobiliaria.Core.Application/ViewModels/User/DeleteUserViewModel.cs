namespace Sis_Inmobiliaria.Core.Application.ViewModels.User
{
    public class DeleteUserViewModel : BasicViewModel<int>
    {      
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }
}
