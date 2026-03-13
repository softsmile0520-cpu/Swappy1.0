using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryPanel : MonoBehaviour
{
    public static CountryPanel instance;
    public Transform content;
    public GameObject countryItem;

    public List<Flag> flagList = new List<Flag>();

    // Start is called before the first frame update
    public static CountryPanel ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("CountryPanel")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<CountryPanel>();
        }

        return instance;
    }

    public void Start()
    {
        instance = this;    
        for (int i = 0; i < GameConfigration.instance.countries.Count; i++)
        {
            GameObject obj = Instantiate(countryItem, content);
            obj.GetComponent<Image>().sprite = GameConfigration.instance.countries[i];
            obj.GetComponent<Flag>().CountryIndex = i;
            flagList.Add(obj.GetComponent<Flag>());
            //GameConfigration.instance.PlayerCountryName = obj.name;
            //obj.GetComponent<CountryItem>().panel = this;
        }
    }

    public void backPressed()
    {
        GameConfigration.instance.PlayerSound(0);
        Destroy(this.gameObject, 0.2f);

    }
}
