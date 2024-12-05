using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum ButtonType
{
    Exit,
    GiveUp,
    WatchAD,
    CoinRevive
}

public class ButtonsClickDetector : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ButtonType ButtonType;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (ButtonType)
        {
            case ButtonType.Exit:
                SceneManager.LoadSceneAsync(0);
                break;
            case ButtonType.GiveUp:
                SceneManager.LoadSceneAsync(0);
                break;
            case ButtonType.WatchAD:
                FakeAD_UIManager.Instance.ShowAd();
                break;
            case ButtonType.CoinRevive:
                GameManager.Instance.CoinRevive();
                break;
        }
    }
}
