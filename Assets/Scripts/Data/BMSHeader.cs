// 플레이 방식(1채널 1인, 2채널 2인, 2채널 1인, 1채널 2인)
public enum PlayerType
{
    ONE_CHANNEL_ONE_PLAYER = 1,
    TWO_CHANNEL_TWO_PLAYER,
    TWO_CHANNEL_ONE_PLAYER,
    ONE_CHANNEL_TWO_PLAYER
}

[System.Serializable]
public class BMSFilePath
{
    /// <summary>
    /// BMS 파일 경로
    /// </summary>
    public string bmsPath;
    /// <summary>
    /// BMS 파일이 존재하는 디렉토리 경로
    /// </summary>
    public string bmsParentPath;
    /// <summary>
    /// 데이터 로드 중에 보여주는 파일
    /// </summary>
    public string stageFileName; 
}

[System.Serializable]
public class BMSMusicInfo
{
    public BMSFilePath bmsFilePath;
    public string genre;
    public string titleName;
    public string subTitleName;
    public string artistName;
    /// <summary>
    /// 패턴 난이도
    /// </summary>
    public int playLevel;
    public int bpm; // BPM은 게임 플레이에서도 중요한 데이터로 활용되지만 Info Data로 활용되기 때문에 먼저 처리될 수 있도록 함.

    public BMSMusicInfo()
    {
        bmsFilePath = new BMSFilePath();
    }
}

[System.Serializable]
public class BMSMusicData
{
    public PlayerType playerNum;
    /// <summary>
    /// 판정 난이도(높을수록 관대)
    /// </summary>
    public int rank;
    public int totalNoteCount;

    public BMSMusicData()
    {
        playerNum = PlayerType.ONE_CHANNEL_ONE_PLAYER;
    }
}

[System.Serializable]
public class BMSHeader
{
    [ReadOnly] public BMSMusicInfo info;
    [ReadOnly] public BMSMusicData data;

    public BMSHeader()
    {
        info = new BMSMusicInfo();
        data = new BMSMusicData();
    }

    public string BmsFilePath { get { return info.bmsFilePath.bmsPath; } set { info.bmsFilePath.bmsPath = value; } }
    public string BmsParentPath { get { return info.bmsFilePath.bmsParentPath; } set { info.bmsFilePath.bmsParentPath = value; } }
}