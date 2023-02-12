using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class fireCtrl : MonoBehaviour
{
    [Header("BulletFire")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]//퍼블릭선언해서 안넣고 스타트에서 미리 갖고시작할려
    private Transform thisTr;
    [SerializeField]
    private MeshRenderer muzzleFlash;
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip gunSound;
    //퍼블릭선언하면 가졋는지 안가졋는지 파악해야해서 좀 느림.
    //그래서 serializefield를 넣는것.
    //네트워크게임에선 속도차이가 큼.
    void Start()
    {
        source = GetComponent<AudioSource>();
        gunSound = Resources.Load<AudioClip>("gun");
        thisTr = GetComponent<Transform>();
        bulletPrefab = Resources.Load<GameObject>("bullet");
        muzzleFlash = GetComponentInChildren<MeshRenderer>();
    }


    void Update()
    {
        //            위치            방향          *거리 = velocity    색깔
        Debug.DrawRay(thisTr.position, thisTr.forward * 20.0f, Color.green);
      if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {   //무엇을       어디서     여기에 쿼터니언넣으면 플레이어 따라서 회전안함
        //Instantiate(bulletPrefab, thisTr.position, thisTr.rotation); => 프로젝타일 무브먼트 방식
        //아래부터 레이캐스트 방식
        //광선을 발사해서 오브젝트에 맞으면
        //맞은 위치를 충돌감지하는 충돌 구조체.
        RaycastHit hit;
        if(Physics.Raycast(thisTr.position, 
            thisTr.forward,out hit, 20.0f))
        {
            if(hit.collider.tag == "MONSTER")
            {   //모든클래스의 부모 혹은 조상 클래스 루트 클래스
                object[] _params = new object[2];
                _params[0] = hit.point;//첫 번째 배열에 맞은 위치 전달
                _params[1] = 20; //몬스터에 입힐 데미지 값을 전달
                hit.collider.gameObject.SendMessage("OnDamage", _params);
                                //다른 오브젝트에 있는 함수 호출이 가능함
            }
            if(hit.collider.tag == "WALL")
            {
                object[] _params = new object[2];
                _params[0] = hit.point;
                _params[1] = 20;
                hit.collider.gameObject.SendMessage("OnDamage", _params);
            }
            if (hit.collider.tag == "BARREL")
            {
                object[] _params = new object[2];
                _params[0] = hit.point;//맞은위치
                _params[1] = thisTr.position;//발사위치
                hit.collider.gameObject.SendMessage("OnDamage", _params);
                                                   //함수를 보내고, 매개변수 보낸다.
            }
        }
        source.PlayOneShot(gunSound, 1.0f);
        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        float _scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * _scale;
        //x,y,z축으로 동일하게 커지거나 작아짐
        Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        
        //부모가 회전시 rotate. 자식이 회전할땐 localRotation
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        muzzleFlash.enabled = false;
    }
   
}
