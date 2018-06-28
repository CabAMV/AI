using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    private Image [] scoreImgs_AI;

    [SerializeField]
    private Image[] scoreImgs_Player;

    int contScore_AI;
    int contScore_Player;

    // Update is called once per frame
    public void SetScore_AI ()
    {
		if (contScore_AI < scoreImgs_AI.Length)
        {
            scoreImgs_AI[contScore_AI].enabled = true;
            ++contScore_AI;
        }
	}

    public void SetScore_Player()
    {
		if (contScore_Player < scoreImgs_Player.Length)
        {
            scoreImgs_Player[contScore_Player].enabled = true;
            ++contScore_Player;
        }
    }
}
