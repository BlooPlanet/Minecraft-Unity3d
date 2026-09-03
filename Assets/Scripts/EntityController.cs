using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EntityController : MonoBehaviour {

    public Transform camT;
    float moveSpeed = 11f;
    float walkSpeed = 4.1f;
    float flySpeed = 11f;
    CharacterController _controller;
    float vy;
    float mouseRotationY;
    public const float gravityConst = -9.8f;
    bool flyToggle;
    
    // Start is called before the first frame update
    void Start() {
        _controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update() {
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector3 moveDir = transform.forward * inputDir.y + transform.right * inputDir.x;

        UpdateFLyToggle();
        if (flyToggle) {
            moveSpeed = flySpeed;
            vy = 0;
            if (Input.GetKey(KeyCode.Space)) {
                vy = 5f;
            }else if (Input.GetKey(KeyCode.LeftShift)) {
                vy = -5f;
            }
        }
        else {
            moveSpeed = walkSpeed;
            if (_controller.isGrounded) {
                vy = -0.5f;
                if (Input.GetKeyDown(KeyCode.Space)) {
                    vy = 5f;
                }
            }
            else {
                vy += gravityConst * Time.deltaTime;
            }
        }
        
       
        
        Vector3 finalVelocity = moveDir * moveSpeed + Vector3.up * vy;
        _controller.Move(finalVelocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * 2f;
        float mouseY = Input.GetAxis("Mouse Y") * 2f;

        mouseRotationY += mouseY;
        mouseRotationY = Mathf.Clamp(mouseRotationY, -90f, 90f);
        
        transform.Rotate(Vector3.up * mouseX);
        camT.eulerAngles = new Vector3(-mouseRotationY, camT.eulerAngles.y, 0);
    }

    float lastJumped;
    public void UpdateFLyToggle() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time - lastJumped < 0.35f) {
                flyToggle = !flyToggle;
            }

            lastJumped = Time.time;
        }
    }
}
