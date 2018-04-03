using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartS : MonoBehaviour {

	 
	IEnumerator Start () {

        yield return new WaitForSeconds(3);
        LoadAssetsMrg.Instance.LoadAsset("main.unity.bytes");
        SceneManager.LoadSceneAsync("Main");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
