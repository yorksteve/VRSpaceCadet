using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class UnderscoreReplace : EditorWindow
{
    [SerializeField]
    private GameObject[] renameObjects;
    private SerializedObject so;
    private SerializedProperty goProp;

    [MenuItem("Tools/UnderscoreReplace")]

    static void CreateUnderscoreReplace()
    {
        EditorWindow.GetWindow<UnderscoreReplace>();
    }

    private void OnEnable()
    {
        so = new SerializedObject(this);
        goProp = so.FindProperty("renameObjects");
    }
    // Start is called before the first frame update

    private void OnGUI()
    {
        EditorGUILayout.PropertyField(goProp, true);
        so.ApplyModifiedProperties();


            if (GUILayout.Button("Rename"))
        {
            Rename();
        }
    }

    

    void Rename()
    {
        foreach (GameObject obj in renameObjects)
        {
            var newName = obj.name.ToString();
            var stringarray = newName.ToCharArray();
            for (int i = 0; i <stringarray.Length; i++)
            {
                
                if (stringarray[i].Equals('_'))
                {

                    stringarray[i] = ':' ;
                    newName = new string (stringarray);
                    Debug.Log(newName);
                    obj.name = newName.ToString();
                    break;
                }


            }

        }
    }

}

