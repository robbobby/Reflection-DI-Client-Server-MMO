using System.IO;
using BuggyNet.Network.PackageParser;

namespace BuggyNet.Network.Packages {
    [PackageRpc(PackageIds.LoginRequest)]
    public class LoginRequest : Package {
        private string username;
        private string password;
        
        public LoginRequest() : base(PackageIds.LoginRequest) { }
        public override void DeserialiseFromStream(BinaryReader reader) {
            username = reader.ReadString();
            password = reader.ReadString();
        }
        public override void SerialiseToStream(BinaryWriter writer) {
            writer.Write((uint)PackageId);
            writer.Write(username);
            writer.Write(password);
        }
    }
    
    
    [PackageRpc(PackageIds.LoginResponse)]
    public class LoginResponse : Package {

        private bool isValidLogin;
        
        public LoginResponse() : base(PackageIds.LoginResponse) { }

        public override void DeserialiseFromStream(BinaryReader reader) {
            isValidLogin = reader.ReadBoolean();
        }
        public override void SerialiseToStream(BinaryWriter writer) {
            writer.Write((uint)PackageId);
            writer.Write(isValidLogin);
        }

    }
}
