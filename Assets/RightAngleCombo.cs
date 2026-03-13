using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class RightAngleCombo : MonoBehaviour
{
    public GameObject swappy;

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;

    //public GameObject Hand1st;
    //public GameObject Hand2nd;
    //public GameObject Hand3rd;
    //public GameObject Hand4th;


    public GameObject Hand5th;


    public Transform Tile1st;



    public Transform Tile2nd;
    public Transform Tile3rd;
    public Transform Tile4th;
    public Transform Tile5th;
    public Transform Tile6th;
    public Transform Tile7th;
    public Transform Tile8th;
    public Transform Tile9th;
    public Transform Tile10th;
    public Transform Tile11th;
    public Transform Tile12th;
    public Transform Tile13th;
    public Transform Tile14th;
    public Transform Tile15th;
    public Transform Tile16th;
    public Transform Tile17th;

    public Transform Tile18th;
    public Transform Tile19th;
    public Transform Tile20th;
    public Transform Tile21th;
    public Transform Tile22th;
    public Transform Tile23th;
    public Transform Tile24th;
    public Transform Tile25th;
    public Transform Tile26th;
    public Transform Tile27th;
    public Transform Tile28th;
    public Transform Tile29th;





    public Image shape;
    public Image shape2;
    public Image shape3;
    public Image shape4;
    public Image shape5;
    public Image shape6;
    public GameObject EndText;

    public GameObject button;


    //public void FifthSwappy()
    //{
    //    GameObject player = Instantiate(swappy);
    //    player.transform.position = Tile1st.transform.position + new Vector3(0, 0, -1);
    //    player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    //    Hand1st.SetActive(false);
    //    Hand2nd.SetActive(true);
    //}

    //public void FourthSwappy()
    //{
    //    GameObject player = Instantiate(swappy);
    //    player.transform.position = Tile2nd.transform.position + new Vector3(0, 0, -1);
    //    player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    //    Hand2nd.SetActive(false);
    //    Hand3rd.SetActive(true);
    //}


    //public void ThirdSwappy()
    //{
    //    GameObject player = Instantiate(swappy);
    //    player.transform.position = Tile3rd.transform.position + new Vector3(0, 0, -1);
    //    player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    //    Hand3rd.SetActive(false);
    //    Hand4th.SetActive(true);
    //}

    //public void SecondSwappy()
    //{
    //    GameObject player = Instantiate(swappy);
    //    player.transform.position = Tile4th.transform.position + new Vector3(0, 0, -1);
    //    player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    //    Hand4th.SetActive(false);
    //    Hand5th.SetActive(true);
    //}

    public void FirstSwappy()
    {
        GameObject player = Instantiate(swappy);
        player.transform.position = Tile1st.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        Hand5th.SetActive(false);
        //LineComplete();
        Invoke("Shape1", 0.5f);
        Invoke("Shape2", 0.9f);
        Invoke("Shape3", 1.3f);
        Invoke("Shape4", 1.7f);
        Invoke("Shape5", 2.1f);
        Invoke("Shape6", 2.5f);

        Invoke("ComboSound", 2.6f);
      

        Invoke("LineComplete", 2.8f);

        //Invoke("TextEnd", 1f);

    }

    public void LineComplete()
    {
        GameObject player = Instantiate(swappy);
        player.transform.position = Tile2nd.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile3rd.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile4th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile5th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile6th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile7th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile8th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        player = Instantiate(swappy);
        player.transform.position = Tile9th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile10th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile11th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile12th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile13th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        Destroy(Enemy1);
        Destroy(Enemy2);
        Destroy(Enemy3);
        Destroy(Enemy4);

        player = Instantiate(swappy);
        player.transform.position = Tile14th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile15th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile16th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile17th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);



        player = Instantiate(swappy);
        player.transform.position = Tile18th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile19th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile20th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        player = Instantiate(swappy);
        player.transform.position = Tile21th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        player = Instantiate(swappy);
        player.transform.position = Tile22th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        player = Instantiate(swappy);
        player.transform.position = Tile23th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        

        player = Instantiate(swappy);
        player.transform.position = Tile24th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        player = Instantiate(swappy);
        player.transform.position = Tile25th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        player = Instantiate(swappy);
        player.transform.position = Tile26th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        player = Instantiate(swappy);
        player.transform.position = Tile27th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile28th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        player = Instantiate(swappy);
        player.transform.position = Tile29th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    }

    public void Shape1()
    {
        shape.gameObject.SetActive(true);
        GameConfigration.instance.PlayerSound(8);

    }

    public void Shape2()
    {
        GameConfigration.instance.PlayerSound(8);

        shape2.gameObject.SetActive(true);
    }

    public void Shape3()
    {
        GameConfigration.instance.PlayerSound(8);

        shape3.gameObject.SetActive(true);
    }

    public void Shape4()
    {
        GameConfigration.instance.PlayerSound(8);

        shape4.gameObject.SetActive(true);
    }

    public void Shape5()
    {
        GameConfigration.instance.PlayerSound(8);

        shape5.gameObject.SetActive(true);
    }

    public void Shape6()
    {
        GameConfigration.instance.PlayerSound(8);

        shape6.gameObject.SetActive(true);
        Invoke("TextEnd", 0.5f);
    }

    //public void OKBTN()

    //{
    //    GameConfigration.instance.PlayerSound(0);
    //    button.SetActive(false);
    //    TextEnd();
    //}

    public void TextEnd()
    {
        EndText.SetActive(true);

    }

    public void ComboSound()
    {
        GameConfigration.instance.PlayerSound(28);

    }

    public void NextBTN()
    {
        GameConfigration.instance.PlayerSound(0);


        SceneManager.LoadScene("DiagonalSwap");
    }
}
