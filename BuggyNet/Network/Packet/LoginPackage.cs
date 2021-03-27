using System.IO;
using BuggyNet.Network.PackageParser;

namespace BuggyNet.Network.Packages {
    [PackageRpc(PackageIds.LoginRequest)]
    public class LoginRequest : Package {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginRequest() : base(PackageIds.LoginRequest) { }
        public override void DeserialiseFromStream(BinaryReader reader) {
            Username = reader.ReadString();
            Password = reader.ReadString();
        }
        public override void SerialiseToStream(BinaryWriter writer) {
            writer.Write((uint)PackageId);
            writer.Write(Username);
            writer.Write(Password);
        }
    }
    
    
    [PackageRpc(PackageIds.LoginResponse)]
    public class LoginResponse : Package {

        public bool IsValidLogin;
        
        public LoginResponse() : base(PackageIds.LoginResponse) { }

        public override void DeserialiseFromStream(BinaryReader reader) {
            IsValidLogin = reader.ReadBoolean();
        }
        public override void SerialiseToStream(BinaryWriter writer) {
            writer.Write((uint)PackageId);
            writer.Write(IsValidLogin);
        }

    }
}
