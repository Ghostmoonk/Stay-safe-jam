using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Capsule : MonoBehaviour
{
    public int charges = 1;
    [SerializeField] float lifeTime = 5f;
    AudioSource bonusAudio;

    private void Start()
    {
        StartCoroutine(WaitToBeDestroyed());
        bonusAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            bonusAudio.Play();
            other.GetComponent<PlayerController>().AddDash(charges);
            StopCoroutine(WaitToBeDestroyed());
            Destroy(gameObject);
        }
    }

    IEnumerator WaitToBeDestroyed()
    {
        yield return new WaitForSeconds(lifeTime);
        StopCoroutine(WaitToBeDestroyed());
        Destroy(gameObject);
    }
}
