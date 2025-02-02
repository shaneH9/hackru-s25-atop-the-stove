using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] ingredients;
    public float spawnInterval = 15f;
    private Vector2 spawnMin = new Vector2(-10,-4);
    private Vector2 spawnMax = new Vector2(10,3);

    private int currentIngredientCount = 0;
    private const int maxIngredients = 3;
    private HashSet<Vector3> ingredientPositions = new HashSet<Vector3>();


    private void Start()
    {                
        StartCoroutine(SpawnIngredients());
    }

    IEnumerator SpawnIngredients()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentIngredientCount < maxIngredients)
            {
                SpawnIngredient();
            }
        }
    }
    private void SpawnIngredient()
    {
        int randomIndex = Random.Range(0, ingredients.Length);

        Vector3 spawnPosition = new Vector3(
        Random.Range(spawnMin.x, spawnMax.x),
        Random.Range(spawnMin.y, spawnMax.y),
        0f
    );
        ingredientPositions.Add(spawnPosition);
        Instantiate(ingredients[randomIndex], spawnPosition, Quaternion.identity);

        currentIngredientCount++;

    }

    public HashSet<Vector3> getIngredientsPositions()
    {
        return ingredientPositions; 
    }

}