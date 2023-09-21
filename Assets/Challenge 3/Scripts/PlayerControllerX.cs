using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;
    public float floatForce = 5.0f;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    private bool isLowEnough = true;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player's Rigidbody component
        playerRb = GetComponent<Rigidbody>();

        // Modify gravity according to gravityModifier
        Physics.gravity *= gravityModifier;

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

        // Get the AudioSource component for playing sounds
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Allow the player to float upward when the space key is pressed and the player is low enough
        if (Input.GetKey(KeyCode.Space) && isLowEnough && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        // Check if the player is low enough
        isLowEnough = (transform.position.y <= 13);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (gameOver) return; // Do nothing if the game is already over

        if (other.gameObject.CompareTag("Bomb"))
        {
            // Handle collision with a bomb
            HandleBombCollision();
        }
        else if (other.gameObject.CompareTag("Money"))
        {
            // Handle collision with money
            HandleMoneyCollision(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            // Handle collision with the ground
            HandleGroundCollision();
        }
    }

    private void HandleBombCollision()
    {
        // Play explosion particle and sound, set game over, and destroy the bomb
        explosionParticle.Play();
        playerAudio.PlayOneShot(explodeSound, 1.0f);
        gameOver = true;
        Debug.Log("Game Over!");
        // Optionally destroy the bomb here: Destroy(other.gameObject);
    }

    private void HandleMoneyCollision(GameObject moneyObject)
    {
        // Play fireworks particle and money sound, and destroy the money object
        fireworksParticle.Play();
        playerAudio.PlayOneShot(moneySound, 1.0f);
        Destroy(moneyObject);
    }

    private void HandleGroundCollision()
    {
        // Apply an upward force and play the bounce sound when colliding with the ground
        playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        playerAudio.PlayOneShot(bounceSound, 1.5f);
    }
}
