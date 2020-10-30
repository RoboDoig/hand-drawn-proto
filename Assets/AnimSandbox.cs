using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimSandbox : ScriptableWizard
{
    public AnimationClip animationClip;

    public string clipName;

    [MenuItem("GameObject/AnimSandbox")]
    static void CreateWizard() {
        ScriptableWizard.DisplayWizard<AnimSandbox>("");
    }

    void OnWizardCreate() {
        Debug.Log(animationClip);  
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(animationClip);
        foreach (EditorCurveBinding curve in curveBindings) {
            Debug.Log(curve.path);
        }
    }
}
