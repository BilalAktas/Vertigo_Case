using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct SlotItemForSpin
{
    public SlotItemProperties slotItemProperty;
    public int id;
    public string itemType;
}

public class SlotManager : Singleton<SlotManager>
{
    #region GARBAGE

    private const float DotTwo = .2f;
    private const float DotThirtyFive = .35f;

    #endregion

    #region Variables

    [SerializeField] private SlotItemProperties bombItemProperty;
    [SerializeField] private SlotItemProperties[] SpecialsFifth;
    [SerializeField] private SlotItemProperties[] SpecialThirtieth;
    [SerializeField] private int loopLevel;

    private List<SlotItemProperties> currentSlotItems = new();
    private Slot[] slots;

    #endregion

    #region Methods

    private void Start()
    {
        slots = GetComponentsInChildren<Slot>();
        SetSlots();
    }

    #endregion

    #region Slots

     public void SetSlots()
    {
        StartCoroutine(SetSlots_CO());
    }

    private void ResetSlots()
    {
        if (currentSlotItems.Count > 0)
        {
            foreach (var _slot in slots)
                _slot.ScaleState(Vector2.zero);
        }        
    }

    private void ShowSlots()
    {
        transform.parent.localEulerAngles=new Vector3(0,0,0);
        if (currentSlotItems.Count > 0)
        {
            foreach (var _slot in slots)
                _slot.ScaleState(Vector2.one);
        }
        
        SpinManager.Instance.Set_OnAgain();
    }

    private void SetSlotChances()
    {
        var s = currentSlotItems.Any(slot => slot.Bomb);
        
        foreach (var _property in currentSlotItems)
            if(!_property.Bomb)
                _property.CurrentChanceRate = !s ? _property.ChanceRate : (100 - bombItemProperty.ChanceRate) / 7;
    }
    
    private IEnumerator SetSlots_CO()
    {
        ResetSlots();

        yield return new WaitForSeconds(DotTwo);
        
        currentSlotItems.Clear();
        foreach (var _slot in slots)
            _slot.Equip(GetSlotItemProperty(_slot.SlotProperties));
        
        yield return new WaitForSeconds(DotThirtyFive);

        ShowSlots();

        SetSlotChances();

    }

    private List<SlotItemProperties> EditAllUsefulSlotProperties(List<SlotItemProperties> allUsefulSlotItemProperties)
    {
        if (SpinLevelManager.Instance.currentLevel % 30 == 0)
        {
            foreach (var _s in SpecialThirtieth)
                allUsefulSlotItemProperties.Add(_s);
        }

        if (SpinLevelManager.Instance.currentLevel % 5 == 0)
        {
            foreach (var _s in SpecialsFifth)
                allUsefulSlotItemProperties.Add(_s);
        }
        else
            allUsefulSlotItemProperties.Add(bombItemProperty);


        return allUsefulSlotItemProperties;
    }
    
    private SlotItemProperties GetSlotItemProperty(SlotProperties slotProperties)
    {
        var _currentLevel = SpinLevelManager.Instance.currentLevel;
        
        var _level = (_currentLevel-1)>=slotProperties.LevelSlotItems.Length ?
            ((_currentLevel -1) % loopLevel) + loopLevel:
            (_currentLevel - 1);
        
        
        var allUsefulSlotItemProperties = EditAllUsefulSlotProperties(slotProperties.LevelSlotItems[_level].slotItemProperties.ToList());
        var _slot = CheckUniqueItemID(allUsefulSlotItemProperties);
        currentSlotItems.Add(_slot);
        
        return _slot;
    }

    public SlotItemForSpin GetSlotItemForSpin()
    {

        var _slotItemForSpin = new SlotItemForSpin();

        // FAIL CON-
        var _ra = Random.Range(0, currentSlotItems.Count);
        _slotItemForSpin.slotItemProperty = currentSlotItems[_ra];
        _slotItemForSpin.id = (_ra+1);
        _slotItemForSpin.itemType = _slotItemForSpin.slotItemProperty.ItemType;
        //
        
        var _r = Random.Range(0f, 100f);
        var _chance = 0f;

        var _id = 1;
        
        foreach (var _slotItem_ in currentSlotItems)
        {
            _chance += _slotItem_.CurrentChanceRate;
            if (_r <= _chance)
            {
                _slotItemForSpin.slotItemProperty = _slotItem_;
                _slotItemForSpin.id = _id;
                _slotItemForSpin.itemType = _slotItem_.ItemType;
                break;
            }

            _id++;
        }

        return _slotItemForSpin;
    }

    private SlotItemProperties CheckUniqueItemID(List<SlotItemProperties> allUsefulSlotItemProperties)
    {
        var _id = 0;
        while (true)
        {
            var r = Random.Range(0, allUsefulSlotItemProperties.Count);
            if (!currentSlotItems.Contains(allUsefulSlotItemProperties[r]))
            {
                _id = r;
                break;
            }
        }
        
     
        return allUsefulSlotItemProperties[_id];
    }

    #endregion
}
