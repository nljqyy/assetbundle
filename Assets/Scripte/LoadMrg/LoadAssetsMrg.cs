using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadAssetsMrg : IDisposable
{
    public const string mMainfestName = "Android";
    private Dictionary<string, Bundle> dic = new Dictionary<string, Bundle>();
    private AssetBundle mMainfestBundle;
    public AssetBundleManifest Mainfest { get; private set; }
    private bool mDisposed = false;
    #region 单例
    private static LoadAssetsMrg _instance;
    public static LoadAssetsMrg Instance
    {
        get { return _instance; }
    }
    private LoadAssetsMrg()
    {
        GetMainfest();
    }
    static LoadAssetsMrg()
    {
        _instance = new LoadAssetsMrg();
    }
    #endregion

    #region 释放
    public void Dispose()
    {
        Dispose(true);
        System.GC.SuppressFinalize(this);
    }
    private void Dispose(bool isdispose)
    {
        if (mDisposed) return;
        if (isdispose)
        {
            ReleaseAllAsset();
        }

    }
    ~LoadAssetsMrg()
    {
        Dispose(true);
    }
    #endregion
    //获得主资源
    private void GetMainfest()
    {
        string mainPath = PathTool.getPersistentPath() + mMainfestName;
        mMainfestBundle = AssetBundle.LoadFromFile(mainPath);
        if (mMainfestBundle != null)
        {
            Mainfest = mMainfestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            mMainfestBundle.Unload(false);
            mMainfestBundle = null;
        }
    }

    private string[] GetDirectDependencies(string _assetName)
    {
        if (Mainfest == null) return null;
        return Mainfest.GetDirectDependencies(_assetName);
    }
    //加载ab包
    public Bundle LoadAsset(string _assetName)
    {
        if (string.IsNullOrEmpty(_assetName)) return null;
        Bundle bd = null;
        if (!dic.ContainsKey(_assetName))
        {
            bd = new Bundle(_assetName);
            dic.Add(_assetName, bd);
            bd.GoLoad();
            LoadDependencies(_assetName);
        }
        else
        {
            bd = dic[_assetName];
            bd.Retain();
        }
        return bd;
    }

    //加载依赖
    private void LoadDependencies(string _assetName)
    {
        string[] deps = GetDirectDependencies(_assetName);
        if (deps != null && deps.Length > 0)
        {
            foreach (string dp in deps)
            {
                LoadAsset(dp);
            }
        }
    }

    public void Remove(string key)
    {
        if (dic.ContainsKey(key))
            dic.Remove(key);
    }
    //卸载资源 依赖资源
    public void ReleaseAsset(string _assetName)
    {
        if (!dic.ContainsKey(_assetName)) return;
        Bundle bd = dic[_assetName];
        bd.Release();
        string[] deps = GetDirectDependencies(_assetName);
        foreach (var assetname in deps)
        {
            ReleaseAsset(assetname);
        }
    }
    //卸载所以资源
    private void ReleaseAllAsset()
    {
        Mainfest = null;
        foreach (var item in dic.Keys)
        {
            ReleaseAsset(item);
        }
        dic.Clear();
        Resources.UnloadUnusedAssets();
    }
}
