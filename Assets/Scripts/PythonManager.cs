using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Scripting.Python;
using UnityEngine;
using UnityEngine.UI;

public class PythonManager : MonoBehaviour
{
    [Sirenix.OdinInspector.FilePath()]
    public string path = "";
    [Button]
    private void PrintHelloWorldFromPython()
    {
        PythonRunner.RunFile(path);
    }
}
