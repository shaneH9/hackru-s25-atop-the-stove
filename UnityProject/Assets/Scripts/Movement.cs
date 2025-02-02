using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private Vector3 startPos;
    private Rigidbody2D body;
    private Vector2 position;
    private String inventory = "";
    private Vector2 spawnMin = new Vector2(-10,-4);
    private Vector2 spawnMax = new Vector2(10,3);
    private Animator animator; 
    private Vector2 movement; 
    

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
       movement.x = Input.GetAxisRaw("Horizontal");
       movement.y = Input.GetAxisRaw("Vertical");

       animator.SetFloat("Horizontal", movement.x);
       animator.SetFloat("Vertical", movement.y);
       animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    private void OnCollisionEnter2D(Collider2D col)
    {
        switch(col.gameObject.name)
        {
            case "Ingredient1":
            inventory = "Ingredient1";
            col.gameObject.SetActive(false);
            break;
            case "Ingredient2":
            inventory = "Ingredient2";
            col.gameObject.SetActive(false);
            break;
            case "Ingredient3":
            inventory = "Ingredient3";
            col.gameObject.SetActive(false);
            break;
            case "Ingredient4":
            inventory = "Ingredient4";
            col.gameObject.SetActive(false);
            break;
            case "hoop":
            if(!(inventory.Equals("")) && Input.GetKey(KeyCode.Space))
            {

            }
            break;
        }
    }


}
