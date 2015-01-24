using UnityEngine;
using System.Collections;

namespace AgonyBartender.Frontend
{

    public class MainMenuController : MonoBehaviour
    {
        public void StartGame()
        {
            Application.LoadLevel("gamescene");
        }

        public void Credits()
        {
            GetComponent<Animator>().Play("ShowCredits");
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void ExitCredits()
        {
            GetComponent<Animator>().Play("ExitCredits");
        }
    }

}