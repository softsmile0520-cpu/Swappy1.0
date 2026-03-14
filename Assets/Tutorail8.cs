using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorail8 : MonoBehaviour
{
    public GameObject Tip1;
    public GameObject Tip2;
    public GameObject Tip3;

    public Button Next;
    public GameObject ReturnMenuButton;

    public GameObject Hand;
    public GameObject HandContainer;
    public Transform HandContainer2;

    public GameObject swappy;
    public GameObject swappyToSwapWith;
    public GameObject ScoreDiff1;
    public GameObject ScoreDiff2;
    public TextMeshProUGUI PlayerScore;
    public TextMeshProUGUI EnemyScore;


    //shape1Tiles
    public GameObject SwappyToSwap;
    public GameObject Tile1;
    public GameObject Tile2;
    public GameObject Tile3;
    public GameObject Tile4;

    public GameObject ShapeMade;
    public GameObject ShapeMade2;
    public GameObject ShapeMade3;
    public GameObject ShapeMade4;
    public GameObject ShapeMade5;
    public GameObject ShapeMade6;
    public GameObject ShapeMade7;
    public GameObject ShapeMade8;

    public GameObject SwappingIdicator;
    //Extras
    int n = 0;
    int b = 0;
    public GameObject Cube1;
    public GameObject Cube2;
    public GameObject Cube3;

    public TextMeshProUGUI textToModify;
    public float duration = 1f;
    public float targetSize = 130f;
    public void NextButton()
    {
        if (n == 0)
        {
            Tip1.SetActive(false);
            Tip2.SetActive(true);
            HandContainer.SetActive(true);
            n = 1;
            Next.gameObject.SetActive(false);
        }
        else if (n == 1)
        {

        }
    }
    private void Start()
    {
        //InvokeRepeating("HandShadking", 0f, 0.2f);

    }
    IEnumerator IncreaseTextSizeOverTime()
    {
        float startTime = Time.time;
        while (Time.time - startTime <= duration)
        {
            float t = (Time.time - startTime) / duration;
            textToModify.fontSize = (int)Mathf.Lerp(0f, targetSize, t);
            yield return null;
        }
        textToModify.fontSize = (int)targetSize;

        // Wait for 2 seconds before toggling off the text

        // Toggle off the text object
        textToModify.gameObject.SetActive(false);
    }
    public void HandShadking()
    {
        Hand.SetActive(!Hand.activeSelf);
    }
    public void MakingSahpe()
    {
        StartCoroutine(makeShape());
    }


    public IEnumerator makeShape()
    {
        Tip2.SetActive(false);
        HandContainer.SetActive(false);
        Destroy(Cube1);

        GameObject player = Instantiate(swappy);
        player.transform.position = SwappyToSwap.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        ShapeMade.SetActive(true);
        ShapeMade2.SetActive(true);
        ShapeMade3.SetActive(true);
        ShapeMade4.SetActive(true);
        ShapeMade5.SetActive(true);
        ShapeMade6.SetActive(true);
        ShapeMade7.SetActive(true);
        ShapeMade8.SetActive(true);
        GameConfigration.instance.PlayerSound(2);
        yield return new WaitForSeconds(0.1f);

        PlayerScorePopUp.ShowUI();
        PlayerScorePopUp.instance.ShowScore(+5);
        //StartCoroutine(IncreaseTextSizeOverTime());

        Destroy(Cube2);
        GameObject player1 = Instantiate(swappy);
        player1.transform.position = Tile1.transform.position + new Vector3(0, 0, -1);
        player1.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        GameObject player2 = Instantiate(swappy);
        player2.transform.position = Tile2.transform.position + new Vector3(0, 0, -1);
        player2.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        GameObject player3 = Instantiate(swappy);
        player3.transform.position = Tile3.transform.position + new Vector3(0, 0, -1);
        player3.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        GameObject player4 = Instantiate(swappy);
        player4.transform.position = Tile4.transform.position + new Vector3(0, 0, -1);
        player4.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        ScoreDiff1.SetActive(true);
        ScoreDiff2.SetActive(true);
        PlayerScore.text = "118";
        EnemyScore.text = "1";
        yield return new WaitForSeconds(0.1f);
        ReturnMenuButton.gameObject.SetActive(true);
        Tip3.SetActive(true);

    }    

    
    public void BackTOMenu()
    {
        SceneManager.LoadScene("MenuScene");
        GameConfigration.instance.PlayerSound(0);
    }
}
