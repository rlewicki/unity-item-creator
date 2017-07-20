using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemCreator : EditorWindow
{
    [MenuItem("Window/Item Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ItemCreator));
    }

    private Object                      _model          = null;
    private Object                      _currentModel   = null;
    private List<string>                _fieldValues    = new List<string>();
    private Dictionary<string, string>  _values         = new Dictionary<string, string>();

    private void OnGUI()
    {
        GUILayout.Label("Create item:", EditorStyles.boldLabel);
        _model = EditorGUILayout.ObjectField(_model, typeof(Object), true);
        EditorGUILayout.Space();

        GUILayout.Label("Item properties:", EditorStyles.boldLabel);
        if(_currentModel != _model)
        {
            ReadProperties();
            _currentModel = _model;
        }
        AssignPropertiesValues();

        EditorGUILayout.Space();
        CreateItemButton();
    }

    private void ReadProperties()
    {
        if (_model != null && _model.GetType() == typeof(MonoScript))
        {
            var script = _model as MonoScript;
            var scriptClass = script.GetClass();
            Debug.Assert(scriptClass.GetProperties().Length != 0);

            _values.Clear();
            _fieldValues.Clear();
            foreach (var property in scriptClass.GetProperties())
            {
                var propertyKey = $"{property.Name} ({property.PropertyType})";
                _fieldValues.Add(propertyKey);
                _values.Add(propertyKey, "");
            }
        }
    }

    private void AssignPropertiesValues()
    {
        EditorGUIUtility.labelWidth = 250;
        foreach(var key in _fieldValues)
        {
            _values[key] = EditorGUILayout.TextField(key, _values[key]);
        }
    }

    private void CreateItemButton()
    {
        if(GUILayout.Button("Create item", GUILayout.MinHeight(40)))
        {
            foreach(var key in _fieldValues)
            {
                if(_values[key] == "")
                {
                    var answear = EditorUtility.DisplayDialog(
                        "Warning", 
                        "You are about to create item with empty fields.", 
                        "Proceed",
                        "Cancel"
                    );

                    if(!answear)
                    {
                        CreateItem();
                    }
                    return;
                }
            }
            CreateItem();
        }
    }

    private void CreateItem()
    {

    }
}
