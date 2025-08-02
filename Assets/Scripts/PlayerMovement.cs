using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject pauseUI;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUI.SetActive(!pauseUI.activeSelf);
        }

        if (canMove) 
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            rb.linearVelocity = moveInput * moveSpeed;
        }
        

        if (pauseUI.activeSelf == true)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void UnpauseUI()
    {
        pauseUI.SetActive(false);
    }

    public void SetCanMove(bool canMove) 
    { 
        this.canMove = canMove;
        rb.linearVelocity = new Vector3(0,0,0); //stop all movement
    }
}
