using UnityEngine;
using UnityEngine.AI;

[System.Serializable] // 클래스 안의 멤버변수가 인스펙터에서 보이게 만듦
//Attribute : 속성. 속성줄때는 [] 를붙임
public class AnimationWizard
{
    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip run;
    public AnimationClip jump;
    public AnimationClip attack;
}

public class Wizard : MonoBehaviour
{
    public AnimationWizard wizardAni;
    private Animation ani;
    private NavMeshAgent navi;
    private RaycastHit hit; // 광선에 맞으면 충돌감지 하는 구조체
    private Ray ray; // 광선
    private Vector3 target = Vector3.zero;
    private Transform tr;
    private bool isAttack;
    private bool isLanding = false;//땅에 닿았니 ? 즉, 무한점프로 캐릭터가 하늘로 치솟는걸 막음
    private bool isIdle;
    private bool isMove;

    void Start()
    {
        ani = GetComponent<Animation>();
        navi = GetComponent<NavMeshAgent>();
        tr = GetComponent<Transform>();
        ani.clip = wizardAni.idle;
        //애니메이션 클립 = wizardAni변수를 이용해 idle 클립을 전달.
        ani.Play();
        //저장된 idle을 재생.
        isAttack = false;
        isIdle = true;
        isMove = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isLanding = true;
        Debug.Log("zz~");
    }

    void Update()
    {

        ClickMove();
        Attack();
        Jump();
        FastRun();
    }

    void ClickMove()
    {
        isAttack = false;
        navi.isStopped = true;
        //광선을 카메라에서 마우스 포인터 지점으로 발사함.
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //화면에서 보는 좌표와 실제 좌표를 정확하게 맞춰줌.
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.yellow);
        //광선 위치      광선 방향

        if (!isAttack)
        {
           
            navi.isStopped = false;
            if (Input.GetMouseButtonDown(1))
            {
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 13))
                {
                    navi.speed = 5f;
                    isIdle = false;
                    target = hit.point;//충돌위치를 타겟에서찾음
                    navi.destination = target;
                    ani.CrossFade(wizardAni.walk.name, 0.3f);
                    navi.isStopped = false;
                    isMove = true;
                }
            }
          
        }
        float dist = Vector3.Distance(target, tr.position);
        if (dist < 0.3f)
        {
            ani.CrossFade(wizardAni.idle.name, 0.3f);
            isIdle = true;
        }



    }
    void Attack()
    {   //레거시는 애니메이션 길이만큼 눌러주어야 함.


        if (Input.GetMouseButton(0))
        {
            isIdle = false;
            navi.isStopped = true;
            ani.Play(wizardAni.attack.name);
            isAttack = true;
            navi.speed = 5f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            navi.isStopped = true;
            ani.Stop(wizardAni.attack.name);
            ani.clip = wizardAni.idle;
            isIdle = false;
            ani.PlayQueued(wizardAni.idle.name);
        }


    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isIdle)
        {
            isLanding = false;
            ani.Play(wizardAni.jump.name);
            Invoke("JumpDelay", 1.0f);   
        }
        if(Input.GetKey(KeyCode.Space) && !isIdle &&
            Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            isLanding = false;
            ani.Play(wizardAni.jump.name);
            Invoke("JumpDelay", 1.0f);
        }
   
    }

    void JumpDelay()
    {
        if(ani.isPlaying)//애니메이션이 플레이중인가 ?
        {
            ani.Stop(wizardAni.jump.name);
            ani.PlayQueued(wizardAni.run.name);
            //Stop과 PlayQueued는 같이 움직여야 함.
        }
    }
    void FastRun()
    {
        if(Input.GetKey(KeyCode.LeftShift)&&Input.GetKey(KeyCode.W))
        {
            if (isMove)
            {
                ani.CrossFade(wizardAni.run.name, 0.3f);
                navi.speed = 22f;
            }
        }
    }
}//메카님이 좋긴한데,,애니메이터가 많아지면서 엄청 복잡해짐
//서로서로 이어주잖아. 가독성도 떨어짐.
//근데 레거시를하게되면 애니메이션을 스크립트로 진행하게 됨.
//그리고 속도 또한 레거시가 더 빠름.
//크로스페이드는 애니메이터 메카님방식에선 걍 해줌
//근디 레거시에서는 써줘야댐
//간단한 잡몹ㅇ들은 애니메이션 컨트롤로 만들어서 메카님으로 하는 게 낫다.
//근데 최종보스나 플레이어는 레거시로 하는 게 나음.
