using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTool
{

    public static string getStreamingPath()
    {
        string src =
#if UNITY_EDITOR
        Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
        "jar:file://" + Application.dataPath + "!/assets/";
#endif
        return src;
    }

    public static string getPersistentPath()
    {
        string path =Application.persistentDataPath + "/";
        return path;
    }
}
