using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuButton : MonoBehaviour
{
    Image image;
    [SerializeField] Color loopColor;
    Color initialColor;
    Tweener tweenColor;
    private void OnEnable()
    {
        image = GetComponent<Image>();
        initialColor = image.color;
    }
    public void ShiftColor(float duration)
    {
        Color color = image.color;
        tweenColor = image.DOColor(loopColor, duration).SetLoops(-1, LoopType.Yoyo);
    }
    public void ResetColor()
    {
        tweenColor.Kill();
        image.color = initialColor;
    }
}
