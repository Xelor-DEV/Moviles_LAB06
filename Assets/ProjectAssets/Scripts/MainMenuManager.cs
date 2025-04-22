using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string shipSelectionScene;

    public void LoadSelectionScene()
    {
        SceneManager.LoadScene(shipSelectionScene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}