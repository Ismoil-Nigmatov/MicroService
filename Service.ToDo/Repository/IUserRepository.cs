using Service.ToDo.Entity;

namespace Service.ToDo.Repository
{
    public interface IUserRepository
    {
        System.Threading.Tasks.Task RegistrationAsync(User user);
        Task<User> GetUserByEmail(string email);
    }
}
