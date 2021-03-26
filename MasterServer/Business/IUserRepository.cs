using System.Collections.Generic;
using MasterServer.Models.User;

namespace MasterServer.Business {
    public interface IUserRepository {
        UserModel GetUser(int id);
        List<UserModel> GetAllUsers();
        void AddUser(string username, string password, string emailAddress);
        void DeleteUser(int id);
        void UpdateUser(UserModel user);
        (bool success, IEnumerable<CharacterModel> charList) GetCharacterList(int userId);
        (bool, int) PasswordOk(string username, string password);
        bool UserExists(string username);
    }
}