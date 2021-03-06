using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class StartHere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.bKey.wasPressedThisFrame)
            {
                // 座位での初期位置を計測して位置姿勢データを保存する
                // 
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MeasurementScene");

            }
            else if (keyboard.cKey.wasPressedThisFrame)
            {
                // 座位のキャリブレーション
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("InitPosition");


            }
            else if (keyboard.rKey.wasPressedThisFrame)
            {
                // 座位の実行
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("TrainingScene");
            }
            else if (keyboard.eKey.wasPressedThisFrame)
            {
                // E　で終了
                #if UNITY_EDITOR
                  UnityEditor.EditorApplication.isPlaying = false;
                #elif UNITY_STANDALONE
                  UnityEngine.Application.Quit();
                #endif
            }
        }
    }
}
