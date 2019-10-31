using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSC : MonoBehaviour
{
    //歩くスピード
    public float walkSPD = 0.5f;
    //戦闘中かどうか
    bool isBattle;
    //攻撃速度
    float time;
    //接触してる敵の情報
    EnemySC nowEnemy;
    //所持金
    public static int money = 10;
    //ガードスキルを使ってるかどうか
    public bool isGuard;
    //ステージをクリアしたかどうか
    bool isClear;
    //死んだかどうか
    bool isDead;
    //プレイヤーの通常攻撃力と防御力
    public static int playerAttack = 1;
    public static int playerDifence = 0;
    //プレイヤーの初期体力と初期MP
    public int playerHP = 10;
    public int playerMP = 10;

    //HPバーとか、ボスを倒した時関連のやつ
    public GameObject gameEndText;
    [SerializeField]
    public Slider hpBar;
    [SerializeField]
    public Slider mpBar;
    [SerializeField]
    public Text hpText;
    [SerializeField]
    public Text mpText;
    [SerializeField]
    public Text equipWeaponName;
    [SerializeField]
    Slider attackGauge;
    GameObject bossDeadUI;

    //MPのデバッグ用
    public int abc = 1;

    // Start is called before the first frame update
    void Start()
    {
        //ボスを倒した時のテキストを取得して、非表示にする
        bossDeadUI = GameObject.Find("BossDeadUI");
        bossDeadUI.SetActive(false);

        //スキルが設定されているかどうかの判断
        if (SkillManagerSC.skillList.Count == 0)
        {
            Debug.Log("Skillが設定されていません(バトルシーン)");
        }
        else
        {
            Debug.Log(SkillManagerSC.skillList[0]);
            //スキルボタンを生成
            string skill = SkillManagerSC.skillList[0].Replace(" ", "");
            string p_name = "Prefabs/Battle_" + skill;
            Debug.Log(p_name);
            GameObject p_skill = (GameObject)Resources.Load(p_name);
            Debug.Log(p_name + "next");
            //オブジェクトをインスタンス化
            GameObject obj1 = Instantiate(p_skill) as GameObject;
            obj1.transform.SetParent(GameObject.Find("Canvas").gameObject.transform, false);
        }

        //装備中の武器を表示、装備がない場合は「なし」と出力
        equipWeaponName.text = "武器：" + EquipManagerSC.equipWeapon + "\n防具：" + EquipManagerSC.equipArmor;
    }

    // Update is called once per frame
    void Update()
    {
        //非戦闘時は常に前進する
        if (!isBattle)
        {
            //ゲームクリア、もしくはゲームオーバー出ない限り前に進み続ける
            if(!isClear && !isDead)
            {
                transform.Translate(Vector2.right * (Time.deltaTime * walkSPD));
            }
            else
            {
                //デバッグ用に一応処理内容を表示させる
                time += Time.deltaTime;
                if(time > 1)
                {
                    time = 0;
                    Debug.Log("ゲームオーバー、もしくはクリア");
                }
            }
        }
        else
        {
            //一定時間経過ごとにプレイヤーに攻撃を与える。攻撃したら経過時間をリセット
            //enemyAttackで個体ごとに攻撃力を設定
            time += Time.deltaTime;
            if (time > 1f && !nowEnemy.isDead)
            {
                nowEnemy.Damaged(playerAttack);
                time = 0;
            } 
        }

        //プレイヤーのHP・MPをバーと連動
        hpBar.value = playerHP;
        mpBar.value = playerMP;
        hpText.text = playerHP + "/10";
        mpText.text = playerMP + "/10";
        attackGauge.value = time;

        //HPが0になるとゲームオーバー。ここはゲームオーバーじゃなくてステージ選択画面に遷移させたい
        if (playerHP <= 0)
        {
            Debug.Log("ゲームオーバー、、、");
            gameEndText.GetComponent<Text>().text = "GAME OVER ...";
            gameEndText.SetActive(true);

            isBattle = false;
            isDead = true;

            ////下に落ちていく。これはこれでありかもしれん
            //GetComponent<BoxCollider2D>().enabled = false;

            //Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //敵に衝突すると戦闘モードに遷移
        //現在接触してる敵を取得。そこかいろんな情報を取ってきたり飛ばしたりする
        if (col.gameObject.tag == "Enemy")
        {
            isBattle = true;
            nowEnemy = col.gameObject.GetComponent<EnemySC>();
        }

        //アイテムゲット
        if (col.gameObject.tag == "Coin")
        {
            //取得するコイン数をランダムに決定
            int r = Random.Range(5, 16);
            money += r;
            Debug.Log(r + "ゴールドゲット！現在の所持金は" + money + "ゴールドです。");
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        //敵と離れると戦闘モードを解除。接敵情報をNullにする
        if (col.gameObject.tag == "Enemy")
        {
            isBattle = false;
            nowEnemy = null;
        }
    }

    //敵からダメージを受けた時の処理
    public void Damaged(int d)
    {
        //ガードスキルが発動してる場合はダメージを無効化
        if (isGuard)
        {
            Debug.Log("敵の攻撃を防いだ！");
        }
        else
        {
            //ダメージ量を計算
            int hitDamage = Mathf.Clamp(d - playerDifence,0,d);
            playerHP -= hitDamage;
            Debug.Log("○○○○○敵の攻撃。" + hitDamage + " ダメージを食らった。");
        }
    }









    //ボスを倒した時の処理
    public IEnumerator BossDead()
    {
        isClear = true;
        bossDeadUI.SetActive(true);
        Text deadText = GameObject.Find("BossDeadText").GetComponent<Text>();
        deadText.text = "a";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        deadText.text = "b";
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        deadText.text = "c.go";
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        bossDeadUI.SetActive(false);
        yield return new WaitForSeconds(3f);
        //ここに徐々にボスが透けていく処理を追加したい
        Destroy(nowEnemy.gameObject);
        StartCoroutine("StageClear");
    }

    //ステージクリア処理
    IEnumerator StageClear()
    {
        isClear = true;
        gameEndText.GetComponent<Text>().text = "STAGE CLEAR!!";
        gameEndText.SetActive(true);
        //デバッグ用に一旦マウス左クリでシーンをリロード
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        string nowScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nowScene);
    }
}
