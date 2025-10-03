using FMODUnity;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] GameObject _popupPanel;
    [SerializeField] EventReference _menuInteractionSound;
    public void OpenPopup()
    {
        _popupPanel.SetActive(true);
        RuntimeManager.PlayOneShot(_menuInteractionSound);
    }
    public void ClosePopup()
    {
        _popupPanel.SetActive(false);
    }
}