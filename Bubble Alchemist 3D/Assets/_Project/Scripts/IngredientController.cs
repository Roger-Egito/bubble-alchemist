using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    [SerializeField] private string type;
    void Start()
    {
        type = IngredientManager.instance.GetRandomIngredientType();       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
