using System.Collections.Generic;

namespace MasterServer.Models.User {
    public class UserModel {
        public int Id { get; set; }
        public int Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public List<CharacterModel> Characters { get; set; }
    }
}