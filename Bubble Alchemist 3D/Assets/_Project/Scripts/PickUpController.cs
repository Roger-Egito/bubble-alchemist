using System.Collections.Generic;
using System.Net.WebSockets;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [Header("Debug")]
    [SerializeField] private List<GameObject> ingredientsInPickUpRange;
    [SerializeField] private GameObject oldClosestIngredient;
    [SerializeField] private GameObject pickedUpObject;
    [SerializeField] private GameObject droppedObject;
    [SerializeField] private float pickUpItemFloatingDistance = 0.1f;

    [SerializeField] private InputAction sInput;
    [SerializeField] private InputAction wInput;
    [SerializeField] private bool wasSPressed;
    [SerializeField] private bool wasWPressed;


    private void Awake()
    {
        sInput = playerInput.actions["PickUp"];
        wInput = playerInput.actions["Throw"];
    }
    private void Update()
    {
        GameObject closestIngredient = GetClosestIngredient();
        if (closestIngredient != null) { closestIngredient.GetComponent<IngredientController>().ShowPickUpGUI(); }

        // Maintain the object above your head, without it spinning
        if (pickedUpObject != null)
        {
            pickedUpObject.transform.position = transform.position + new Vector3(0, pickUpItemFloatingDistance, 0);
            pickedUpObject.transform.rotation = Quaternion.identity;
        }

        if (sInput.WasPressedThisFrame())
        {
            if (pickedUpObject != null) Drop(pickedUpObject);
            if (closestIngredient != null && closestIngredient != droppedObject) PickUp(closestIngredient);
            droppedObject = null;
        }
        else if (wInput.WasPressedThisFrame())
        {
            {
                if (pickedUpObject != null) Throw(pickedUpObject);
            }
        }
    }

    private GameObject GetClosestIngredient()
    {
        float closestDistanceSqr = Mathf.Infinity;
        GameObject closestIngredient = null;

        foreach (GameObject potentialChoice in ingredientsInPickUpRange)
        {
            if (potentialChoice == pickedUpObject) continue;
            Vector3 directionToChoice = potentialChoice.transform.position - transform.position;
            float dSqrToChoice = directionToChoice.sqrMagnitude;
            if (dSqrToChoice < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToChoice;
                closestIngredient = potentialChoice;
            }
        }

        if (oldClosestIngredient != null) oldClosestIngredient.GetComponent<IngredientController>().HidePickUpGUI();
        oldClosestIngredient = closestIngredient;
        return closestIngredient;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Ingredient")) return;
        ingredientsInPickUpRange.Add(collision.gameObject);
        //PickUp(collision.gameObject);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.gameObject.CompareTag("Ingredient")) return;
        ingredientsInPickUpRange.Remove(collision.gameObject);
    }

    private void PickUp(GameObject item)
    {
        
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        item.transform.position = transform.position + new Vector3(0, pickUpItemFloatingDistance, 0);
        item.transform.rotation = Quaternion.identity;
        pickedUpObject = item;
        pickedUpObject.GetComponent<IngredientController>().ShowThrowGUI();
    }

    private void Drop(GameObject item)
    {
        if (item == null) return;
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        item.GetComponent<Rigidbody2D>().AddForceY(200);
        pickedUpObject.GetComponent<IngredientController>().HideThrowGUI();
        pickedUpObject = null;
        droppedObject = item;
    }

    private void Throw(GameObject item)
    {
        if (item == null) return;
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        item.GetComponent<Rigidbody2D>().AddForceY(200);
        if (PlayerMovement.instance.isFacingRight)
        {
            item.GetComponent<Rigidbody2D>().AddForceX(400);
        } else
        {
            item.GetComponent<Rigidbody2D>().AddForceX(-400);
        }
        
        pickedUpObject = null;
    }
}
