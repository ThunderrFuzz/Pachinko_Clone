using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BouncerAddVel : MonoBehaviour
{
    public ParticleSystem hatParticleSystem; // Particle system in order to toggle the particles on upon contact with a hat
    public AudioSource audioSource; // Audio source and clips to be set in the engine 
    public AudioClip hatHitSound; // Win sound effect
    public AudioClip hatMissSound; // Loss sound effect
    public AudioClip bouncerHitSound;
    public AudioClip diceHitSound;
    public AudioClip gameWon;
    public Text messageText; // UI text component for displaying basic text game over, resetting, and game won messages 
    public Rigidbody2D rb; // Rigidbody for impulse manipulations
    public int bouncerVel; // Addition to velocity for bouncer block 
    public int bounceDefault; // Base bounce force of a regular box
    // Private variables
    private bool gameStopped = false; // Checks if the game should be paused or not based on resetting, etc.
    private int pointsEarned = 0;  // Keeps check of points earned 
    private Vector3 startingPos = new Vector3(1.4f, 6f, 0f); // Marble starting position 

    void Start()
    {
        // Get the AudioSource component from the GameObject
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
            Vector2 collisionNormal = col.contacts[0].normal;  // Accesses the normal of what the ball's most recent collision 
            float collisionAngle = Vector2.Angle(collisionNormal, Vector2.up); // Gets the angle of the impact
            Vector2 relativeVelocity = col.relativeVelocity; // Grabs the relative velocity in order to be used in the reflection calculation      
            Vector2 invertedAngle = Vector2.Reflect(relativeVelocity, collisionNormal); // Inverts the impact angle using relative velocity and the collision normal
            SpriteRenderer diceRenderer = col.gameObject.GetComponent<SpriteRenderer>(); // Adds the sprite renderer component to the function locally in order to use it later in the dice box code

            // Case select for checking tags
            switch (col.gameObject.tag)
            {
                case "Hat":
                    GameWon();
                    break;
                case "HatMiss":
                    GameLost();
                    break;
                case "Bouncer":
                    // Adds force based on the marble's inverted angle
                    rb.AddForce(invertedAngle * bouncerVel * 200 * Time.deltaTime, ForceMode2D.Impulse);
                    audioSource.PlayOneShot(bouncerHitSound);
                    pointsEarned += 25;
                    break;
                case "Dice":
                    int diceForce = CalculateDiceForce(diceRenderer); // Calls dice force to figure out what level of force to be added
                    rb.AddForce(invertedAngle * diceForce * Time.deltaTime, ForceMode2D.Impulse); // Adds force based on function return
                    audioSource.PlayOneShot(diceHitSound);
                    pointsEarned += diceForce / 25;  // Points added differently from your 'roll'
                    break;
                default:
                    rb.AddForce(invertedAngle * bounceDefault * 100 * Time.deltaTime, ForceMode2D.Impulse);  // Colliding with anything not tagged will default to this
                    audioSource.PlayOneShot(bouncerHitSound);
                    pointsEarned += 5;
                    break;
            }

        }
    }

    public void RestartGame()
    {
        transform.position = startingPos;//
        rb.constraints = RigidbodyConstraints2D.None; // remove movement constrains from 
        gameStopped = false; // Start the game 
        pointsEarned = 0;  // Reset points
        messageText.text = "Game Reset"; // Tells the player the game has been reset
        audioSource = GetComponent<AudioSource>(); // reassignes the audiosoruce hopefully.
        SceneManager.LoadScene("SampleScene"); // Reload the scene
    }

    void GameWon()
    {
        rb.velocity = Vector2.zero; // set vel to 0
        rb.constraints = RigidbodyConstraints2D.FreezeAll; //freezes all interactions on the marbles postion
        messageText.text = "Game won! You scored : " + pointsEarned.ToString() + " Points. Press 'R' to reset.";
        audioSource.PlayOneShot(hatHitSound); // Play the hat hit sound
        audioSource.PlayOneShot(gameWon); // Play the winning sound
        hatParticleSystem.Play(); // Play the particle system
    }
    void GameLost()
    {
        gameStopped = true;
        messageText.text = "Game Lost! You missed the hats! Press 'R' to reset";
        Invoke("RestartGame", 3f); // Invoke the restart method after a delay
       
        audioSource.PlayOneShot(hatMissSound); // Play the hat miss sound
        
    }

    int CalculateDiceForce(SpriteRenderer diceRenderer)
    {
        // Case selector for dice 'side', force added based on the name of the sprite, dependent on when marble hits 
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
