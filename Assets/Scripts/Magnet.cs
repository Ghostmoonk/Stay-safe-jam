using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] Collider2D leftBooster;
    [SerializeField] Collider2D rightBooster;
    [SerializeField] float strength;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().isInTheAir)
            {
                Collider2D closestBooster = GetClosestBooster(collision.gameObject.transform.position);
                collision.gameObject.GetComponent<PlayerController>().PushToDirection(closestBooster.bounds.center - collision.gameObject.transform.position, strength);
            }
        }
    }

    Collider2D GetClosestBooster(Vector3 position)
    {
        //Si la position envoyée est plus proche du booster à gauche que du booster à droite
        if ((position - leftBooster.bounds.center).magnitude < (position - rightBooster.bounds.center).magnitude)
        {
            return leftBooster;
        }
        else
        {
            return rightBooster;
        }
    }
}
