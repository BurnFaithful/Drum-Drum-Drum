using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.U2D;

[RequireComponent(typeof(BMSParser))]
public class LoadManager : MonoSingleton<LoadManager>
{
    static UnityWebRequest loader;

    public BMSParser Parser { get; private set; }

    public Dictionary<string, string> SoundData { get; private set; }
    public Dictionary<string, AudioClip> AudioClipData { get; private set; }
    public Dictionary<string, string> ImageData { get; private set; }

    public List<BGSound> BGSoundData { get; private set; }
    public List<Note> NoteData { get; private set; }

    private Sprite[] numberSprites;
    public Sprite[] NumberSprites => numberSprites;

    protected override void Initialize()
    {
        Parser = GetComponent<BMSParser>();

        SoundData = new Dictionary<string, string>();
        AudioClipData = new Dictionary<string, AudioClip>();
        ImageData = new Dictionary<string, string>();

        BGSoundData = new List<BGSound>();
        NoteData = new List<Note>();
    }

    public void Awake()
    {
        this.Initialize();

        numberSprites = new Sprite[10];

        string filePath = "file://" + Path.Combine(Application.streamingAssetsPath, "AssetBundles", "atlas.hd");
        LoadSpriteBundle(filePath, "Atlas_UI");
    }

    public void LoadSpriteBundle(string filePath, string assetName)
    {      
        StartCoroutine(CoLoadSpriteBundle(filePath, assetName));
    }

    IEnumerator CoLoadSpriteBundle(string filePath, string assetName)
    {
        using (loader = UnityWebRequestAssetBundle.GetAssetBundle(filePath, 0))
        {
            yield return loader.SendWebRequest();

            if (loader.error != null)
            {
                DebugWrapper.LogError("어셋 번들 로딩 중에 문제가 발생했습니다.");
            }
            else
            {
                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(loader);
                    
                SpriteAtlas atlas = assetBundle.LoadAsset<SpriteAtlas>(assetName);
                for (int i = 0; i < numberSprites.Length; ++i)
                {
                    numberSprites[i] = atlas.GetSprite($"JudgeAtlas_{i + 1}");
                }
            }
        }
    }

    public void LoadAudioSource(string filePath, Dictionary<string, string> wavData)
    {
        StartCoroutine(CoLoadAudioSource(filePath, wavData));
    }

    IEnumerator CoLoadAudioSource(string filePath, Dictionary<string, string> wavData)
    {
        UnityWebRequest www = null;

        foreach (KeyValuePair<string, string> p in wavData)
        {
            string url = Path.Combine(filePath, p.Value);

            AudioType type = AudioType.OGGVORBIS;

            int extensionCompareCount = 0;
            if (File.Exists(url))
            {
                while (extensionCompareCount < GlobalDefine.AVAILABLE_AUDIO_EXTENSIONS.Length)
                {
                    if (Path.GetExtension(url).Equals(GlobalDefine.AVAILABLE_AUDIO_EXTENSIONS[extensionCompareCount])) break;
                    extensionCompareCount++;
                }
            }
            else
                DebugWrapper.LogError($"{url} : 해당 경로가 존재하지 않습니다.");

            switch (extensionCompareCount)
            {
                case 0:
                    type = AudioType.OGGVORBIS;
                    break;
                case 1:
                    type = AudioType.WAV;
                    break;
                case 2:
                    type = AudioType.MPEG;
                    break;
            }

            www = UnityWebRequestMultimedia.GetAudioClip(
                "file://" + url, type);

            yield return www.SendWebRequest();

            if (www.downloadHandler.data.Length != 0)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);

                if (clip.LoadAudioData())
                {
                    AudioClipData.Add(p.Key, clip);
                }
            }
        }
        www.Dispose();

        SceneManager.LoadSceneAsync((int)eSceneName.SCENE_MAINGAME, LoadSceneMode.Single);
    }

    public void AddBGSound(int bar, int beatLength, float beat, string soundKey)
    {
        BGSoundData.Add(new BGSound(bar, beatLength, beat, soundKey));
    }

    public void AddNote(int line, int bar, int beatLength, int beat, string soundKey)
    {
        NoteData.Add(new Note(bar, beatLength, beat, line, soundKey));
    }
}
