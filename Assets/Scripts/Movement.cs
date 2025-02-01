using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed; 
    private Rigidbody2D body;
    Vector2 movement; 

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement.Set(InputManager.Movement.x *speed,InputManager.Movement.y *speed);
        
    }
}
