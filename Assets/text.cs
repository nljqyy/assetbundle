using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class text : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(4);
       // LoadAssetsMrg.Instance.LoadAsset("text.unity.bytes");
        SceneManager.LoadSceneAsync("Text");
        LoadAssetsMrg.Instance.ReleaseAsset("main.unity.bytes");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
