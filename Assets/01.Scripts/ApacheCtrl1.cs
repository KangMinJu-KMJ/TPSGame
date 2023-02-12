using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheCtrl1 : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Apache _apache;

    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        _apache = this.transform.parent.GetComponent<Apache>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.SetActive(false);
            _apache.isGetIn = true;
            Camera.main.transform.GetComponent<FollowCamera>().target
                = this.transform;
        }
    }
    public void GetOutAparche()
    {
        if(Input.GetKeyDown(KeyCode.F) && _apache.isGround)
        {
            player.SetActive(true);
            _apache.isGetIn = false;
            Camera.main.transform.GetComponent<FollowCamera>().target
                = player.transform;
            player.transform.position = new Vector3(_apache.transform.position.x + 5.0f,
                _apache.transform.position.y, _apache.transform.position.z);
        }
    }

    void Update()
    {
        GetOutAparche();
    }
}
