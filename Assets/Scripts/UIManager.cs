using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button retryBtn;
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        retryBtn.onClick.AddListener(()=>
        {
            gameOverPanel.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
