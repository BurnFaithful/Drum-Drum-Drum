using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class BMSFileSystem : MonoSingleton<BMSFileSystem>
{
    private string rootPath;
    [SerializeField] private DirectoryInfo[] dirInfoArr;

    private List<MusicInfo> findMusicInfoList;
    public List<MusicInfo> FindMusicInfoList => findMusicInfoList;

    protected override void Initialize()
    {
        rootPath = Path.Combine(Application.dataPath, GlobalDefine.RESOURCE_BASE_PATH);
        dirInfoArr = new DirectoryInfo(rootPath).GetDirectories();
        findMusicInfoList = new List<MusicInfo>();
    }

    private void Awake()
    {
        this.Initialize();
    }

    public void AllDirectoryRecursiveFileFind()
    {
        if (string.IsNullOrEmpty(rootPath))
        {
#if UNITY_EDITOR
            rootPath = @"D:\BMSFiles\";
#elif DEVELOPMENT_BUILD
            rootPath = "???????????????????";
#endif
            dirInfoArr = new DirectoryInfo(rootPath).GetDirectories();
        }

        foreach (DirectoryInfo dirInfo in dirInfoArr)
        {
            RecursiveFileFind(dirInfo, ref findMusicInfoList);
        }
    }

    // Select Scene에서 필요한 정보들만 미리 파싱
    public void RecursiveFileFind(DirectoryInfo bmsDirectory, ref List<MusicInfo> musicInfoList)
    {
        string[] files = Directory.EnumerateFiles(bmsDirectory.FullName, "*.*", SearchOption.AllDirectories)
            .Where(file => GlobalDefine.AVAILABLE_BMS_EXTENSIONS.Any(available => file.EndsWith(available, StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        if (files.Length == 0)
        {
            DebugWrapper.LogWarning("BMS File not exist.");
            return;
        }

        MusicInfo sendMusicInfo = new MusicInfo(bmsDirectory.Name);
        
        for (int fileIndex = 0; fileIndex < files.Length; ++fileIndex)
        {
            string[] textLine = File.ReadAllLines(files[fileIndex], System.Text.Encoding.Default);
            if (textLine.Length > 0)
            {
                BMSMusicInfo musicInfo = new BMSMusicInfo();

                musicInfo.bmsFilePath.bmsPath = files[fileIndex];
                musicInfo.bmsFilePath.bmsParentPath = Path.GetDirectoryName(files[fileIndex]).ToString();

                char[] separator = { ' ', ':' };
                string[] splitText;

                for (int lineIndex = 0; lineIndex < textLine.Length; ++lineIndex)
                {
                    textLine[lineIndex] = textLine[lineIndex].Trim();

                    if (string.IsNullOrEmpty(textLine[lineIndex]) || // 빈 라인에 대한 처리
                        !textLine[lineIndex].StartsWith("#")) // '#'으로 시작하는 라인만 유효(나머지는 보통 주석)
                        continue;

                    splitText = textLine[lineIndex].Split(separator);
                    if (splitText.Length <= 1) continue;

                    // GENRE
                    if (splitText[0].Equals("#GENRE"))
                    {
                        for (int i = 1; i < splitText.Length; ++i)
                            musicInfo.genre += splitText[i] + " ";
                    }
                    // TITLE
                    if (splitText[0].Equals("#TITLE"))
                    {
                        musicInfo.titleName = musicInfo.subTitleName = string.Empty;
                        int index = 1;
                        //do
                        //{
                        //    musicInfo.titleName += splitText[index];
                        //    index++;
                        //} while (!splitText[index].StartsWith("["));

                        for (int i = index; i < splitText.Length; ++i)
                        {
                            musicInfo.subTitleName += splitText[i];
                        }
                        musicInfo.subTitleName = musicInfo.subTitleName.Trim('[', ']');
                    }
                    // ARTIST
                    else if (splitText[0].Equals("#ARTIST")) { musicInfo.artistName = splitText[1]; }
                    else if (splitText[0].Equals("#BPM"))
                    {
                        if (int.TryParse(splitText[1], out musicInfo.bpm))
                        {

                        }
                    }
                    // PLAY LEVEL
                    else if (splitText[0].Equals("#PLAYLEVEL"))
                    {
                        if (int.TryParse(splitText[1], out musicInfo.playLevel))
                        {
                        }
                    }
                    // STAGE FILE
                    else if (splitText[0].Equals("#STAGEFILE")) { musicInfo.bmsFilePath.stageFileName = splitText[1]; }

                    if (char.IsNumber(splitText[0][1])) break; // FileSystem에선 Data Field까지 전부 파싱할 필요 없음. Data Field 도달 시 파싱 종료.
                }
                sendMusicInfo.BmsFileList.Add(musicInfo);
            }
        }
        musicInfoList.Add(sendMusicInfo);
    }
}
