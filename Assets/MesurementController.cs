using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p
using System.IO;
using System;

// 
// Oculus���猩���R���g���[���i���E�j�̈ʒu�Ǝp�����v������
// �ub�v�������ƍ��ʂ̌v��
// �u���v�������Ɨ��ʂ̌v��
// 
public class MesurementController : MonoBehaviour
{
    // ���s���[�h
    int runMode = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start MesurementController !");

        //�uMODE�v�Ƃ����L�[�ŕۑ�����Ă���Int�l��ǂݍ���
        runMode = PlayerPrefs.GetInt("MODE");
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger) || OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            Debug.Log("�E�܂��͍��̒��w�O���b�v��������!");

            //  �E��ƍ���@�R���g���[���[�̈ʒu���擾
            Vector3 LocalPos_R = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 LocalPos_L = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            Quaternion LocalRotation_R = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            Quaternion LocalRotation_L = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

            //            Debug.Log(LocalPos_R+","+LocalRotation_R + "," + LocalPos_L + "," + LocalRotation_L);

            // �f�[�^�����Ă����t�@�C��
            string DataFile = "C:/Users/raspberry/UTfolder/PosController.json";
            // �t�@�C�������݂��Ă����珑������������B�����łȂ���ΐV�����쐬����
            if (File.Exists(DataFile)) // �t�@�C�������݂���ꍇ
            {
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
                    Debug.Log("1 �t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + ex);
                }

                ControllerPos controllerPos = JsonUtility.FromJson<ControllerPos>(datastr);

                if (runMode == 1) // ���ʂ̃f�[�^������
                {
                    StreamWriter writer;
                    controllerPos.sitPosR = LocalPos_R;
                    controllerPos.sitRotationR = LocalRotation_R;
                    controllerPos.sitPosL = LocalPos_L;
                    controllerPos.sitRotationL = LocalRotation_L;
                    string jsonstr = JsonUtility.ToJson(controllerPos);
                    Debug.Log("Json " + jsonstr);
                    try
                    {
                        writer = new StreamWriter(DataFile, false);
                        writer.Write(jsonstr);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.Log("2 �t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + ex);
                    }
                }
                else // ���ʂ̃f�[�^������
                {
                    StreamWriter writer;
                    controllerPos.standPosR = LocalPos_R;
                    controllerPos.standRotationR = LocalRotation_R;
                    controllerPos.standPosL = LocalPos_L;
                    controllerPos.standRotationL = LocalRotation_L;
                    string jsonstr = JsonUtility.ToJson(controllerPos);
                    try
                    {
                        writer = new StreamWriter(DataFile, false);
                        Debug.Log("Json " + jsonstr);
                        writer.Write(jsonstr);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.Log("3 �t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + ex);
                    }
                }

            }
            else // �t�@�C�������݂��Ȃ��ꍇ
            {
                if (runMode == 1) // ���ʂ̃f�[�^������
                {
                    StreamWriter writer;
                    ControllerPos controllerPos = new ControllerPos();
                    controllerPos.sitPosR = LocalPos_R;
                    controllerPos.sitRotationR = LocalRotation_R;
                    controllerPos.sitPosL = LocalPos_L;
                    controllerPos.sitRotationL = LocalRotation_L;
                    string jsonstr = JsonUtility.ToJson(controllerPos);
                    Debug.Log("Json " + jsonstr);
                    try
                    {
                        writer = new StreamWriter(DataFile, append: true);
                        writer.Write(jsonstr);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.Log("4 �t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + ex);
                    }
                }
                else // ���ʂ̃f�[�^������
                {
                    StreamWriter writer;
                    ControllerPos controllerPos = new ControllerPos();
                    controllerPos.standPosR = LocalPos_R;
                    controllerPos.standRotationR = LocalRotation_R;
                    controllerPos.standPosL = LocalPos_L;
                    controllerPos.standRotationL = LocalRotation_L;
                    string jsonstr = JsonUtility.ToJson(controllerPos);
                    try
                    {
                        writer = new StreamWriter(DataFile, append: true);
                        Debug.Log("Json " + jsonstr);
                        writer.Write(jsonstr);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.Log("5 �t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + ex);
                    }
                }
            }
            // �X�^�[�g���j���[�ɐ؂�ւ���
            SceneManager.LoadScene("StartHere");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // �X�^�[�g���j���[�ɐ؂�ւ���
            SceneManager.LoadScene("StartHere");
        }
    }

    //
    // class ���쐬����
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
