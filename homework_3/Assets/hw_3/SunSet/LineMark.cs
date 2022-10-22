
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LineMark : MonoBehaviour 
{
	private GameObject clone;  
	private LineRenderer line;  
	private int i;  
	public GameObject obs;  
	private GameObject run;  
	Vector3 RunStart;
	Vector3 RunNext;
 
	// Use this for initialization
	void Start () 
	{
		run = this.gameObject;
		RunStart = run.transform.position;
		clone = (GameObject)Instantiate(obs, run.transform.position, run.transform.rotation);//克隆一个带有LineRender的物体   
		line = clone.GetComponent<LineRenderer>();//获得该物体上的LineRender组件  
		float line_width = this.gameObject.transform.localScale.x/4;
		line.startWidth = line_width ;//设置宽度 
		line.endWidth = line_width; 
		i = 0;
	}
 
	// Update is called once per frame  
	void Update () 
	{  
		if(Time.time>=3)
		{
			RunNext = run.transform.position;
			if (RunStart != RunNext) 
			{
				i++;
				line.positionCount = i;//设置顶点数
				line.SetPosition(i-1, run.transform.position);
			}
			RunStart = RunNext;
		}
	
	}  
}