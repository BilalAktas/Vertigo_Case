using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    #region Variables

    [SerializeField] private GameObject loseScreen;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;

    [SerializeField] private Image spinButton;
    private const float SpinButtonActiveDelay = .5f;

    #endregion
    
    #region Methods

    private void Start()
    {
        //Application.targetFrameRate = 60;
        
        Invoke(nameof(ActiveSpinButton), SpinButtonActiveDelay);
    }

    private void ActiveSpinButton() => spinButton.enabled = true;


    public void HandleSpinResult(SlotItemProperties currentSlotItem)
    {
        if (currentSlotItem.Bomb)
        {
            loseSound.Play();
            loseScreen.SetActive(true);
        }
        else
        {
            winSound.Play();
            SpinLevelManager.Instance.IncreaseLevel();
            CollectedItemsManager.Instance.AddItem(currentSlotItem);
        }
    }

    public void Ad_Force()
    {
        loseScreen.SetActive(false);
        SpinLevelManager.Instance.IncreaseLevel();
        SlotManager.Instance.SetSlots();
    }

    public void CoinRevive()
    {
        loseScreen.SetActive(false);
        SpinLevelManager.Instance.IncreaseLevel();
        SlotManager.Instance.SetSlots();
    }

    #endregion
    
}
