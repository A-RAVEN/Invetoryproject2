using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.EventSystems;

namespace IventorySystem
{
    [LuaCallCSharp]
    public class Bag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private List<Slot> availableSlots = new List<Slot>();
        private List<Slot> occupiedSlots = new List<Slot>();
        GameObject pref = null;
        // Use this for initialization
        void Start()
        {
            pref = (GameObject)Resources.Load("Item");
        }

        // Update is called once per frame
        void Update()
        { }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            Item itm = eventData.pointerDrag.GetComponent<Item>();
            if (itm != null && itm.bFloating())
            {
                itm.inBag = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            Item itm = eventData.pointerDrag.GetComponent<Item>();
            if (itm != null && itm.bFloating())
            {
                itm.inBag = false;
            }
        }

        public Slot[] getSlots()
        {
            ArrayList list = new ArrayList();
            return GetComponentsInChildren<Slot>();

        }

        public List<Slot> getAvailableSlots()
        {
            return availableSlots;
        }

        public List<Slot> getOccupiedSlots()
        {
            return occupiedSlots;
        }

        public void addNewItem(Item itm)
        {
            MouseManager.managerInstance.OnBagAddItemEvent.Invoke(this, itm);
        }

        public void TestButtonAddItem()
        {
            
            GameObject obj = GameObject.Instantiate(pref);
            addNewItem(obj.GetComponent<Item>());
        }
    }
}