using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject fish; // drag the fish game object into this field in the inspector
    private GameObject camera;
     public Sprite img1 , img2;



    [SerializeField]
    private GameObject intro;

    [SerializeField]
    private GameObject playerPicture;

    private Rigidbody2D fishRigidbody;
    private bool fishDropped = false;
    private Vector3 initialPosition;
    private Vector3 initialCameraPosition;
    private bool wasSwinging = false; 
    public static bool isSlideable = false;
    private bool hasStarted = false;
    public AudioClip audioClip;
    public AudioClip swingClip;

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
        playerPicture.GetComponent<SpriteRenderer>().sprite = img1; 
    }


    private void Start()
    {
        // copy the initial position of the fish
        initialPosition = fish.transform.position;
        
        // check if initialCamera position is null
        initialCameraPosition = Camera.main.transform.position;

        var audioSource = gameObject.AddComponent<AudioSource>();

        // Set the audio clip
        audioSource.clip = audioClip;
        audioSource.loop = true;

        // Play the audio
        audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hasStarted) 
            {
                if (GetComponent<Collider2D>() == null)
                {
                    gameObject.AddComponent<BoxCollider2D>();
                    fish.AddComponent<BoxCollider2D>();
                    GetComponent<Collider2D>().isTrigger = true;
                }
                hasStarted = true;
            }

            intro.SetActive(false);
            if (!fishDropped)
            {
                DropFish();
            }
            else if (!wasSwinging)
            {
                SwingBat();
                PlaySwingSound();
                playerPicture.GetComponent<SpriteRenderer>().sprite = img2; 
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

    private void PlaySwingSound() 
    {
        var audioSource = gameObject.AddComponent<AudioSource>();

        // Set the audio clip
        audioSource.clip = swingClip;

        // Play the audio
        audioSource.Play();
    }

    private void MoveCamera()  
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 fishPos = fish.transform.position;
        if (cameraPos.x >= fishPos.x)
        {
        cameraPos.x = fishPos.x;//Mathf.Lerp(cameraPos.x, fishPos.x - 5, 0.01f); // 5 is the left offset
        Camera.main.transform.position = cameraPos;
        }
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
        float maxForce = 500;
        float randomForce = Random.Range(minForce, maxForce);

        float force = Mathf.Clamp((distance * 2500 / 150), 0, 100) + randomForce / 70;
        if (Physics2D.IsTouching(GetComponent<Collider2D>(), fish.GetComponent<Collider2D>()))
        {
            fishRigidbody.velocity = new Vector2(fishRigidbody.velocity.x, 0);
            fishRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

}
