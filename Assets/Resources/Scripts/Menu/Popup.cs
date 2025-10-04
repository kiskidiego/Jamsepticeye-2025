using FMODUnity;
using UnityEngine;

/// <summary>
/// Manages popup panels in the game.
/// </summary>
public class Popup : MonoBehaviour
{
    [SerializeField] GameObject _popupPanel;
    [SerializeField] EventReference _menuInteractionSound;

    /// <summary>
    /// Opens the popup panel.
    /// </summary>
    public void OpenPopup()
    {
        _popupPanel.SetActive(true);
        RuntimeManager.PlayOneShot(_menuInteractionSound);
    }

    /// <summary>
    /// Closes the popup panel.
    /// </summary>
    public void ClosePopup()
    {
        _popupPanel.SetActive(false);
    }
}