using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EntityController : MonoBehaviour {

    CharacterController controller;
    float moveSpeed;
    float walkSpeed = 4f;
    float flySpeed = 11f;

    float mouseRotationY;
    public Transform pAnchorT;
    bool flyToggle;
    public const float gravityConst = -9.8f;
    float velocityY;
    
    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        moveSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
    
        UpdateFlyToggle();
        
        if (flyToggle) {
            velocityY = 0;
            moveSpeed = flySpeed;
            
            if (Input.GetKey(KeyCode.LeftControl)) {
                float sprintSpeed = flySpeed * 2f;
                moveSpeed = sprintSpeed;
            }
            
            if (Input.GetKey(KeyCode.Space)) {
                velocityY = 6f;
            }else if (Input.GetKey(KeyCode.LeftShift)) {
                velocityY = -6f;
            }
        }
        else {
            moveSpeed = walkSpeed;
            if (controller.isGrounded) {
                velocityY = -0.5f;
                if (Input.GetKeyDown(KeyCode.Space)) {
                    velocityY = 5f;
                }
            }
            else {
                velocityY += gravityConst * Time.deltaTime;
            }
           
        }

        Vector3 finalVelocity = moveDir * moveSpeed + Vector3.up * velocityY;
        controller.Move(finalVelocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * 2f;
        float mouseY = Input.GetAxis("Mouse Y") * 2f;
        
        mouseRotationY += mouseY;
        mouseRotationY = Mathf.Clamp(mouseRotationY, -80, 90);
        
        transform.Rotate(Vector3.up * mouseX);
        pAnchorT.eulerAngles = new Vector3(-mouseRotationY, pAnchorT.transform.eulerAngles.y, 0);
    }

    float lastJump;
    public void UpdateFlyToggle() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time - lastJump < 0.35f) {
                flyToggle = !flyToggle;
            }
            lastJump = Time.time;
        }
    }
}
