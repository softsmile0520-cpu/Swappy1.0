using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AISwappiesInfo : MonoBehaviour
{
    public bool firstInst = false;

    public Tiles FirstTile;

    public bool AiSwappy = true;

    public bool Dead = false;

    public bool turn = false;

    public Sprite face;
    public Sprite Country;

    public string CountryName;

    public string Name;

    public int score = 0;

    public int Position = 0;

    public Difficulty PlayerDifficulty;
   
}
