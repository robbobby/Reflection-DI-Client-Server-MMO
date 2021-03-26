using System.Collections.Generic;
using System.Data;
using System.Linq;
using BuggyNet.Network;
using Dapper;
using MasterServer.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using NLog;
using ILogger = NLog.ILogger;

namespace MasterServer.Business {
    class UserRepository : IUserRepository {

        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private IDbConnection connection => new MySqlConnection(connectionString);
        private readonly ILogger<UserRepository> logger;
        
        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger) {
            this.configuration = configuration;
            this.logger = logger;
            connectionString = configuration.GetConnectionString("MySQL");
        }

        public UserModel GetUser(int id) {
            throw new System.NotImplementedException();
        }

        public List<UserModel> GetAllUsers() {
            throw new System.NotImplementedException();
        }

        public void AddUser(string username, string password, string emailAddress) {
            throw new System.NotImplementedException();
        }

        public void DeleteUser(int id) {
            throw new System.NotImplementedException();
        }

        public void UpdateUser(UserModel user) {
            throw new System.NotImplementedException();
        }

        public (bool success, IEnumerable<CharacterModel> charList) GetCharacterList(int userId) {
            throw new System.NotImplementedException();
        }
        
                        /* TODO: Combine PasswordOk and UserExists into 1 api call */

        public (bool, int) PasswordOk(string username, string password) {
            using (IDbConnection dbConnection = connection) {
                const string query = @"SELECT * FROM User WHERE username=@username AND active='1'";
                dbConnection.Open();
                UserModel user = dbConnection.Query<UserModel>(query, new {Username = username}).FirstOrDefault();
                if (user == null) {
                    logger.LogInformation("User does not exist");
                    return (false, -1);
                }
                bool validPassword = Encryption.ValidatePassword(password, user.Password);
                    
                if (validPassword) {
                    logger.LogInformation("Password is correct");
                    return (true, user.Id);
                }
                logger.LogInformation("Password is Incorrect");
                return (false, -1);
            }
        }

        public bool UserExists(string username) {
            using (IDbConnection dbConnection = connection) {
                const string query = @"SELECT * FROM User WHERE username=@username AND active='1'";
                dynamic user = dbConnection.Query(query, new {Username = username}).FirstOrDefault();

                if (user?.UserName == null) {
                    logger.LogInformation("Username doesn't exist");
                    return false;
                } else {
                    logger.LogInformation("Username exists");
                    return true;
                }
            }
        }
    }
}