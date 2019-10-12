using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    SkillManagerSC sManager;
    // Start is called before the first frame update
    void Start()
    {
        sManager = GameObject.Find("SkillManager").GetComponent<SkillManagerSC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkillChoice()
    {
        if(sManager.nowListLength < sManager.listMax)
        {
            //ボタンと同じ名前のスキルをプレハブから引っ張ってくる
            string skill = this.gameObject.name;
            string p_name = "Prefabs/Choice" + skill;
            GameObject p_skill = (GameObject)Resources.Load(p_name);
            //オブジェクトをインスタンス化
            GameObject obj1 = Instantiate(p_skill) as GameObject;
            obj1.transform.SetParent(GameObject.Find("Canvas").gameObject.transform, false);
            //１度選択されたボタンを使用不可に
            GetComponent<Button>().interactable = false;
            //スキル選択最大数に引っ掛ける
            sManager.nowListLength++;
        }
        else
        {
            Debug.Log("最大スキル数です");
        }
    }

    public void SkillCancel()
    {
        //スキル名を取得。名前の差別化のための「Choice」とPrefabから作成した際に出る「(Clone)」を削除
        //文字列の場所を指定するとバグが起こる可能性があったから、文字列指定で置換した
        string skillName = this.gameObject.name.Replace("Choice", "").Replace("(Clone)","");
        Debug.Log(skillName);
        GameObject.Find(skillName).GetComponent<Button>().interactable = true;
        sManager.nowListLength--;
        Destroy(this.gameObject);
    }

    public void SkillDecide()
    {
        //既に選択されてるスキルを一度全リセットして新しく設定されたスキルを反映させる
        SkillManagerSC.skillList.Clear();
        List<GameObject> decisionList = new List<GameObject>();
        decisionList.AddRange(GameObject.FindGameObjectsWithTag("Decision"));
        SkillManagerSC.skillList.AddRange(decisionList.Select(obj => obj.ToString().Replace("(UnityEngine.GameObject)", "").Replace("Choice", "").Replace("(Clone)", "")));
        if(SkillManagerSC.skillList.Count == 0)
        {
            Debug.Log("Skillが設定されていません");
        }
        else
        {
            Debug.Log(SkillManagerSC.skillList[0]);
        }

    }
}
