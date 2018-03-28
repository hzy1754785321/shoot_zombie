using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour {
    Transform m_transform;
    Player m_player;   //主角
    NavMeshAgent m_agent;  //寻路组件
    Animator m_ani;   //动画组件
    public float m_movSpeed = 3.0f;   //移动速度
    float m_rotSpeed = 5.0f;   //旋转速度
    float m_timer = 2;   //定时器
    public int m_life = 15;  //生命值
    protected EnemySpawn m_spawn;
    
    // Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_ani = GetComponent<Animator>();    //获得动画播放器
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();  //获得主角
        m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();    //获取寻路器组件
        m_agent.speed = m_movSpeed;    //设置寻路器行走速度
        m_agent.SetDestination(m_player.m_transform.position);     //设置寻路目标
	}
	
    public void Init(EnemySpawn spawn)   //初始化
    {
        m_spawn=spawn;
        m_spawn.m_enemyCount++;
    }
    void RotateTo()
    {
        Vector3 targetdir =m_player.m_transform.position-m_transform.position;  //获取目标方向
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime,0.0f); //计算新方向
        m_transform.rotation = Quaternion.LookRotation(newDir);  //旋转到新方向
    }

	// Update is called once per frame
	void Update () {
        if (m_player.m_life <= 0)
            return;



        m_timer = m_timer - Time.deltaTime;    //更新计时器
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);  //获取当前动画状态

        //如果处于待机状态，不是过度状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);
            if (m_timer > 0)
                return;

            //如果距离主角小与2.5m,进入攻击状态
            if(Vector3.Distance(m_transform.position,m_player.m_transform.position)<2.5f)
             {
                m_agent.ResetPath();   //停止寻路
                m_ani.SetBool("attack", true);    //开始攻击
             }
            else
             {
                m_timer = 1;    //重置计时器
                m_agent.SetDestination(m_player.m_transform.position);    //重新设置寻路目标点
                m_ani.SetBool("run", true);     //进入跑步动画状态
             }
        }

        if(stateInfo.fullPathHash==Animator.StringToHash("Base Layer.run")&&!m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);

            if(m_timer<=0)      //每过1秒重新定位主角位置
            {
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;
            }

            if(Vector3.Distance(m_transform.position,m_player.m_transform.position)<=2.5f)
            {
                m_agent.ResetPath();     //停止寻路
                m_ani.SetBool("attack", true);   //开始攻击
            }
        }

        if(stateInfo.fullPathHash==Animator.StringToHash("Base Layer.attack")&&!m_ani.IsInTransition(0))
        {
            RotateTo();   //面向主角
            m_ani.SetBool("attack", false);  //防止重复攻击

            if(stateInfo.normalizedTime>=1.0f)
            {
                m_ani.SetBool("idle", true);   //重置计时器，攻击延时为2秒
                m_timer = 1;
                m_player.OnDamager(1);
                return;
            }
        }

        if(stateInfo.fullPathHash==Animator.StringToHash("Base Layer.death")&&!m_ani.IsInTransition(0))
        {
            m_ani.SetBool("death", false); //防止重复播放动画
            if(stateInfo.normalizedTime>=1.0f)
            {
                m_spawn.m_enemyCount--;                
                GameManager.Instance.SetScore(100); //增加分数
                Destroy(this.gameObject);  //销毁自身
            }
        }

     /*   if(stateInfo.fullPathHash==Animator.StringToHash("Base Layer.attack")&&!m_ani.IsInTransition(0))
        {
            RotateTo();   //面向主角
            m_ani.SetBool("attack", false);

            if(stateInfo.normalizedTime>=1.0f)
            {
                m_ani.SetBool("idle", true);
                m_timer = 2;  //重置计时器
                m_player.OnDamager(1);
            }
        }
       */
	}

    public void OnDamage(int damger)
    {
        m_life = m_life - damger;
        if(m_life<=0)
        {
            m_ani.SetBool("death", true);  //播放死亡动画
            m_agent.ResetPath();   //停止寻路
        }
    }
}
