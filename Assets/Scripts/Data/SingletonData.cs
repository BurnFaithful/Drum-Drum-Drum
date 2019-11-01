using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonData : Singleton<SingletonData>
{
    public BMSMusicInfo selectedMusicInfo;
    public JudgeScore resultData;
}
