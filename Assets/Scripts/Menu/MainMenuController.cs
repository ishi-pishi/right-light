using Helper;
using Helper.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public partial class MainMenuController : UIBehaviourBase
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InitializeComponent();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        [BindClick("Start_Btn")]
        void StartBtn_Click()
        {
            Debug.Log("StartBtn_Click Invoked");
            
        }

        [BindClick("Exit_Btn")]
        void ExitBtn_Click()
        {
            Debug.Log("ExitBtn_Click Invoked");
            Application.Quit();
        }
    }
}
