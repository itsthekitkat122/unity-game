using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.18f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        //ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //resetting the default velocity
        if(isGrounded && velocity.y < 0);



    }
}
