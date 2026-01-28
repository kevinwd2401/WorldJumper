using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Slider jumpBar;
    public Material activeMat, disabledMat;
    [SerializeField] private Player[] players;
    [SerializeField] bool firstPlayerActive = true;

    [SerializeField] int health = 5;

    private float speed = 10;
    private bool isMoving;
    private Vector2 moveInput;
    private Quaternion q;

    private bool holdingJump;
    private float jumpHoldTime;

    // Start is called before the first frame update
    void Awake() {
        Instance = this;
    }
    void Start()
    {
        q = Quaternion.AngleAxis(-45f, Vector3.up);
    }

    void Update() {
        if (holdingJump)
        {
            jumpHoldTime += Time.deltaTime;
            jumpBar.value = jumpHoldTime;
            realigPos();

            if (jumpHoldTime > 1.0f) {
                if (Toggle()) {
                    Debug.Log("World Jump!");
                } else {
                    Debug.Log("Jump Failed!");
                    WorldPeak(false);
                }

                holdingJump = false;
                jumpHoldTime = 0;
                jumpBar.value = 0;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isMoving || holdingJump) return;
        players[firstPlayerActive ? 0 : 1].Movement(speed * (q * new Vector3(moveInput.x, 0f, moveInput.y)));

        if (firstPlayerActive) {
            players[1].transform.position = players[0].transform.position + new Vector3(100, 0, 0);
        } else {
            players[0].transform.position = players[1].transform.position - new Vector3(100, 0, 0);
        }
    }

    bool Toggle() {
        if (health <= 0) return false;
        realigPos();
        if (players[firstPlayerActive ? 1 : 0].CheckCollision()) return false;

        players[firstPlayerActive ? 0 : 1].TogglePlayer(false);
        players[firstPlayerActive ? 0 : 1].cam.gameObject.SetActive(false);
        players[firstPlayerActive ? 0 : 1].gameObject.GetComponent<MeshRenderer>().material = disabledMat;

        players[firstPlayerActive ? 1 : 0].TogglePlayer(true);
        players[firstPlayerActive ? 1 : 0].cam.gameObject.SetActive(true);
        players[firstPlayerActive ? 1 : 0].gameObject.GetComponent<MeshRenderer>().material = activeMat;

        firstPlayerActive = !firstPlayerActive;
        return true;
    }
    private void WorldPeak(bool enter) {
        realigPos();

        players[firstPlayerActive ? 0 : 1].cam.gameObject.SetActive(!enter);
        players[firstPlayerActive ? 1 : 0].cam.gameObject.SetActive(enter);
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

    private void realigPos() {
        if (firstPlayerActive) {
            players[1].transform.position = players[0].transform.position + new Vector3(100, 0, 0);
        } else {
            players[0].transform.position = players[1].transform.position - new Vector3(100, 0, 0);
        }
    }

    public void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
        isMoving = moveInput != Vector2.zero;
    }

    public void OnJump(InputValue value) {
        if (value.isPressed)
        {
            holdingJump = true;
            jumpHoldTime = 0f;
            WorldPeak(true);
        }
        else
        {
            if (holdingJump && jumpHoldTime < 1.0f)
            {
                WorldPeak(false);
            }

            holdingJump = false;
            jumpBar.value = 0;
        }
    }


}
