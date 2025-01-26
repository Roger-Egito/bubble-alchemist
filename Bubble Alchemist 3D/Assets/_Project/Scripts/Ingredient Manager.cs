using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager instance { get; private set; }

    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<string> ingredientTypes;
    [SerializeField] private List<GameObject> spawnedIngredients;
    [Range(0f, 10f)]
    [SerializeField] private float spawnCooldown = 5f;
    [SerializeField] private int spawnLimit = 30;
    private float timeStamp;

    public string GetRandomIngredientType()
    {
        return ingredientTypes[UnityEngine.Random.Range(0, ingredientTypes.Count)]; ;
    }

    public List<GameObject> GetSpawnedIngredients()
    {
        return spawnedIngredients;
    }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this); else instance = this;
        timeStamp = Time.time;
    }
    
    void Start()
    {
        //SpawnIngredientsAtAllSpawns();
        SpawnIngredientAtRandomSpawn(5);
    }

    void Update()
    {
        if (timeStamp <= Time.time && spawnedIngredients.Count < spawnLimit)
        {
            SpawnIngredientAtRandomSpawn();
            timeStamp = Time.time + spawnCooldown;
        }
    }

    private void SpawnIngredientsAtAllSpawns()
    {
        for (int i = 0; i < spawnPoints.Count; i++) SpawnIngredient(spawnPoints[i].position);
    }

    public void SpawnIngredientAtRandomSpawn(int amount=1)
    {
        for (int i=0; i < amount; i++)
        {
            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            SpawnIngredient(spawnPoint);
        }
    }

    private void SpawnIngredient(Vector3 position)
    {
        GameObject ingredient = Instantiate(ingredientPrefab, position, ingredientPrefab.transform.rotation);
        spawnedIngredients.Add(ingredient);
    }
}
