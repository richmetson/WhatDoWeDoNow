using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AgonyBartender
{
    public enum ButtonSide
    {
        LeftButton,
        RightButton
    }

    [RequireComponent(typeof(Button))]
    public class ScrollingButtonManager : MonoBehaviour
    {
        public ButtonSide ButtonSide;
        public BarManager BarManager;
        // Use this for initialization
        void Start()
        {
            // Set the buttons interactable to be the correct value to start with
            ButtonPressed();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ButtonPressed()
        {
            switch (ButtonSide)
            {
                case ButtonSide.LeftButton:
                    gameObject.GetComponent<Button>().interactable = BarManager.CanMoveLeft();
                    break;
                case ButtonSide.RightButton:
                    gameObject.GetComponent<Button>().interactable = BarManager.CanMoveRight();
                    break;
                default:
                    break;
            }
        }
    }
}
