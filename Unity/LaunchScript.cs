using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using CraftopiaRNGTool;

public class BootScript : MonoBehaviour
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

    IntPtr hWnd;
    private bool loadFlag = false;

    void Awake()
    {
        hWnd = FindWindow(null, "CraftopiaRNGTool");
        Program.Launch();
        Debug.Log("起動！");  // ログを出力
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Program.isExit)
        {
            Quit();
        }

        if (!loadFlag)
        {
            if (!Program.isLoading)
            {
                loadFlag = true;
                ShowWindow(hWnd, 2);
            }
        }
    }

    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
             UnityEngine.Application.Quit();
        #endif
    }
}
