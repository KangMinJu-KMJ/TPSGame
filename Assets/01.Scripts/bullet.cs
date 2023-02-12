using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rbody;
    public float speed = 50.0f;
    public int damage = 20;
    //데미지값은 항상 공격하는 입장이 가지고 있어야 함.
    
    void Start()
    {
        rbody.GetComponent<Rigidbody>();
        //Dontdestroyonload : 총알쏘면 다음씬으로 넘어감. A에서쏘면 B로연결됨
        rbody.AddForce(transform.forward * speed,ForceMode.Impulse);
        //vector3을넣으면 캐릭터 몸을 돌려도 고정적으로나감.
        //스피드가 느려도 impulse(가속도)를 넣으면 빨리나감.
        Destroy(gameObject, 2.0f);
    }


}
