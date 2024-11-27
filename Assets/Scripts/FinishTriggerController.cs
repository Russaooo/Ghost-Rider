using UnityEngine;
// Скрипт обрабатывающий приход к финишу
public class FinishTriggerController : MonoBehaviour
{
    [SerializeField]
    private Main main;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.name == main.currentCar.name)
        {
            // меняем current state, далее в Main.GameStatesController() появится соответствующая ситуации менюшка
            main.currentState = Main.GameStates.LevelFinished;
            Debug.Log("Level Finished");
        }
    }

    private void OnValidate()
    {
        if (main!=null && GameObject.Find("Main - DO NOT DELETE"))
        {
            GameObject.Find("Main - DO NOT DELETE").TryGetComponent<Main>(out main);
        }
    }
}
