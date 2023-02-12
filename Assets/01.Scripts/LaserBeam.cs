using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private Transform tr;
    private LineRenderer line;
    private Ray ray;//광선 자료형
    //Ray : 광선구조체, ray : 광선구조체 변수
    private RaycastHit hit;
    //광선 충돌 구조체
    void Start()
    {
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false; //라인렌더러에 유즈월드스페이스 잇는거 끔
        line.enabled = false;
        //line.SetWidth(0.3f, 0.01f);//라인 시작 폭, 종료 폭을 정함.
    }

    void Update()
    {//ray : 광선 발사 위치와 방향을 설정하였음.
        ray = new Ray(tr.position + (Vector3.up * 0.02f)
            , tr.forward) ;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);
        if(Input.GetMouseButtonDown(0))
        {                                       //2차원좌표 -> 3차원좌표
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin));
            //라인 렌더러 첫번째 점의 위치 설정
            //광선을 발사했는데 어떠한 오브젝트에 맞았다면?
            if(Physics.Raycast(ray,out hit,100f))
            {
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));
                //맞은 지점이 끝의 지점 혹은 두 번째 지점이 됨.
            }
            else
            {
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100.0f)));
            }
            StartCoroutine(ShowLaserBeam());

        }
    }
    IEnumerator ShowLaserBeam()
    {
        line.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        line.enabled = false;
    }
}
