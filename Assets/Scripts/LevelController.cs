using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LevelController : MonoBehaviour
{
    # region Level Parameter
    public RFIBManager rFIBManager;
    public GameController gameController;
    public CardHandler cardHandler;
    public LevelParameter levelParameter;

    public GameObject levelProcess;

    public int startX;
    public int startY;

    public Tuple<string, int>[,,] cardMap;

    public GameObject[] chef;
    public GameObject[] ingredientStep;

    public GameObject dish;
    public GameObject basket;

    private int[] loopCount;
    public bool[] ifLoop;
    public GameObject[] hints;
    public GameObject[] bugs;
    public int[] answer;

    public bool gameFail;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        cardMap = new Tuple<string, int>[RFIBParameter.stageCol, RFIBParameter.stageRow, RFIBParameter.maxHight];
        SetCardMap(levelParameter.cardMapStr);

        loopCount = new int[levelParameter.stageLoop];
        ifLoop = new bool[levelParameter.stageLoop];
        ResetDish();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCooking()
    {
        gameFail = false;
        SetHints(false);
        SetAllBug(false);
        cardHandler.SetCardTrans(true);
        Debug.Log("Game Start!");
        StartCoroutine(CookingProcess());
    }

    public void FinishCooking()
    {
        basket.SetActive(false);
        Debug.Log("Game Finish!!!");

        gameController.playing = false;
        cardHandler.SetCardTrans(false);
        SetHints(true);
    }

    private void FailCooking()
    {
        gameFail = true;
        gameController.playing = false;
        cardHandler.SetCardTrans(false);
        ResetDish();
    }

    IEnumerator CookingProcess()
    {
        if (!gameFail)
        {
            yield return StartCoroutine(DishMove("Right", 1));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(CheckLoop(0, 0, 0));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(DishMove("Right", 2));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(CheckProcess(1, 1, 0, new int[] { 0 }, new int[] { 3 }));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(DishMove("Right", 1));
        }

        if (ifLoop[0])
        {
            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Up", 2));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Left", 3));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Down", 2));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(CheckLoop(0, 0, 0));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Right", 2));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(CheckProcess(1, 1, 0, new int[] { 1 }, new int[] { 4 }));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Right", 1));
            }
        }

        if (ifLoop[0])
        {
            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Up", 2));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Left", 3));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Down", 2));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(CheckLoop(0, 0, 0));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Right", 2));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(CheckProcess(1, 1, 0, new int[] { 2 }, new int[] { 5 }));
            }

            if (!gameFail)
            {
                yield return StartCoroutine(DishMove("Right", 1));
            }
        }

        if (!gameFail)
        {
            yield return StartCoroutine(DishMove("Right", 1));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(CheckProcess(2, 2, 1, new int[] { 3, 4, 5 }, new int[] { 6 }, 0));
        }

        if (!gameFail)
        {
            yield return StartCoroutine(DishMove("Right", 3));
        }

        if (!gameFail)
        {
            FinishCooking();
        }
    }

    public IEnumerator DishMove(string direction, int step)
    {
        for (int i = 0; i < step * 75; i++)
        {
            yield return new WaitForSeconds(0.01f);
            switch (direction)
            {
                case "Up":
                    dish.transform.localPosition += new Vector3(0f, 0.1f, 0f);
                    break;
                case "Left":
                    dish.transform.localPosition += new Vector3(-0.1f, 0f, 0f);
                    break;
                case "Down":
                    dish.transform.localPosition += new Vector3(0f, -0.1f, 0f);
                    break;
                case "Right":
                    dish.transform.localPosition += new Vector3(0.1f, 0f, 0f);
                    break;
                default:
                    Debug.Log("Move direction error");
                    break;
            }
        }
    }

    public IEnumerator CheckProcess(int cardId, int ansId, int chefNum, int[] hideIngredient, int[] showIngredient, int loopNum = -1)
    {
        bool ifIngredientRight = true;
        string[] chefPosXY = levelParameter.canPlaceCardPos[cardId].Split(',');

        for (int i = 0; i < hideIngredient.Length; i++)
        {
            if (!ingredientStep[hideIngredient[i]].activeSelf)
            {
                ifIngredientRight = false;
            }
        }

        if (RFIBParameter.SearchCard(rFIBManager.blockId[int.Parse(chefPosXY[0]), int.Parse(chefPosXY[1]), 0]) == answer[ansId])
        {
            if (ifIngredientRight)
            {
                for (int i = 0; i < hideIngredient.Length; i++)
                {
                    ingredientStep[hideIngredient[i]].SetActive(false);
                }
                chef[chefNum].GetComponent<ChefAct>().StartAct();
                yield return new WaitForSeconds(2f);
                for (int i = 0; i < showIngredient.Length; i++)
                {
                    ingredientStep[showIngredient[i]].SetActive(true);
                }
            }
            else
            {
                if (loopNum != -1)
                {
                    SetBug(loopNum, true);
                }
                yield return new WaitForSeconds(1.5f);
                FailCooking();
            }
        }
        else
        {
            SetBug(ansId, true);
            yield return new WaitForSeconds(1.5f);
            FailCooking();
        }
    }

    public IEnumerator CheckLoop(int cardId, int ansId, int loopNum)
    {
        yield return new WaitForSeconds(0.5f);

        string[] loopPosXY = levelParameter.canPlaceCardPos[cardId].Split(',');

        if (RFIBParameter.SearchCard(rFIBManager.blockId[int.Parse(loopPosXY[0]), int.Parse(loopPosXY[1]), 0]) == answer[ansId] ||
            RFIBParameter.SearchCard(rFIBManager.blockId[int.Parse(loopPosXY[0]), int.Parse(loopPosXY[1]), 0]) == -1)
        {
            if (loopCount[loopNum] > 1)
            {
                loopCount[loopNum]--;
                ifLoop[loopNum] = true;
            }
            else
            {
                ifLoop[loopNum] = false;
            }
        }
        else
        {
            SetBug(ansId, true);
            yield return new WaitForSeconds(1.5f);
            FailCooking();
        }
    }

    private void ResetDish()
    {
        ResetIngredient(new int[] { 0, 1, 2 });
        dish.transform.localPosition = new Vector3(
            startX * GameParameter.stageGap,
            startY * GameParameter.stageGap,
            0);

        SetHints(true);
    }

    private void SetHints(bool TorF)
    {
        for (int i = 0; i < hints.Length; i++)
        {
            hints[i].SetActive(TorF);
        }
    }

    public void SetBug(int bugId, bool TorF)
    {
        bugs[bugId].SetActive(TorF);
        Debug.Log("Game Over...");
    }

    public void SetAllBug(bool TorF)
    {
        for (int i = 0; i < bugs.Length; i++)
        {
            bugs[i].SetActive(TorF);
        }
    }

    public void SetLoopCount(int loopId, int count)
    {
        loopCount[loopId] = count;
    }

    private void ResetIngredient(int[] ingredientId)
    {
        for (int i = 0; i < ingredientStep.Length; i++)
        {
            if (ingredientId.Contains(i))
            {
                ingredientStep[i].SetActive(true);
            }
            else
            {
                ingredientStep[i].SetActive(false);
            }
        }
    }

    private void SetCardMap(string[] cardMapStr)
    {
        for (int i = 0; i < cardMapStr.Length; i++)
        {
            string[] cardInfo = cardMapStr[i].Split(',');
            cardMap[int.Parse(cardInfo[0]), int.Parse(cardInfo[1]), int.Parse(cardInfo[2])] = Tuple.Create(cardInfo[3], int.Parse(cardInfo[4]));
        }
    }
}
