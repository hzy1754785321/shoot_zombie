using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public Transform m_enemy;   //敌人的prefab
    public int m_enemyCount = 0;  //生成的敌人数量
    public int m_maxEnemy = 3;     //最大敌人数量
    public float m_timer = 1.0f;    //生成时间间隔
    protected Transform m_transform;
    protected GameManager m_manager;
	// Use this for initialization
	void Start () {
        m_transform = this.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_enemyCount >= m_maxEnemy)
            return;
        m_timer = m_timer - Time.deltaTime; 
        if(m_timer<=0)
        {
          /*  if (m_manager.m_score > 1000)
            {
                m_timer = Random.value * 5.0f;
            }
            else
            {   */
                m_timer = Random.value * 15.0f;
        //  }
            if (m_timer < 5)
                m_timer = 5;
            Transform obj = (Transform)Instantiate(m_enemy, m_transform.position, Quaternion.identity); //生成敌人
            Enemy enemy = obj.GetComponent<Enemy>();  //获取敌人
            enemy.Init(this);  //初始化
        }
	}

    void  OnDrawGimos()
    {
        Gizmos.DrawIcon(transform.position, "item.png", true);
    }
}
