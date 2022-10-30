using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : ISystem {

	public static CameraSystem ctrl;
	[Header("目標")]
	public Transform target;
    public Transform minimapCamera;
	[Header("距離")]
	public float distance = 5f;
	public float minDis = 2f, maxDis = 10f;
	[Header("角度")]
	public float angX, angY, angZ;
	public float minAng = 15f, maxAng = 40f;
	private Vector2 startScreenPoint, endScreenPoint;
	private Quaternion cameraRot
	{
		get { return Quaternion.Euler(angX, angY, 0); }
	}
	private void Awake()
	{
		ctrl = this;
	}
	private void Update()
	{
		Follow();
	}
	void Follow()
	{
        minimapCamera.position = target.position + Vector3.up * 100f;
		transform.position = target.position + checkAng() * CheckDis();//角度*向量
        transform.LookAt(target);
	}
	Vector3 CheckDis()
	{
		distance -= Input.mouseScrollDelta.y;
		if (distance <= minDis)
		{
			distance = minDis;
		}
		else if (distance >= maxDis)
		{
			distance = maxDis;
		}
		return Vector3.back * distance;
	}
	Quaternion checkAng()
	{
		if (Input.GetMouseButtonDown(1))
		{
			startScreenPoint = Input.mousePosition;
		}
		if (Input.GetMouseButton(1))
		{
			endScreenPoint = Input.mousePosition;
			angX += (startScreenPoint - endScreenPoint).y / 10f;
			angY += (startScreenPoint - endScreenPoint).x / 10f;
			if (angX <= minAng)
			{
				angX = minAng;
			}
			else if (angX >= maxAng)
			{
				angX = maxAng;
			}
            startScreenPoint = endScreenPoint;
		}
		return cameraRot;
	}

    public override void Inital()
    {
        target = GM.player.transform;//呼叫攝影機單例指定它的目標 = player
    }
    /*public Vector3 Facing()
    {
        return transform.forward;
    }*/
}
