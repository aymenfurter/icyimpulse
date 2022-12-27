using UnityEngine;
using UnityEngine.UI;

public class FishScript : MonoBehaviour
{
    private Rigidbody2D fishRigidbody;
    private bool sliding = false;
    private Vector2 slidingDirection = Vector2.zero;
    private TextMesh scoreText;
    private GameObject scoreTextObject;
    private GUIStyle labelStyle = new GUIStyle();
    public static string score = "";

    private void Start()
    {
        labelStyle.fontSize = 90;
        labelStyle.normal.textColor = Color.grey;
        labelStyle.alignment = TextAnchor.MiddleCenter;
    }

    private void Update()
    {
        if (sliding)
        {
            GameObject fish = this.gameObject;
            if (fish.GetComponent<Rigidbody2D>() == null)
            {
                fish.AddComponent<Rigidbody2D>();
            }
            Rigidbody2D fishRigidbody = fish.GetComponent<Rigidbody2D>();
            fishRigidbody.AddForce(slidingDirection * 10, ForceMode2D.Impulse);
            sliding = false;
        }

       
    }


private void OnGUI()
{
    GUIStyle labelStyle = new GUIStyle();

    // specify the font size and font color for the label
    labelStyle.fontSize = 90;
    labelStyle.normal.textColor = Color.black;

    // specify the content offset for the label
    labelStyle.contentOffset = new Vector2(1, 0);

    // draw the label with the specified style and content offset
    GUI.Label(new Rect(Screen.width / 2 - 230, 50, 100, 50),  score, labelStyle);

    // change the content offset for the label
    labelStyle.contentOffset = new Vector2(-1, 0);

    // draw the label with the modified content offset
    GUI.Label(new Rect(Screen.width / 2 - 230, 50, 100, 50),  score, labelStyle);

    // change the content offset for the label
    labelStyle.contentOffset = new Vector2(0, 1);

    // draw the label with the modified content offset
    GUI.Label(new Rect(Screen.width / 2 - 230, 50, 100, 50),  score, labelStyle);

    // change the content offset for the label
    labelStyle.contentOffset = new Vector2(0, -1);

    // draw the label with the modified content offset
    GUI.Label(new Rect(Screen.width / 2 - 230, 50, 100, 50),  score, labelStyle);

    // reset the content offset for the label
    labelStyle.contentOffset = Vector2.zero;

    // draw the label with the original content offset
    labelStyle.normal.textColor = Color.white;
    GUI.Label(new Rect(Screen.width / 2 - 230, 50, 100, 50),  score, labelStyle);
}

    private void OnCollisionEnter2D(Collision2D collision)
    {
            // calculate the sliding direction based on the angle of the ground
            if (PersonScript.isSlideable) 
            {
            Vector2 groundNormal = collision.contacts[0].normal;
            slidingDirection = Vector2.Perpendicular(groundNormal);

            // set the sliding flag to true
            sliding = true;
            }
            
            int currentScore = Mathf.RoundToInt(Mathf.Abs(transform.position.x));
            score = "Score: " + string.Format("{0:000}", currentScore);
    } 
}
