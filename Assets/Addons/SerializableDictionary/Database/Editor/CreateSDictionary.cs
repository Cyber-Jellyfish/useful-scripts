#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Database
{
    public class CreateSDictionary : EditorWindow
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

        private string _className = String.Empty;

        private Types _keyType = Types.StringType;
        private string _key = "";
        private Types _valueType = Types.StringType;
        private string _value = "";

        private TextAsset _sDictionaryTemplate;
        private string _data = String.Empty;

        #endregion

        #region WINDOW METHODS

        public static CreateSDictionary _window;

        [MenuItem("Dictionary/Create/S-Dictionary", priority = 20)]
        public static void OpenWindow()
        {
            _window = GetWindow<CreateSDictionary>("Create S Dictionary");
            _window.Show();
            _window.maxSize = new Vector2(450, 200);
        }

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _sDictionaryTemplate = Resources.Load<TextAsset>("template_s_dictionary");
        }

        public void OnGUI()
        {
            EditorGUILayout.ObjectField("Template:", _sDictionaryTemplate, typeof(TextAsset), false);
            EditorGUILayout.Space();

            _className = EditorGUILayout.TextField("Class Name:", _className);
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

            if (GUILayout.Button("Create New SDictionary"))
            {
                if (string.IsNullOrEmpty(_className))
                {
                    Debug.LogError("Cannot have Empty Class Name!");
                    return;
                }

                _data = _sDictionaryTemplate.text.Replace("%S_DICTIONARY", _className);
                _data = _data.Replace("%KEY", _keyType     != Types.CustomType ? _types[_keyType] : _key);
                _data = _data.Replace("%VALUE", _valueType != Types.CustomType ? _types[_valueType] : _value);
                string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, _className, "cs");
                File.WriteAllText(path, _data);
                AssetDatabase.Refresh();
            }
        }

        #endregion

        #region METHODS

        #endregion
    }
}
#endif