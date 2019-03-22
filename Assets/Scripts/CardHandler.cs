using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    public RFIBManager rFIBManager;
    public GameController gameController;
    public LevelParameter levelParameter;

    //待改善
    public LevelController levelController;

    #region Card Parameter
    public GameObject parentTransform;
    public GameObject[] cards;

    public bool[,,] canPlaceCard;

    private GameObject[,,] cardInstance;
    public string[,,] lastBlockId;
    private bool[,,] hasPlaced;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        canPlaceCard = new bool[RFIBParameter.stageCol, RFIBParameter.stageRow, RFIBParameter.maxHight];
        
        cardInstance = new GameObject[RFIBParameter.stageCol, RFIBParameter.stageRow, RFIBParameter.maxHight];
        hasPlaced = new bool[RFIBParameter.stageCol, RFIBParameter.stageRow, RFIBParameter.maxHight];
        lastBlockId = new string[RFIBParameter.stageCol, RFIBParameter.stageRow, RFIBParameter.maxHight];

        for (int i = 0; i < RFIBParameter.stageCol; i++)
        {
            for (int j = 0; j < RFIBParameter.stageRow; j++)
            {
                for (int k = 0; k < RFIBParameter.maxHight; k++)
                {
                    canPlaceCard[i, j, k] = false;
                    hasPlaced[i, j, k] = false;
                    lastBlockId[i, j, k] = "0000";
                }
            } 
        }

        SetCanPlaceCard(levelParameter.canPlaceCardPos, true);
    }

    // Update is called once per frame
    void Update()
    {
        updateCards();
    }

    private void updateCards()
    {
        for (int i = 0; i < RFIBParameter.stageCol; i++)
        {
            for (int j = 0; j < RFIBParameter.stageRow; j++)
            {
                for (int k = 0; k < RFIBParameter.maxHight; k++)
                {
                    if (lastBlockId[i, j, k] != rFIBManager.blockId[i, j, k] && canPlaceCard[i, j, k])
                    {
                        if (rFIBManager.blockId[i, j, k] != "0000")
                        {
                            PlaceCard(i, j, k);
                        }
                        else
                        {
                            DestroyCard(i, j, k);
                        }
                        lastBlockId[i, j, k] = rFIBManager.blockId[i, j, k];
                    }
                }
            }
        }
    }

    private void PlaceCard(int x, int y, int z)
    {
        cardInstance[x, y, z] = Instantiate(cards[RFIBParameter.SearchCard(rFIBManager.blockId[x, y, z])], parentTransform.transform);
        cardInstance[x, y, z].transform.localPosition = new Vector3(
            x * GameParameter.stageGap,
            y * GameParameter.stageGap,
            0);
        cardInstance[x, y, z].GetComponent<SpriteRenderer>().sortingOrder = GameParameter.cardOrderInLayer + z;

        hasPlaced[x, y, z] = true;

        //待改善
        levelController.SetBugs(false);
    }

    private void DestroyCard(int x, int y, int z)
    {
        Destroy(cardInstance[x, y, z]);
        cardInstance[x, y, z] = null;
        hasPlaced[x, y, z] = false;

        //待改善
        levelController.SetBugs(false);
    }

    public void SetCanPlaceCard(string[] posSeries, bool TorF)
    {
        for (int i = 0; i < posSeries.Length; i++)
        {
            string[] newPosXYZ = posSeries[i].Split(',');
            canPlaceCard[int.Parse(newPosXYZ[0]), int.Parse(newPosXYZ[1]), int.Parse(newPosXYZ[2])] = TorF;
        }
    }

    public void SetCardTrans(bool TorF)
    {
        for (int i = 0; i < RFIBParameter.stageCol; i++)
        {
            for (int j = 0; j < RFIBParameter.stageRow; j++)
            {
                for (int k = 0; k < RFIBParameter.maxHight; k++)
                {
                    if (hasPlaced[i, j, k] && TorF)
                    {
                        cardInstance[i, j, k].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    else if (hasPlaced[i, j, k])
                    {
                        cardInstance[i, j, k].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    }
                }
            }
        }
    }
}
