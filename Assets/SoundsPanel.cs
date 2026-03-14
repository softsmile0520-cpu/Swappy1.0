using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsPanel : MonoBehaviour
{
    public static SoundsPanel instance;
    public List<Button> BGSoundsButtons;
    public Slider BGVolumeSlider;
    public Slider FXVolumeSlider;
    private void Awake()
    {
        instance = this;
    }
    public static SoundsPanel ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("SoundsPanel")) as GameObject;
            
            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<SoundsPanel>();
        }

        return instance;
    }

    private void Start()
    {
        BGVolumeSlider.value = PlayerPrefs.GetFloat("BGVolumeValue", 1);
        FXVolumeSlider.value = PlayerPrefs.GetFloat("FXVolumeValue", 1);
    }

    public void EditProfileButton()
    {
        SettingPanel.ShowUI();
        backPressed();
    }
    public void ControlBGVolumeButton()
    {
        float a = BGVolumeSlider.value;
        GameConfigration.instance.VolumeControll(a);
        PlayerPrefs.SetFloat("BGVolumeValue", a);
    }

    public void ControlFXVolumeButton()
    {
        GameConfigration.instance.PlayerSound(0);
        float a = FXVolumeSlider.value;
        GameConfigration.instance.FXVolumeControll(a);
        PlayerPrefs.SetFloat("FXVolumeValue", a);
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
