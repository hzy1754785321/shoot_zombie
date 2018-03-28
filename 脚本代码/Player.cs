using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Transform m_transform;
    public static Player Instance = null;   //静态实例
    CharacterController m_ch;     //角色控制器组件
    public float m_movSpeed = 6.0f;    //速度
    public float m_gravity = 2.0f;    //重力
    public int m_life = 5;     //生命值
    Transform m_camTransform;  //摄像机的transform
    Vector3 m_camRot;    //摄像机旋转角度
    float m_camHeight = 1.4f;    //表示主角的身高
    Transform m_muzzlepoint;   //枪口transform
    public LayerMask m_layer;    //射击时，射线能接触的碰撞层
    public Transform m_fx;    //射中目标后的粒子效果
    public AudioClip m_audio;   //射击音效
    public float m_shootTimer = 0;   //射击计时器
    public AudioClip m_background;   //背景音乐
    public bool shoot_flag = true;

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();  //获取角色控制器组件
        this.GetComponent<AudioSource>().PlayOneShot(m_background);    //播放背景音乐
        m_camTransform = Camera.main.transform;   //获取摄像机
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0); //设置摄像机初始位置
        m_camTransform.rotation = m_transform.rotation;
        m_camRot = m_camTransform.eulerAngles;     //使摄像机旋转方向与主角一致

        Cursor.lockState = CursorLockMode.Locked;   //锁定鼠标
        Cursor.visible = false;


        m_muzzlepoint = m_camTransform.Find("M16/weapon/muzzlepoint").transform;   //因为枪口在模型较深层,所以用/分隔
	}
	
	// Update is called once per frame
	void Update () {
        if (m_life <= 0)
            return;
        Control();
        Instance = this;
        m_shootTimer = m_shootTimer - Time.deltaTime;
        if(Input.GetMouseButton(0)&&m_shootTimer<=0&&shoot_flag)
        {
            m_shootTimer = 0.1f;
            this.GetComponent<AudioSource>().PlayOneShot(m_audio);
            GameManager.Instance.SetAmmo(1);  //减少弹药

            RaycastHit info;  //用来保存射线探测结果
            bool hit = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info,100, m_layer);
               //发射射线
            if(hit)
             {
                if(info.transform.tag.CompareTo("enemy")==0)
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                }
                Instantiate(m_fx, info.point, info.transform.rotation);
             }
        }
	}

    void Control()    //主角移动函数
    {
        float xm = 0, ym = 0, zm = 0;
        ym = ym - m_gravity * Time.deltaTime;  //重力运动

        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");    //获取鼠标移动距离

        m_camRot.x = m_camRot.x - rv;
        m_camRot.y = m_camRot.y + rh;
        m_camTransform.eulerAngles = m_camRot;  //旋转摄像机

        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0;
        camrot.z = 0;
        m_transform.eulerAngles = camrot;    //使主角面向的方向与摄像机一致

        if(Input.GetKey(KeyCode.W))
        {
            zm = zm + m_movSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            zm = zm - m_movSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            xm = xm - m_movSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            xm = xm + m_movSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.W)&&Input.GetKey(KeyCode.LeftShift))
        {
            m_movSpeed = 10;
            zm = zm + m_movSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.R))
        {
            GameManager.Instance.LoadAmmo();
            shoot_flag = true;
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));  
        //使用角色控制器提供的move函数进行移动，它会自动检测碰撞
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0); //更新摄像机位置
    }

    void OnDrawGizmos()    //在编辑器中给主角显示一个图标
    { 
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }

    public void OnDamager(int damage)
    {
        m_life = m_life - damage;
        print(m_life);
        GameManager.Instance.SetLife(m_life); //更新UI
        if (m_life <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void changeShoot()
    {
            shoot_flag = false;
    }
}






