using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class StopMotionAnimationClip : ScriptableWizard
{
    public AnimationClip animationClip;
    public int keepEveryNFrames = 10; 

    public string clipSaveName = "default";

    [MenuItem("Assets/StopMotionAnimationClip")]
    static void CreateWizard() {
        ScriptableWizard.DisplayWizard<StopMotionAnimationClip>("");
    }

    void OnWizardCreate() {
        // First copy the asset and reload so we can work on the copy
        string originalPath = AssetDatabase.GetAssetPath(animationClip);
        string newPath = Path.Combine(Path.GetDirectoryName(originalPath), clipSaveName + ".anim");
        AssetDatabase.CopyAsset(originalPath, newPath);
        AnimationClip modifiedClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(newPath, typeof(AnimationClip));

        // Get all curve bindings for the clip
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(animationClip);

        // Loop through all bindings in the selected clip
        foreach (EditorCurveBinding binding in curveBindings) {
            Debug.Log(binding.propertyName);

            // For each binding (skeleton property) get the associated curve
            AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, binding);

            // Create a new curve to add keys
            AnimationCurve modCurve = new AnimationCurve();

            // Determine how many frames this curve has
            int nFrames = curve.keys.Length;

            // Grab the first frame and add it to the modified new curve
            Keyframe lastFrame = curve.keys[0];
            modCurve.AddKey(lastFrame.time, lastFrame.value);

            // Loop through the original curve frames
            for (int i = 1; i < nFrames; i++) {
                // If we reach a frame we want to keep, update the copy frame
                if (i % keepEveryNFrames == 0) {
                    lastFrame = curve.keys[i];
                } 

                // Add the most recent copy frame to the modified curve
                modCurve.AddKey(curve.keys[i].time, lastFrame.value);
                modCurve.keys[i].inTangent = Mathf.Infinity;
                modCurve.keys[i].outTangent = Mathf.Infinity;
            } 

            // Replace the curve in the new clip
            AnimationUtility.SetEditorCurve(modifiedClip, binding, modCurve);   

            // Remember to set both tangents to constant in dopesheet for step-movement 
        }
    }
}
