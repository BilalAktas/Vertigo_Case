using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Level_SlotItems
{
    public int level;
    public SlotItemProperties[] slotItemProperties;
}

[CreateAssetMenu(fileName = "Slot Properties", menuName = ("Objects/Slot Properties"))]
public class SlotProperties : ScriptableObject
{
    [SerializeField] private Level_SlotItems[] levelSlotItems;
    public Level_SlotItems[] LevelSlotItems => levelSlotItems;
}
