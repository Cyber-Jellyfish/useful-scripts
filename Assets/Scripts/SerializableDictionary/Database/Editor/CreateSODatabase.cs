#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Database
{
    public class CreateSODatabase : EditorWindow
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

        private string _soClassName = String.Empty;
        private string _sClassName = String.Empty;

        private Types _keyType = Types.StringType;
        private string _key = "";
        private Types _valueType = Types.StringType;
        private string _value = "";

        private TextAsset _soDatabaseTemplate;
        private string _data = String.Empty;

        #endregion

        #region WINDOW METHODS

        public static CreateSODatabase _window;

        [MenuItem("Dictionary/Create/SO-Database", priority = 20)]
        public static void OpenWindow()
        {
            _window = GetWindow<CreateSODatabase>("Create SO Database");
            _window.Show();
            _window.minSize = new Vector2(450, 200);
        }

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _soDatabaseTemplate = Resources.Load<TextAsset>("template_so_database");
        }

        public void OnGUI()
        {
            EditorGUILayout.ObjectField("Template:", _soDatabaseTemplate, typeof(TextAsset), false);
            EditorGUILayout.Space();

            _soClassName = EditorGUILayout.TextField("SO Database Class Name:", _soClassName);
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
                if (string.IsNullOrEmpty(_soClassName) || string.IsNullOrEmpty(_sClassName))
                {
                    Debug.LogError("Cannot have Empty Class Name!");
                    return;
                }

                _data = _soDatabaseTemplate.text.Replace("%NAME", _soClassName);
                _data = _data.Replace("%S_DICTIONARY", _sClassName);
                _data = _data.Replace("%KEY", _keyType     != Types.CustomType ? _types[_keyType] : _key);
                _data = _data.Replace("%VALUE", _valueType != Types.CustomType ? _types[_valueType] : _value);

                string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, _soClassName, "cs");
                File.WriteAllText(path, _data);
                AssetDatabase.Refresh();
            }

            #endregion
        }
    }
}
#endif