using System.IO;
using BuggyNet.PackageParser;

namespace BuggyNet.Package {
    [PackageRpc(PackageIds.CharacterClassRequest)]
    public class CharacterClass : PackageParser.Package {
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
    public class CharacterClassResponse : PackageParser.Package {
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
