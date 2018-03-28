using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;   //静态实例
    public int m_score = 0;//游戏得分
    public static int m_best = 0;   //游戏最高分
    public int m_ammo = 100;   //子弹数
    Player m_player;  //主角

    //UI文字
    Text Text_ammo;   
    Text Text_best;
    Text Text_score;
    Text Text_life;
    Button Button_restart;


	// Use this for initialization
	void Start () {
        Instance = this;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();  //获得主角        
        GameObject uicanvas = GameObject.Find("Canvas");
        foreach(Transform t in uicanvas.transform.GetComponentInChildren<Transform>())
        {
            if(t.name.CompareTo("Text_ammo")==0)
            {
                Text_ammo = t.GetComponent<Text>();
                Text_ammo.text = "100 / 100";
            }
            else if(t.name.CompareTo("Text_best")==0)
            {
                Text_best = t.GetComponent<Text>();
                Text_best.text="High Score  " + m_best;
            }
            else if(t.name.CompareTo("Text_life")==0)
            {
                Text_life = t.GetComponent<Text>();
                Text_life.text = "生命值  6";
            }
            else if(t.name.CompareTo("Text_score")==0)
            {
                Text_score=t.GetComponent<Text>();
                Text_score.text = "分数  0";

            }
            else if(t.name.CompareTo("Button_restart")==0)
            {
                Button_restart = t.GetComponent<Button>();
                Button_restart.onClick.AddListener(delegate()         //设置重新开始关卡按钮
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);  //读取当前关卡
                });
                Button_restart.gameObject.SetActive(false);    //隐藏重新开始按钮
            }
        }
	}
	
   public void SetScore(int score)
    {
        m_score = m_score + score;
        if (m_score > m_best)
             m_best = m_score;
        Text_score.text = "Score <color=yellow>" + m_score + "</color>";
        Text_best.text = "Best Score " + m_best;
    }

    public void SetAmmo(int ammo)
   {
       m_ammo = m_ammo - ammo;
       if (m_ammo <= 0)
       {
           Player.Instance.changeShoot();
       }
       Text_ammo.text = m_ammo.ToString() + "/100";
   }

    public void LoadAmmo()
    {
        m_ammo=100;
        Text_ammo.text = m_ammo.ToString() + "/100";
    }

    public void SetLife(int life)
    {
        Text_life.text = "生命值  " +(life/46).ToString(); ;
        if (life <= 0)
            Button_restart.gameObject.SetActive(true);
    }

}
