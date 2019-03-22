using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefAct : MonoBehaviour
{
    public GameObject chefAct;
    public GameObject chefTable;

    public void StartAct()
    {
        StartCoroutine(Act());
    }

    IEnumerator Act()
    {
        chefTable.SetActive(false);
        chefAct.SetActive(true);
        yield return new WaitForSeconds(2f);
        chefAct.SetActive(false);
        chefTable.SetActive(true);
    }
}
