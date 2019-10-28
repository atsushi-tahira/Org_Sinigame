using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentElementSC : MonoBehaviour
{
    [SerializeField]
    bool isWeapon;
    string emarkPrefabName;
    string emarkPrefabPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChoiceEquipment()
    {
        //処理の初めに武器か防具かを判断して、それぞれに必要な変数等を分ける
        if (isWeapon)
        {
            //武器用のEマークのプレハブの名前をパスを指定
            emarkPrefabName = "EMark(Clone)";
            emarkPrefabPath = "Prefabs/EMark";
            //装備している武器の名前をEquipManagerに渡す
            EquipManagerSC.equipWeapon = this.gameObject.GetComponentInChildren<Text>().text;
        }
        else
        {
            //防具用のEマークのプレハブの名前をパスを指定
            emarkPrefabName = "EMark_Armor(Clone)";
            emarkPrefabPath = "Prefabs/EMark_Armor";
            //装備している防具の名前をEquipManagerに渡す
            EquipManagerSC.equipArmor = this.gameObject.GetComponentInChildren<Text>().text;
        }

        //Eマークの重複を防ぐために現在ついているEマークを削除する
        Destroy(GameObject.Find(emarkPrefabName));

        //オブジェクトをインスタンス化
        GameObject eMark = (GameObject)Resources.Load(emarkPrefabPath);
        GameObject obj1 = Instantiate(eMark);
        obj1.transform.SetParent(this.gameObject.transform, false);



        //武器・防具それぞれ装備していない場合、”なし”を表示
        EquipManagerSC.equipWeapon = string.IsNullOrEmpty(EquipManagerSC.equipWeapon) ? "なし" : EquipManagerSC.equipWeapon;
        EquipManagerSC.equipArmor = string.IsNullOrEmpty(EquipManagerSC.equipArmor) ? "なし" : EquipManagerSC.equipArmor;
        Debug.Log("武器は " + EquipManagerSC.equipWeapon +  " を、防具は " + EquipManagerSC.equipArmor +  " を装備中");
    }

}
