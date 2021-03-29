using System.Threading.Tasks;
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

    private readonly ClientConnection clientConnection;
    private readonly ClientConnectionDispatcher connectionDispatcher;
    private readonly MenuManager menuManager;

    public ClientManager() {
        clientConnection = GameContext.ServiceProvider.GetRequiredService<ClientConnection>();
        connectionDispatcher = GameContext.ServiceProvider.GetRequiredService<ClientConnectionDispatcher>();
    }

    public void Execute() {
        Debug.Log("Build Connection");
        MenuManager menuManager = GetComponentInChildren<MenuManager>(true);
        clientConnection.Connect("127.0.0.1", 3456);
        connectionDispatcher.Start();
        string username = usernameInput.text;
        string password = passwordInput.text;

        connectionDispatcher.SendPackage(new LoginRequest() { Username = username, Password = password });
        
        Debug.Log("Waiting to receive Login Response Package");
        LoginResponse packageData = connectionDispatcher.WaitForPackage<LoginResponse>().Result; // TODO: automate this
        LogPackageResponse(packageData, packageData.IsValidLogin);
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
        connectionDispatcher.SendPackage(new CharacterClass() {CharacterClassId = characterId});
        
        Debug.Log("Receive CharacterClass Response package");
        CharacterClassResponse packageData = connectionDispatcher.WaitForPackage<CharacterClassResponse>().Result;
        LogPackageResponse(packageData, packageData.CharacterClassId);

        bool charExists = false;
        MenuManager menuManager = GetComponentInChildren<MenuManager>(true);
        if (charExists) {
            menuManager.CharacterSelectionMenu();
        } else {
            menuManager.CharacterCreationMenu();
        }
    }
    private static void LogPackageResponse<T>(Package packageData, T dataToLog) {

        Debug.Log($"Received package from server Type{packageData.PackageId} RESULT: {dataToLog}");
    }


}
