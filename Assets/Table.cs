using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p
using System.IO;


public class Table : MonoBehaviour
{
    // �v���L�^���ꂽ�R���g���[�� L��R �̈ʒu�E�p��
    Vector3 recordedLpos = new Vector3(0.0f, 0.0f, 0.0f);
    Quaternion recordedLrot = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {

        //�uMODE�v�Ƃ����L�[�ŕۑ�����Ă���Int�l��ǂݍ���
        int runMode = PlayerPrefs.GetInt("MODE");
        // �ǂݍ���
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
            Debug.Log("�t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + ex);
        }
        ControllerPos initialControllerPos = JsonUtility.FromJson<ControllerPos>(datastr);
        if (runMode == 1) // ���ʂ̃f�[�^������
        {

            // ���O�Ɍv�����ꂽ�AOculus���猩���R���g���[���̑��Έʒu
            recordedLpos = initialControllerPos.sitPosL;
            recordedLrot = initialControllerPos.sitRotationL;
        }
        else // ���ʂ̃f�[�^������
        {
            // ���O�Ɍv�����ꂽ�AOculus���猩���R���g���[���̑��Έʒu
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
