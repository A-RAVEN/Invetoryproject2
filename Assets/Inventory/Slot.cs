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
        private MouseManager manager = null;

        // Use this for initialization
        void Start()
        {
            owningItem = GetComponentInChildren<Item>();
            manager = FindObjectOfType<MouseManager>();
        }

        // Update is called once per frame
        void Update()
        { }

        public void OnPointerEnter(PointerEventData eventData)
        {
            manager.CurrentSlot = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            manager.CurrentSlot = null;
        }
    }
}
