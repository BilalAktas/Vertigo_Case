using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    #region GARBAGE

    private const float DotTwo = .2f;

    #endregion
    
    private Image itemImage;
    private TextMeshProUGUI itemAmountText;
    private SlotItemProperties slotItemProperties;

    public SlotProperties SlotProperties;
    
    private void Start()
    {
        itemImage = transform.GetChild(0).GetComponentInChildren<Image>();
        itemAmountText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        
    }

    public void Equip(SlotItemProperties _slotItemProperties)
    {
        slotItemProperties = _slotItemProperties;

        itemImage.sprite = slotItemProperties.Sprite;
        itemAmountText.text = "x" + slotItemProperties.Amount;
    }

    public void ScaleState(Vector2 _scale)
    {
        transform.DOScale(_scale, DotTwo).SetEase(Ease.Linear);
    }
}
