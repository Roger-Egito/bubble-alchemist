using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    public Sprite sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public bool CompareItems(IngredientController other)
    {
        if(sprite == other.sprite) return true;
        return false;
    }
}
