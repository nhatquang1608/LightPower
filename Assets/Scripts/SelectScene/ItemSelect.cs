using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
    // [SerializeField] private GameObject levelImage;
    // [SerializeField] private GameObject lockImage;
    // [SerializeField] private Star[] stars;
    // [SerializeField] private Button selectButton;

    // public void InitData(ListLevels.LevelDetails levelDetails)
    // {
    //     if(levelDetails.isLock)
    //     {
    //         levelImage.SetActive(false);
    //         lockImage.SetActive(true);
    //         foreach(Star star in stars)
    //         {
    //             star.gameObject.SetActive(false);
    //         }
    //     }
    //     else
    //     {
    //         levelImage.SetActive(true);
    //         lockImage.SetActive(false);
    //         for(int i=0; i<stars.Length; i++)
    //         {
    //             stars[i].gameObject.SetActive(true);
    //             if(i < levelDetails.pickedStars)
    //             {
    //                 stars[i].SetPicked();
    //             }
    //         }
    //     }

    //     if(!levelDetails.isLock) selectButton.onClick.AddListener(() => OnSelect(levelDetails.levelId));
    // }

    // private void OnSelect(int levelId)
    // {
    //     SaveLoadData.Instance.level = levelId;
    //     SceneManager.LoadScene("GameScene");
    // }
}
