using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class ScoreDiffText : MonoBehaviour
{
    public int moveScore = 0;

    public TextMeshProUGUI Movtext;

    private void OnEnable()
    {
        if (moveScore > 0)
        {
            Movtext.text = "<color=green>+" + moveScore;
        }
        else if (moveScore < 0)
        {
            Movtext.text = "<color=red>" + moveScore;
        }
        Invoke(nameof(HideText), 1f);
    }

    void HideText()
    {
        Movtext.text = string.Empty;
        this.gameObject.SetActive(false);
    }
}
