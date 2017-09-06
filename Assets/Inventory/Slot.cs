using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.EventSystems;

namespace IventorySystem
{
    [LuaCallCSharp]
    public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Item owningItem = null;
        public Bag bag = null;
        //private MouseManager manager = null;

        // Use this for initialization
        void Start()
        {
            owningItem = GetComponentInChildren<Item>();
            bag = GetComponentInParent<Bag>();
            if(bag == null)
            {
                bag = GetComponent<Bag>();
            }
            if(bag != null)
            {
                if(owningItem != null)
                {
                    bag.getOccupiedSlots().Add(this);
                }
                else
                {
                    bag.getAvailableSlots().Add(this);
                }
            }
            //manager = FindObjectOfType<MouseManager>();
        }

        // Update is called once per frame
        void Update()
        { }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //
            if (eventData.pointerDrag == null)
                return;

            Item itm = eventData.pointerDrag.GetComponent<Item>();
            if(itm != null&&itm.bFloating())
            {
                itm.currentOverSlot = this;
            }
            //
            //manager.CurrentSlot = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //
            if (eventData.pointerDrag == null)
                return;

            Item itm = eventData.pointerDrag.GetComponent<Item>();
            if (itm != null && itm.bFloating())
            {
                itm.currentOverSlot = null;
            }
            //
            //manager.CurrentSlot = null;
        }
    }
}
