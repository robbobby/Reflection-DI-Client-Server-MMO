using System.Collections.Generic;
using System.Data;
using System.Linq;
using BuggyNet;
using Dapper;
using MasterServer.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace MasterServer.Business {
    class UserRepository : IUserRepository {

        // private readonly IConfiguration configuration;
        private readonly string connectionString;
        private IDbConnection connection => new MySqlConnection(connectionString);
        private readonly ILogger<IUserRepository> logger;
        private IConfiguration configuration;

        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger) {
            this.configuration = configuration;
            this.logger = logger;
            connectionString = configuration.GetConnectionString("MySQL");
            // connection.Open();
        }

        public UserModel GetUser(int id) {
            return null;
        }

        public List<UserModel> GetAllUsers() {
            return null;
        }

        public void AddUser(string username, string password, string emailAddress) {
            using (IDbConnection dbConnection = connection) {
                string query = @"INSERT INTO Users (user, password, email) 
                   VALUES (@username, @password, @emailAddress)})";
                var result = dbConnection.Execute(query, 
                    new { Username = username, Password = Encryption.GetHashPassword(password), EmailAdress = emailAddress });
            }
        }

        public void DeleteUser(int id) {
        }

        public void UpdateUser(UserModel user) {
        }

        public (bool success, IEnumerable<CharacterModel> charList) GetCharacterList(int userId) {
            return (false, null);
        }
        
                        /* TODO: Combine PasswordOk and UserExists into 1 api call */

        public (bool, int) PasswordOk(string username, string password) {
            using (IDbConnection dbConnection = connection) {
                const string query = @"SELECT * FROM User WHERE username=@username AND active='1'";
                dbConnection.Open();
                UserModel user = dbConnection.Query<UserModel>(query, new {Username = username}).FirstOrDefault();
                bool validPassword = Encryption.ValidatePassword(password, user.Password);
                    
                if (validPassword) {
                    logger.LogInformation("Password is correct");
                    return (true, user.Id);
                }
                logger.LogInformation("Password is Incorrect");
                return (false, -1);
            }
        }

        public (bool, bool, int) UserExists(string username, string password) {
        logger.LogCritical(connectionString);
        using (IDbConnection dbConnection = connection) {
            // const string query = @"SELECT * FROM User WHERE username=@username AND active='1'";
        //         UserModel user = dbConnection.Query<UserModel>(query, new {Username = username}).FirstOrDefault();
        //
        //         if (user?.Username == null) {
        //             logger.LogInformation("Username doesn't exist");
        //             return (false, false, -1);
        //         } else {
        //             logger.LogInformation("Username exists");
        //             return PasswordOk(user, password);
        //         }
        }
        return (false, false, 1);
        }
        private (bool, bool, int) PasswordOk(UserModel user, string password) {
            bool validPassword = Encryption.ValidatePassword(password, user.Password);
            
            if (validPassword) {
                logger.LogInformation("Password is correct");
                return (true, true, user.Id);
            }
            logger.LogInformation("Password is Incorrect");
            return (true, false, -1);
        return (false, false, 1);
        }
    }
}