using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HightPath : MonoBehaviour
{
    public LevelController levelController;
    public GameObject path;
    public int hightPathId;
    private int shine;

    // Start is called before the first frame update
    void Start()
    {
        shine = 0;
    }

    // Update is called once per frame
    void Update()
    {
        path.SetActive(levelController.ifLoop[hightPathId]);

        SpriteRenderer[] childrenSprite = path.GetComponentsInChildren<SpriteRenderer>();

        if (shine == 20)
        {
            foreach (var chidSprite in childrenSprite)
            {
                chidSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }
        else if (shine == 0)
        {
            foreach (var chidSprite in childrenSprite)
            {
                chidSprite.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        shine++;
        shine = shine % 40;
    }
}
