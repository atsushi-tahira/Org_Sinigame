using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSC : MonoBehaviour
{
    public float walkSPD = 0.5f;

    bool isBattle;    
    float time;
    EnemySC nowEnemy;
    public static int money = 10;
    public bool isGuard;
    bool isClear;
    public static int playerAttack = 1;
    int playerDifence = 0;

    public int playerHP = 10;
    public GameObject gameEndText;
    public Slider hpBar;
    public Text equipWeaponName;
    GameObject bossDeadUI;

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
        if (!isBattle && !isClear)
        {
            transform.Translate(Vector2.right * (Time.deltaTime * walkSPD));
        }
        else
        {
            //一定時間経過ごとにプレイヤーに攻撃を与える。攻撃したら経過時間をリセット
            //enemyAttackで個体ごとに攻撃力を設定
            time += Time.deltaTime;
            if (time > 1f && !nowEnemy.isDead)
            {
                nowEnemy.Damaged(playerAttack);
                Debug.Log("●●●●●プレイヤーの攻撃。敵HP -" + playerAttack);
                time = 0;
            } 
        }

        //プレイヤーのHPをバーと連動
        hpBar.value = playerHP;

        //HPが0になるとゲームオーバー。ここはゲームオーバーじゃなくてステージ選択画面に遷移させたい
        if (playerHP <= 0)
        {
            Debug.Log("ゲームオーバー、、、");
            gameEndText.GetComponent<Text>().text = "GAME OVER ...";
            gameEndText.SetActive(true);
            Destroy(this.gameObject);
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
            Debug.Log("Enemy_Enter");
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
            Debug.Log("Enemy_Exit");
        }
    }

    //敵からダメージを受ける時の処理
    public void Damaged(int d)
    {
        //ガードスキルが発動してる場合はダメージを無効化
        if (!isGuard)
        {
            playerHP -= d;
        } else
        {
            Debug.Log("敵の攻撃を防いだ！");
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
