using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//When curves are added to an imported animation clip, it is difficult to adjust them. 
//This is an editor window to update hidden curves on duplicated animation clips. 
//Note: There is no protection for overwriting yet, so you need to make sure the clips match. 
//Replace the curveBinding property name with the name of the added curve and duplicate the snippet for as many bindings as needed.
public class AnimationCurveEditor : EditorWindow
{

    AnimationClip clipToCopyTo;
    AnimationClip clipToCopyFrom;

    private bool checkForUpdate = false;

    AnimationCurve curvePositionToCopy;
    AnimationCurve curveRotationToCopy;

    EditorCurveBinding positionBinding;
    EditorCurveBinding rotationBinding;

    [MenuItem("Examples/Edit IK Curves")]
    static void Init()
    {
        AnimationCurveEditor window = (AnimationCurveEditor)EditorWindow.GetWindow(typeof(AnimationCurveEditor));
        window.Show();
    }

    void OnGUI()
    {
        clipToCopyTo = EditorGUILayout.ObjectField("Clip to Update", clipToCopyTo, typeof(AnimationClip), false) as AnimationClip;

        EditorGUILayout.LabelField("Current IK Curves:");

        if (clipToCopyTo != null)
        {
            foreach (EditorCurveBinding curveBinding in AnimationUtility.GetCurveBindings(clipToCopyTo))
            {
                //Check for the positionWeight curve
                if (curveBinding.propertyName == "positionWeight")
                {

                    AnimationCurve curvePosition = AnimationUtility.GetEditorCurve(clipToCopyTo, curveBinding);
                    curvePosition = EditorGUILayout.CurveField(curveBinding.propertyName, curvePosition);


                }
                
                //Check for the rotationWeight curve
                if (curveBinding.propertyName == "rotationWeight")
                {

                    AnimationCurve curveRotation = AnimationUtility.GetEditorCurve(clipToCopyTo, curveBinding);
                    curveRotation = EditorGUILayout.CurveField(curveBinding.propertyName, curveRotation);

                }
            }
       }

        GUILayout.Space(10);

        checkForUpdate = EditorGUILayout.Toggle("Check for updates?", checkForUpdate, EditorStyles.radioButton);

        if (checkForUpdate)
        {
            EditorGUILayout.LabelField("MAKE SURE YOU ARE COPYING FROM THE RIGHT CLIP!", EditorStyles.helpBox);

            clipToCopyFrom = EditorGUILayout.ObjectField("Import Clip:", clipToCopyFrom, typeof(AnimationClip), false) as AnimationClip;

            EditorGUILayout.LabelField("New IK Curves:");

            if (clipToCopyFrom != null)
            {
                foreach (EditorCurveBinding curveBindingToCopy in AnimationUtility.GetCurveBindings(clipToCopyFrom))
                {
                    //Check for the positionWeight curve
                    if (curveBindingToCopy.propertyName == "positionWeight")
                    {

                        curvePositionToCopy = AnimationUtility.GetEditorCurve(clipToCopyFrom, curveBindingToCopy);
                        curvePositionToCopy = EditorGUILayout.CurveField(curveBindingToCopy.propertyName, curvePositionToCopy);
                        positionBinding = curveBindingToCopy;
                    }

                    //Check for the rotationWeight curve
                    if (curveBindingToCopy.propertyName == "rotationWeight")
                    {

                        curveRotationToCopy = AnimationUtility.GetEditorCurve(clipToCopyFrom, curveBindingToCopy);
                        curveRotationToCopy = EditorGUILayout.CurveField(curveBindingToCopy.propertyName, curveRotationToCopy);
                        rotationBinding = curveBindingToCopy;
                    }
                }

                if (GUILayout.Button("Update Curve"))
                    UpdateCurveFromImportClip(curvePositionToCopy, curveRotationToCopy, positionBinding, rotationBinding);

            }
        }
    }

    void UpdateCurveFromImportClip(AnimationCurve _positionWeight, AnimationCurve _rotationWeight, EditorCurveBinding _positionBinding, EditorCurveBinding _rotationBinding)
    {

        AnimationUtility.SetEditorCurve(clipToCopyTo, _positionBinding, _positionWeight);
        AnimationUtility.SetEditorCurve(clipToCopyTo, _rotationBinding, _rotationWeight);
    }



}
