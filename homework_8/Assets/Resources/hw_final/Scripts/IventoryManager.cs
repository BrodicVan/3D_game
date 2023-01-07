using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IventoryManager : MonoBehaviour
{
    static IventoryManager instance ;
    public Iventory myBag;
    public GameObject slotGrid;
    public GameObject emptySlot;
    public List<GameObject> slots = new List<GameObject>();

    void Awake()
    {
        // if(instance != null)
        // {
        //     Destroy(this);
        // }
        instance = this;
    }
    
    private void OnEnable()
    {
        RefreshItem();
    }

    void Update()
    {
        // RefreshItem();
    }
   

    public static void RefreshItem()
    {
        for(int i = 0; i < instance.slotGrid.transform.childCount;i++)
        {
            if(instance.slotGrid.transform.childCount==0)
            {
                break;
            }
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < instance.myBag.itemList.Count;i++)
        {
            // CreateNewItem(instance.myBag.itemList[i]);
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slotID = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemList[i]);
        }
    }
}
