using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationCopyCurves : EditorWindow
{
    //The animation clips
    AnimationClip clipToCopyTo;
    AnimationClip clipToCopyFrom;

    
    private bool checkForUpdate = false;

    //The transforms to search for
    public string oldTransformToSearch;
    public string newTransformToSearch;

    //The actual curves to change
    AnimationCurve curvePositionToCopyTo;
    AnimationCurve curvePositionToCopyFrom;

    EditorCurveBinding curveBinding;

    [MenuItem("Examples/Copy Curves")]
    static void Init()
    {
        AnimationCopyCurves window = (AnimationCopyCurves)EditorWindow.GetWindow(typeof(AnimationCopyCurves));
        window.Show();
    }

    void OnGUI()
    {
        clipToCopyTo = EditorGUILayout.ObjectField("Clip to Update", clipToCopyTo, typeof(AnimationClip), false) as AnimationClip;

        oldTransformToSearch = EditorGUILayout.TextField("Transform:", oldTransformToSearch);

        if (oldTransformToSearch != null)
        {

            EditorGUILayout.LabelField("Curves:");

            if (clipToCopyTo)
            {
                foreach (EditorCurveBinding curveBinding in AnimationUtility.GetCurveBindings(clipToCopyTo))
                {
                    //Check for the all of the similar curves to change
                    if (curveBinding.propertyName.Contains(oldTransformToSearch))
                    {
                        
                        curvePositionToCopyTo = AnimationUtility.GetEditorCurve(clipToCopyTo, curveBinding);
                        curvePositionToCopyTo = EditorGUILayout.CurveField(curveBinding.propertyName, curvePositionToCopyTo);                  

                    }

                }
            }

        }


        GUILayout.Space(10);

        //Click to add an animation clip to copy curves from
        checkForUpdate = EditorGUILayout.Toggle("Check for updates?", checkForUpdate, EditorStyles.radioButton);

        if (checkForUpdate)
        {
            //No undo function right now, so this message is just to prevent me from ruining perfectly good clips
            EditorGUILayout.LabelField("MAKE SURE YOU ARE COPYING FROM THE RIGHT CLIP!", EditorStyles.helpBox);

            //The new transform to search for. It's just matching the old ones for now anyway, but this is here in hopes that I can add mirror functionality
            newTransformToSearch = EditorGUILayout.TextField("Transform:", newTransformToSearch);

            clipToCopyFrom = EditorGUILayout.ObjectField("Import Clip:", clipToCopyFrom, typeof(AnimationClip), false) as AnimationClip;

            EditorGUILayout.LabelField("New Curves:");

            if (clipToCopyFrom)
            {
                foreach (EditorCurveBinding curveBindingToCopy in AnimationUtility.GetCurveBindings(clipToCopyFrom))
                {
                    
                    if (curveBindingToCopy.propertyName.Contains(newTransformToSearch))
                    {

                        curvePositionToCopyFrom = AnimationUtility.GetEditorCurve(clipToCopyFrom, curveBindingToCopy);
                        curvePositionToCopyFrom = EditorGUILayout.CurveField(curveBindingToCopy.propertyName, curvePositionToCopyFrom);
                    }
                }           

                if (GUILayout.Button("Update Curve"))
                    UpdateCurveFromImportClip();

            }
        }
    }

    void UpdateCurveFromImportClip()
    {

        foreach (EditorCurveBinding curveBindingToCopy in AnimationUtility.GetCurveBindings(clipToCopyFrom))
        {
            //This is where the new curve overwrites the old one. 
            if (curveBindingToCopy.propertyName.Contains(oldTransformToSearch))
            {                
                curvePositionToCopyTo = AnimationUtility.GetEditorCurve(clipToCopyFrom, curveBindingToCopy);
                AnimationUtility.SetEditorCurve(clipToCopyTo, curveBindingToCopy, curvePositionToCopyTo);

            }


        }


    }



}