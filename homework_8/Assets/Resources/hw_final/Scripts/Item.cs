using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Iventory/New Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;// 物品名称
    public Sprite itemImage;// 物品图片
    public int itemNum;// 物品数量
}
