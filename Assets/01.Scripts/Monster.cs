using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum MonsterState
{
    idle,trace,attack,die
};

public class Monster : MonoBehaviour//MonoBehaviour를 지우면 컴퍼넌트를 못불러온다.
    //저놈을 지우면 걍 C#클래스인데, 붙이게 되면 유니티 클래스가 됨. 구분.
{
    public MonsterState monsterState = MonsterState.idle;
    //몬스터의 현재 상태정보를 저장 할 열거 변수

    [Header("Comparent")]
    [SerializeField]
    private Transform monsterTr;
    [SerializeField]
    private Transform playerTr;//몬스터와 플레이어의 위치를 알아야 거리를 알 수 있음
    [SerializeField]
    private Rigidbody rbody;//너무 가벼우면 밀려남. 메쉬 조절용
    [SerializeField]
    private NavMeshAgent navi;//추적하려고
    [SerializeField]
    private Animator ani;
    //추적 사정거리
    public float traceDist = 15.0f;
    //공격 사정거리
    public float attackDist = 2.5f;
    private bool isDie;
    [Header("UI")]
    [SerializeField]
    private Image HPbar;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Canvas hudCanvas;
    [SerializeField]
    public int HP = 100;
    public int HPinit;
    [SerializeField]
    private GameObject BloodDecal;
    [SerializeField]
    private GameUI gameUI;

    private void Awake()//늘 컴퍼넌트를 우선으로 잡으렴.
    {
        monsterTr = GetComponent<Transform>();
        rbody = GetComponent<Rigidbody>();
        navi = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        navi.destination = playerTr.position;
        gameUI = GameObject.Find("GameManager").GetComponent<GameUI>();
        HPinit = HP;
        isDie = false;
        BloodDecal = Resources.Load<GameObject>("Effects/BloodDecal");
        HPbar = gameObject.transform.GetChild(14)
            .transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        hudCanvas = gameObject.transform.GetChild(14).GetComponent<Canvas>();
        HPbar.color = Color.green;
        hpText = gameObject.transform.GetChild(14)
            .transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
    }
    private void OnEnable()//활성화 될때 스타트 함수보다 빨리 호출된다.
    {
        StartCoroutine(checkmonsterState());
        StartCoroutine(MonsterAction());
    }

    void Start()
    {
              
    }
    IEnumerator checkmonsterState()
    {
        while (!isDie)
        {

            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);
            if (dist <= attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }
        }
    }
    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch(monsterState)
            {
                case MonsterState.idle:
                    navi.isStopped = true;
                    ani.SetBool("Is Trace", false);
                    break;
                case MonsterState.trace:
                    navi.isStopped = false;
                    navi.destination = playerTr.position;
                    ani.SetBool("Is Trace", true);
                    ani.SetBool("Is Attack", false);
                    break;
                case MonsterState.attack:
                    navi.isStopped = true;
                    ani.SetBool("Is Attack", true);
                    break;
            }
            yield return null;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag=="BULLET")
    //    {
    //        Destroy(collision.gameObject);
    //        ani.SetTrigger("Is Hit");
    //        HP -= collision.gameObject.GetComponent<bullet>().damage;
    //        //collision ? 충돌체 .
    //        HPManager();
    //        CreateBlood();
    //        if (HPbar.fillAmount <= 0.0f)
    //            MonsterDie();
    //    }
    //}
    public void OnDamage(object []_params)
    {
        ani.SetTrigger("IsHit");
        HP -= (int)_params[1];
        HPManager();
        CreateBlood();
        if (HPbar.fillAmount <= 0.0f)
            MonsterDie();
    }
    void MonsterDie()
    {
        StopAllCoroutines();//모든 코루틴 중지
        navi.isStopped = true;
        isDie = true;
        GetComponent<CapsuleCollider>().enabled = false;
        hudCanvas.enabled = false;
        ani.SetTrigger("Is Die");
        rbody.isKinematic = true;//리지드바디 키네마틱을 켜면 물리력이 사라진다.
        foreach (Collider col in GetComponentsInChildren<SphereCollider>())
        {                        //자기자신 하위 오브젝트에 있는 스피어 콜라이더를 찾음(배열임)
            col.enabled = false;
        }
        gameUI.DisplayScore(20);
        StartCoroutine(PushObjectPool());
    }
    IEnumerator PushObjectPool()
    {
        yield return new WaitForSeconds(3.0f);
        navi.isStopped = false;
        isDie = false;
        HP = 100;
        HPbar.color = Color.green;
        HPbar.fillAmount = 1.0f;
        GetComponent<CapsuleCollider>().enabled = true;
        hudCanvas.enabled = true;
        //ani.SetTrigger("Is Die");
        rbody.isKinematic = false;//리지드바디 키네마틱을 켜면 물리력이 사라진다.
        foreach (Collider col in GetComponentsInChildren<SphereCollider>())
        {                        //자기자신 하위 오브젝트에 있는 스피어 콜라이더를 찾음(배열임)
            col.enabled = true;
        }
        gameObject.SetActive(false);
    }
    void HPManager()
    {
        HPbar.fillAmount = (float)HP / (float)HPinit;
        if (HPbar.fillAmount <= 0.3f)
            HPbar.color = Color.red;
        else if (HPbar.fillAmount <= 0.6f)
            HPbar.color = Color.yellow;

        hpText.text = "HP :" + HP.ToString();
    }
    void CreateBlood()
    {
        Vector3 decalPos = monsterTr.position + (Vector3.up*0.25f);
        //몬스터 위치보다 조금 위에서. 왜? 발바닥에 파묻혀서 안보임,,
        Quaternion decalRot = Quaternion.Euler(90.0f, 0, Random.Range(0, 360));//z축 회전
                                            //세운놈 눕히려고 90도
        //쿼터니언.오일러 : 벡터3도 쿼터니언으로 바꾸기 가능
        GameObject blooddecal = Instantiate(BloodDecal, decalPos, decalRot);
        float _scale = Random.Range(1.5f, 3.5f);
        blooddecal.transform.localScale = Vector3.one * _scale;
        Destroy(blooddecal, 4.0f);

    }
}
