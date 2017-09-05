using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.EventSystems;

namespace IventorySystem
{
    [LuaCallCSharp]
    public class Bag : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        // Use this for initialization
        void Start()
        {
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

        public bool addNewItem(Item itm)
        {
            return false;
        }
    }
}