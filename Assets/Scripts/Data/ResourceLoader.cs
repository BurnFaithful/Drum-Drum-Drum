using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class ResourceLoader
{
    private static DirectoryInfo[] directoryArr;

    /// <summary>
    /// Resources 폴더의 리소스를 로딩. Load할 때의 path는 Resources 폴더 하위 디렉토리명에서부터 시작. 
    /// </summary>
    public static void Load()
    {
        foreach (DirectoryInfo dirInfo in directoryArr)
        {
            //Resources.LoadAll(dirInfo.Name);
        }
    }

    public static void RecursiveFindDirectory(string rootDirectory, out string[] dirArr)
    {
        if (!Directory.Exists(rootDirectory))
        {
            Debug.LogError($"{rootDirectory} : 해당 경로에 디렉토리가 없습니다.");
        }

        dirArr = Directory.EnumerateDirectories(rootDirectory, "*", SearchOption.AllDirectories).ToArray();
    }
}
