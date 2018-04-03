using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CopyAssets : MonoBehaviour
{
    //player setting设置里 write permision 要设置为 SD卡

    string fileName = "FileName.txt";

    private void Awake()
    {
        StartCoroutine(LoadAssetBd());
    }

    IEnumerator LoadAssetBd()
    {
        string src = PathTool.getStreamingPath();
        string des = PathTool.getPersistentPath();
        Debug.Log("src---" + src);
        Debug.Log("des---" + des);
        WWW www = new WWW(src + fileName);
        yield return www;
        Debug.Log("text-----" + www.bytes.Length + "-----www---" + www);
        string content = www.text;
        if (string.IsNullOrEmpty(content)) yield break;
        string[] names = content.Split('|');
        FileStream fstream;
        foreach (var item in names)
        {
            if (string.IsNullOrEmpty(item)) continue;
            string path = src + "Android/" + item;
            www = new WWW(path);
            yield return www;
            string desPath = des + item;
            if (File.Exists(desPath))
            {
                File.Delete(desPath);
            }
            Debug.Log(item+"----length---"+www.bytes.Length);
            fstream = File.Create(desPath);
            fstream.Write(www.bytes, 0, www.bytes.Length);
            fstream.Flush();
            fstream.Close();
        }
        Debug.Log("copy成功");
    }


}
