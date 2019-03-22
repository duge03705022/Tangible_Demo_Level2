using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressController : MonoBehaviour
{
    public GameObject loopCard;
    public GameObject timesCard;
    public GameObject loopCard3;
    public GameObject cookCard;

    public GameObject hint1;
    public GameObject hint2;

    public SpriteRenderer loopCard3Sprite;
    public SpriteRenderer cookCardSprite;

    public GameObject hightPathParent;
    public SpriteRenderer[] hightPath;

    public GameObject dish;
    public GameObject basket;

    public GameObject[] tomato;
    public GameObject[] tomatoSoup;
    
    public GameObject chefCook;
    public GameObject chefTable;

    private int soupCount;

    // Start is called before the first frame update
    void Start()
    {
        soupCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("p"))
        {
            StartCoroutine(PathShine(75));
            loopCard3Sprite.color = new Color(1f, 1f, 1f, 0.5f);
            cookCardSprite.color = new Color(1f, 1f, 1f, 0.5f);
            hint1.SetActive(false);
            hint2.SetActive(false);
        }

        if (Input.GetKeyUp("1"))
        {
            loopCard.SetActive(true);
            hightPathParent.SetActive(true);
        }
        if (Input.GetKeyUp("2"))
        {
            loopCard.SetActive(false);
            timesCard.SetActive(true);
        }
        if (Input.GetKeyUp("3"))
        {
            timesCard.SetActive(false);
            loopCard3.SetActive(true);
        }
        if (Input.GetKeyUp("4"))
        {
            cookCard.SetActive(true);
        }

        if (Input.GetKeyUp("w"))
        {
            //basket.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
            StartCoroutine(MoveUp(75));
        }
        if (Input.GetKeyUp("a"))
        {
            //basket.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
            StartCoroutine(MoveLeft(150));
        }
        if (Input.GetKeyUp("s"))
        {
            //basket.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 270f));
            StartCoroutine(MoveDown(75));
        }
        if (Input.GetKeyUp("d"))
        {
            //basket.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            StartCoroutine(MoveRight(75));
        }

        if (Input.GetKeyUp("e"))
        {
            tomato[soupCount].SetActive(false);
            tomatoSoup[soupCount].SetActive(true);
            StartCoroutine(Cook());
            soupCount += 1;
        }
        if (Input.GetKeyUp("q"))
        {
            hightPathParent.SetActive(false);
        }
        if (Input.GetKeyUp("space"))
        {
            basket.SetActive(false);
        }
    }

    IEnumerator Cook()
    {
        chefTable.SetActive(false);
        chefCook.SetActive(true);
        yield return new WaitForSeconds(2f);
        chefCook.SetActive(false);
        chefTable.SetActive(true);
    }

    IEnumerator MoveUp(int step)
    {
        for (int i = 0; i < step; i++)
        {
            yield return new WaitForSeconds(0.01f);
            dish.transform.localPosition += new Vector3(0f, 0.1f, 0f);
        }
    }
    IEnumerator MoveLeft(int step)
    {
        for (int i = 0; i < step; i++)
        {
            yield return new WaitForSeconds(0.01f);
            dish.transform.localPosition += new Vector3(-0.1f, 0f, 0f);
        }
    }
    IEnumerator MoveDown(int step)
    {
        for (int i = 0; i < step; i++)
        {
            yield return new WaitForSeconds(0.01f);
            dish.transform.localPosition += new Vector3(0f, -0.1f, 0f);
        }
    }
    IEnumerator MoveRight(int step)
    {
        for (int i = 0; i < step; i++)
        {
            yield return new WaitForSeconds(0.01f);
            dish.transform.localPosition += new Vector3(0.1f, 0f, 0f);
        }
    }

    IEnumerator PathShine(int step)
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 5; i++)
            {
                hightPath[i].color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 5; i++)
            {
                hightPath[i].color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
