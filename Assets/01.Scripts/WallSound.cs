using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(AudioSource))]
//반드시 필요한 컴퍼넌트를 명시해서 해당
//컴퍼넌트가 삭제되는 것을 방지하는 Attribute
//attribute ? 속성. 

public class WallSound : MonoBehaviour
{
    [Header("Wall Hit Effect")]
    [SerializeField]
    private GameObject effect;
   
    [SerializeField]
    private AudioClip hitSound;

    void Start()
    {
        hitSound = Resources.Load<AudioClip>("hit");
        
        effect = Resources.Load<GameObject>("Flare");
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "BULLET")
    //    {
    //        Destroy(collision.gameObject);
    //        GameManager.gmr.PlaySfx(collision.transform.position
    //            , hitSound);
    //        GameObject eff = Instantiate(effect,
    //            collision.transform.position, Quaternion.identity);
    //        //collision : 맞은 위치.
    //        //effect를 맞은위치에서 생성한다.
    //        Destroy(eff,1.5f);
    //        //effect를 넣지 않는이유 : 원본이 사라지기에, eff라는 사본을 만듦
    //    }
    //}
    void OnDamage(object[] _params)
    {
        Vector3 hitpos = (Vector3)_params[0];
            GameManager.gmr.PlaySfx(hitpos, hitSound);
            GameObject eff = Instantiate(effect,
                hitpos, Quaternion.identity);
        Destroy(eff,3.0f);
    }
}
