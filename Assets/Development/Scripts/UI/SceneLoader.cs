using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image loaderBar;
    [SerializeField] private bool isLoad = false;
    [SerializeField] private bool isLoadable = true;
    [SerializeField] private float loadSpeed = 0.5f;
    [SerializeField] private int sceneIndex;

    void Update()
    {
        if(!isLoad && isLoadable)
        {
            if (loaderBar.fillAmount < 1f)
            {
                loaderBar.fillAmount += Time.deltaTime * loadSpeed; 
            }
            else
            {
                LoadSceneAsync(sceneIndex);
                isLoad = true; 
            }
        }
    }

    public void LoadSceneAsync(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
    }
}
