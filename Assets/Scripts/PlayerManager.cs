using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [SerializeField] private Player[] players;
    [SerializeField] bool firstPlayerActive = true;

    [SerializeField] int health = 5;

    private float speed = 10;
    private bool isMoving;
    private Vector2 moveInput;
    private Quaternion q;

    // Start is called before the first frame update
    void Awake() {
        Instance = this;
    }
    void Start()
    {
        q = Quaternion.AngleAxis(-45f, Vector3.up);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isMoving) return;
        players[firstPlayerActive ? 0 : 1].Movement(speed * (q * new Vector3(moveInput.x, 0f, moveInput.y)));

        if (firstPlayerActive) {
            players[1].transform.position = players[0].transform.position + new Vector3(100, 0, 0);
        } else {
            players[0].transform.position = players[1].transform.position - new Vector3(100, 0, 0);
        }
    }

    void Toggle() {
        if (health <= 0) return;
        if (firstPlayerActive) {
            players[1].transform.position = players[0].transform.position + new Vector3(100, 0, 0);
        } else {
            players[0].transform.position = players[1].transform.position - new Vector3(100, 0, 0);
        }
        if (players[firstPlayerActive ? 1 : 0].CheckCollision()) return;

        players[firstPlayerActive ? 0 : 1].TogglePlayer(false);
        players[firstPlayerActive ? 0 : 1].cam.gameObject.SetActive(false);

        players[firstPlayerActive ? 1 : 0].TogglePlayer(true);
        players[firstPlayerActive ? 1 : 0].cam.gameObject.SetActive(true);

        firstPlayerActive = !firstPlayerActive;
    }

    public void TakeDamage() {
        GameManager.Instance.UpdateHealthUI(--health);
        if (health <= 0) {
            GameManager.Instance.LossScreen();
        }
    }
    public Vector3 GetPlayerPos() {
        return players[firstPlayerActive ? 0 : 1].transform.position;
    }

    public void OnMove(InputValue value) {
        Debug.Log("Move!");
        moveInput = value.Get<Vector2>();
        isMoving = moveInput != Vector2.zero;
    }

    public void OnJump(InputValue value) {
        Debug.Log("Jump!");
        Toggle();
    }


}
