using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    Canvas defeatCanvas;
    bool playing;

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
    private void OnEnable()
    {
        playing = true;
        defeatCanvas = GameObject.FindGameObjectWithTag("GameUI").GetComponent<Canvas>();
        defeatCanvas.gameObject.SetActive(false);

    }

    public void DisplayDefeatHUD()
    {
        if (defeatCanvas != null)
            defeatCanvas.gameObject.SetActive(true);
        playing = false;
    }

    private void Update()
    {
        if (!playing && Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
