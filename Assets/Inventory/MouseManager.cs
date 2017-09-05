using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.Events;

namespace IventorySystem
{
    [LuaCallCSharp]
    public class MouseManager : MonoBehaviour
    {
        [LuaCallCSharp]
        public class DropEvent : UnityEvent<Item, Slot> { }

        [LuaCallCSharp]
        public class ItemEvent : UnityEvent<Item> { }

        public ItemEvent OnDragBeginEvent = new ItemEvent();                //开始拖拽
        public ItemEvent OnDiscardEvent = new ItemEvent();                  //丢弃
        public DropEvent OnDropEvent = new DropEvent();                     //放置到新槽位

        public static MouseManager managerInstance = null;

        //public Item DraggingItem = null;
        //public Slot CurrentSlot = null;
        //public bool InBag = false;

        private void Awake()
        {
            managerInstance = this;
            enabled = false;
        }

        // Use this for initialization
        void Start()
        { }

        // Update is called once per frame
        void Update()
        { }

        public void OnItemDragBegin(Item itm)
        {
            OnDragBeginEvent.Invoke(itm);                                   //开始拖拽
        }

        public void OnItemDiscard(Item itm)
        {
            OnDiscardEvent.Invoke(itm);                                     //丢弃
        }

        public void OnItemDrop(Item draggingItem,Slot dstSlot)
        {
            OnDropEvent.Invoke(draggingItem, dstSlot);                      //放置到新槽位
        }

        public static MouseManager GetManager()
        {
            return FindObjectOfType<MouseManager>();
        }
    }
}
