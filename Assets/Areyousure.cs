using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Areyousure : MonoBehaviour
{
    public static Areyousure instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public static Areyousure ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("Areyousure")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<Areyousure>();
        }

        return instance;
    }

    public void ShowSoundOptions()
    {
        GameConfigration.instance.PlayerSound(0);
        SoundsPanel.ShowUI();
    }

    public void Resume()
    {
        GameConfigration.instance.PlayerSound(0);
        if (PopUpTimer.instance != null)
        {
            PopUpTimer.instance.audio.Play();
        }
        Time.timeScale = 1;
        backPressed();
    }
    public void MenuScene()
    {
        GameConfigration.instance.PlayerSound(0);
        GameConfigration.instance.RandomSelected = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Restart()
    {
        GameConfigration.instance.PlayerSound(0);
        GameConfigration.instance.RandomSelected = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
    public void backPressed()
    {
        Gamemanager.instance.SettingOpened = false;
        GameConfigration.instance.PlayerSound(0);
        Destroy(this.gameObject, 0.2f);

    }
}
