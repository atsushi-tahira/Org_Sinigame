using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkillManagerSC : MonoBehaviour
{
    public int listMax = 1;
    public int nowListLength = 0;
    public static List<string> skillList = new List<string>();

    PlayerSC pSC;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(string.Join(",", skillList.Select(obj => obj.ToString())));
        //Debug.Log(string.Join(",", skillList.Select(obj => obj)));

        //バトルシーンでのみプレイヤーSCを取得
        if(SceneManager.GetActiveScene().name == "BattleScene")
        {
            pSC = GameObject.Find("Player").GetComponent<PlayerSC>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ListTest()
    {
        Debug.Log(string.Join(",", skillList.Select(obj => obj)));
    }

    //ガードスキルのをボタンから発動させるための処理
    //連続で押せないように押したらボタンを無効化
    public void Skill_Guard()
    {
        StartCoroutine("Guard");
        GetComponent<Button>().interactable = false;
    }

    //ガードスキルの詳細
    //指定秒間isGuardを有効にする
    //一定時間経てば再度ボタンが使用可能
    public IEnumerator Guard()
    {
        int useMP = 1;
        if(pSC.playerMP >= useMP)
        {
            Debug.Log("スキル発動　\"ガード\"");
            pSC.playerMP -= useMP;
            Color pColor = GameObject.Find("Player").GetComponent<Renderer>().material.color;
            Color nowColor = pColor;
            pColor = Color.cyan;
            pSC.isGuard = true;
            yield return new WaitForSeconds(3f);
            pSC.isGuard = false;
            pColor = nowColor;
            yield return new WaitForSeconds(3f);
            GetComponent<Button>().interactable = true;
        }
        else
        {
            Debug.Log("MPが不足しています");
        }

       
    }

    public void Skill_Heal()
    {
        int useMP = 2;
        if (pSC.playerMP >= useMP)
        {
            pSC.playerMP -= useMP;
            pSC.playerHP += 3;
            GetComponent<Button>().interactable = false;
            Debug.Log("スキル発動　\"ヒール\"");
        }
        else
        {
            Debug.Log("MPが不足しています");
        }
    }

    public void Skill_Fire()
    {
        
    }

    public void Skill_PowerUp()
    {
        int useMP = 3;
        if (pSC.playerMP >= useMP)
        {
            pSC.playerMP -= useMP;
            PlayerSC.playerAttack += 1;
            Debug.Log("スキル発動　\"強化\"");
        }
        else
        {
            Debug.Log("MPが不足しています");
        }
    }
}
