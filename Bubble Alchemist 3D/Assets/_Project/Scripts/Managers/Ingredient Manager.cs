using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager instance { get; private set; }

    [SerializeField] private GameObject[] ingredientPrefab;
    [SerializeField] private Transform[] spawnPoints;
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

    public IngredientController GetRandomIngredient()
    {
        int n = Random.Range(0, spawnedIngredients.Count);
        return spawnedIngredients[n].GetComponent<IngredientController>();
    }

    private void SpawnIngredientsAtAllSpawns()
    {
        for (int i = 0; i < spawnPoints.Length; i++) SpawnIngredient(spawnPoints[i].position);
    }

    public void SpawnIngredientAtRandomSpawn()
    {
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        SpawnIngredient(spawnPoint);
    }

    private void SpawnIngredient(Vector3 position)
    {
        GameObject i = ingredientPrefab[Random.Range(0, ingredientPrefab.Length)];
        GameObject ingredient = Instantiate(i, position, Quaternion.identity); 
        spawnedIngredients.Add(ingredient);
    }
}
