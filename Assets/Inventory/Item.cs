﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.EventSystems;
//using UnityEngine.Events;

namespace IventorySystem
{
    [LuaCallCSharp]
    public class Item : MonoBehaviour,IBeginDragHandler,IPointerUpHandler
    {
        public Slot slot = null;
        private bool floating = false;
        private bool homing = false;
        private bool destroyed = false;
        private RectTransform rcTransform;
        private MouseManager manager = null;
        private CanvasGroup canvasGroup;
        private Transform backGround;

        // Use this for initialization
        void Start()
        {
            rcTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            manager = FindObjectOfType<MouseManager>();
            backGround = FindObjectOfType<Canvas>().gameObject.GetComponent<RectTransform>();
            slot = gameObject.GetComponentInParent<Slot>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!destroyed)
            {
                if (!homing && floating)
                {
                    Vector3 FinalPos = Input.mousePosition;
                    FinalPos.x -= Screen.width / 2;
                    FinalPos.y -= Screen.height / 2;
                    rcTransform.localPosition = FinalPos;
                }
                if (homing)
                {
                    LayDown(slot);
                }
            }
            else
            {
                UpdateDestroyItem();
            }
        }

        //前往新的槽位
        public void flyTo(Slot newslot)
        {
            FloatUp();
            slot = newslot;
            homing = true;
        }
        //返回原先的槽位
        public void returnBack()
        {
            if(floating&&!homing)
            {
                homing = true;
            }
        }
        //销毁该图标
        public void destroyItem()
        {
            destroyed = true;
        }

        //物品进入可移动的漂浮状态
        public void FloatUp()
        {
            if (!floating)
            {
                //Transform parentTransforn = FindObjectOfType<Canvas>().gameObject.GetComponent<RectTransform>();
                transform.SetParent(backGround);
                slot.owningItem = null;
                canvasGroup.blocksRaycasts = false;
                floating = true;
            }
        }

        private void LayDown(Slot newslot)
        {
            if (floating && homing)
            {
                Transform parentTransforn = newslot.gameObject.transform;
                transform.SetParent(parentTransforn);
                transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                newslot.owningItem = this;
                slot = newslot;
                canvasGroup.blocksRaycasts = true;
                floating = false;
                homing = false;
            }
        }

        private void UpdateDestroyItem()
        {
            GameObject.Destroy(this.gameObject);
        }

        private MouseManager getMouseManager()
        {
            return manager;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            manager.OnItemDragBegin(this);//开始拖拽时通知MouseManager
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (floating)
            {
                if (getMouseManager().InBag)
                {
                    if (getMouseManager().CurrentSlot == null)
                    {
                        homing = true;
                    }
                    else
                    {
                        if (getMouseManager().CurrentSlot != slot)
                        {
                            manager.OnItemDrop(this, manager.CurrentSlot);//放入新槽位时通知MouseManager
                        }
                        else
                        {
                            homing = true;
                        }
                    }
                }
                else
                {
                    manager.OnItemDiscard(this);//丢弃时通知MouseManager
                }
            }
        }
    }
}