using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayerProfileInfo : MonoBehaviour
{
    public static PlayerProfileInfo instance;

    public GameObject trophiesCollected;

    public GameObject content;

    [NonSerialized]
    public PlayerProfileInfoTab playerInfo;

    public PlayerProfileInfoTab PlayerInfoTabPrefab;

    public Transform ProfilePrefabPos;

    public static PlayerProfileInfo ShowUI()
    {
        if (instance == null)
        {
            GameObject obj = Instantiate(Resources.Load("PlayerProfile")) as GameObject;

            obj.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);

            instance = obj.GetComponent<PlayerProfileInfo>();
        }

        return instance;
    }

    private void Start()
    {
        playerInfo = Instantiate(PlayerInfoTabPrefab, ProfilePrefabPos);

        SetInfoOfTrophies();
    }

    public void SetInfoOfTrophies()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject Obj1 =  Instantiate(trophiesCollected, content.transform);
            Obj1.GetComponent<TrophiesCollectedSection>().SetTrophieData(i);
        }
    }
    public void goBack()
    {
        GameConfigration.instance.PlayerSound(0);
        PlayerProfileInfo.ShowUI();
        GameConfigration.instance.RandomSelected = false;
        backPressed();
    }
    public void BackToMenu()
    {
        GameConfigration.instance.PlayerSound(0);
        PlayerProfileInfo.ShowUI();
        GameConfigration.instance.RandomSelected = false;
        backPressed();
    }
    public void backPressed()
    {
        Destroy(this.gameObject);
    }
}
