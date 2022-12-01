using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Item
{
    public string itemName;
    public Sprite itemImage;
    public int itemPrice;
    public int parcelCharges;
    public int preparationTime;
    public int consumingTime;
    public FooditemTypes itemVariety;
    [SerializeField] string _cumma;
    public string cumma { get;}
}
public class FoodList : MonoBehaviour
{
    public Item[] AllItems;
}

public enum FooditemTypes
{
    dosa,
    idli,
    vada,
    VaazhakkaBajji,
    PaavBajji,
    Paanipuri,
    puri,
    ChickenFriedRice,
    MushroomSoup,
    Paaya
}
