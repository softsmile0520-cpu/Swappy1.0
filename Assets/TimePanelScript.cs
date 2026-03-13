using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePanelScript : MonoBehaviour
{

    public static TimePanelScript instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public static TimePanelScript ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("TimePanel")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<TimePanelScript>();
        }

        return instance;
    }

    public void ModeSelection(int index)
    {
        GameConfigration.instance.PlayerSound(0);

        GameConfigration.instance.GameMode = (mode)index;

        PlayerSelectionScript.ShowUI();
        backPressed();

    }
    public void goBack()
    {
        GameConfigration.instance.PlayerSound(0);
        backPressed();
    }
    public void backPressed()
    {
        Destroy(this.gameObject);
    }
}
