using UnityEngine;
using System.Collections;


namespace AgonyBartender
{
    public class PatronDefinition : MonoBehaviour
    {
        public Patron Patron;

        Problem ActiveProblem;

        public Problem GetActiveProblem()
        {
            if (ActiveProblem == null)
            {
                ActiveProblem = Patron.SelectProblem();
            }

            return ActiveProblem;
        }
    }

}
