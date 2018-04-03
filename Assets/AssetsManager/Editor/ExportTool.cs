using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class ExportTool
{
    const string NeedExportResPath = "Assets/NeedExportRes";
    const string suffixName = ".bytes";
    const string fileName = "FileName.txt";
    //    static string ImportResPath()
    //    {
    //        string filePath =
    //#if UNITY_ANDROID && !UNITY_EDITOR
    //          Application.dataPath + "!/assets/";
    //#elif UNITY_IPHONE && !UNITY_EDITOR
    //               file:// + Application.dataPath +"/Raw/"
    //#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
    //         Application.dataPath + "/StreamingAssets/";
    //#else
    //             string.Empty;  
    //#endif
    //        return filePath;
    //    }


    [MenuItem("Tools/ExportAssetsBuild")]
    static void ExportAssetsBuild()
    {
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        DirectoryInfo d = new DirectoryInfo(NeedExportResPath);
        FileInfo[] fs = d.GetFiles("*.*", SearchOption.AllDirectories);
        string currentPath = Directory.GetCurrentDirectory() + "\\";
        foreach (FileInfo file in fs)
        {
            if (file.Name.EndsWith(".meta")) continue;
            AssetBundleBuild tbuild = new AssetBundleBuild();
            tbuild.assetBundleName = file.Name + suffixName;
            string relativePath = file.FullName;
            relativePath = relativePath.Replace(currentPath, "");
            relativePath = relativePath.Replace("\\", "/");
            tbuild.assetNames = new string[] { relativePath };
            builds.Add(tbuild);
        }
        GoExport(builds.ToArray());

    }

    static void GoExport(AssetBundleBuild[] _buids)
    {
        if (_buids.Length == 0) return;
        string ImortPath = PathTool.getStreamingPath() + "Android/";
        if (!Directory.Exists(ImortPath))
        {
            Directory.CreateDirectory(ImortPath);
        }
        else
        {
            string[] spath = Directory.GetFiles(ImortPath);
            foreach (var item in spath)
            {
                File.Delete(item);
            }
        }
        BuildPipeline.BuildAssetBundles(ImortPath, _buids, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        AssetDatabase.Refresh();

        string filep = "Assets/FileName.txt";//保存资源包名字
        File.Delete(filep);
        StreamWriter sw = File.CreateText(filep);
        StringBuilder sbuider = new StringBuilder();
        DirectoryInfo dstream = new DirectoryInfo(ImortPath);
        FileInfo[] finfo = dstream.GetFiles();
        foreach (var f in finfo)
        {
            if (f.Name.EndsWith(".meta")) continue;
            sbuider.Append(f.Name);
            sbuider.Append("|");
        }
        sw.Write(sbuider.ToString());
        sw.Dispose();
        sw.Close();
        if (File.Exists(PathTool.getStreamingPath() + fileName))
            File.Delete(PathTool.getStreamingPath() + fileName);
        File.Copy(filep, PathTool.getStreamingPath() + fileName);//复制到streamingAssets
        AssetDatabase.Refresh();
        Debug.Log("导出完成");
    }



}
