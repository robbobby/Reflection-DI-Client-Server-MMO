namespace MasterServer.Models.User {
    public class CharacterModel {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Level { get; set; }
        public string CharacterName { get; set; }
        public int Class { get; set; }
    }
}