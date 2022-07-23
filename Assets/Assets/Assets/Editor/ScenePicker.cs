using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using FileSystemEntities;

public class ScenePicker : EditorWindow
{
    private FileSystemTree _fileSystemTree;
    private Stack<FileSystemEntity> _fileSystemEntities = new Stack<FileSystemEntity>();

    [MenuItem("Window/Custom Windows/Scene Picker")]
    public static void ShowWindow()
    {
        GetWindow<ScenePicker>("Scene Picker");
    }

    private void OnGUI()
    {
        _fileSystemTree = _fileSystemTree ?? new FileSystemTree(Application.dataPath + "/Scenes/");

        if (GUILayout.Button("Refresh"))
        {
            _fileSystemTree.GenerateTreeStructure();
        }

        _fileSystemEntities.Push(_fileSystemTree.Root);
        while (_fileSystemEntities.Count > 0)
        {
            var current = _fileSystemEntities.Pop();

            EditorGUI.indentLevel = current.IndentLevel;

            if (current is Folder folder)
            {
                folder.IsFolded = EditorGUILayout.Foldout(folder.IsFolded, current.Name);
                if (folder.IsFolded)
                {
                    foreach (var entity in _fileSystemTree.GetSubItems(folder.Path))
                    {
                        _fileSystemEntities.Push(entity);
                    }
                }
            }
            else
            {
                var file = (FileSystemEntities.File)current;
                if (GUILayout.Button(file.Name))
                {
                    EditorSceneManager.OpenScene(file.Path);
                }
            }
        }

        EditorGUI.indentLevel = 0;
    }
}
