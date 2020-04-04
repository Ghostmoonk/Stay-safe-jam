using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    AudioSource audioSplash;
    //Quand le joueur touche la table
    void Start()
    {
        audioSplash = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Player")
        {
            //Trigger la défaite
            audioSplash.Play();
            collision.gameObject.GetComponent<PlayerController>().UpdateControl(false);
            GameManager.Instance.DisplayDefeatHUD();
        }
    }
}
