using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    void Start()
    {

    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToogleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
