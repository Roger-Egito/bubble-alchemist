using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager instance { get; private set; }

    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<GameObject> spawnedIngredients;
    [Range(0f, 10f)]
    [SerializeField] private float spawnCooldown = 5f;
    private float timeStamp;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this); else instance = this;
        timeStamp = Time.time;
    }
    
    void Start()
    {
        SpawnIngredientsAtAllSpawns();
    }

    void Update()
    {
        if (timeStamp <= Time.time)
        {
            SpawnIngredientAtRandomSpawn();
            timeStamp = Time.time + spawnCooldown;
        }
    }

    private void SpawnIngredientsAtAllSpawns()
    {
        for (int i = 0; i < spawnPoints.Count; i++) SpawnIngredient(spawnPoints[i].position);
    }

    public void SpawnIngredientAtRandomSpawn()
    {
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        SpawnIngredient(spawnPoint);
    }

    private void SpawnIngredient(Vector3 position)
    {
        GameObject ingredient = Instantiate(ingredientPrefab, position, ingredientPrefab.transform.rotation);
        spawnedIngredients.Add(ingredient);
    }
}
