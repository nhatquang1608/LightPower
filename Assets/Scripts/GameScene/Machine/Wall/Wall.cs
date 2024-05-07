using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool isShow;
    [SerializeField] private GameObject showedObject;

    private void Start()
    {
        ShowHide(isShow);
    }

    public void ShowHide(bool show)
    {
        isShow = show;
        showedObject.SetActive(show);
    }
}
