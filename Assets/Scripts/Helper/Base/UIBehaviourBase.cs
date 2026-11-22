using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Reflection;

namespace Helper.Base
{
    public class UIBehaviourBase: MonoBehaviour
    {
        public VisualElement Root;
        
        public void InitializeComponent()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            InitializeFields();
            InitializeClick();
        }

        private void InitializeFields()
        {
            var fields = GetType().GetFields(
                BindingFlags.NonPublic | 
                BindingFlags.Public |
                BindingFlags.Instance
            );// Can be public or not. Cannot be static

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<BindElement>();
                if (attr is null) continue;
                if (field.IsInitOnly)
                {
                    Debug.LogError($"Field {field.Name} is readonly. Cannot initialize field.");
                    continue;
                }
                var elementName = attr.ElementName ?? field.Name;
                var element = Root.Q(elementName);
                if (element is null)
                {
                    Debug.LogError($"Cannot find element {elementName}.");
                    continue;
                }

                if (!field.FieldType.IsInstanceOfType(element))
                {
                    Debug.LogError($"Element {elementName} is not an instance of {field.FieldType}.");
                    continue;
                }
                
                field.SetValue(this, element);
            }
        }

        private void InitializeClick()
        {
            var methods = GetType().GetMethods(
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );// Can be public or not. Can be static or not

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<BindClick>();
                if (attr is null) continue;
                var button = Root.Q<Button>(attr.BindingButton);
                var action = (Action)Delegate.CreateDelegate(
                    typeof(Action),
                    method.IsStatic ? null : this,
                    method
                );
                button.clicked += action;
            }
        }
    }
}