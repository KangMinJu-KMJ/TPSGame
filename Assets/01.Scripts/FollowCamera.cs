using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //카메라가 플레이어를 쫓아다닌다.
    //플레이어 위치를 알아야 함.
    //느리고 부드러운 속도
    public Transform target; //타겟
    [SerializeField]
    private Transform thisTr; //카메라 자기자신
    public float dist = 10.0f; //카메라와 타겟의 거리
    public float height = 5.0f;//카메라의 높이
    public float smoothRotate = 3.0f;//카메라의 부드러운 회전

    void Start()
    {
        thisTr = GetComponent<Transform>();
    }

    void LateUpdate()//업데이트보다 좀 늦게 실행되는?
    {                //캐릭터가 먼저 움직이고 카메라가 따라가야해서
        float angle = Mathf.LerpAngle(thisTr.eulerAngles.y,
            target.eulerAngles.y,smoothRotate*Time.deltaTime);//Mathr = 수학공식만 모아 둔 클래스
                                                              //eulerAngles : 회전각
        
        Quaternion rot = Quaternion.Euler(0.0f, angle, 0.0f);
        //angle(오일러 타입{float})을 quaternion자료형으로 바꿨음.
        thisTr.position = target.position - (rot*Vector3.forward * dist)
            + (Vector3.up * height);
        //플레이어보다 뒤에 있으면서 높이도 더 높게끔 함.
        //그리고 양옆으로 회전을 할 수 있게끔 rot를 곱함.
        thisTr.LookAt(target.transform);
        //카메라가 타겟을 쳐다봄
    }
}
