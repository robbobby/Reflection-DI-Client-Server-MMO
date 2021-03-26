using System.IO;
using BuggyNet.Network.PackageParser;

namespace BuggyNet.Network.Packages {
    [PackageRpc(PackageIds.CharacterClassRequest)]
    public class CharacterClass : Package {
        public int CharacterClassId { get; set; }
        
        public CharacterClass() : base(PackageIds.CharacterClassRequest) {
        }
        public override void DeserialiseFromStream(BinaryReader reader) {
            CharacterClassId = reader.ReadInt16();
        }
        public override void SerialiseToStream(BinaryWriter writer) {
            writer.Write((uint)base.PackageId);
            writer.Write(CharacterClassId);
        }
    }

    [PackageRpc(PackageIds.CharacterClassResponse)]
    public class CharacterClassResponse : Package {
        public int CharacterClassId { get; set; }

        public CharacterClassResponse() : base(PackageIds.CharacterClassResponse) {
        }
        public override void DeserialiseFromStream(BinaryReader reader) {
            CharacterClassId = reader.ReadInt16();
        }
        public override void SerialiseToStream(BinaryWriter writer) {
            writer.Write((uint)PackageId);
            writer.Write(CharacterClassId);
        }
    }
}
