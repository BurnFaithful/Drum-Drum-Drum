using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    public class KeyConfig
    {
        public KeyCode[] keys;

        public KeyConfig() { }

        public KeyConfig(params KeyCode[] keys)
        {
            this.keys = keys;
        }
    }

    private KeyConfig keySetter;
    public KeyConfig KeySetter { get { return keySetter; } set { keySetter = value; } }
    private KeyConfig keySetter_test;
    public KeyConfig KeySetter_test { get { return keySetter_test; } set { keySetter_test = value; } }

    protected override void Initialize()
    {
        //keySetter = new KeyConfig(KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K);
        keySetter = new KeyConfig(KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L);
    }

    private void Awake()
    {
        this.Initialize();
    }

    public bool AnyKeyDown()
    {
        for (int i = 0; i < keySetter.keys.Length; ++i)
        {
            if (Input.GetKeyDown(keySetter.keys[i]))
                return true;
        }
        return false;
    }

    public List<KeyCode> GetDownKeyList()
    {
        List<KeyCode> downKeyList = new List<KeyCode>();

        for (int i = 0; i < keySetter.keys.Length; ++i)
        {
            if (Input.GetKeyDown(keySetter.keys[i]))
                downKeyList.Add(keySetter.keys[i]);
        }

        return downKeyList;
    }

    public bool GetKeyDown(KeyCode inputKey)
    {
        Debug.LogWarning("등록되지 않은 입력 키의 사용.");
        return false;
    }
}
