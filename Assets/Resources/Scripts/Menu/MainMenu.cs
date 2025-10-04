using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    [SerializeField] BaseMenu _optionsMenu;
    public void PlayGame()
    {
        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);
        _optionsMenu.OpenMenu();
    }

    public void OpenURL(string url)
    {
        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);
        Application.OpenURL(url);
    }

    public override void OpenMenu()
    {
    }

    public override void CloseMenu()
    {
    }
}
