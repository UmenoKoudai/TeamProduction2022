using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KARIenemy : MonoBehaviour
{
    //HPゲージ
    int enemyHP = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //被ダメージ処理
    public void ReceveDamage(int damageScore)
    {
        enemyHP -= damageScore;
        Debug.Log("enemyHP:" + enemyHP);
        //オーバーキルでHPが0を飛び越えても対処できるよう、判定を0以下にしておく
        if (enemyHP <= 0)
        {
            //このスクリプトがアタッチされているオブジェクトを消す
            Destroy(this.gameObject);
        }
    }
}