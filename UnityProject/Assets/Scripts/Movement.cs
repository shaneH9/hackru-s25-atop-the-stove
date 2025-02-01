using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private Vector3 startPos;
    private Rigidbody2D body;
    
    

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.position = startPos;
    }

    private void Update()
    {
       body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") *speed, Input.GetAxis("Vertical")*speed);

    }
}
