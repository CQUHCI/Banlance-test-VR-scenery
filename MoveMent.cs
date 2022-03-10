using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Valve.VR;

public enum FloorType
{
    Floor01,
    Floor02,
    Floor03
}

public class MoveMent : MonoBehaviour
{
    [SerializeField] float recordTimmer;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform target;
    private SteamVR_Action_Vector2 remoter2;
    public SteamVR_Behaviour_Pose pose;
    FloorType mType = FloorType.Floor01;

    float timmer;
    string targetDataAll;
    string targetData1;
    string targetData2;
    string targetData3;
    private void Start()
    {
        timmer = recordTimmer;
        remoter2 = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("remoter2");
    }
    private void Update()
    {
        //只要手放在触控板上，即可触发
        if (remoter2.GetChanged(pose.inputSource))
        {
            float AxisY = remoter2.GetAxis(pose.inputSource).y;
            transform.Translate(AxisY * Vector3.forward * moveSpeed * Time.deltaTime, Space.World);

            timmer -= Time.deltaTime;
            if (timmer < 0)
            {
                timmer = recordTimmer;
                //string data = "坐标位置：" + target.position.ToString() + "    欧拉角：" + target.eulerAngles + "\n";
                string data = GetTargetStr(target.position, target.eulerAngles) + "\n";//target.position.ToString() + target.eulerAngles + "\n";
                switch (mType)
                {
                    case FloorType.Floor01:
                        targetData1 += data;
                        break;
                    case FloorType.Floor02:
                        targetData2 += data;
                        break;
                    case FloorType.Floor03:
                        targetData3 += data;
                        break;
                }
                targetDataAll += data;
            }
        }
    }

    bool isEnd = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isEnd)
            return;

        if (other.transform.name == "EndTrigger")
        {
            isEnd = true;
            string filePath = Application.streamingAssetsPath;
            Debug.Log("filePath:" + filePath);
            string fileName = System.DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "").Trim() + System.DateTime.Now.ToString("hh:mm:ss").Replace(":", "").Trim() + ".txt";
            CreateTextToFile(filePath, fileName, targetDataAll);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        switch (collision.transform.name)
        {
            case "Floor1":
                mType = FloorType.Floor01;
                break;
            case "Floor2":
                mType = FloorType.Floor02;
                break;
            case "Floor3":
                mType = FloorType.Floor03;
                break;
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="data"></param>
    public void CreateTextToFile(string filePath, string fileName, string data)
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string fileAllPath = Path.Combine(filePath, fileName);
        string filePath1 = Path.Combine(filePath, "01_"+ fileName);
        string filePath2 = Path.Combine(filePath, "02_" + fileName);
        string filePath3 = Path.Combine(filePath, "03_" + fileName);

        StreamWriter swAll = new StreamWriter(fileAllPath, false, Encoding.UTF8);
        StreamWriter sw1 = new StreamWriter(filePath1, false, Encoding.UTF8);
        StreamWriter sw2 = new StreamWriter(filePath2, false, Encoding.UTF8);
        StreamWriter sw3 = new StreamWriter(filePath3, false, Encoding.UTF8);

        swAll.WriteLine(targetDataAll);
        sw1.WriteLine(targetData1);
        sw2.WriteLine(targetData2);
        sw3.WriteLine(targetData3);

        swAll.Close();
        swAll.Dispose();
        sw1.Close();
        sw1.Dispose();
        sw2.Close();
        sw2.Dispose();
        sw3.Close();
        sw3.Dispose();
    }

    string GetTargetStr(Vector3 pos,Vector3 angle)
    {
        string value = "";
        value =  pos.x + "," + pos.y + "," + pos.z + "," + angle.x + "," + angle.y + "," + angle.z ;
        return value;
            
    }
}
