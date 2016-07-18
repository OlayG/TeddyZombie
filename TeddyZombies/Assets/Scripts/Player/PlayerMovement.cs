using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    GameObject player;
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    int floorMask;
    float camRayLength = 100f;
    bool powerUp;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement >();
        playerHealth = player.GetComponent<PlayerHealth>();
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lightning"))
        {
            powerUp = true;
            playerMovement.speed = 10;
            other.gameObject.SetActive(false);
        }else if (other.gameObject.CompareTag("Coin"))
        {
            powerUp = true;
            ScoreManager.score += 50;
            other.gameObject.SetActive(false);
        }else if (other.gameObject.CompareTag("Health"))
        {
            powerUp = true;
            playerHealth.currentHealth += 50;
            playerHealth.healthSlider.value = playerHealth.currentHealth;
            if (playerHealth.currentHealth > 100)
            {
                playerHealth.currentHealth = 100;
                playerHealth.healthSlider.value = playerHealth.currentHealth;
            }
            other.gameObject.SetActive(false);
        }else
        {
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Lightning"))
        {
            other.gameObject.SetActive(true);
        }else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(true);
        }
        else if (other.gameObject.CompareTag("Health"))
        {
            other.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);

        if (powerUp)
        {
            playerMovement.speed = 10;
        }
    }

    // Method to move
    void Move (float h, float v)
    {
        movement.Set(h, 0f, v);
        // Prevent unfair diagonal movement
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }
    // Method to turn
    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            // Used to store rotation
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        // If nothing is pressed you are walking
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
