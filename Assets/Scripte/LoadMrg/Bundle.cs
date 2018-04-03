using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bundle
{
    public const string suffixName = ".bytes";

    public string mAssetsName
    {
        get; private set;
    }
    public Object mAsset
    {
        get; set;
    }
    public bool mIsLoaded { get; private set; }
    public bool mStartLoad { get; private set; }
    private int mPcount = 0;

    public Bundle(string _assetname)
    {
        mAssetsName = _assetname;
        mIsLoaded = false;
        mStartLoad = true;
        mPcount = 0;
    }
    public static string CombineSuffixName(string _assetname)
    {
        if (!_assetname.EndsWith(suffixName))
        {
            return _assetname + suffixName;
        }
        return _assetname;
    }
    public static string DeleteSuffixName(string _assetname)
    {
        if (_assetname.EndsWith(suffixName))
        {
            return _assetname.Replace(suffixName, "");
        }
        return _assetname;
    }
    //计数
    public void Retain()
    {
        mPcount++;
    }
    public void Release()
    {
        if (!mIsLoaded) return;
        mPcount -= 1;
        if (mPcount == 0)
        {
            Debug.Log("卸载资源---"+ mAssetsName);
            mAsset = null;
            Resources.UnloadUnusedAssets();
            LoadAssetsMrg.Instance.Remove(mAssetsName);
        }
    }

    public void GoLoad()
    {
        string pathName = PathTool.getPersistentPath() + mAssetsName;
        Debug.Log("加载assetbundle--" + mAssetsName);
        AssetBundle  mAssetBundle = AssetBundle.LoadFromFile(pathName);
        if (mAssetBundle != null)
        {
            if (mAssetBundle.isStreamedSceneAssetBundle)
                mAsset = mAssetBundle.mainAsset;
            else
                mAsset = mAssetBundle.LoadAsset(DeleteSuffixName(mAssetsName));

            Retain();
            mIsLoaded = true;
            mAssetBundle.Unload(false);
            mAssetBundle = null;
        }
    }
}
