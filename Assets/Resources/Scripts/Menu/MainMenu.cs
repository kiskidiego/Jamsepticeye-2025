using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] EventReference _pressSound;

    public void PlayButtonSound()
    {
        AudioManager.instance.PlayOneShot(_pressSound, transform.position);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        OptionsMenu.SetActive(true);
        OptionsMenu.GetComponent<OptionsMenu>().OpenOptionsMenu();
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
