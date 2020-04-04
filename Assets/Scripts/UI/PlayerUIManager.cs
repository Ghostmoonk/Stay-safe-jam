using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; private set; }

    [SerializeField] Text textCount;
    [SerializeField] Image capsuleImg;

    [Tooltip("De combien réduit-on l'alpha de l'image quand il n'y a plus de dash disponible")]
    [Range(0f, 1f)]
    [SerializeField] float noDashImgOpacity;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateDashAmount(int amount)
    {
        textCount.text = amount.ToString();

        //Si on a plus de dash, on met en légère opacité sur l'image
        if (amount == 0)
        {
            capsuleImg.color = new Color(capsuleImg.color.r, capsuleImg.color.g, capsuleImg.color.b, noDashImgOpacity);
        }
        else if (capsuleImg.color.a == noDashImgOpacity)
        {
            capsuleImg.color = new Color(capsuleImg.color.r, capsuleImg.color.g, capsuleImg.color.b, 1f);
        }
    }
}
