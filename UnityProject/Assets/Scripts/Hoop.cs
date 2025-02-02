using UnityEngine;

public class Hoop : MonoBehaviour
{
    private GameObject hoop; 
    private Vector2 spawnMin = new Vector2(-10, -4);
    private Vector2 spawnMax = new Vector2(10, 3);
    public void spawn()
    {

        Vector3 spawnPosition = new Vector3(
        Random.Range(spawnMin.x, spawnMax.x),
        Random.Range(spawnMin.y, spawnMax.y),
        0f
    );
        Instantiate(hoop, spawnPosition, Quaternion.identity);
    }
}