using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupScript : MonoBehaviour
{
    public static PopupScript instance;
    public TextMeshProUGUI ExtraInfoPopUp;
    public Image MedalPopUp;
    public Image GameOver;
    public Image Fight;
    public Image YouWin;
    public Image YouLose;

    public Image _PopImage;
    public Sprite GoodS;
    public Sprite GreatG;
    public Sprite AmazingG;
    public Sprite ExcellentG;
    public Sprite ExceptionalG;
    public Sprite IncredibalG;
    public Sprite LegendaryG;
    public Sprite Fightsprite;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public static PopupScript ShowUI(string msg)
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("PopUPPanel")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<PopupScript>();
        }
        //instance.PopUpText.text = msg;
        instance.PopImage(msg);
        return instance;
    }
    public void ExtraPopUp(string msg)
    {
        ExtraInfoPopUp.text = msg;
    }
    public void PopImage(string i)
    {
        switch (i)
        {
            case "0":
                _PopImage.sprite = GoodS;
                return;
            case "1":
                _PopImage.sprite = GreatG;
                return;
            case "2":
                _PopImage.sprite = AmazingG;
                return;
            case "3":
                _PopImage.sprite = ExcellentG;
                return;
            case "4":
                _PopImage.sprite = ExceptionalG;
                return;
            case "5":
                _PopImage.sprite = IncredibalG;
                return;
            case "6":
                _PopImage.sprite = LegendaryG;
                return ;
            case "7":
                GameConfigration.instance.PlayerSound(32);
                _PopImage.sprite = Fightsprite;
                return;
        }
    }
    public void backPressed()
    {
        Destroy(this.gameObject);
    }

}
