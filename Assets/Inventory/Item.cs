using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.EventSystems;
//using UnityEngine.Events;

namespace IventorySystem
{
    [LuaCallCSharp]
    public class Item : MonoBehaviour,IBeginDragHandler,IPointerUpHandler,IDragHandler
    {
        public Slot slot = null;
        public Slot currentOverSlot = null;
        public bool inBag = false;
        private bool floating = false;
        private bool homing = false;
        private bool destroyed = false;
        private RectTransform rcTransform;
        //private MouseManager manager = null;
        private CanvasGroup canvasGroup;
        private Transform backGround;

        private void Awake()
        {
            rcTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Use this for initialization
        void Start()
        {

            //manager = FindObjectOfType<MouseManager>();

            slot = gameObject.GetComponentInParent<Slot>();
            currentOverSlot = slot;
            if (GetComponentInParent<Bag>() != null)
                inBag = true;

        }

        // Update is called once per frame
        void Update()
        {
            //if (!destroyed)
            //{
            //    //if (!homing && floating)
            //    //{
            //    //    Vector3 FinalPos = Input.mousePosition;
            //    //    FinalPos.x -= Screen.width / 2;
            //    //    FinalPos.y -= Screen.height / 2;
            //    //    rcTransform.localPosition = FinalPos;
            //    //}
            //    if (homing)
            //    {
            //        LayDown(slot);
            //    }
            //}
            //else
            //{
            //    UpdateDestroyItem();
            //}
        }

        //前往新的槽位
        public void flyTo(Slot newslot)
        {
            FloatUp();
            slot = newslot;
            homing = true;
            LayDown(slot);
        }
        //返回原先的槽位
        public void returnBack()
        {
            if(floating&&!homing)
            {
                homing = true;
                LayDown(slot);
            }
        }
        //销毁该图标
        public void destroyItem()
        {
            destroyed = true;
            UpdateDestroyItem();
        }

        //物品进入可移动的漂浮状态
        public void FloatUp()
        {
            if (!floating)
            {
                Canvas back = GetComponentInParent<Canvas>();
                if(back != null)
                {
                    backGround = back.gameObject.GetComponent<RectTransform>();
                    transform.SetParent(backGround);
                }

                if(slot != null)
                {
                    slot.owningItem = null;
                    slot.bag.getOccupiedSlots().Remove(slot);
                    slot.bag.getAvailableSlots().Add(slot);
                }
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
                rcTransform.offsetMin = new Vector2(0.0f, 0.0f);
                rcTransform.offsetMax = new Vector2(0.0f, 0.0f);
                rcTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                newslot.owningItem = this;
                slot = newslot;
                slot.bag.getAvailableSlots().Remove(slot);
                slot.bag.getOccupiedSlots().Add(slot);
                currentOverSlot = newslot;
                canvasGroup.blocksRaycasts = true;
                inBag = true;
                floating = false;
                homing = false;
            }
        }

        public bool bFloating()
        {
            return floating; 
        }

        private void UpdateDestroyItem()
        {
            if (!floating && slot != null)
            {
                slot.owningItem = null;
            }
            GameObject.Destroy(this.gameObject);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            MouseManager.managerInstance.OnDragBeginEvent.Invoke(this);//开始拖拽时通知MouseManager
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (floating)
            {
                if (inBag)
                {
                    if (currentOverSlot == null)
                    {
                        homing = true;
                        LayDown(slot);
                    }
                    else
                    {
                        if (currentOverSlot != slot)
                        {
                            MouseManager.managerInstance.OnDropEvent.Invoke(this, currentOverSlot);//放入新槽位时通知MouseManager
                        }
                        else
                        {
                            homing = true;
                            LayDown(slot);
                        }
                    }
                }
                else
                {
                    MouseManager.managerInstance.OnDiscardEvent.Invoke(this);//丢弃时通知MouseManager
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!homing && floating)
            {
                Vector3 FinalPos = Input.mousePosition;
                FinalPos.x -= Screen.width / 2;
                FinalPos.y -= Screen.height / 2;
                rcTransform.localPosition = FinalPos;
            }
        }
    }
}