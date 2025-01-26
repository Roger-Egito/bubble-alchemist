using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    [SerializeField] private UnityEvent OnCorrectItem;
    [SerializeField] private UnityEvent OnWrongItem;
    [SerializeField] private Image itemImage;

    public IngredientController currentItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetNextItem();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IngredientController>(out var item))
        {
            if (item.CompareItems(currentItem))
            {
                CorrectItem();
                Destroy(item.gameObject);
            }
        }

        WrongItem();
    }

    private void GetNextItem()
    {
        currentItem = IngredientManager.instance.GetRandomIngredient();
        itemImage.sprite = currentItem.sprite;
    }

    public void CorrectItem()
    {
        OnCorrectItem?.Invoke();
        GetNextItem();
    }

    public void WrongItem()
    {
        OnWrongItem?.Invoke();
    }
}
