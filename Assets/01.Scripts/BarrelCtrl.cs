using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    [Header("gameObject")]
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private GameObject boomEffect;

    [Header("Component")]
    [SerializeField]
    private AudioClip hitSound;
   
    [SerializeField]
    private AudioClip expSound;

    public int hitCount = 0;
    private Transform tr;
    public Texture[] textures;

    void Start()
    {
        hitEffect = Resources.Load<GameObject>("Flare");
        hitSound = Resources.Load<AudioClip>("hit");
       
        boomEffect = Resources.Load<GameObject>("Effects/Exp");
        expSound = Resources.Load<AudioClip>("explosion");
        textures = Resources.LoadAll<Texture>("Textures");//2D할때 많이들어감
        //대신 그림이 순서대로 있어야함
        tr = GetComponent<Transform>();

        int idx = Random.Range(0, textures.Length);
        GetComponentInChildren<MeshRenderer>().material.mainTexture = textures[idx];
        //get~children : 자식 오브젝트
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //   if(collision.gameObject.tag=="BULLET")
    //    {
    //        Destroy(collision.gameObject);
    //        GameManager.gmr.PlaySfx(transform.position,hitSound);
    //        GameObject eff = Instantiate(hitEffect,
    //            collision.transform.position, Quaternion.identity);
    //        Destroy(eff, 2.0f);
    //        hitCount++;
    //        if(hitCount>=3)
    //        {
    //            Explosion();

    //        }
            
    //    }
    
    //}
    void OnDamage(object[] _params)
    {
        Vector3 hitPos = (Vector3)_params[0];//맞은위치
        Vector3 firePos = (Vector3)_params[1];
        //입사벡터 (Ray의 각도)
        Vector3 incomeVector = hitPos - firePos; //거리가 나옴.
        incomeVector = incomeVector.normalized;
        //정규화가 진행되어 값을 알아서 만들게 됨.
        //정규화를 안 하면 물건이 이상하게 넘어짐.
        //정규화? 값이 물리에 맞게 변경됨.
        GetComponent<Rigidbody>().
            AddForceAtPosition(incomeVector*1000.0f,hitPos);
        GameManager.gmr.PlaySfx(hitPos, hitSound);
        GameObject eff = Instantiate(hitEffect,
                hitPos, Quaternion.identity);
       
        Destroy(eff, 1.5f);

       
        if (++hitCount >= 3)
        {
            Explosion();

        }
    }
    void Explosion()
    {
        GameObject eff = Instantiate(boomEffect,transform.position,
                                      Quaternion.identity);
        Destroy(eff, 1.8f);
        GameManager.gmr.PlaySfx(transform.position, expSound);

        Collider[] colls = Physics.OverlapSphere(transform.position,
                               15.0f);
        //25 반경 안에서 스피어가 겹치는 리지디바디나 콜라이더를 달고잇는
        //오브젝트를 찾아서 colls라는 콜라이더 배열에 넣는다.
        foreach(Collider col in colls)
        {//배열에 있는것을 순서대로 꺼내서 가져옴.
            Rigidbody rb = col.GetComponent<Rigidbody>();
            //리지디바디를 하나하나 넣어준다.
            //왜? 이미 들어가있지만 그 값을 고치려고!
            if(rb!=null)//리지디바디를 가지고 있다면!
            {
                if (col.gameObject.tag != "Player")//플레이어 제외
                {
                    rb.mass = 1.0f;
                    rb.AddExplosionForce(1000f, transform.position, 25.0f, 650.0f);
                    //폭파력  /  위치            / 반경  / 위로 솟구치는 힘
                }
                col.gameObject.SendMessage("MonsterDie",
                    SendMessageOptions.DontRequireReceiver);
                //함수이름 틀리거나 이러면 에러가 나는데, 그것을 막는게 SendMessageOptions.DontRequireReceiver
                //매개변수는 전달 할 필요가 업음...몬스터다이에 매개변수가 업자나
            }
        }
        Destroy(gameObject, 6.0f);
    }
    



}
