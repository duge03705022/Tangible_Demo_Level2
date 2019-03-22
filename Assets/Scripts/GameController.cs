using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    # region Game Parameter
    public RFIBManager rFIBManager;
    public TouchHandler touchHandler;
    public CardHandler cardHandler;
    public GameObject levelController;

    public bool playing;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
