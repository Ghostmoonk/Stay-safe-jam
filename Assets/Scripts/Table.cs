using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    //Quand le joueur touche la table
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Player")
        {
            //Trigger la défaite
            collision.gameObject.GetComponent<PlayerController>().UpdateControl(false);
            GameManager.Instance.DisplayDefeatHUD();
        }
    }
}
