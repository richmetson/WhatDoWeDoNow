using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AgonyBartender
{
public class ScoreDisplayController : MonoBehaviour {

    public Text ShiftCompleteText;
    public Text MoneyText;
    public Text PeopleServerdText;
    public Text BeerDispensedText;
    public Text LiverFailuresText;

    public Text GradeText;

    public void DisplayResults(GameSession Session)
    {
        ShiftCompleteText.text = "Shift " + Session.ShiftNumber + " complete!";

        MoneyText.text = Session.TipsMade.ToString("C");
        PeopleServerdText.text = Session.PatronsServedThisShift.ToString();
        BeerDispensedText.text = string.Format("{0}ml", Mathf.RoundToInt(Session.BeerDispensedThisShift * 568.0f));
        LiverFailuresText.text = Session.PatronsPoisonedThisShift.ToString();

        // TODO
        GradeText.text = "A+";

        GetComponent<Animator>().Play("DoShiftComplete");
    }

    public void DisplayFinalResults(GameSession Session)
    {
        ShiftCompleteText.text = "Game Over";

        MoneyText.text = Session.TotalTipsMade.ToString("C");
        PeopleServerdText.text = Session.TotalPatronsServed.ToString();
        BeerDispensedText.text = string.Format("{0}ml", Mathf.RoundToInt(Session.TotalBeerDispensed * 568.0f));
        LiverFailuresText.text = Session.TotalPatronsPoisoned.ToString();

        // TODO
        GradeText.text = "A+";

        GetComponent<Animator>().Play("DoShiftComplete");
    }
}
}