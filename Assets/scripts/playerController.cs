using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class playerController : MonoBehaviour
{
    public float walkSpeed = 0;
    public float jumpSpeed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private float movementZ;
    private int jumpTracker; 
    private InputAction spacebarAction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent <Rigidbody>();
        count = 0; 
        SetCountText();
        winTextObject.SetActive(false);
        jumpTracker = 0;
        movementZ = 0;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText() 
    {
        countText.text =  "Count: " + count.ToString();
        if (count >= 8)
        {
            winTextObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (jumpTracker < 2)
            {
                movementZ = 1;
                jumpTracker ++;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX * walkSpeed, movementZ * jumpSpeed, movementY * walkSpeed);
        rb.AddForce(movement);
        movementZ = 0; // Reset Jump Velocity (causes jump to act like a burst).
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Solid")) 
        {
            // Refresh jumps after touching the ground.
            // Known bug: allows wall jumps. For this simple project, I think this is acceptable.
            jumpTracker = 0;
        }
    }
}
