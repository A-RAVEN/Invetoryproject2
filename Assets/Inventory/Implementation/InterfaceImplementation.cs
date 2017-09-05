using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using IventorySystem;

//C#的接口实现
public class InterfaceImplementation : MonoBehaviour
{
    MouseManager manager = null;

    public UnityAction<Item> BeginDragAction;
    public UnityAction<Item, Slot> DropAction;
    public UnityAction<Item> DiscardAction;

    // Use this for initialization
    void Start()
    {
        BeginDragAction = new UnityAction<Item>(OnItemDragFunc);
        DiscardAction = new UnityAction<Item>(OnItemDiscardFunction);
        DropAction = new UnityAction<Item, Slot>(OnItemDropFunction);
        manager = MouseManager.GetManager();
        if (manager != null)
        {
            manager.OnDragBeginEvent.AddListener(BeginDragAction);
            manager.OnDropEvent.AddListener(DropAction);
            manager.OnDiscardEvent.AddListener(DiscardAction);
        }
    }

    // Update is called once per frame
    void Update()
    { }

    public void OnItemDragFunc(Item itm)                                //开始拖拽回调函数
    {
        if (true)                   //一些判断条件，物品是否能拖动
        {
            itm.FloatUp();
        }
    }

    public void OnItemDiscardFunction(Item itm)                         //丢弃回调函数
    {
        if (true)                   //一些判断条件，物品是否能丢弃或销毁
        {
            itm.destroyItem();
        }
        else
        {
            itm.returnBack();
        }
    }

    public void OnItemDropFunction(Item itm, Slot slot)                 //放置到新槽位回调函数
    {
        if (true)                   //一些判断条件，物品是否能够放入这个槽位
        {
            if (slot.owningItem != null)
            {
                if (true)           //一些判断条件,两个槽位内的物品是否能交换
                {
                    slot.owningItem.flyTo(itm.slot);
                    itm.flyTo(slot);
                }
                else
                {
                    itm.returnBack();
                }
            }
            else
            {
                itm.flyTo(slot);
            }
        }
        else
        {
            itm.returnBack();
        }
    }

}
