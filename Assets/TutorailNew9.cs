using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TutorailNew9 : MonoBehaviour
{
    public GameObject Tip1;
    public GameObject Tip2;
    public GameObject Tip3;
    public GameObject Tip4;
    public GameObject Tip5;

    public Button Next;

    public GameObject Hand;
    public GameObject HandContainer;
    public GameObject HandContainer2;

    public GameObject swappy;
    public GameObject ScoreDiff1;
    public GameObject ScoreDiff2;
    public TextMeshProUGUI PlayerScore;
    public TextMeshProUGUI EnemyScore;

    public TextMeshProUGUI canvasScoreIncrease1;
    public TextMeshProUGUI canvasScoreIncrease2;
    public TextMeshProUGUI canvasScoreIncrease3;

    public GameObject canvasTimeIncrease1;
    public GameObject canvasTimeIncrease2;
    public GameObject canvasTimeIncrease3;

    //shape1Tiles
    public GameObject SwappyToPlace;
    public GameObject Tile1;
    public GameObject Tile2;
    public GameObject Tile3;
    public GameObject Tile4;

    //shape2Tiles
    public GameObject SwappyToPlace2;
    public GameObject Tile1_2;
    public GameObject Tile2_2;
    public GameObject Tile3_2;
    public GameObject Tile4_2;

    //MyTiles
    public GameObject SwappyToPlaceM;
    public GameObject Tile1M;
    public GameObject Tile2M;
    public GameObject Tile3M;
    public GameObject Tile4M;

    public GameObject ShapeMade;

    public GameObject Partirion;
    public GameObject FirstCorner;
    public GameObject SecondCorner;

    //Extras
    int n = 0;
    int b = 0;

    public TextMeshProUGUI textToModify;
    public float duration = 0.5f;
    public float targetSize = 140f;
    public void NextButton()
    {
         if (n == 0)
        {
            canvasScoreIncrease1.transform.parent.gameObject.SetActive(true);
            canvasScoreIncrease1.text = "+1";
            Tip1.SetActive(false);
            Tip2.SetActive(true);
            canvasTimeIncrease2.SetActive(true);
            FirstCorner.SetActive(true);
            canvasTimeIncrease1.SetActive(false);
            GameObject player = Instantiate(SwappyToPlace);
            player.transform.position = Tile1.transform.position + new Vector3(0, 0, -1);
            player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
            GameConfigration.instance.PlayerSound(33);
            GameConfigration.instance.PlayerSound(2);
            GameConfigration.instance.PlayerSound(0);
            n++;
        }
        else if (n == 1)
        {
            Tip2.SetActive(false);
            Tip3.SetActive(true);
            HandContainer.SetActive(true);
            Next.gameObject.SetActive(false);
            GameConfigration.instance.PlayerSound(0);
            n++;
        }
        else if (n == 2)
        {
            Tip4.SetActive(true);

            GameConfigration.instance.PlayerSound(0);
            HandContainer2.SetActive(true);
            GameObject player = Instantiate(SwappyToPlace);
            player.transform.position = Tile3.transform.position + new Vector3(0, 0, -1);
            player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
            GameObject player_2 = Instantiate(SwappyToPlace);
            player_2.transform.position = Tile4.transform.position + new Vector3(0, 0, -1);
            player_2.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

            //GameObject player3 = Instantiate(SwappyToPlace2);
            //player3.transform.position = Tile2_2.transform.position + new Vector3(0, 0, -1);
            //player3.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
            GameObject player3_2 = Instantiate(SwappyToPlace2);
            player3_2.transform.position = Tile2_2.transform.position + new Vector3(0, 0, -1);
            player3_2.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
            GameObject player3_3 = Instantiate(SwappyToPlace2);
            player3_3.transform.position = Tile3_2.transform.position + new Vector3(0, 0, -1);
            player3_3.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

            GameObject playerM = Instantiate(SwappyToPlaceM);
            playerM.transform.position = Tile2M.transform.position + new Vector3(0, 0, -1);
            playerM.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
            GameObject playerM_2 = Instantiate(SwappyToPlaceM);
            playerM_2.transform.position = Tile3M.transform.position + new Vector3(0, 0, -1);
            playerM_2.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

            canvasTimeIncrease1.SetActive(false);
            canvasTimeIncrease2.SetActive(true);

            Next.gameObject.SetActive(false);
            n++;
        }
        else if (n == 3)
        {
            SceneManager.LoadScene("Vertical 7");
            GameConfigration.instance.PlayerSound(0);
        }
    }

    private void Start()
    {
        //InvokeRepeating("HandShadking", 0f, 0.2f);
        Invoke(nameof(FirstInstruction), 0.5f);

    }
    public void FirstInstruction()
    {
        Tip1.SetActive(true);
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
    public void MakingSahpe1()
    {
        //StartCoroutine(makeShape());
        StartCoroutine(FirstPSwappyPlace());

    }
    public IEnumerator FirstPSwappyPlace()
    {
        HandContainer.SetActive(false);
        Tip3.SetActive(false);
        GameConfigration.instance.PlayerSound(33);

        GameObject player = Instantiate(SwappyToPlaceM);
        player.transform.position = Tile1M.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
        canvasScoreIncrease1.transform.parent.gameObject.SetActive(false);
        canvasScoreIncrease2.transform.parent.gameObject.SetActive(true);
        canvasScoreIncrease2.text = "+1";

        canvasTimeIncrease2.SetActive(false);
        canvasTimeIncrease3.SetActive(true);

        PlayerScorePopUp.ShowUI();
        PlayerScorePopUp.instance.ShowScore(+1);

        yield return new WaitForSeconds(2);
        GameConfigration.instance.PlayerSound(33);
        GameObject player3 = Instantiate(SwappyToPlace2);
        player3.transform.position = Tile1_2.transform.position + new Vector3(0, 0, -1);
        player3.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
        canvasScoreIncrease2.transform.parent.gameObject.SetActive(false);
        canvasScoreIncrease3.transform.parent.gameObject.SetActive(true);
        canvasScoreIncrease3.text = "+1";
        SecondCorner.SetActive(true);
        canvasTimeIncrease3.SetActive(false);
        canvasTimeIncrease1.SetActive(true);

        yield return new WaitForSeconds(2);
        GameConfigration.instance.PlayerSound(2);
        GameObject player1 = Instantiate(SwappyToPlace);
        player1.transform.position = Tile2.transform.position + new Vector3(0, 0, -1);
        player1.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
        canvasScoreIncrease3.transform.parent.gameObject.SetActive(false);
        canvasScoreIncrease1.transform.parent.gameObject.SetActive(true);
        canvasScoreIncrease1.text = "+1";
        Next.gameObject.SetActive(true);

    }

    public void MakingSahpe2()
    {
        //StartCoroutine(makeShape());
        StartCoroutine(SecondPSwappyPlace());

    }
    public IEnumerator SecondPSwappyPlace()
    {
        HandContainer2.SetActive(false);
        GameConfigration.instance.PlayerSound(2);

        Tip4.SetActive(false);
        GameObject playerM = Instantiate(SwappyToPlaceM);
        playerM.transform.position = Tile4M.transform.position + new Vector3(0, 0, -1);
        playerM.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
        canvasScoreIncrease1.transform.parent.gameObject.SetActive(false);
        canvasScoreIncrease2.transform.parent.gameObject.SetActive(true);
        canvasScoreIncrease2.text = "+1";
        PlayerScorePopUp.ShowUI();
        PlayerScorePopUp.instance.ShowScore(+1);
        canvasTimeIncrease2.SetActive(false);
        canvasTimeIncrease3.SetActive(true);
        yield return new WaitForSeconds(2);
        GameConfigration.instance.PlayerSound(2);

        GameObject player3 = Instantiate(SwappyToPlace2);
        player3.transform.position = Tile4_2.transform.position + new Vector3(0, 0, -1);
        player3.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
        yield return new WaitForSeconds(1);
        FirstCorner.SetActive(false);
        Partirion.SetActive(false);
        SecondCorner.SetActive(false);
        canvasTimeIncrease3.SetActive(false);
        canvasTimeIncrease1.SetActive(true);
        canvasScoreIncrease2.transform.parent.gameObject.SetActive(false);
        canvasScoreIncrease3.transform.parent.gameObject.SetActive(true);
        canvasScoreIncrease3.text = "+1";
        Tip4.SetActive(false);
        Tip5.SetActive(true);
        PopupScript.ShowUI("7");
        Next.gameObject.SetActive(true);
    }
    public void BackTOMenu()
    {
        SceneManager.LoadScene("MenuScene");
        GameConfigration.instance.PlayerSound(0);
    }
}
