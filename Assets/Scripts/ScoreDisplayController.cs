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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayResults(GameSession Session)
    {
        ShiftCompleteText.text = "Shift " + Session.ShiftNumber + " complete!";

        MoneyText.text = Session.TipsMade.ToString("C");
        PeopleServerdText.text = Session.PatronsServedThisShift.ToString();
        BeerDispensedText.text = Mathf.RoundToInt(Session.BeerDispensedThisShift * 568.0f).ToString() + "ml";
        LiverFailuresText.text = Session.TotalPatronsPoisoned.ToString();

        // TODO
        GradeText.text = "A+";
    }
}
}