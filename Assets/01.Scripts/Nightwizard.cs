using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]

//public class NightWizardAnimation
//{
//    public AnimationClip idle;
//    public AnimationClip run;
//    public AnimationClip attack;
//    public AnimationClip walk;
//}

public class Nightwizard : MonoBehaviour
{
    private NavMeshAgent navi;
    //public NightWizardAnimation nightAni;
    private Transform tr;
    private Animator ani;
    private bool isAttack;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 target = Vector3.zero;

    void Start()
    {
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        navi = GetComponent<NavMeshAgent>();
        //ani.Play(nightAni.idle.name);
        isAttack = false;
    }

    
    void Update()
    {
        Run();
        Attack();
    }

    void Run()
    {
        isAttack = false;
        navi.isStopped = true;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.grey);

        if(!isAttack)
        {
            navi.isStopped = false;
            if (Input.GetMouseButtonDown(1))
            {
                if(Physics.Raycast(ray,out hit, Mathf.Infinity, 1<<13))
                {
                    target = hit.point;
                    navi.destination = target;
                    ani.SetBool("MouseRight", true);
                    ani.SetBool("Attack", false);
                }

            }
            float dist = Vector3.Distance(target, tr.position);
            if(dist<0.3f)
            {
                navi.isStopped = true;
                ani.SetBool("MouseRight", false);
            }
        }
    }

    void Attack()
    {
        
        if(Input.GetMouseButton(0))
        {
            isAttack = true;
            navi.isStopped = true;
            ani.SetBool("MouserLeft",true);
        }
        if(Input.GetMouseButtonUp(0))
        {
            isAttack = false;
            navi.isStopped = false;
        }
        if(Input.GetMouseButtonDown(1))
        {
            ani.SetBool("Attack", true);
            navi.isStopped = false;
            isAttack = false;
        }    
    }

}
