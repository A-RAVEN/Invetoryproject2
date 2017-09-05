using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XLua;

[System.Serializable]
public class Injection
{
    public string name;
    public GameObject value;
}

//[LuaCallCSharp]
//public class BasicMono : MonoBehaviour
//{
//    public TextAsset luaScript;
//    public Injection[] injections;

//    internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
//    internal static float lastGCTime = 0;
//    internal const float GCInterval = 1.0f;//1 second 

//    [CSharpCallLua]
//    public delegate void EventDe();

//    private EventDe luaStart;
//    private EventDe luaUpdate;
//    private EventDe luaOnDestroy;

//    private LuaTable scriptEnv;

//    void Awake()
//    {
//        scriptEnv = luaEnv.NewTable();

//        LuaTable meta = luaEnv.NewTable();
//        meta.Set("__index", luaEnv.Global);
//        scriptEnv.SetMetaTable(meta);
//        meta.Dispose();

//        scriptEnv.Set("self", this);
//        foreach (var injection in injections)
//        {
//            scriptEnv.Set(injection.name, injection.value);
//        }

//        luaEnv.DoString(luaScript.text, "LuaBehaviour", scriptEnv);

//        Action luaAwake = scriptEnv.Get<Action>("awake");
//        scriptEnv.Get("start", out luaStart);
//        scriptEnv.Get("update", out luaUpdate);
//        scriptEnv.Get("ondestroy", out luaOnDestroy);

//        if (luaAwake != null)
//        {
//            luaAwake();
//        }
//    }

//    // Use this for initialization
//    void Start()
//    {
//        if (luaStart != null)
//        {
//            luaStart();
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (luaUpdate != null)
//        {
//            luaUpdate();
//        }
//        if (Time.time - BasicMono.lastGCTime > GCInterval)
//        {
//            luaEnv.Tick();
//            BasicMono.lastGCTime = Time.time;
//        }
//    }

//    void OnDestroy()
//    {
//        if (luaOnDestroy != null)
//        {
//            luaOnDestroy();
//        }
//        luaOnDestroy = null;
//        luaUpdate = null;
//        luaStart = null;
//        scriptEnv.Dispose();
//        injections = null;
//    }
//}

[LuaCallCSharp]
public class BasicMono : MonoBehaviour
{
    public TextAsset luaScript;
    public Injection[] injections;
    //public Text txt;
    internal static float lastGCTime = 0;
    internal const float GCInterval = 1;//1 second 
    private XLua.LuaEnv luaEnv;

    private Action luaStart;
    private Action luaUpdate;
    private Action luaFixedUpdate;
    private Action luaOnDestroy;

    private LuaTable scriptEnv;

    private void Awake()
    {
        luaEnv = new XLua.LuaEnv();

        scriptEnv = luaEnv.NewTable();

        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        scriptEnv.Set("mono", this);
        foreach (var injection in injections)
        {
            scriptEnv.Set(injection.name, injection.value);
        }

        luaEnv.DoString(luaScript.text, "LuaBehaviour", scriptEnv);

        Action luaAwake = scriptEnv.Get<Action>("awake");
        scriptEnv.Get("start", out luaStart);
        scriptEnv.Get("update", out luaUpdate);
        scriptEnv.Get("fixedupdate", out luaFixedUpdate);
        scriptEnv.Get("ondestroy", out luaOnDestroy);

        if (luaAwake != null)
        {
            luaAwake();
        }
    }

    // Use this for initialization
    void Start()
    {

        if (luaStart != null)
        {
            luaStart();
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (luaFixedUpdate != null)
        {
            luaFixedUpdate();
        }
    }

    private void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
        if (Time.time - BasicMono.lastGCTime > GCInterval)
        {

            BasicMono.lastGCTime = Time.time;
        }
    }

    private void OnDestroy()
    {
        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaStart = null;
        luaUpdate = null;
        luaFixedUpdate = null;
        luaOnDestroy = null;
        scriptEnv.Dispose();
        injections = null;
    }

    public void reboot()
    {
        luaEnv.DoString(luaScript.text, "LuaBehaviour", scriptEnv);
        luaStart = null;
        scriptEnv.Get("start", out luaStart);
        scriptEnv.Get("update", out luaUpdate);
        scriptEnv.Get("fixedupdate", out luaFixedUpdate);
        scriptEnv.Get("ondestroy", out luaOnDestroy);
        if(luaStart != null)
        {
            luaStart();
        }
    }

    public double getAxisRaw(string p)
    {
        return Input.GetAxisRaw(p);
    }

    public bool luaRayCastWithHit(Ray ray, out RaycastHit hitinfo, float rayLength, int layerMask)
    {
        return Physics.Raycast(ray, out hitinfo, rayLength, layerMask);
    }
}
