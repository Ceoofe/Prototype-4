using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables //
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private GameObject focalPoint;
    public bool hasPowerup;
    public bool hasPowerup2;
    public bool hasPowerup3;
    public bool isJump = false;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    public GameObject powerupIndicator2;
    public GameObject powerupIndicator3;
    public GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); // Finds player rigidbody
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0); // Sets the indicators position 
        powerupIndicator2.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupIndicator3.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (hasPowerup2 && Input.GetKeyDown("f"))
        {
            StartCoroutine(BulletSpawn());
        }
        if (hasPowerup3 && Input.GetKeyDown("space") && isJump == false)
        {
            playerRb.AddForce(Vector3.up * powerupStrength, ForceMode.Impulse); // Add force up axis
            isJump = true;
            StartCoroutine(Velocity());
        }

        if (transform.position.y < -2)
        {
            transform.position = new Vector3(0,0,0); // If player falls, player position is set to the center
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup") && hasPowerup2 == false && hasPowerup3 == false) // Collects the powerup and activites it
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
        if (other.CompareTag("Powerup2") && hasPowerup == false && hasPowerup3 == false)
        {
            hasPowerup2 = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator2.gameObject.SetActive(true);
        }
        if (other.CompareTag("Powerup3") && hasPowerup2 == false && hasPowerup == false)
        {
            hasPowerup3 = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator3.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup) // Pushes the enemy with force
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("Ground") && hasPowerup3 && isJump == true) // Sets a explosion once on ground
        {
            for (int i = 0; i < GameObject.FindObjectsOfType<Enemy>().Length; i++)
            {
                var eneimes = GameObject.FindObjectsOfType<Enemy>();
                eneimes[i].GetComponent<Rigidbody>().AddExplosionForce(500.0f, transform.position, 100.0f, 3.0F);
            }
             // Add explosionforce
            isJump = false;
        }
    }

    IEnumerator PowerupCountdownRoutine() // Sets a timer for the powerup
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        hasPowerup2 = false;
        hasPowerup3 = false;
        powerupIndicator.gameObject.SetActive(false);
        powerupIndicator2.gameObject.SetActive(false);
        powerupIndicator3.gameObject.SetActive(false);
    }

    IEnumerator BulletSpawn() // Clones a bullet in the scene
    {
            
            yield return new WaitForSeconds(0.03f);
            Instantiate(projectile, transform.position, transform.rotation);
    }

    IEnumerator Velocity()
    {
        yield return new WaitForSeconds(1f);
        playerRb.velocity = new Vector3 (0, -50, 0);// Increase player's vecolcity falldown
    }
}
