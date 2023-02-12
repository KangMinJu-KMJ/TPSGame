using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]//속성 애튜리뷰트
    private Transform tr;
    [SerializeField]//Attribute 개발끝나면 지워야돼
    private Animation ani;
             float h, v;
    public float movespeed = 10f;
    public float rotspeed = 100f;
  
    void Start()
    {
        tr = this.GetComponent<Transform>();
        ani = this.GetComponent<Animation>();
        ani.Play("idle");
    }

   
    void Update()
    {
        PlayerControl();
        PlayerAnimaton();


    }
    void PlayerControl()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Debug.Log("H= " + h.ToString());
        Debug.Log("v=" + v.ToString());
        //tr.Translate(Vector3.right * h * movespee * Time.deltaTime);
        //tr.Translate(Vector3.forward * v * movespee * Time.deltaTime);
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir.normalized * Time.deltaTime * movespeed, Space.Self);
        //상대좌표 
        //노말라이즈드 는 정규화 
        tr.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotspeed * Time.deltaTime);
    }
    void PlayerAnimaton()
    {
        if (h > 0.01f)
        {
            ani.CrossFade("runRight", 0.3f);
        }
        else if (h < -0.01f)
        {
            ani.CrossFade("runLeft", 0.3f);
        }
        else if (v > 0.01f)
        {
            ani.CrossFade("runForward", 0.3f);
        }
        else if (v < -0.01f)
        {
            ani.CrossFade("runBackward", 0.3f);
        }
        else
        {
            ani.CrossFade("idle", 0.3f);
        }
    }
}
