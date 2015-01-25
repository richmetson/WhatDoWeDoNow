using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AgonyBartender
{
    public class ScoreDisplayController : MonoBehaviour
    {

        public Text ShiftCompleteText;
        public Text MoneyText;
        public Text PeopleServerdText;
        public Text BeerDispensedText;
        public Text LiverFailuresText;

        public Text GradeText;

        public Text ContinueButton;

        public void DisplayResults(GameSession Session)
        {
            ShiftCompleteText.text = "Shift " + Session.ShiftNumber + " complete!";

            MoneyText.text = "$" + (Session.TipsMade / 100.0f).ToString("f2");
            MoneyText.color = (Session.TipsMade < 0) ? Color.red : Color.black;
            PeopleServerdText.text = Session.PatronsServedThisShift.ToString();
            BeerDispensedText.text = string.Format("{0}ml", Mathf.RoundToInt(Session.BeerDispensedThisShift * 568.0f));
            LiverFailuresText.text = Session.PatronsPoisonedThisShift.ToString();

            GradeText.text = Session.GetGrade(Session.TipsMade);

            GetComponent<Animator>().Play("DoShiftComplete");
        }

        public void DisplayFinalResults(GameSession Session)
        {
            ShiftCompleteText.text = "Game Over";

            MoneyText.text = "$" + (Session.TotalTipsMade / 100.0f).ToString("f2");
            MoneyText.color = (Session.TipsMade < 0) ? Color.red : Color.black;
            PeopleServerdText.text = Session.TotalPatronsServed.ToString();
            BeerDispensedText.text = string.Format("{0}ml", Mathf.RoundToInt(Session.TotalBeerDispensed * 568.0f));
            LiverFailuresText.text = Session.TotalPatronsPoisoned.ToString();

            GradeText.text = Session.GetGrade(Session.TipsMade);

            ContinueButton.text = "End Game";

            GetComponent<Animator>().Play("DoShiftComplete");
        }
    }
}