using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorManager : MonoBehaviour
{
    public GameObject errorPanel;
    public TMP_Text errorText;

    public void ShowError(string text){
        errorText.text = text;
        errorPanel.SetActive(true);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Pots filtrar segons el tipus de missatge (Error, Warning, Log, etc.)
        if (type == LogType.Error || type == LogType.Exception)
        {
            //Debug.Log($"Error Capturat: {logString}");
            //Debug.Log($"StackTrace: {stackTrace}");
            ShowError(logString);
        }
        /*else if (type == LogType.Warning)
        {
            Debug.Log($"Advertència Capturada: {logString}");
        }
        else if (type == LogType.Log)
        {
            Debug.Log($"Missatge Capturat: {logString}");
        }*/
    }
}
