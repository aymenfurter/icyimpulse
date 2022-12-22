using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject fish; // drag the fish game object into this field in the inspector
    private GameObject camera;

    private Rigidbody2D fishRigidbody;
    private bool fishDropped = false;
    private Vector3 initialPosition;
    private Vector3 initialCameraPosition;
    private bool wasSwinging = false; 

    public void Reset()
    {
        Destroy(fish.GetComponent<Rigidbody2D>());
        fishDropped = false;
        wasSwinging = false;
        // remove momentum from the fish
        fish.transform.position = initialPosition;
        Camera.main.transform.position = initialCameraPosition;
        // Logger 
        Debug.Log("Reset");
    }


    private void Start()
    {
        // copy the initial position of the fish
        initialPosition = fish.transform.position;
        initialCameraPosition = Camera.main.transform.position;

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
            fish.AddComponent<BoxCollider2D>();
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (!fishDropped)
            {
                DropFish();
            }
            else if (!wasSwinging)
            {
                SwingBat();
                wasSwinging = true;
            } else 
            {
                Reset();
            }
        }

    }

    private void DropFish()
    {
        // add a Rigidbody2D component to the fish game object if it doesn't already have one
        if (fish.GetComponent<Rigidbody2D>() != null)
        {
            Destroy(fish.GetComponent<Rigidbody2D>());
        }
        fish.AddComponent<Rigidbody2D>();
        // remove old Rigidbody2D component and add a new one

        fishRigidbody = fish.GetComponent<Rigidbody2D>();
        fishDropped = true;
    }
    private void SwingBat()
    {
        Vector2 personPos = transform.position;
        Vector2 fishPos = fish.transform.position;
        float distance = Vector2.Distance(personPos, fishPos);

        // interpolate between the left direction and the direction to the person based on the distance
        Vector2 direction = Vector2.Lerp(Vector2.left, personPos - fishPos, distance / 10);
        // add an upward or downward vector to the direction based on the relative positions of the fish and person
        if (personPos.y > fishPos.y)
        {
            direction += Vector2.down;
        }
        else if (personPos.y < fishPos.y)
        {
            direction += Vector2.up;
        }

        float force = Mathf.Clamp(distance * 15, 0, 100);


        // check if the person and fish objects are colliding
        if (Physics2D.IsTouching(GetComponent<Collider2D>(), fish.GetComponent<Collider2D>()))
        {
            fishRigidbody.velocity = new Vector2(fishRigidbody.velocity.x, 0);
            fishRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

}
