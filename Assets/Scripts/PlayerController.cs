using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public bool canMove;

    [Header("Health Components")]
    public int health;
    public int damageToDeal;
    public List<Image> healthUI;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    
    [Space(10)]
    [Header("GameObjects")]
    public GameObject verticalFocalPoint;
    public GameObject horizontalFocalPoint;
    public GameObject playerIndicator;
    
    [Header("Audio")]
    public AudioClip hurtSound;
    public AudioClip pointSound;
    public AudioSource playerAudio;
    
    private Rigidbody rigidbody;
    private Vector3 spawnPosition;
    private GameManager gameManager;
    

    private float horizontalInput, verticalInput;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidbody = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        spawnPosition = transform.position;
        health = healthUI.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isGameActive) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        playerIndicator.transform.position = new Vector3(transform.position.x,  playerIndicator.transform.position.y, transform.position.z);

         if (transform.position.y < -14)
         {
             StartCoroutine(hurtPlayer(1));
             canMove = true;
             transform.position = spawnPosition;
         }

         if (health <= 0)
         {
             gameManager.isGameActive = false;
             StartCoroutine(gameManager.GameOver());
         }
    }

    void FixedUpdate()
    {
        if (canMove)
        { 
            rigidbody.AddForce(verticalFocalPoint.transform.forward * moveSpeed * verticalInput); 
            rigidbody.AddForce(horizontalFocalPoint.transform.right * moveSpeed * horizontalInput);
        }
            
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, 5);
    }

    IEnumerator hurtPlayer(int amountToHurt)
    {
        if (health >= 0)
        {
            playerAudio.PlayOneShot(hurtSound, 0.45f);
            health -= amountToHurt;
            healthUI[health - amountToHurt + 1].sprite = emptyHeart;
            canMove = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            yield return new WaitForEndOfFrame();
            rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Falling Ground") && gameManager.reachedBottom)
        {
            Debug.Log("COLLIDED WITH PLAYER");
            damageToDeal++;
        
            if (damageToDeal == 1)
            {
                StartCoroutine(hurtPlayer(1));
            }
        }
    }
}
