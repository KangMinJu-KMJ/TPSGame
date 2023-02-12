using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image hpbar;
    public int hp = 100;
    public int hpInit = 100;//유저에게 보여주는것은 정수여야하기에 int
    private int damage = 15;
    [SerializeField]
    private AudioClip dieSound;
    [SerializeField]
    private AudioSource source;

    void Start()
    {
        hpbar = GameObject.Find("Canvas_HP").transform.GetChild(0).transform.GetChild(0)
            .GetComponent<Image>();
        hpbar.color = Color.green;
        source = GetComponent<AudioSource>();
        dieSound = Resources.Load<AudioClip>("player_killed");
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PUNCH")
        {
            hp -= damage;
            hpbar.fillAmount = (float)hp / (float)hpInit;

            if (hpbar.fillAmount <= 0.3f)
                hpbar.color = Color.red;
            else if (hpbar.fillAmount <= 0.6f)
                hpbar.color = Color.yellow;

            if(hp<=0)
            {
                PlayerDie();
            }
        }
    }
    void PlayerDie()
    {
        GameManager.gmr.IsGameOver = true;
        
    }
}
