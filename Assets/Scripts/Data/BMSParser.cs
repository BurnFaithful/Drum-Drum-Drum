using UnityEngine;
using System;
using System.IO;

public class BMSParser : MonoBehaviour {

    private bool isLoadInfo; // 이전 Scene에서 미리 파싱한 헤더 데이터를 확인. Editor에서 씬을 Direct로 실행하는 등의 경우를 검사.

    [Header("Header Field")]
    [SerializeField] private BMSHeader bmsHeader;
    public BMSHeader BmsHeader => bmsHeader;

    private string testPath;

    void Awake()
    {
        bmsHeader = new BMSHeader();
        DebugWrapper.Assert(bmsHeader != null, "BMS Header가 설정되지 않았습니다.");

        isLoadInfo = SingletonData.Instance.selectedMusicInfo != null ? true : false;

        testPath = Path.Combine(Application.dataPath, GlobalDefine.RESOURCE_BASE_PATH, @"happiness\happiness_5b.bms");
        bmsHeader.BmsFilePath = isLoadInfo ? SingletonData.Instance.selectedMusicInfo.bmsFilePath.bmsPath : testPath;
        bmsHeader.BmsParentPath = isLoadInfo ? SingletonData.Instance.selectedMusicInfo.bmsFilePath.bmsParentPath : Path.GetDirectoryName(testPath);
    }

    public void ParseBMS(string filePath)
    {
        this.Awake();

        // 이전 씬(보통 음악선택 씬)에서 미리 받아둔 음악정보 데이터가 있다면, 음악 정보에 관련된 파싱은 생략하고 해당 데이터를 사용.
        if (isLoadInfo)
        {
            bmsHeader.info = SingletonData.Instance.selectedMusicInfo;
            DebugWrapper.Log("이전 Scene에서 받아온 음악 정보를 사용합니다.");
        }

        string[] textLine = File.ReadAllLines(bmsHeader.BmsFilePath, System.Text.Encoding.Default);
        if (textLine.Length > 0)
        {
            char[] separator = { ' ', ':' };
            string[] splitText;

            if (!isLoadInfo)
                DebugWrapper.Log("이전 Scene에서 받아온 음악 정보가 없기 때문에 별도의 Parsing을 진행합니다.");

            for (int lineIndex = 0; lineIndex < textLine.Length; ++lineIndex)
            {
                textLine[lineIndex] = textLine[lineIndex].Trim();

                if (string.IsNullOrEmpty(textLine[lineIndex]) || !textLine[lineIndex].StartsWith("#")) continue;

                splitText = textLine[lineIndex].Split(separator);
                if (splitText.Length <= 1) continue;

                if (!isLoadInfo) Parsing_ShowHeader(ref splitText);
                Parsing_GameHeader(ref splitText);
                Parsing_GameData(ref splitText);
            }
        }
        
    }

    // 게임 내에서 음악 및 파일에 관련된 정보를 표시하는 헤더 데이터
    // Editor에서 MainGame Scene 테스트를 하거나, 이전 Scene에서 파싱이 되지 않았을 경우 파싱을 수행
    public void Parsing_ShowHeader(ref string[] splitText)
    {
        if (splitText[0].Equals("#GENRE"))
        {
            for (int i = 1; i < splitText.Length; ++i)
                bmsHeader.info.genre += splitText[i] + " ";
        }
        else if (splitText[0].Equals("#TITLE"))
        {
            bmsHeader.info.titleName = bmsHeader.info.subTitleName = string.Empty;
            int index = 1;
            do
            {
                bmsHeader.info.titleName += splitText[index];
                index++;
            } while (!splitText[index].StartsWith("["));

            for (int i = index; i < splitText.Length; ++i)
            {
                bmsHeader.info.subTitleName += splitText[i];
            }
            bmsHeader.info.subTitleName = bmsHeader.info.subTitleName.Trim('[', ']');
        }
        else if (splitText[0].Equals("#ARTIST")) { bmsHeader.info.artistName = splitText[1]; }
        else if (splitText[0].Equals("#BPM"))
        {
            if (int.TryParse(splitText[1], out bmsHeader.info.bpm))
            {

            }
        }
        else if (splitText[0].Equals("#PLAYLEVEL"))
        {
            if (int.TryParse(splitText[1], out bmsHeader.info.playLevel))
            {

            }
        }
        else if (splitText[0].Equals("#STAGEFILE"))
        {
            bmsHeader.info.bmsFilePath.stageFileName = splitText[1];
        }
    }

    // 게임 설정 값, 메인 데이터의 의존성 타깃 역할을 하는 헤더 데이터
    public void Parsing_GameHeader(ref string[] splitText)
    {
        if (splitText[0].Equals("#PLAYER"))
        {
            int playerNum = 0;
            if (int.TryParse(splitText[1], out playerNum))
            {
                bmsHeader.data.playerNum = (PlayerType)playerNum;
            }
        }
        else if (splitText[0].Equals("#RANK"))
        {
            if (int.TryParse(splitText[1], out bmsHeader.data.rank))
            {

            }
        }
        else if (splitText[0].Equals("#TOTAL"))
        {
            if (int.TryParse(splitText[1], out bmsHeader.data.totalNoteCount))
            {

            }
        }
        // WAV
        else if (splitText[0].Substring(0, 4).Equals("#WAV"))
        {
            string key = splitText[0].Substring(4, 2);
            string value = splitText[1];
            LoadManager.Instance.SoundData.Add(key, value);
        }
        // BMP
        else if (splitText[0].Substring(0, 4).Equals("#BMP"))
        {
            string key = splitText[0].Substring(4, 2);
            string value = splitText[1];
            LoadManager.Instance.ImageData.Add(key, value);
        }
        // DIFFICULTY
        else if (splitText[0].Equals("#DIFFICULTY")) { }
        // LNOBJ
        else if (splitText[0].Equals("#LNOBJ")) { }
    }

    void Parsing_GameData(ref string[] splitText)
    {
        /*
        Data Field는 :으로 구분되며,
        # xxxyy:aabbcc ... 형식으로 되어있다.
        xxx : 마디. (좌표로 생각하면 y축과 관련)
        yy : 채널. 앞의 값에 따라 채널 종류가 구분된다. 0번은 백그라운드 채널, 1번이 실제 출력될 노트 채널. (그 외는 추후)
        뒤의 값은 채널 라인. (좌표로 생각하면 x축과 관련)
        aabbcc : 실제 노트의 데이터로, 2개씩 끊어서 구분한다.
        노트 데이터에 따라 노트의 마디 내 위치, 키음, WAV, BMP 파일 출력 등이 결정된다.
        */
        if (char.IsNumber(splitText[0][1])) // Data Field
        {
            // 1, 2, 4, 8, 16
            int beatLength = splitText[1].Length / 2;

            for (int j = 0; j < splitText[1].Length; j += 2)
            {
                if (splitText[1].Substring(j, 2).Equals("00")) continue;

                int bar = int.Parse(splitText[0].Substring(1, 3));
                int channel = int.Parse(splitText[0].Substring(4));
                int beat = j / 2;
                string data = splitText[1].Substring(j, 2);

                /*
                 channelKind
                 - 0번 : 이벤트 채널
                    * 1번 : 배경음
                    * 2번 : 마디 단축
                    * 3번 : BPM
                    * 4번 : BGA
                    * 5번 : BM98 확장
                    * 6번 : POOR BGA
                    * 7번 : BGA 레이어
                    * 8번 : 확장 BPM
                    * 9번 : 시퀀스 정지
                 - 1번 : 오브젝트 채널
                    * 1번 : 1번 건반
                    * 2번 : 2번 건반
                    * 3번 : 3번 건반
                    * 4번 : 4번 건반
                    * 5번 : 5번 건반
                    * 6번 : 턴테이블, 혹은 스크래치
                    * 7번 : 페달
                    * 8번 : 6번 건반
                    * 9번 : 7번 건반
                 */
                int channelKind = channel / 10;
                switch (channelKind)
                {
                    case 0:
                        if (channel % 10 == 1)
                            LoadManager.Instance.AddBGSound(bar, beatLength, beat, data);
                        break;
                    case 1:
                        int line = channel % 10; // Second Channel
                        LoadManager.Instance.AddNote(line, bar, beatLength, beat, data);
                        break;
                }
            }
        }
    }
}
