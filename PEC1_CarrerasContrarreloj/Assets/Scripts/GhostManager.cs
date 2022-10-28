using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class GhostManager : MonoBehaviour
{
    public float timeBetweenSamples = 0.25f;
    public LapData bestLapSO;               // Ghost Data of the Best Lap
    public LapData currentLapSO;            // Ghost Data of the Current Lap
    public LapData lastRaceSO;           // Ghost Data of the Current Race
    public GameObject playableCar;      // GameObject of the car to record
    public GameObject ghostCar;        // GameObject of the ghost

    // RECORD VARIABLES
    private bool shouldRecord = false;
    private float totalRecordedTime = 0.0f;
    private float currentTimeBetweenSamples = 0.0f;
    private int recordedSamples = 0;

    // REPLAY VARIABLES
    private bool shouldPlay = false;
    private float totalPlayedTime = 0.0f;
    private float currentTimeBetweenPlaySamples = 0.0f;
    private int currentSampleToPlay = 0;
    private LapData lapDataToReplay;
    private GameObject carToReplay;

    // POSITIONS/ROTATIONS
    private Vector3 lastSamplePosition = Vector3.zero;
    private Quaternion lastSampleRotation = Quaternion.identity;
    private Vector3 nextPosition;
    private Quaternion nextRotation;

    private void Start()
    {
        LapLine.OnNewLap += StartPlaying;
        LapLine.OnNewLap += StartRecording;
        LapLine.OnNewBestLap += UpdateBestLapSO;
        LevelManager.OnShowRepetition += StartRepetition;
        LevelManager.OnEndRepetition += StopRepetition;

        playableCar = LevelManager.Instance.GetCar();
        ghostCar = LevelManager.Instance.GetGhost();
    }

    #region RECORD GHOST DATA
    void StartRecording(bool isFirstLap, bool isLastLap)
    {
        shouldRecord = !isLastLap;
        //shouldPlay = false;

        // Seteamos los valores iniciales
        totalRecordedTime = 0;
        currentTimeBetweenSamples = 0;
        recordedSamples = 0;

        // Limpiamos el scriptable object
        if (!isLastLap)
            currentLapSO.Reset();
        if (isFirstLap)
            lastRaceSO.Reset();
    }

    void StopRecording()
    {
        shouldRecord = false;
    }
    #endregion

    #region PLAY GHOST DATA
    void StartPlaying(bool isFirstLap, bool isLastLap)
    {
        if (isFirstLap)
            return;

        shouldPlay = !isLastLap;
        //shouldRecord = false;

        // Seteamos los valores iniciales
        totalPlayedTime = 0;
        currentSampleToPlay = 0;
        currentTimeBetweenPlaySamples = 0;

        ghostCar.SetActive(true);

        lapDataToReplay = bestLapSO;
        carToReplay = ghostCar;

        // Desactivamos el control del coche
        /*carToPlay.GetComponent<CarController>().enabled = false;
        carToPlay.GetComponent<CarUserControl>().enabled = false;*/
    }

    void StopPlaying()
    {
        shouldPlay = false;

        ghostCar.SetActive(false);

        // Devolvemos el control al coche por si fuera necesario (opcional)
        //carToPlay.GetComponent<CarController>().enabled = true;
        //carToPlay.GetComponent<CarUserControl>().enabled = true;

    }
    #endregion

    #region PLAY REPETITION DATA
    void StartRepetition()
    {
        shouldPlay = true;

        // Seteamos los valores iniciales
        totalPlayedTime = 0;
        currentSampleToPlay = 0;
        currentTimeBetweenPlaySamples = 0;

        lapDataToReplay = lastRaceSO;
        carToReplay = playableCar;

        // Desactivamos el control y los colliders del coche
        playableCar.GetComponent<CarController>().enabled = false;
        playableCar.GetComponent<CarUserControl>().enabled = false;
        playableCar.GetComponent<Rigidbody>().isKinematic = true;
        Collider[] colliders = playableCar.GetComponentsInChildren<Collider>();
        foreach(Collider coll in colliders)
        {
            coll.enabled = false;
        }
    }

    void StopRepetition()
    {
        shouldPlay = false;
    }
    #endregion

    private void Update()
    {
        if (shouldRecord)
        {
            RecordCurrentData();
        }

        if (shouldPlay)
        {
            UpdatePlayCarTransform();
        }
    }

    private void UpdatePlayCarTransform()
    {
        // A cada frame incrementamos el tiempo transcurrido 
        totalPlayedTime += Time.deltaTime;
        currentTimeBetweenPlaySamples += Time.deltaTime;

        // Si el tiempo transcurrido es mayor que el tiempo de muestreo
        if (currentTimeBetweenPlaySamples >= timeBetweenSamples)
        {
            // De cara a interpolar de una manera fluida la posición del coche entre una muestra y otra,
            // guardamos la posición y la rotación de la anterior muestra
            lastSamplePosition = nextPosition;
            lastSampleRotation = nextRotation;

            // Si se acaban los datos guardados, dejamos de reproducir
            if(currentSampleToPlay < lapDataToReplay.GetNumberOfSamples())
            {
                // Cogemos los datos del scriptable object
                lapDataToReplay.GetDataAt(currentSampleToPlay, out nextPosition, out nextRotation);
            }
            else
            {
                StopPlaying();
                return;
            }

            // Dejamos el tiempo extra entre una muestra y otra
            currentTimeBetweenPlaySamples -= timeBetweenSamples;

            // Incrementamos el contador de muestras
            currentSampleToPlay++;
        }

        // De cara a crear una interpolación suave entre la posición y rotación entre una muestra y la otra, 
        // calculamos a nivel de tiempo entre muestras el porcentaje en el que nos encontramos
        float percentageBetweenFrames = currentTimeBetweenPlaySamples / timeBetweenSamples;

        // Aplicamos un lerp entre las posiciones y rotaciones de la muestra anterior y la siguiente según el procentaje actual.
        carToReplay.transform.position = Vector3.Slerp(lastSamplePosition, nextPosition, percentageBetweenFrames);
        carToReplay.transform.rotation = Quaternion.Slerp(lastSampleRotation, nextRotation, percentageBetweenFrames);
    }

    private void RecordCurrentData()
    {
        // A cada frame incrementamos el tiempo transcurrido 
        totalRecordedTime += Time.deltaTime;
        currentTimeBetweenSamples += Time.deltaTime;

        // Si el tiempo transcurrido es mayor que el tiempo de muestreo
        if (currentTimeBetweenSamples >= timeBetweenSamples)
        {
            // Guardamos la información para el fantasma y la repetición
            currentLapSO.AddNewData(playableCar.transform);
            lastRaceSO.AddNewData(playableCar.transform);
            // Dejamos el tiempo extra entre una muestra y otra
            currentTimeBetweenSamples -= timeBetweenSamples;
            recordedSamples++;
        }
    }

    /// <summary>
    /// Method activated by the New Best Lap event that saves the last lap's data into the best lap data SO
    /// </summary>
    public void UpdateBestLapSO()
    {
        bestLapSO.Reset();
        Vector3 position;
        Quaternion rotation;
        for (int i = 0; i < recordedSamples; i++)
        {
            currentLapSO.GetDataAt(i, out position, out rotation);
            bestLapSO.AddNewData(position, rotation);
        }
    }

    private void OnDestroy()
    {
        LapLine.OnNewLap -= StartPlaying;
        LapLine.OnNewLap -= StartRecording;
        LapLine.OnNewBestLap -= UpdateBestLapSO;
        LevelManager.OnShowRepetition -= StartRepetition;
        LevelManager.OnEndRepetition -= StopRepetition;
    }

    #if UNITY_EDITOR
    [ContextMenu("Clean ScriptableObjects")]
    public void ResetSOs()
    {
        bestLapSO.Reset();
        currentLapSO.Reset();
        lastRaceSO.Reset();
    }
    #endif
}
