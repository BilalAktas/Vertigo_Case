using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot Item", menuName = "Objects/Slot Item")]
public class SlotItemProperties : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private int amount;
    [SerializeField] private float chanceRate;
    [SerializeField] private bool bomb;
    [SerializeField] private string itemType;
    
    public Sprite Sprite => sprite;
    public float ChanceRate => chanceRate;
    public int Amount => amount;
    public bool Bomb => bomb;
    
    public string ItemType => itemType;
    

    public float CurrentChanceRate;
}
