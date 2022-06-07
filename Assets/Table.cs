using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using System.IO;


public class Table : MonoBehaviour
{
    // 計測記録されたコントローラ LとR の位置・姿勢
    Vector3 recordedLpos = new Vector3(0.0f, 0.0f, 0.0f);
    Quaternion recordedLrot = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {

        //「MODE」というキーで保存されているInt値を読み込み
        int runMode = PlayerPrefs.GetInt("MODE");
        // 読み込み
        string DataFile = "C:/Users/raspberry/UTfolder/PosController.json";
        string datastr = "";
        StreamReader reader;
        try
        {
            reader = new StreamReader(DataFile);
            datastr = reader.ReadToEnd();
            reader.Close();
        }
        catch (System.IO.IOException ex)
        {
            Debug.Log("ファイルを開くときにエラーになりました" + ex);
        }
        ControllerPos initialControllerPos = JsonUtility.FromJson<ControllerPos>(datastr);
        if (runMode == 1) // 座位のデータを入れる
        {

            // 事前に計測された、Oculusから見たコントローラの相対位置
            recordedLpos = initialControllerPos.sitPosL;
            recordedLrot = initialControllerPos.sitRotationL;
        }
        else // 立位のデータを入れる
        {
            // 事前に計測された、Oculusから見たコントローラの相対位置
            recordedLpos = initialControllerPos.standPosL;
            recordedLrot = initialControllerPos.standRotationL;
        }
        Vector3 offset = new Vector3(0.2f, -0.03f, -0.1f);
        this.transform.position = recordedLpos + offset;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        //
    // class を作成する
    //
    [System.Serializable]
    public class ControllerPos
    {
        public Vector3 sitPosR = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion sitRotationR = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 sitPosL = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion sitRotationL = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 standPosR = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion standRotationR = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 standPosL = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion standRotationL = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    }
}
