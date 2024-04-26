using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    private void Awake()
    {
        startButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClicked);
    }
    void OnStartButtonClicked()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    void Update()
    {
        
    }
}
