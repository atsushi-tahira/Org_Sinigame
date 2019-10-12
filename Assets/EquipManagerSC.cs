using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


//アイテムの購入、所持金の管理とかを主にしてるクラス
public class EquipManagerSC : MonoBehaviour
{

    public Text moneyText;
    public GameObject haveEquip;

    public static string equipWeapon;
    public static string equipArmor;

    int weaponAttack = 0;
    int armorDifence = 0;

    public static List<string> EquipList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = PlayerSC.money.ToString();
    }

    public void Buy(string name)
    {
        int price = name == "bou" ? 1 : (name == "sword" ? 3 : (name == "nuke" ? 5 : 0));
        if (PlayerSC.money >= price)
        {
            //所持金から必要金額を引く
            //装備リストに購入した装備を追加
            PlayerSC.money -= price;
            EquipList.Add(name);

            //インベントリに買った装備を追加する処理。てなるとSkillManager.skillListにぶち込まなあかん
            ////オブジェクトをインスタンス化
            GameObject obj1 = Instantiate(haveEquip) as GameObject;
            obj1.transform.SetParent(GameObject.Find("Canvas").gameObject.transform, false);
            obj1.GetComponentInChildren<Text>().text = name;

            //ただ、Updateで毎秒処理するんもだるいから、変更があった時だけ処理を回すようにしたい

            //どう処理内でリストのケツに購入した装備を突っ込んだから、装備リストのケツのやつを出力。デバッグ
            Debug.Log("武器を購入しました。" + EquipList[EquipList.Count - 1]);
        }
        else
        {
            Debug.Log("Gが足りません");
        }

        Debug.Log(string.Join(",",EquipList.Select(obj => obj)));
    }

    //店を出た時にプレイヤー攻撃力を初期化して、装備品ごとの攻撃力を加算確定
    public void BattleEquip()
    {
        PlayerSC.playerAttack = 1;
        PlayerSC.playerAttack += equipWeapon == "sword" ? 1 : (equipWeapon == "bow" ? 2 : (equipWeapon == "axe" ? 3 : 0));
    }
}
