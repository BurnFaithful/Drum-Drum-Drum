using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSceneName
{
    SCENE_TITLE,
    SCENE_SELECT,
    SCENE_LOADING,
    SCENE_MAINGAME,
    SCENE_RESULT
}

public enum eDirection
{
    LEFT = 1 << 0,
    RIGHT = 1 << 1,
    UP = 1 << 2,
    DOWN = 1 << 3
}

public static class GlobalDefine {
    
    #region Global_Const_Value
    public const string RESOURCE_BASE_PATH = "BMSFiles";
    public const int SCREEN_WIDTH = 1920;
    public const int SCREEN_HEIGHT = 1080;
    public const int STD_BEAT = 4;
    public const int BEAT_PER_BAR = 16;

    public static readonly string[] AVAILABLE_BMS_EXTENSIONS = { ".bms", ".bme" };
    public static readonly string[] AVAILABLE_AUDIO_EXTENSIONS = { ".ogg", ".wav", ".mp3" };

    public static readonly Color ALPHA_ZERO = new Color(1, 1, 1, 0);
    public static readonly Color ALPHA_MAX = new Color(1, 1, 1, 1);
    #endregion

    #region GamePlay_Static_Data
    // BMS의 데이터는 실제 노트 라인과 인덱스(라인넘버) 값이 달라 매칭이 필요
    public static Dictionary<int, int> noteLineMatch = new Dictionary<int, int>() 
    {
        {1, 1},
        {2, 2},
        {3, 3},
        {4, 4},
        {5, 5},
        {6, 0},
        {8, 6},
        {9, 7}
    };
    public static readonly short KEY_NUM;
    public static readonly float NOTE_WIDTH;
    public static readonly float NOTE_HEIGHT;
    public static readonly float JUDGELINE_POS_Y;
    #endregion

    static GlobalDefine()
    {
        KEY_NUM = 8;
        NOTE_WIDTH = 0.86f;
        NOTE_HEIGHT = 0.3f;
        JUDGELINE_POS_Y = -2.3f;
    }
}
