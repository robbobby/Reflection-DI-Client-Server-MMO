using System.IO;
using BuggyNet.PackageParser;

namespace BuggyNet.Package {
    [PackageRpc(PackageIds.LoginRequest)]
    public class PackageLogin {
        public class LoginRequest : PackageParser.Package {
            public string Username { get; set; }
            public string Password { get; set; }
            public LoginRequest() : base(PackageIds.LoginRequest) { }

            public override void DeserialiseFromStream(BinaryReader reader) {
                Username = reader.ReadString();
                Password = reader.ReadString();
            }

            public override void SerialiseToStream(BinaryWriter writer) {
                writer.Write((uint) PackageId);
                writer.Write(Username);
                writer.Write(Password);
            }
        }

        [PackageRpc(PackageIds.LoginResponse)]
        public class LoginResponse : PackageParser.Package {
            public bool IsValidLogin;
            public LoginResponse() : base(PackageIds.LoginResponse) { }
            public override void DeserialiseFromStream(BinaryReader reader) { IsValidLogin = reader.ReadBoolean(); }

            public override void SerialiseToStream(BinaryWriter writer) {
                writer.Write((uint) PackageId);
                writer.Write(IsValidLogin);
            }
        }
    }
}