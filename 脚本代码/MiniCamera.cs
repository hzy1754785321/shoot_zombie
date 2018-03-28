using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float ratio=(float)Screen.width/(float)Screen.height;   //获得屏幕分辨率比例
        this.GetComponent<Camera>().rect=new Rect((1-0.2f),(1-0.2f*ratio),0.2f,0.2f*ratio);   
        //使视图始终为正方形，rect前面两个参数为XY位置，后面两个参数为XY大小
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
