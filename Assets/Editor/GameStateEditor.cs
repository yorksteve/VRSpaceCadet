using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Scripts.Managers;


[CustomEditor(typeof(GameSceneManager))]
public class GameStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameSceneManager gameScene = (GameSceneManager)target;

        if (GUILayout.Button("Change game scene"))
        {
            gameScene.ChangeGameScene(gameScene._gameScene);
        }
    }
}
