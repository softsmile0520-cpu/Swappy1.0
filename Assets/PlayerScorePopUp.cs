using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScorePopUp : MonoBehaviour
{
    public static PlayerScorePopUp instance;

    public TextMeshProUGUI scoreCount;
    private void Awake()
    {
        instance = this;
    }

    public void ShowScore(int moveScore)
    {
        if (moveScore > 0)
        {
            scoreCount.text = "<color=green>+" + moveScore;
        }
        else if (moveScore < 0)
        {
            GameConfigration.instance.PlayerSound(29);
            scoreCount.text = "<color=red>" + moveScore;
        }
        Invoke("backPressed", 3f);
    }
    // Start is called before the first frame update
    public static PlayerScorePopUp ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("PlayerScorePopUp")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<PlayerScorePopUp>();
        }

        return instance;
    }

    public void backPressed()
    {
        Destroy(this.gameObject);
    }


}
