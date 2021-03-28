using System.Collections.Generic;
using System.Data;
using MasterServer.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Dapper;

namespace MasterServer.Business {
    public class UserRepositoryORM : IUserRepository {
        private readonly string connectionString;
        private IDbConnection connection => new MySqlConnection(connectionString);
        private readonly ILogger<IUserRepository> logger;
        private IConfiguration configuration;

        public UserRepositoryORM(IConfiguration configuration ,ILogger<UserRepositoryORM> logger) {
            this.configuration = configuration;
            this.logger = logger;
        }
        
        public UserModel GetUser(int id) { throw new System.NotImplementedException(); }
        public List<UserModel> GetAllUsers() { throw new System.NotImplementedException(); }
        public void AddUser(string username, string password, string emailAddress) { throw new System.NotImplementedException(); }
        public void DeleteUser(int id) { throw new System.NotImplementedException(); }
        public void UpdateUser(UserModel user) { throw new System.NotImplementedException(); }
        public (bool success, IEnumerable<CharacterModel> charList) GetCharacterList(int userId) { throw new System.NotImplementedException(); }
        public (bool, int) PasswordOk(string username, string password) { throw new System.NotImplementedException(); }
        public (bool, bool, int) UserExists(string username, string password) { throw new System.NotImplementedException(); }
    }
}