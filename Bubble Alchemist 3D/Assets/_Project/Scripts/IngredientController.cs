using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    [SerializeField] private string type;
    [SerializeField] private GameObject downArrow;
    [SerializeField] private GameObject upArrow;

    void Start()
    {
        type = IngredientManager.instance.GetRandomIngredientType();       
    }

    public void ShowPickUpGUI()
    {
        downArrow.SetActive(true);
    }

    public void ShowThrowGUI()
    {
        upArrow.SetActive(true);
    }

    public void HidePickUpGUI()
    {
        downArrow.SetActive(false);
    }

    public void HideThrowGUI()
    {
        upArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
