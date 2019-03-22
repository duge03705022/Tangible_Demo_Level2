using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchHandler : MonoBehaviour
{
    public RFIBManager rFIBManager;
    public GameController gameController;
    public LevelController levelController;

    # region Touch Parameter
    private bool ifTouch;
    private int touchTime;
    private int notTouchTime;

    private Tuple<int, int> nowTouch;
    private Tuple<int, int> lastTouch;

    private string touchAction;

    private int clickCount;

    private Tuple<int, int>[] touchHistory;
    private int touchHistoryCount;
    private string swipeDirection;

    # endregion

    public GameObject recipeInstance;

    // Start is called before the first frame update
    void Start()
    {
        ifTouch = false;
        touchTime = 0;
        notTouchTime = 0;

        nowTouch = Tuple.Create(-1, -1);
        lastTouch = Tuple.Create(-1, -1);

        touchAction = "Idle";

        clickCount = 0;

        touchHistory = new Tuple<int, int>[RFIBParameter.maxTouch];
        touchHistoryCount = 0;
        swipeDirection = "None";
    }

    // Update is called once per frame
    void Update()
    {
        SenseTouch();
        DoAction();
        KeyPressed();

        // Debug msg
        bool ifDebug = false;
        if (ifDebug)
        {
            Debug.Log(string.Format("touchTime:{0} notTouchTime:{1} - {2}({4}) - clickCount:{3}", touchTime, notTouchTime, touchAction, clickCount, swipeDirection));

            string str = "Position history: ";
            for (int i = 0; i < touchHistoryCount; i++)
            {
                str += string.Format("({0}, {1}) ", touchHistory[i].Item1, touchHistory[i].Item2);
            }
            if (touchHistoryCount != 0)
            {
                Debug.Log(str);
            }
        }
    }

    private void SenseTouch()
    {
        ifTouch = false;
        nowTouch = Tuple.Create(-1, -1);
        // Find touching position (*Guarantee one touch per frame)
        for (int i = 0; i < RFIBParameter.touchCol; i++)
        {
            for (int j = 0; j < RFIBParameter.touchRow; j++)
            {
                if (rFIBManager.touchBlock[i, j])
                {
                    ifTouch = true;
                    nowTouch = Tuple.Create(i, j);
                }
            }
        }

        // Touch calculating
        if (ifTouch)
        {
            if (touchTime == 0 || !nowTouch.Equals(lastTouch))
            {
                if (!nowTouch.Equals(lastTouch))
                {
                    clickCount = 0;
                }

                clickCount++;

                touchAction = "ClickAgain";
                touchHistory[touchHistoryCount] = Tuple.Create(nowTouch.Item1, nowTouch.Item2);
                touchHistoryCount++;
            }

            touchTime++;
            notTouchTime = 0;
            lastTouch = Tuple.Create(nowTouch.Item1, nowTouch.Item2);
        }
        else
        {
            touchTime = 0;
            notTouchTime++;
        }

        // Identify the touch action
        IdentifyAction();
    }

    private void IdentifyAction()
    {
        // Click
        if (clickCount == 1 && touchAction != "ClickDone")
        {
            touchAction = "Click";
        }
        // DoubleClick
        if (clickCount == 2 && touchAction != "DoubleClickDone")
        {
            touchAction = "DoubleClick";
        }
        // Hold
        if (touchTime >= 30 && touchAction != "HoldDone" && touchAction != "SwipeDone")
        {
            touchAction = "Hold";
            clickCount = 0;
        }
        // Swipe
        if (touchHistoryCount >= 3 && touchAction != "SwipeDone")
        {
            if (touchHistory[2].Item1 > touchHistory[1].Item1 &&
                touchHistory[1].Item1 > touchHistory[0].Item1 &&
                touchHistory[2].Item2 == touchHistory[1].Item2 &&
                touchHistory[1].Item2 == touchHistory[0].Item2)
            {
                touchAction = "Swipe";
                swipeDirection = "Left";
                clickCount = 0;
            }
            else if (touchHistory[2].Item1 == touchHistory[1].Item1 &&
                touchHistory[1].Item1 == touchHistory[0].Item1 &&
                touchHistory[2].Item2 > touchHistory[1].Item2 &&
                touchHistory[1].Item2 > touchHistory[0].Item2)
            {
                touchAction = "Swipe";
                swipeDirection = "Up";
                clickCount = 0;
            }
            else if (touchHistory[2].Item1 < touchHistory[1].Item1 &&
                touchHistory[1].Item1 < touchHistory[0].Item1 &&
                touchHistory[2].Item2 == touchHistory[1].Item2 &&
                touchHistory[1].Item2 == touchHistory[0].Item2)
            {
                touchAction = "Swipe";
                swipeDirection = "Right";
                clickCount = 0;
            }
            else if (touchHistory[2].Item1 == touchHistory[1].Item1 &&
                touchHistory[1].Item1 == touchHistory[0].Item1 &&
                touchHistory[2].Item2 < touchHistory[1].Item2 &&
                touchHistory[1].Item2 < touchHistory[0].Item2)
            {
                touchAction = "Swipe";
                swipeDirection = "Down";
                clickCount = 0;
            }
        }
        // Idle
        if (notTouchTime >= 40 && touchAction != "IdleDone")
        {
            touchAction = "Idle";
            ResetTouch();
        }
    }

    private void DoAction()
    {
        switch (touchAction)
        {
            case "Click":
                Click(touchHistory[touchHistoryCount - 1].Item1, touchHistory[touchHistoryCount - 1].Item2);
                break;
            case "DoubleClick":
                DoubleClick(touchHistory[touchHistoryCount - 1].Item1, touchHistory[touchHistoryCount - 1].Item2);
                break;
            case "Hold":
                Hold(touchHistory[touchHistoryCount - 1].Item1, touchHistory[touchHistoryCount - 1].Item2);
                break;
            case "Swipe":
                Swipe(swipeDirection, touchHistory[touchHistoryCount - 3].Item1, touchHistory[touchHistoryCount - 3].Item2);
                break;
            case "Idle":
                Idle();
                break;
        }
    }

    private void Click(int x, int y)
    {
        if (GameParameter.playBtnPos.Equals(Tuple.Create(x / 3, y / 3)))
        {
            levelController.StartCooking();
            gameController.playing = true;
        }
        else if (GameParameter.recipeBtnPos.Equals(Tuple.Create(x / 3, y / 3)))
        {
            recipeInstance.SetActive(!recipeInstance.activeSelf);
        }

        touchAction = "ClickDone";
    }

    private void DoubleClick(int x, int y)
    {


        touchAction = "DoubleClickDone";
    }

    private void Hold(int x, int y)
    {


        touchAction = "HoldDone";
    }

    private void Swipe(string direction, int x, int y)
    {


        touchAction = "SwipeDone";
    }

    private void Idle()
    {


        touchAction = "IdleDone";
    }

    private void ResetTouch()
    {
        touchHistory = new Tuple<int, int>[RFIBParameter.maxTouch];
        clickCount = 0;
        touchHistoryCount = 0;
        swipeDirection = "None";
        lastTouch = Tuple.Create(-1, -1);
    }

    private void KeyPressed()
    {
        if (Input.GetKey("r"))
        {
            rFIBManager.touchBlock[19, 13] = true;
        }
        else
        {
            rFIBManager.touchBlock[19, 13] = false;
        }

        if (Input.GetKey("p"))
        {
            rFIBManager.touchBlock[25, 1] = true;
        }
        else
        {
            rFIBManager.touchBlock[25, 1] = false;
        }
    }
}
