using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * (Time.deltaTime * 5f));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("トリガーエンター" + col.gameObject.name);
        if (col.gameObject.tag == "Enemy")
        {
            col.GetComponent<EnemySC>().Damaged(5);
            Destroy(this.gameObject);
        }
    }
}
