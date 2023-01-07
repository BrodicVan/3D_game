using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    public int slotID;
    public Item slotItem;
    public Image itemImage;
    public Text slotNum;
    public GameObject itemInSlot;

    public void SetupSlot(Item item)
    {
        if(item==null)
        {
            itemInSlot.SetActive(false);
            
            return;
        }
        itemImage.sprite = item.itemImage;
        slotNum.text = item.itemNum==1?"":item.itemNum.ToString();
    }
}
