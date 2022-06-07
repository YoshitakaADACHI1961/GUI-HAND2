using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using System.IO;


//
// 手のコントロール
//

public class InitPosition : MonoBehaviour
{

    // 現在のコントローラ LとR の位置・姿勢
    Vector3 controllerLpos = new Vector3(0.0f, 0.0f, 0.0f);
    Quaternion controllerLrot = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    Vector3 controllerRpos = new Vector3(0.0f, 0.0f, 0.0f);
    Quaternion controllerRrot = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    // 計測記録されたコントローラ LとR の位置・姿勢
    Vector3 recordedLpos = new Vector3(0.0f, 0.0f, 0.0f);
    Quaternion recordedLrot = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    Vector3 recordedRpos = new Vector3(0.0f, 0.0f, 0.0f);
    Quaternion recordedRrot = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    // コントローラ LからR に向かう位置・姿勢
    Vector3 LtoRpos = new Vector3(0.0f, 0.0f, 0.0f);
    Quaternion LtoRrot = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    // Oculusから見た手の位置・姿勢
    Vector3 OtoHandpos = new Vector3(0.0f, 0.0f, 0.0f);
    Quaternion OtoHandrot = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    //------------------------------------------------
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
            recordedRpos = initialControllerPos.sitPosR;
            recordedRrot = initialControllerPos.sitRotationR;
        }
        else // 立位のデータを入れる
        {
            // 事前に計測された、Oculusから見たコントローラの相対位置
            recordedLpos = initialControllerPos.standPosL;
            recordedLrot = initialControllerPos.standRotationL;
            recordedRpos = initialControllerPos.sitPosR;
            recordedRrot = initialControllerPos.sitRotationR;
        }
        Debug.Log($"Lpos: {recordedLpos:0.000}  Lrot:{recordedLrot:0.000}  Rpos:{recordedRpos:0.000} Rrot:{recordedRrot:0.000}");
    // コントローラ LからR に向かう位置・姿勢を求める
        LtoRpos = recordedRpos - recordedLpos;
        Debug.Log($"LtoRpos: {LtoRpos:0.000}  LtoRrot:{LtoRrot:0.000}");
    }

    // Update is called once per frame
    void Update()
    {
        //
        // 手をOculusの追従させて移動させる
        //
        // Oculusから見たコントローラLの相対位置
        controllerLpos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);

        // Oculusから見た手の位置・姿勢
        OtoHandpos = controllerLpos + LtoRpos;

        //
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            // Debug.Log("キーボード");
            if (keyboard.qKey.wasPressedThisFrame)
            {
                // スタートメニューに切り替える
                SceneManager.LoadScene("StartHere");
            }
        }


        // 手を移動

        Vector3 _axis = Vector3.right;
        var angle = 45;
        Quaternion rotation = recordedRrot * Quaternion.AngleAxis(angle, _axis); // * recordedRrot;
        _axis = Vector3.up;
        angle = 90;
        this.transform.rotation = rotation * Quaternion.AngleAxis(angle, _axis); // * recordedRrot;
        Vector3 offset = new Vector3(-0.14f, 0.0f, 0.0f);
        this.transform.position = OtoHandpos + offset;   // 併進

        Debug.Log($"Now: {controllerLpos:0.000}  LtoRpos:{LtoRpos:0.000}  Scale:{this.transform.localScale:0.0}");
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
