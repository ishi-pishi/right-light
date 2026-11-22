using System;
using UnityEngine.Scripting;

namespace Helper
{
    [AttributeUsage(AttributeTargets.Method)]
    // Only methods
    public class BindClick: PreserveAttribute
    {
        public readonly string BindingButton;
        
        public BindClick(string bindingButton)
        {
            BindingButton = bindingButton;
        }
    }
}