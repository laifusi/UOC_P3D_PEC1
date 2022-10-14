using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class GhostManager : MonoBehaviour
{
    public float timeBetweenSamples = 0.25f;
    public LapData bestLapSO;               // Ghost Data of the Best Lap
    public LapData currentLapSO;            // Ghost Data of the Current Lap
    public GameObject carToRecord;
    public GameObject carToPlay;

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
    }

    #region RECORD GHOST DATA
    void StartRecording(bool isFirstLap)
    {
        shouldRecord = true;
        //shouldPlay = false;

        // Seteamos los valores iniciales
        totalRecordedTime = 0;
        currentTimeBetweenSamples = 0;

        // Limpiamos el scriptable object
        currentLapSO.Reset();
    }

    void StopRecording()
    {
        shouldRecord = false;
    }
    #endregion

    #region PLAY GHOST DATA
    void StartPlaying(bool isFirstLap)
    {
        if (isFirstLap)
            return;

        shouldPlay = true;
        //shouldRecord = false;

        // Seteamos los valores iniciales
        totalPlayedTime = 0;
        currentSampleToPlay = 0;
        currentTimeBetweenPlaySamples = 0;

        carToPlay.SetActive(true);

        // Desactivamos el control del coche
        /*carToPlay.GetComponent<CarController>().enabled = false;
        carToPlay.GetComponent<CarUserControl>().enabled = false;*/
    }

    void StopPlaying()
    {
        shouldPlay = false;

        carToPlay.SetActive(false);

        // Devolvemos el control al coche por si fuera necesario (opcional)
        //carToPlay.GetComponent<CarController>().enabled = true;
        //carToPlay.GetComponent<CarUserControl>().enabled = true;

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
            UpdateGhostTransform();
        }
    }

    private void UpdateGhostTransform()
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

            if(currentSampleToPlay < bestLapSO.GetNumberOfSamples())
            {
                // Cogemos los datos del scriptable object
                bestLapSO.GetDataAt(currentSampleToPlay, out nextPosition, out nextRotation);
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
        carToPlay.transform.position = Vector3.Slerp(lastSamplePosition, nextPosition, percentageBetweenFrames);
        carToPlay.transform.rotation = Quaternion.Slerp(lastSampleRotation, nextRotation, percentageBetweenFrames);
    }

    private void RecordCurrentData()
    {
        // A cada frame incrementamos el tiempo transcurrido 
        totalRecordedTime += Time.deltaTime;
        currentTimeBetweenSamples += Time.deltaTime;

        // Si el tiempo transcurrido es mayor que el tiempo de muestreo
        if (currentTimeBetweenSamples >= timeBetweenSamples)
        {
            // Guardamos la información para el fantasma
            currentLapSO.AddNewData(carToRecord.transform);
            // Dejamos el tiempo extra entre una muestra y otra
            currentTimeBetweenSamples -= timeBetweenSamples;
            recordedSamples++;
        }
    }

    void HandleTestActionInputs()
    {
        // START/STOP RECORDING
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (shouldRecord)
                StopRecording();
            else
                StartRecording(true);
        }

        // PLAY RECORDED LAP
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (shouldPlay)
                StopPlaying();
            else
                StartPlaying(true);
        }

        // RESET
        if (Input.GetKeyDown(KeyCode.Delete))
            bestLapSO.Reset();
    }


    public void UpdateBestLapSO()
    {
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
    }

    #if UNITY_EDITOR
    [ContextMenu("Clean ScriptableObjects")]
    public void ResetSOs()
    {
        bestLapSO.Reset();
        currentLapSO.Reset();
    }
    #endif
}
