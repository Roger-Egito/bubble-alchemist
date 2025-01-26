using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    public Sprite sprite;
    [SerializeField] private GameObject downArrow;
    [SerializeField] private GameObject upArrow;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>().sprite;
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

    public bool CompareItems(IngredientController other)
    {
        if(sprite == other.sprite) return true;
        return false;
    }
}
