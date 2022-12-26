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
    public static bool isSlideable = false;

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
        FishScript.score = "";
    }


    private void Start()
    {
        // copy the initial position of the fish
        initialPosition = fish.transform.position;
        
        // check if initialCamera position is null
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

        if (fishDropped && wasSwinging)
        {
            MoveCamera();
        }

    }

    private void MoveCamera()  
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 fishPos = fish.transform.position;
        cameraPos.x = Mathf.Lerp(cameraPos.x, fishPos.x - 5, 0.01f); // 5 is the left offset
        Camera.main.transform.position = cameraPos;
    }

    private void DropFish()
    {
        if (fish.GetComponent<Rigidbody2D>() != null)
        {
            Destroy(fish.GetComponent<Rigidbody2D>());
        }
        fish.AddComponent<Rigidbody2D>();
        fishRigidbody = fish.GetComponent<Rigidbody2D>();
        fishDropped = true;
    }
    private void SwingBat()
    {
        Vector2 personPos = transform.position;
        Vector2 fishPos = fish.transform.position;
        //float distance = Vector2.Distance(personPos, fishPos);

        float distance = (personPos - fishPos).sqrMagnitude;

        Vector2 direction = Vector2.Lerp(Vector2.left, personPos - fishPos, distance / 10);

        if (personPos.y > fishPos.y)
        {
            direction += Vector2.down;
            PersonScript.isSlideable = true;
        }
        else if (personPos.y < fishPos.y)
        {

            direction += Vector2.up;
            PersonScript.isSlideable = false;
        }

        float minForce = 50;
        float maxForce = 100;
        float randomForce = Random.Range(minForce, maxForce);

        float force = Mathf.Clamp((distance * 2500 / 100), 0, 100) + randomForce / 70;
        if (Physics2D.IsTouching(GetComponent<Collider2D>(), fish.GetComponent<Collider2D>()))
        {
            fishRigidbody.velocity = new Vector2(fishRigidbody.velocity.x, 0);
            fishRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

}
