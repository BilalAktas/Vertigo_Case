using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectedItemsManager : Singleton<CollectedItemsManager>
{
    #region GARBAGE

    private readonly Vector3 CollectedItemPunchScale = new Vector3(.5f, .5f, .5f);
    private const float CollectedAnimDuration = .35f;
    private const float DotFive = .5f;
    private const float DotZeroFive = .05f;
    private const float DotTwo = .2f;
    private const float Zero = 0f;
    private const float InitialPositionOffsetX = -30f;
    
    #endregion

    #region Variables

    [Header("Collect")]
    [SerializeField] private GameObject collectedItemAnimObject;
    [SerializeField] private Image[] anim_CollectedItemImages;
    [SerializeField] private Animation collectedItemEffectAnim;
    [SerializeField] private GameObject collectedItemPrefab;
    
    private Dictionary<SlotItemProperties, Transform> collectedItemsDict = new();
    private VerticalLayoutGroup verticalLayoutGroup;
    private List<RectTransform> anim_CollectedItemRects = new();
    
    #endregion

    #region Methods

    private void Start()
    {
        foreach (var _aImage in anim_CollectedItemImages)
            anim_CollectedItemRects.Add(_aImage.GetComponent<RectTransform>());

        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
    }
    
    
    #region Item_Actions

    public void AddItem(SlotItemProperties _item)
    {
        if (collectedItemsDict.ContainsKey(_item))
            UpdateExistingItem(_item, _item.Amount);
        else
        {
            foreach (var _item_ in collectedItemsDict)
            {
                if (_item_.Key.ItemType == _item.ItemType)
                {
                    UpdateExistingItem(_item_.Key, _item.Amount);
                    return;
                }
            }
            
            AddNewCollectedItem(_item);
        }
    }

    private void UpdateExistingItem(SlotItemProperties _item, int amount)
    {
        var itemTransform = collectedItemsDict[_item];
        StartCoroutine(CollectedItemEffectCoroutine(_item, itemTransform, itemTransform.GetChild(0)));
        //Debug.Log("Amount: " + _item.Amount + "   " + "item. " + _item.name);
        UpdateItemAmount(itemTransform, amount);
    }

    private void UpdateItemAmount(Transform itemTransform, int amountToAdd)
    {
        var amountText = itemTransform.GetChild(1).GetComponent<TextMeshProUGUI>();
        var currentAmount = int.Parse(amountText.text);
        
        amountText.text = (currentAmount + amountToAdd).ToString();
    }
    
    private void AddNewCollectedItem(SlotItemProperties _item)
    {
        var _clone = Instantiate(collectedItemPrefab, transform);
        var image = _clone.transform.GetChild(0).GetComponent<Image>();
        image.sprite = _item.Sprite;
        var amountText = _clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        amountText.text = _item.Amount.ToString();
        
        collectedItemsDict[_item] = _clone.transform;

        StartCoroutine(CollectedItemEffectCoroutine(_item, _clone.transform, _clone.transform.GetChild(0)));

        verticalLayoutGroup.spacing = verticalLayoutGroup.spacing < -270f ? verticalLayoutGroup.spacing + 20f : -200f;
    }
    
    
    private IEnumerator CollectedItemEffectCoroutine(SlotItemProperties _item, Transform _clonedCollectedItem, Transform _image)
    {
        foreach (var _cImage in anim_CollectedItemImages)
        {
            _cImage.sprite = _item.Sprite;
        }
        
        collectedItemAnimObject.SetActive(true);
        collectedItemEffectAnim.Play();

        yield return new WaitForSeconds(CollectedAnimDuration);
        
        var _pos = _clonedCollectedItem.GetComponent<RectTransform>().position + new Vector3(InitialPositionOffsetX, 0);
        collectedItemAnimObject.transform.DOMove(_pos, DotFive);
        
        foreach (var _cRect in anim_CollectedItemRects)
            _cRect.DOLocalMove(Vector2.zero, DotTwo);

        yield return new WaitForSeconds(DotFive);
        
        collectedItemAnimObject.transform.DOScale(Vector2.zero, DotZeroFive).OnComplete(() =>
        {
            _image.transform.DOPunchScale(CollectedItemPunchScale, DotFive, 0, 0).SetEase(Ease.Linear);
        });

        yield return new WaitForSeconds(DotTwo);

     
        collectedItemAnimObject.SetActive(false);
        ResetCollectedItemAnimationObject();


        SlotManager.Instance.SetSlots();
    }

    private void ResetCollectedItemAnimationObject()
    {
        collectedItemAnimObject.transform.localScale = Vector2.one;
        collectedItemAnimObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(290.5f, 85f, 0);
        
        foreach (var _cRect in anim_CollectedItemRects)
        {
            _cRect.localScale = Vector2.one;
            _cRect.anchoredPosition = Vector2.zero;
        }
    }

    #endregion

    
    
    #endregion
    
}
