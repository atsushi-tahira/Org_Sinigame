using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySC : MonoBehaviour
{
    float time;
    string enemyLog;
    bool isBattle;
    int enemyHP = 5;
    string enemyDeath;
    PlayerSC playerSC;
    int enemyAttack = 1;
    GameObject coin;
    public bool isBoss;
    public bool isDead;
    [SerializeField]
    public Slider eHPBar;
    [SerializeField]
    public Text eHPBarText;
    //GameObject bossDeadUI;

    // Start is called before the first frame update
    void Start()
    {
        //if (this.gameObject.name == "Enemy")
        //{
        //    enemyDeath = "敵１を倒した！";
        //    enemyAttack = 1;
        //}
        if (this.gameObject.name == "Boss")
        {
            enemyDeath = "ボスを倒した！";
            enemyAttack = 3;
            isBoss = true;
        }

        coin = (GameObject)Resources.Load("Prefabs/Coin");
        //bossDeadUI = GameObject.Find("BossDeadUI");
        //bossDeadUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //戦闘モード
        if (isBattle)
        {
            //一定時間経過ごとにプレイヤーに攻撃を与える。攻撃したら経過時間をリセット
            //enemyAttackで個体ごとに攻撃力を設定
            time += Time.deltaTime;
            if (time > 1.3f)
            {
                playerSC.Damaged(enemyAttack);
                time = 0;
            }
        }

        //死ぬとログを出して、敵を消す。将来的には衝突判定を消して倒れるっていう感じにしたい
        //アイテムを落とす。ここは後々確率とかを出してコインがドロップするのかアイテムがドロップするのか調整する
        if (enemyHP <= 0 && !isDead)
        {
            if (!isBoss)
            {
                //Debug.Log(enemyDeath);
                Instantiate(coin, this.gameObject.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            else
            {
                isDead = true;
                isBattle = false;
                StartCoroutine(playerSC.BossDead());
            }
        }

        //敵のHPをバーに表示
        eHPBar.value = enemyHP;
        eHPBarText.text = enemyHP + "/5";
    }

    //プレイヤーと接触すると敵が戦闘モードになる
    //複数の敵が戦闘モードにならんようにするために個体ごとに衝突判定を実装
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            isBattle = true;
            playerSC = col.gameObject.GetComponent<PlayerSC>();
        }
    }

    //プレイヤーからのダメージを受ける
    public void Damaged(int d)
    {
        enemyHP -= d;
        Debug.Log("●●●●●プレイヤーの攻撃。敵HP -" + d);
    }
}
