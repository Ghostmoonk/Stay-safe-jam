using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceVerreAnim : MonoBehaviour
{
    private Animator anim;
    [SerializeField] float animSpeed;
    [SerializeField] float animSpeedCritical;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent <Animator> ();
        anim.speed = animSpeed;
    }

    public void PlayOpen(bool isCritical)
    {
        anim.SetTrigger("Open");
        anim.SetBool("isCritical", isCritical);
        if(isCritical)
            anim.speed = animSpeedCritical;
        else
            anim.speed = animSpeed;
    }
}
