#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Database
{
    
    public class CreateCDatabase : EditorWindow
    {
        #region VARIABLES

        private readonly Dictionary<Types, string> _types = new Dictionary<Types, string>
        {
            {Types.StringType, "string"},
            {Types.CharType, "char"},
            {Types.Int32Type, "int"},
            {Types.DoubleType, "double"},
            {Types.FloatType, "float"},
            {Types.BoolType, "bool"},
            {Types.GameObjectType, "GameObject"},
            {Types.TransformType, "Transform"},
        };

        private string _cClassName = String.Empty;
        private string _sClassName = String.Empty;

        private Types _keyType = Types.StringType;
        private string _key = "";
        private Types _valueType = Types.StringType;
        private string _value = "";

        private TextAsset _cDatabaseTemplate;
        private string _data = String.Empty;

        #endregion

        #region WINDOW METHODS

        public static CreateCDatabase _window;

        [MenuItem("Dictionary/Create/C-Database", priority = 20)]
        public static void OpenWindow()
        {
            _window = GetWindow<CreateCDatabase>("Create C Database");
            _window.Show();
            _window.minSize = new Vector2(450, 200);
        }

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _cDatabaseTemplate = Resources.Load<TextAsset>("template_c_database");
        }

        public void OnGUI()
        {
            EditorGUILayout.ObjectField("Template:", _cDatabaseTemplate, typeof(TextAsset), false);
            EditorGUILayout.Space();

            _cClassName = EditorGUILayout.TextField("C Database Class Name:", _cClassName);
            _sClassName = EditorGUILayout.TextField("S Dictionary Class Name:", _sClassName);

            EditorGUILayout.Space();

            _keyType = (Types) EditorGUILayout.EnumPopup("Select Key Type", _keyType);
            _valueType = (Types) EditorGUILayout.EnumPopup("Select Value Type", _valueType);

            if (_keyType == Types.CustomType)
            {
                _key = EditorGUILayout.TextField("Custom Key Type", _key);
            }

            if (_valueType == Types.CustomType)
            {
                _value = EditorGUILayout.TextField("Custom Value Type", _value);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Create New SODatabase"))
            {
                if (string.IsNullOrEmpty(_cClassName) || string.IsNullOrEmpty(_sClassName))
                {
                    Debug.LogError("Cannot have Empty Class Name!");
                    return;
                }

                _data = _cDatabaseTemplate.text.Replace("%C_DATABASE", _cClassName);
                _data = _data.Replace("%S_DICTIONARY", _sClassName);
                _data = _data.Replace("%KEY", _keyType     != Types.CustomType ? _types[_keyType] : _key);
                _data = _data.Replace("%VALUE", _valueType != Types.CustomType ? _types[_valueType] : _value);

                string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, _cClassName, "cs");
                File.WriteAllText(path, _data);
                AssetDatabase.Refresh();
            }

            #endregion
        }
    }
}
#endif