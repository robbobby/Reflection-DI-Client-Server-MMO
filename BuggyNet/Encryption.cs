namespace BuggyNet {
    public class Encryption {
        public static string GetHashPassword(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool ValidatePassword(string password, string correctHash) {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}