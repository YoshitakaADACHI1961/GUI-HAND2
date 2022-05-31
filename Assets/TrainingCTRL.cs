using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p
using System.IO;


//
// �Î~��p
//
//
// 
//[System.Serializable]
// ����p�p�����[�^�@JSON�t�@�C���`���Ŏw�肷�邱��
//
public class Control_Info
{
    public string ParamFile; // ����p�����[�^�t�@�C����
    public float Period; // �����i�b�j
    public float Duty; // �f���[�e�B��i<1.0�j
    public float PULSE_STEP; // �萔
    public float PULSE_MAX; // �萔
    public float PULSE_INITIAL; // �萔
    public float PULSE_MIN; // �萔
    public float DUTY_INITIAL; // �萔
    public float DUTY_MIN; // �萔
    public float DUTY_MAX; // �萔
    public float DUTY_STEP; // �萔
    public float PulseStartTime; // �����J�n�����i�b�j
    public int PulseStartFrame; // �����J�n�����i�t���[�����Z�j
    public float PulseStopTime; // ������~�����i�b�j
    public int PulseStopFrame; // ������~�����i�t���[�����Z�j


    // �����񂩂�JSON�f�[�^�����
    public static Control_Info CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Control_Info>(jsonString);
    }

    // JSON�f�[�^���當��������
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    // �������V�����f�[�^�ŏ���������
    public void OverWrite(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}

//
// Raspberry Pi �Ƃ̏������Ɏg�p����
//
public class RaspControl
{
    public string RunMode; // START or STOP
    public float Period; // �����i�b�j
    public float Duty; // �f���[�e�B��i<1.0�j

    // �����񂩂�JSON�f�[�^�����
    public static RaspControl CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<RaspControl>(jsonString);
    }

    // JSON�f�[�^���當��������
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    // �������V�����f�[�^�ŏ���������
    public void OverWrite(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}


//
// 
//
//[Serializable]
public class TrainingCTRL : MonoBehaviour
{
    Control_Info InitialParam = new Control_Info(); // �������p�t�@�C��������͂����p�����[�^
    RaspControl ControlParam = new RaspControl(); // Raspberry Pi �̐���p
    private float ProgramStartTime; // �v���O�����̊J�n����
    private float PulseStartTime; // �����J�n�����i�b�j
    private int PulseStartFrame; // �����J�n�����i�t���[�����Z�j
    private float PulseStopTime; // ������~�����i�b�j
    private int PulseStopFrame; // ������~�����i�t���[�����Z�j
    //
    // Start is called before the first frame update
    //
    void Start()
    {
        Debug.Log("Control Program Start!");
        Debug.Log(" Time.deltaTime : " + Time.deltaTime);
        // �����ݒ�p�t�@�C���̓ǂݍ���
//        string initFile = File.ReadAllText("C:/Users/raspberry/UTfolder/InitControl.json");
        string initFile = "C:/Users/raspberry/UTfolder/InitControl.json";
//        Debug.Log("�����ݒ�p�t�@�C�� : " + initFile);
        if (File.Exists(initFile)) // �t�@�C�������݂���ꍇ
        {
            string datastr = "";
            StreamReader reader;
            try
            {
                reader = new StreamReader(initFile);
                datastr = reader.ReadToEnd();
                InitialParam = JsonUtility.FromJson<Control_Info>(datastr);
  //              Debug.Log("InitialParam : " + InitialParam);
                reader.Close();
            }
            catch (System.IO.IOException ex)
            {
                Debug.Log("�����ݒ�p�t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + initFile + "  ERROR:" + ex);
            }
        }
        else
        {
            Debug.Log("�����ݒ�p�t�@�C�������݂��܂��� : "+ initFile);
            // Unity���I��������
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif

        }



        //        InitialParam = JsonUtility.FromJson<Control_Info>(initFile);

        // Raspberry Pi ����p�����[�^�̐ݒ�
        ControlParam.RunMode = "STOP";
        ControlParam.Period = InitialParam.Period;
        ControlParam.Duty = InitialParam.Duty;

        // Raspberry Pi ����p�����[�^�t�@�C���̏�������
//        Debug.Log("test : " + InitialParam.ParamFile + " : " + ControlParam.SaveToString());
        try
        {
            File.WriteAllText(InitialParam.ParamFile, ControlParam.SaveToString());
            Debug.Log("�Î~��p�@Raspberry Pi ����p�����[�^�t�@�C���̏�������");
        }
        catch (System.IO.IOException ex)
        {
            Debug.Log("�Î~��p�@����p�����[�^�t�@�C���̏������݂ŃG���[�ɂȂ�܂���" + InitialParam.ParamFile + "  ERROR:" + ex);
        }

        ProgramStartTime = Time.time; // ���݂̎���
        PulseStartTime = InitialParam.PulseStartTime + ProgramStartTime; // �����J�n�����i�b�j
        PulseStartFrame = (int)(PulseStartTime / Time.deltaTime); // �����J�n�����i�t���[�����Z�j
        PulseStopTime = InitialParam.PulseStopTime + ProgramStartTime; // ������~�����i�b�j
        PulseStopFrame = (int)(PulseStopTime / Time.deltaTime); // ������~�����i�t���[�����Z�j
        Debug.Log("START time : " + PulseStartTime + " PulseStopTime" + PulseStopTime);
    }

    //
    // Fixed Update 
    // Edit�v���uProject Setting�v���uTime�v FixedTimestep 0.5
    //
    private void FixedUpdate()
    {
        /*
            if ((Time.time > PulseStartTime)&& (Time.time < PulseStopTime) && (ControlParam.RunMode == "STOP")) // �����J�n�������߂����ꍇ
            {
                ControlParam.RunMode = "START";
                Debug.Log("Pulse START !  " + Time.time);
            }
            else if ( (Time.time > PulseStopTime) && (ControlParam.RunMode == "START"))
            {
                ControlParam.RunMode = "STOP";
                Debug.Log("Pulse STOP !  " + Time.time);
            }
        */
        Debug.Log("ControlParam.RunMode  :  " + ControlParam.RunMode);
        // ����p�����[�^�t�@�C���̏�������
        File.WriteAllText(InitialParam.ParamFile, ControlParam.SaveToString());

    }

    //
    // Update is called once per frame
    //
    void Update()
    {


        //
        // �����̐���
        //
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.upArrowKey.wasPressedThisFrame)
            {
                ControlParam.Period -= InitialParam.PULSE_STEP;
                if (ControlParam.Period < InitialParam.PULSE_MIN)
                    ControlParam.Period = InitialParam.PULSE_MIN;
            }
            else if (keyboard.downArrowKey.wasPressedThisFrame)
            {
                ControlParam.Period += InitialParam.PULSE_STEP;
                if (ControlParam.Period > InitialParam.PULSE_MAX)
                    ControlParam.Period = InitialParam.PULSE_MAX;
            }
            else if (keyboard.rightArrowKey.wasPressedThisFrame)
            {
                ControlParam.Duty += InitialParam.DUTY_STEP;
                if (ControlParam.Duty > InitialParam.DUTY_MAX)
                    ControlParam.Duty = InitialParam.DUTY_MAX;
            }
            else if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                ControlParam.Duty -= InitialParam.DUTY_STEP;
                if (ControlParam.Duty < InitialParam.DUTY_MIN)
                    ControlParam.Duty = InitialParam.DUTY_MIN;
            }
            else if (keyboard.spaceKey.wasPressedThisFrame)
            {
                if (ControlParam.RunMode == "STOP")
                {
                    ControlParam.RunMode = "START";
                    Debug.Log("Set to START");
                }
                else
                {
                    ControlParam.RunMode = "STOP";
                    Debug.Log("Set to STOP");
                }
            }
            else if (keyboard.qKey.wasPressedThisFrame)
            {
//                WebCamTexture webCamTexture = new WebCamTexture();
//                webCamTexture.Stop(); // �J�������~

               ControlParam.RunMode = "STOP";
                File.WriteAllText(InitialParam.ParamFile, ControlParam.SaveToString());
                // �X�^�[�g���j���[�ɐ؂�ւ���
                SceneManager.LoadScene("StartHere");
            }
        }
    }
}

