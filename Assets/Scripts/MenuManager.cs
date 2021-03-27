using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public void ClassSelectionMenu() { SceneManager.LoadScene("ClassSelection", LoadSceneMode.Single); }

    public void CharacterSelectionMenu() { SceneManager.LoadScene("CharacterSelection", LoadSceneMode.Single); }

    public void CharacterCreationMenu() { SceneManager.LoadScene("CharacterCreation", LoadSceneMode.Single); }


}