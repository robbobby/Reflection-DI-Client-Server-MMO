using System;

namespace BuggyNet.Network.Packages {
    public enum PackageIds : UInt32 {
        LoginRequest = 0x001,
        LoginResponse = 0x002,
        
        CharacterClassRequest = 0x003,
        CharacterClassResponse = 0x004,
        
        CharRequest = 0x005,
        CharResponse = 0x006,
        
        KeepAlive = 0xFFFE,
        Error = 0xFFFF,
    }
}