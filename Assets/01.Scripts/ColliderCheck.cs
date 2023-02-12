using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Apache apache;

    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        apache = this.transform.parent.GetComponent<Apache>();
        
                //자기자신에서 부모컴퍼넌트에 AparcheCtrl 스크립트를 찾음
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {//player태그와 충돌하면 플레이어의 오브젝트를 끈다.
            player.SetActive(false);
            //print("test");
            apache.isGetIn = true;
            Camera.main.transform.GetComponent<FollowCamera>().target
                = this.transform;
        }
    }
    public void GetOutHellgi()
    {
        if (Input.GetKeyDown(KeyCode.F) && apache.isGround)
            {
                player.SetActive(true);
                //print("test");
                apache.isGetIn = false;
                Camera.main.transform.GetComponent<FollowCamera>().target
                    = player.transform;
                player.transform.position =
                    new Vector3(apache.transform.position.x + 5.0f,
                    apache.transform.position.y, apache.transform.position.z);
                //GetComponent<Rigidbody>().useGravity = true;
                //GetComponent<Rigidbody>().mass = 100;
            }
        
    }
    void Update()
    {
        GetOutHellgi();
    }
}
