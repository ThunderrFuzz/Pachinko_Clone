using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BouncerAddVel : MonoBehaviour
{
    public ParticleSystem hatParticleSystem; // particle system in order to toggle the particles on upon contact with a hat
    public AudioSource audioSource; //audio source and clips to be set in engine 
    public AudioClip hatHitSound; // win sfx
    public AudioClip hatMissSound; // loss sfx
    public AudioClip bouncerHitSound; 
    public AudioClip diceHitSound;
    public AudioClip gameWon;
    public Text messageText; // ui text component for displaying basic text game over, resetting, and game won messages 
    public Rigidbody2D rb; // rigigd body for impulse manipulations
    public int bouncerVel; // addtion to velocity for bouncer block 
    public int bounceDefault; // base bounce force of a regular box
    //priv vars
    private bool gameStopped = false; // checks if the game should be paused or not based on resetting etc
    private int pointsEarned = 0;  // keeps check of points earned 
    private Vector3 startingPos = new Vector3(1.4f, 6f, 0f); // marble starting pos 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        Time.timeScale = gameStopped ? 0f : 1f;
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!gameStopped)
        {
            Vector2 collisionNormal = col.contacts[0].normal;  // accesses the normal of what the balls most recent collision 
            float collisionAngle = Vector2.Angle(collisionNormal, Vector2.up); // gets the angle of the impact
            Vector2 relativeVelocity = col.relativeVelocity; //grabs the relative velocity in order to be used in the reflection calculation      
            Vector2 invertedAngle = Vector2.Reflect(relativeVelocity, collisionNormal); // inverts the impact angle using relative vel and the col normal
            SpriteRenderer diceRenderer = col.gameObject.GetComponent<SpriteRenderer>(); // adds the sprite renderer companant to the funcion locally in order to use it later in in dice box code
            //case select for checking tags
            switch (col.gameObject.tag)
            {
                case "Hat":
                    GameWon();
                    break;
                case "HatMiss":
                    GameLost();
                    break;
                case "Bouncer":
                    //adds force based to the marbel at the inverted angle
                    rb.AddForce(invertedAngle * bouncerVel * 200 * Time.deltaTime, ForceMode2D.Impulse);
                    audioSource.PlayOneShot(bouncerHitSound);
                    pointsEarned += 25;
                    break;
                case "Dice":
                    int diceForce = CalculateDiceForce(diceRenderer); // calls dice force to figure out what level of force to be added
                    rb.AddForce(invertedAngle * diceForce * Time.deltaTime, ForceMode2D.Impulse); // adds force based on func return
                    pointsEarned += diceForce / 25;  // points added differently from your 'roll'
                    audioSource.PlayOneShot(diceHitSound);
                    break;
                default:
                    rb.AddForce(invertedAngle * bounceDefault * 100 * Time.deltaTime, ForceMode2D.Impulse);  // colliding with anything not tagged will default to this
                    pointsEarned += 5;
                    audioSource.PlayOneShot(bouncerHitSound);
                    break;
            }

        }
    }
    public void RestartGame()
    {
        gameStopped = false; // starts the game 
        
        SceneManager.LoadScene("SampleScene");
        pointsEarned = 0;  // resets pointds
        messageText.text = "Game Reset"; // tells player game has been reset
        //reset marble position
        transform.position = startingPos; // reset marbles position
    }
    void GameWon()
    {
        gameStopped = true;
        messageText.text = "Game won! You scored : " + pointsEarned.ToString() + " Points. Press 'R' to reset.";
        audioSource.PlayOneShot(hatHitSound); // hat hit
        audioSource.PlayOneShot(gameWon); // winning sound
        hatParticleSystem.Play();        
    }
    void GameLost()
    {
        gameStopped = true;
        messageText.text = "Game Lost! You missed the hats! Press 'R' to reset";
        Invoke("RestartGame()", 3f);
        audioSource.PlayOneShot(hatMissSound);
    }
    int CalculateDiceForce(SpriteRenderer diceRenderer)
    {
        //case selector for dice 'side', force added based on the name of the sprite, dependant on when marble hits 
        switch (diceRenderer.sprite.name)
        {
            case "Dice_0":
                return 100;
            case "Dice_1":
                return 200;
            case "Dice_2":
                return 300;
            case "Dice_3":
                return 400;
            case "Dice_4":
                return 500;
            case "Dice_5":
                return 1200;
            default:
                return 0;
        }
    }
}
