using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;

public class ItemCreator : EditorWindow
{
    [MenuItem("Window/Item Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ItemCreator));
    }

    private Object                      _model          = null;
    private Object                      _currentModel   = null;
    private List<string>                _keys           = new List<string>();
    private Dictionary<string, string>  _values         = new Dictionary<string, string>();
    private Dictionary<string, System.Type> _types      = new Dictionary<string, System.Type>();

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

            _keys.Clear();
            _values.Clear();
            _types.Clear();
            foreach (var property in scriptClass.GetProperties())
            {
                var propertyKey = $"{property.Name} ({property.PropertyType})";
                _keys.Add(propertyKey);
                _values.Add(propertyKey, "");
                _types.Add(propertyKey, property.PropertyType);
            }
        }
    }

    private void AssignPropertiesValues()
    {
        EditorGUIUtility.labelWidth = 250;
        foreach(var key in _keys)
        {
            _values[key] = EditorGUILayout.TextField(key, _values[key]);
        }
    }

    private void CreateItemButton()
    {
        if(GUILayout.Button("Create item", GUILayout.MinHeight(40)))
        {
            foreach(var key in _keys)
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
        var script          = _model as MonoScript;
        var type            = script.GetClass();
        var objectInstance  = System.Activator.CreateInstance(type);
        var finalObject     = System.Convert.ChangeType(objectInstance, type);
        var model           = (IModel)finalObject;

        string[] values = new string[_values.Count];
        int i = 0;

        foreach(var key in _keys)
        {
            values[i] = _values[key];
            i++;
        }

        model.SetValues(values);
        var jsonText = JsonMapper.ToJson(model);
        Debug.Log(jsonText);
    }
}
