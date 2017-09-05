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
        private MouseManager manager = null;

        // Use this for initialization
        void Start()
        {
            manager = FindObjectOfType<MouseManager>();
        }

        // Update is called once per frame
        void Update()
        { }

        public void OnPointerEnter(PointerEventData eventData)
        {
            manager.InBag = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            manager.InBag = false;
        }
    }
}