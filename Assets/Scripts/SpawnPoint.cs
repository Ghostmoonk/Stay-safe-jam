using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // ------------------------ [ Gestion ] ------------------------------ \\
    public bool isHavingAGlassAbove;
    [SerializeField] LanceVerreAnim lanceVerreAnim;

    public void PlayAnim(bool isCritical)
    {
        lanceVerreAnim.PlayOpen(isCritical);
    }
}
