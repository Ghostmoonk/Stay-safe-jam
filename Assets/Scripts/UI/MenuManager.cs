using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Image oliveImg;

    public void MoveImage(Transform buttonTransform)
    {
        oliveImg.transform.position = new Vector3(oliveImg.transform.position.x, buttonTransform.position.y);
    }

    public void ShiftColor(Button baseButton)
    {

    }
}
