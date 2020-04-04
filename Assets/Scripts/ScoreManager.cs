using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    [SerializeField] TextMeshProUGUI textScore;
    private GlassManager glassManager;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        glassManager = FindObjectOfType<GlassManager>();
    }

    // Update is called once per frame
    void Update()
    {
        score += 1;
        textScore.text = "Score = " + score;

        if(score%600 == 0)
        {
            if(GlassManager.nbrOfGlassPerMinutes>6)
            {
                GlassManager.nbrOfGlassPerMinutes -= 1;
            }

            if(GlassManager.difficulty<8)
            {
                GlassManager.difficulty = 1;
            }
        }

    }
}
