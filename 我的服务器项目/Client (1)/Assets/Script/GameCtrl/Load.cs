using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour 
{

    AsyncOperation operation;
    public Text text;
    public Slider slider;
    public string sceneName;
	void Start () 
	{
        StartCoroutine(LoadScene(sceneName));
        text.text = "加载进度：" + (operation.progress * 100).ToString("0") + "%";
        slider.value = operation.progress;
	}
	
	// 每帧都调用
	void Update () 
	{
        text.text = "加载进度：" + (operation.progress * 100).ToString("0") + "%";
        slider.value = operation.progress;
	}
    IEnumerator LoadScene(string _scene)
    {

        operation = SceneManager.LoadSceneAsync(_scene);
        yield return operation;
    }
}
