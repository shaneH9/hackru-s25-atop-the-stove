using UnityEngine;
using System.Collections;
public class IngredientSpawner : MonoBehaviour
{
    public GameObject[] ingredients;
    public Transform spawnArea;
    public float spawnInterval = 15f;

    private int currentIngredientCount = 0;
    private const int maxIngredients = 3;
    private bool isBomb;

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
        Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
        Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2),
        0f
    );
        Instantiate(ingredients[randomIndex], spawnPosition, Quaternion.identity);

        currentIngredientCount++;

    }

}