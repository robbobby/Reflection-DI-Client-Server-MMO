using DefaultNamespace;
using Microsoft.Extensions.DependencyInjection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using BuggyNet.Package;
using BuggyNet.PackageParser;

public class ClientManager : MonoBehaviour {
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text characterClassIdField;

    private readonly IPackageParser packageParser;
    private readonly ClientConnection clientConnection;
    private readonly MenuManager menuManager;

    public ClientManager() {
        clientConnection = GameContext.ServiceProvider.GetRequiredService<ClientConnection>();
        packageParser = GameContext.ServiceProvider.GetRequiredService<IPackageParser>();
    }

    public void Execute() {
        Debug.Log("Build Connection");
        MenuManager menuManager = GetComponentInChildren<MenuManager>(true);
        clientConnection.Connect("127.0.0.1", 3456);
        string username = usernameInput.text;
        string password = passwordInput.text;
        
        packageParser.ParsePackageToStream(new LoginRequest() 
            {Username = username, Password = password}, clientConnection.Writer);
        
        Debug.Log("Waiting to receive Login Response Package");
        // Package packageData = packageParser.ParsePackageFromStream(clientConnection.Reader);
        // Debug.Log($"Receive login response packages Type: {packageData.GetType()} RESULT: {(packageData as LoginResponse).IsValidLogin}");
    }

    private void Start() {
        DontDestroyOnLoad(this);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
    }

    public void CharacterClass() {
        Debug.Log("Writer character package");
        const int characterId = 1;
        packageParser.ParsePackageToStream(
            new CharacterClass() {CharacterClassId = characterId}, clientConnection.Writer);
        
        Debug.Log("Receive CharacterClass Response package");
        CharacterClassResponse packageData = (CharacterClassResponse) packageParser.ParsePackageFromStream(clientConnection.Reader);
        Debug.Log($"Received CharacterClassResponse package TYPE:{packageData.GetType()} Result: {packageData.CharacterClassId}");
        characterClassIdField.text = packageData.CharacterClassId.ToString();

        bool charExists = false;
        MenuManager menuManager = GetComponentInChildren<MenuManager>(true);
        if (charExists) {
            menuManager.CharacterSelectionMenu();
        } else {
            menuManager.CharacterCreationMenu();
        }
    }
    
    
}
