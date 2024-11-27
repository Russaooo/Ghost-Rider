using Ashsvp;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class Main : MonoBehaviour
{
    [InspectorLabel("Машина игрока"), SerializeField]
    public GameObject currentCar;
    [InspectorLabel("Rigidbody машины игрока"), SerializeField]
    private Rigidbody currentCarRB;
    // для хранения параметров скорости во время паузы
    private Vector3 carVelocity;
    [InspectorLabel("Контроллер триггера финиша"), SerializeField]
    private FinishTriggerController finishTriggerController;
    
    // Призрачный гонщик
    private GameObject ghostRacer;
    private Rigidbody ghostRacerRB;
    private SimcadeVehicleController ghostRacerController;
    [InspectorLabel("Скорость призрачного гонщика"), SerializeField]
    private float ghostRacerVelocity = 20f;
    private Material ghostMaterial;
    
    [InspectorLabel("Слой UI"), SerializeField]
    public PanelSelector uiPanelSelector;
    // ссылка на экземпляр класса призрачного гонщика с хранимыми в нём координатами движения
    
    [InspectorLabel("Регенерировать ссылки"), SerializeField]
    public bool regenerateLinks;
    
    private SimcadeVehicleController vehicleController;
    // Список возможных состояний игры
    public enum GameStates
    {
        MenuOpened, // открыто меню
        LevelScreen, // открыт экран выбора уровней
        RaceBeforeStart, // предстартовая подготовка (ожидание игрока начать гонку)
        Racing, // сам процесс гонки
        GamePaused, // игрок поставил игру на паузу
        LevelFinished // уровень завершён
    }

    // хранит текущее состояние игры
    public GameStates currentState = GameStates.MenuOpened;
    
    // эвент для апдейта информации о номере заезда на этом треке
    public UnityEvent raceRestarted;

    // заморозка машинки игрока, используется для полной остановки на экране паузы и в предстартовом состоянии
    public void FreezeCarForFrame()
    {
        vehicleController.CanDrive = false;
        vehicleController.CanAccelerate = false;
        carVelocity = currentCarRB.velocity;
        currentCarRB.Sleep();
    }
    // разморозка машинки игрока
    public void UnFreezeCar()
    {
        vehicleController.CanDrive = true;
        vehicleController.CanAccelerate = true;
        currentCarRB.isKinematic = false;
        currentCarRB.velocity = carVelocity;
    }
    // контроллер состояний, обрабатывает текущее состояние игры currentState
    public void GameStatesController()
    {
        switch (currentState)
        {
            case GameStates.MenuOpened:
                break;
            case GameStates.LevelScreen:
                break;
            case GameStates.RaceBeforeStart:
                if (uiPanelSelector.startPreparationPanel== false)
                {
                    uiPanelSelector.startPreparationPanel.SetActive(true);
                }
                break;
            case GameStates.Racing:
                uiPanelSelector.racingPanel.SetActive(true);
                UnFreezeCar();
                break;
            case GameStates.GamePaused:
                uiPanelSelector.pausePanel.SetActive(true);
                FreezeCarForFrame();
                break;
            case GameStates.LevelFinished:
                uiPanelSelector.finishedPanel.SetActive(true);
                FreezeCarForFrame();
                break;
            default:
                break;
        }
    }
    private void Start()
    {
        currentState = GameStates.RaceBeforeStart;
        vehicleController = currentCar.GetComponent<SimcadeVehicleController>();
        currentCarRB = currentCar.GetComponent<Rigidbody>();
        // сбрасываем старый набор координат с предыдущего заезда
        GhostRiderCoordinates.DropPreviousCoordinates();
        ghostMaterial = Resources.Load<Material>("Materials/Ghost Material");
        CrossSceneData.RaceNumberLevel1 += 1;
        raceRestarted.Invoke();
    }

    private void Update()
    {
        if (currentState == GameStates.RaceBeforeStart)
        {
            if (GhostRiderCoordinates.OldPlayerPath.Count > 0 && !GameObject.Find("Ghost Racer"))
            {
                // При не-первом заезде генерим и настраиваем призрачного гонщика
                CoordinateAndRotation coords = GhostRiderCoordinates.OldPlayerPath.First.Value;
                ghostRacer = Instantiate(currentCar.gameObject, coords.Coordinate, coords.Rotation);
                ghostRacerRB = ghostRacer.GetComponent<Rigidbody>();
                ghostRacer.name = "Ghost Racer";
                ghostRacer.layer = 6;
                ghostRacer.transform.Find("Body Collider").gameObject.layer = 6;
                ghostRacer.GetComponent<SimcadeVehicleController>().CanDrive = false;
                ghostRacer.GetComponent<SimcadeVehicleController>().CanAccelerate = true;
                ghostRacer.GetComponent<SimcadeVehicleController>().MaxSpeed = 20;
                ghostRacer.GetComponent<GearSystem>().enabled = false;
                ghostRacer.GetComponent<ResetVehicle>().enabled = false;
                ghostRacer.GetComponent<AudioSystem>().enabled = false;
                ghostRacerRB.isKinematic = true;
                ghostRacerController = ghostRacer.GetComponent<SimcadeVehicleController>();
                ghostRacerController.enabled = true;
                Destroy(ghostRacer.transform.Find("CM vcam1").gameObject);
                GhostRiderCoordinates.inRacePrevPos = GhostRiderCoordinates.OldPlayerPath.First.Value.Coordinate;
                GhostRiderCoordinates.OldPlayerPath.RemoveFirst();
                GhostRiderCoordinates.wheelFL = ghostRacer.transform.Find("Wheels").Find("wheel FL");
                GhostRiderCoordinates.wheelFR = ghostRacer.transform.Find("Wheels").Find("wheel FR");
                var bodyObjects = ghostRacer.transform.Find("Body Mesh").GetChild(0).childCount;
                // замена материалов на мешах
                for (int i = 0; i < bodyObjects; i++)
                {
                    var bodyElement = ghostRacer.transform.Find("Body Mesh").GetChild(0).GetChild(i);
                    MeshRenderer bodyElementMeshRenderer = bodyElement.GetComponent<MeshRenderer>();
                    var materials = bodyElementMeshRenderer.materials;
                    for (int j = 0; j < materials.Length; j++)
                    {
                        materials[j] = ghostMaterial;
                        materials[j].shader = ghostMaterial.shader;
                    }

                    bodyElementMeshRenderer.materials = materials;
                }

                
            }
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                currentState = GameStates.Racing;
                GameStatesController();
            }
        }

        if (currentState == GameStates.Racing)
        {
            GhostRiderCoordinates.WriteCoordinates(currentCarRB.position,currentCar.transform.rotation);

            
            
            if (Keyboard.current.escapeKey.isPressed)
            {
                currentState = GameStates.GamePaused;
                GameStatesController();
            }
        }

        if (currentState == GameStates.GamePaused)
        {
            if (Keyboard.current.wKey.isPressed)
            {
                currentState = GameStates.Racing;
                GameStatesController();
            }
        }

        if (currentState == GameStates.LevelFinished)
        {
            GhostRiderCoordinates.WriteCoordinates(currentCarRB.position,currentCar.transform.rotation);
            GameStatesController();
            
        }
    }

    private void FixedUpdate()
    {
        void RotateWheelX(Vector3 nextPos,float multiplier)
        {
            var angle = Vector3.Angle(GhostRiderCoordinates.inRacePrevPos, nextPos);
            var angleX = GhostRiderCoordinates.wheelFL.rotation.eulerAngles.x;
            var angleY = GhostRiderCoordinates.wheelFL.rotation.eulerAngles.y;
            var angleZ = GhostRiderCoordinates.wheelFL.rotation.eulerAngles.z;

            GhostRiderCoordinates.wheelFL.rotation = Quaternion.Euler(angleX,angleY+multiplier*angle,angleZ);
            GhostRiderCoordinates.wheelFR.rotation = Quaternion.Euler(angleX,angleY+multiplier*angle,angleZ);
        }

        if (currentState == GameStates.Racing){
            // При не-первом заезде на сцене присутствует призрачный гонщик, эта часть отвечает за его движение и повороты
            // движение идёт по вейпоинтам
            if (ghostRacer)
            {
                if (GhostRiderCoordinates.OldPlayerPath.Count > 1)
                {
                    {
                        if ( (GhostRiderCoordinates.OldPlayerPath.First.Value.Coordinate != ghostRacerRB.position)/*.magnitude>0.5f*/ )
                        {
                            var position = ghostRacerRB.position;
                            var pos = Vector3.MoveTowards(position,GhostRiderCoordinates.OldPlayerPath.First.Value.Coordinate,Time.fixedDeltaTime*ghostRacerVelocity);
                            var rot = Quaternion.RotateTowards(ghostRacerRB.rotation,GhostRiderCoordinates.OldPlayerPath.First.Value.Rotation, Time.fixedDeltaTime*ghostRacerVelocity*3);
                            ghostRacerRB.Move(pos,rot);
                            
                            if (GhostRiderCoordinates.inRacePrevPos.x < pos.x)
                            {
                                RotateWheelX(pos,-1);
                            }
                            else
                            {
                                RotateWheelX(pos,1);
                            }
                            GhostRiderCoordinates.inRacePrevPos = position;
                            // когда призрачный гонщик добирается в очередной вейпоинт, удаляем информацию о нём из старого пути
                            if (GhostRiderCoordinates.OldPlayerPath.First.Value.Coordinate == pos)
                            {
                                GhostRiderCoordinates.OldPlayerPath.RemoveFirst();
                                FixedUpdate();
                            }
                        }
                    }
                }
            }
        }
    }

    // в OnValidate выполняем автоматическое получение ссылок на объекты,
    // при нажатии на поле regenerate links в инспекторе выполняется принудительный вызов onValidate()
    private void RegenerateLinks()
    {
        if (currentCar == null && GameObject.Find("Player Car"))
            {
                currentCar = GameObject.Find("Player Car");
            }

            if (currentCarRB == null && GameObject.Find("Player Car"))
            {
                currentCarRB = GameObject.Find("Player Car").GetComponent<Rigidbody>();
            }
            if (finishTriggerController == null && GameObject.Find("Finish Trigger"))
            {
                GameObject.Find("Finish Trigger").TryGetComponent(out finishTriggerController);
            }
        }
    private void Awake()
    {
        // регенерация ссылкок, если что-то было закосячено
        RegenerateLinks();
    }

    private void OnValidate()
    {
        // принудительная регенерация ссылок в инспекторе
        if (regenerateLinks)
        {
            
            RegenerateLinks();
            regenerateLinks = false;
        }
    }
}
