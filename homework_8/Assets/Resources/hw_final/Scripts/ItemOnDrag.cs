using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Transform originalParant;
    public Iventory myBag;
    public int currentItemID;
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParant = transform.parent;
        currentItemID = originalParant.GetComponent<Slot>().slotID;
        transform.SetParent(transform.root);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        // Debug.Log(originalParant.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        // Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject tar_object = eventData.pointerCurrentRaycast.gameObject;
        
        Debug.Log(tar_object.name);
        if(tar_object.name == "Item Image" || tar_object.name == "number")// 交换物品
        {
            int tar_id = tar_object.GetComponentInParent<Slot>().slotID;
            transform.SetParent(tar_object.transform.parent.parent);
            transform.localPosition = Vector3.zero;

            var tem = myBag.itemList[currentItemID];
            myBag.itemList[currentItemID] = myBag.itemList[tar_id];
            myBag.itemList[tar_id] = tem;

            tar_object.transform.parent.SetParent(originalParant);
            tar_object.transform.parent.localPosition = Vector3.zero;
            
        }
        else if(tar_object.name == "slot(Clone)")// 放到空格子
        {
            int tar_id = tar_object.GetComponentInParent<Slot>().slotID;
            var tem = myBag.itemList[tar_id];
            myBag.itemList[tar_id] = myBag.itemList[currentItemID];
            myBag.itemList[currentItemID] = tem;
            transform.SetParent(tar_object.transform);
            transform.localPosition = Vector3.zero;
        }
        else// 回到原位
        {
            transform.SetParent(originalParant);
            transform.localPosition = Vector3.zero;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
