using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpTimer : MonoBehaviour
{
    public static PopUpTimer instance;
    public TextMeshProUGUI Text;
    public AudioSource audio;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public static PopUpTimer ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("PopUpTimer")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<PopUpTimer>();
        }

        return instance;
    }
    private void Start()
    {
        StartCoroutine(StartAnim());
    }
    public IEnumerator StartAnim()
    {
        Text.text = "5";
        yield return new WaitForSeconds(1);
        Text.text = "4";
        yield return new WaitForSeconds(1);
        Text.text = "3";
        yield return new WaitForSeconds(1);
        Text.text = "2";
        yield return new WaitForSeconds(1);
        Text.text = "1";
        yield return new WaitForSeconds(1);
        Text.text = "0";
        yield return null;
    }
    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
