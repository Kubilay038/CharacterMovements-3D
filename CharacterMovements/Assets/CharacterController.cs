using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    float horizontal = 0, vertical = 0;
    Animator animator;
    Rigidbody physical;
    public GameObject HeadCamera;
    float headRotUpIn = 0, headRotRightLeft = 0;
    public float speed = 0;
    bool shootController = false;
    Vector3 distanceBetweenCamera;
    RaycastHit hit;
    GameObject camera, position1, position2;
    public GameObject cross;


    void Start()
    {
        animator = GetComponent<Animator>();
        physical = GetComponent<Rigidbody>();
        distanceBetweenCamera = HeadCamera.transform.position - transform.position;
        camera = Camera.main.gameObject;
        position1 = HeadCamera.transform.Find("Position1").gameObject;
        position2 = HeadCamera.transform.Find("Position2").gameObject;

       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("JumpParam", true);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= 2;
            animator.SetBool("RunParam", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= 2;
            animator.SetBool("RunParam", false);
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            cross.SetActive(true);
            shootController = true;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            cross.SetActive(false);
            shootController = false;
        }

    }
   
    void FixedUpdate()
    {
        Move();
        if (!shootController)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, position1.transform.position, 0.1f);
            Rotation();
        }
        else 
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, position2.transform.position, 0.1f);
            Rotation2();
        }
        
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }
    void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        Vector3 vec = new Vector3(horizontal, 0 , vertical);
        vec = transform.TransformDirection(vec);
        vec.Normalize();
        physical.position += vec * Time.fixedDeltaTime * speed;

    }
    void Rotation2()
    {
        HeadRotation();

        Physics.Raycast(Vector3.zero, HeadCamera.transform.GetChild(0).forward, out hit);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z)), 0.3f);
        Debug.DrawLine(Vector3.zero, hit.point);

    }
    void Rotation()
    {
        HeadRotation();

        if (horizontal != 0 || vertical != 0)
        {
            Physics.Raycast(Vector3.zero, HeadCamera.transform.GetChild(0).forward, out hit);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.point.x, 0 , hit.point.z)), 0.3f);
            Debug.DrawLine(Vector3.zero, hit.point);
        }
    }
    void JumpParamFalse()
    {
        animator.SetBool("JumpParam", false);
    }
    void JumpAddForce()
    {
        physical.AddForce(0,190, 0);
    }
    void HeadRotation()
    {
        HeadCamera.transform.position = transform.position + distanceBetweenCamera;
        headRotUpIn += Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * -150;
        headRotRightLeft += Input.GetAxis("Mouse X") * Time.fixedDeltaTime * 150;
        headRotUpIn = Mathf.Clamp(headRotUpIn, -20, +20);
        HeadCamera.transform.rotation = Quaternion.Euler(headRotUpIn, headRotRightLeft, transform.eulerAngles.z);
    }
}
