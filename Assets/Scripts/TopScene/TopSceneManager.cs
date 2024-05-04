using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject guidPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button guidButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene("SelectScene"));
        guidButton.onClick.AddListener(() => ShowGuid(true));
        exitButton.onClick.AddListener(() => ShowGuid(false));

        ShowGuid(false);
    }

    private void ShowGuid(bool show)
    {
        guidPanel.SetActive(show);
    }
}
