using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoad : MonoBehaviour
{
    public Slider progressBar;
    public TextMeshProUGUI loadText;

    void Start()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Play");
        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            yield return null;
            if(progressBar.value<0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 0.9f, Time.deltaTime);
            }
            else if(operation.progress>=0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime);
            }
            if (progressBar.value >= 1f)
            {
                loadText.text = "Touch to Start!!";
            }

            if(Input.GetMouseButtonDown(0) && progressBar.value >=1f && operation.progress >=0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }

    }
    

    
    void Update()
    {
        
    }
}
