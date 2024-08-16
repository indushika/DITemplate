using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

//[InitializeOnLoad]
public class CompilationListener : MonoBehaviour
{
    static  CompilationListener()
    {
        CompilationPipeline.compilationFinished += OnCompilationFinished;
    }

    private static void OnCompilationFinished(object obj)
    {
        Debug.Log("Compilation finished, generating native structs...");
        //GenericNativeDataGenerator.GenerateNativeDataScripts();
    }
}
