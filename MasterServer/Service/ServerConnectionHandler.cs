using BuggyNet.Network;
using BuggyNet.Network.PackageParser;
using BuggyNet.Network.Packages;
using MasterServer.Business;
using MasterServer.Service.ServiceModels;
using Microsoft.Extensions.Logging;

namespace MasterServer.Service {
    public class ServerConnectionHandler : ConnectionHandlerBase<ClientConnection>, IServerConnectionHandler {

        private readonly ILogger<ServerConnectionHandler> logger;
        private readonly IPackageParser packageParser;
        private readonly IUserRepository userRepository;
        public ServerConnectionHandler(ILogger<ServerConnectionHandler> logger, IPackageParser packageParser, IUserRepository userRepository) {
            this.logger = logger;
            this.packageParser = packageParser;
            this.userRepository = userRepository;
        }
        
        protected override void HandleUnknownPacket(ClientConnection connection, object parsedData, PackageIds type) {
            
        }

        [PackageHandler(PackageIds.LoginRequest)]
        public void HandleLoginRequest(ClientConnection connection, LoginRequest parseObjectData) {
            logger.LogInformation("Login Request received");
            logger.LogDebug($"Login DATA user: {parseObjectData.Username} PW: {parseObjectData.Password}");

            (bool userNameExists, bool passwordCorrect, int userId) =
                (userRepository.UserExists(parseObjectData.Username, parseObjectData.Password));
            if (!userNameExists) {
                logger.LogInformation("Username does not exist");
                return;
            }
            if (!passwordCorrect) {
                logger.LogInformation("Incorrect password");
                return;
            }
            packageParser.ParsePackageToStream(new LoginResponse() {IsValidLogin = true}, connection.Writer);
        }

        [PackageHandler(PackageIds.CharacterClassRequest)]
        public void HandleCharacterClassRequest(ClientConnection connection, CharacterClass parsedObjectData) {
            logger.LogInformation($"Character class Request: {parsedObjectData.CharacterClassId}");
            
            packageParser.ParsePackageToStream(new CharacterClassResponse() 
                { CharacterClassId = parsedObjectData.CharacterClassId}, connection.Writer);
            
        }

    }
}
