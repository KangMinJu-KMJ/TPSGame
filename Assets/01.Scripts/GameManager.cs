using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //1.몬스터 프리팹 2.태어날 위치 3. 몇마리 4. 몇초간격?
    [SerializeField]
    private GameObject monsterPrefab;
        //몬스터 프리팹을 미리 생성해 저장 할 리스트 자료형.
    public List<GameObject> monsterPool = new List<GameObject>();
        // 어떤 자료형이던지 다 담을 수 있는 배열 클래스.
    [SerializeField]
    private Transform[] points;
    public bool IsGameOver;
    public int maxCount = 10;
    //싱글톤 패턴을 위한 인스턴스 변수 선언
    public static GameManager gmr;
    [Header("Sound Manager")]
    public float sfxVoulumn = 1.0f;//소리크기
    public bool isSfxMute = false;//음소거
    private void Awake()//Awake? Start보다 먼저 호출된다.
    {
        gmr = this;//this = new
    }

    void Start()
    {
       
        IsGameOver = false;
        monsterPrefab = Resources.Load<GameObject>("monster");
        //(GameObject)Resources.Load("monster");도 가능.
        points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        
        for(int i=0; i<maxCount; i++)
        {   //몬스터 프리팹 생성
            GameObject monster = (GameObject)Instantiate(monsterPrefab);
            //Instantiate : 함수 오버로딩
            monster.name = "monster_" + i.ToString();//생성한 몬스터의 이름 설정
            monster.SetActive(false);//생성한 몬스터 비활성화
            monsterPool.Add(monster);//생성한 몬스터를 오브젝트 풀에 추가함
        }

        if (points.Length > 0)//points 는 배열. length는 길이. 배열이 0보다 크다면 무조건 코루틴호출
        {
            StartCoroutine(CreateMonster());
        }
        
    }

    IEnumerator CreateMonster()
    {
        while(!IsGameOver)
        {
            yield return new WaitForSeconds(3.0f);//3초마다 오브젝트 껐켰할거임
            if (IsGameOver) yield break; //근데 플레이어가 죽으면 밑으로 내려가지 말어라.
            foreach(GameObject _monster in monsterPool)//몬.풀에는 포문으로 프리팹을 하나씩 담앗삼
            {
               if(!_monster.activeSelf)//activeSelf? 활성화인지 비활성화인지 !를붙엿으니 비활성화
                {
                    int idx = Random.Range(1, points.Length);
                    _monster.transform.position = points[idx].position;
                    _monster.SetActive(true); //SetActive 헬기탈때 플레이어 오브젝트 껐었음.
                    break; //오브젝트 풀에서 몬스터프리팹을 하나 활성한 후 foreach 빠져나감
                }
            }


            //int monsterCount = GameObject.FindGameObjectsWithTag("MONSTER").Length;
            ////GameObject 앞에 (int)를 붙이기도 함. 강제형변환!(명시적형변환)
            //if (maxCount > monsterCount)
            //{
            //    yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
            //    //int idx = Random.Range(1, points.Length);//0으로하면 스폰포인트가 잡힘.
            //    //Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            //}
            //else
            //{
            //    yield return null;
            //}
        }
    }
    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        if (isSfxMute) return;
        GameObject soundObj = new GameObject("sfx");
        soundObj.transform.position = pos;
        AudioSource audiosource = soundObj.AddComponent<AudioSource>();
        //addcomponent = getcomponent
        audiosource.clip = sfx;
        audiosource.minDistance = 10.0f;
        audiosource.maxDistance = 30.0f;
        audiosource.volume = sfxVoulumn;
        audiosource.Play();
        Destroy(soundObj, sfx.length);
    }




}
