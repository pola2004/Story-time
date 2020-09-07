using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public bool debug;
    public AudioTrack[] tracks;

    private Hashtable audioTable;
    private Hashtable jobTable;

    [System.Serializable]
    public class AudioTrack
    {
        public AudioSource source;
        public AudioObject[] audio;

    }

    [System.Serializable]
    public class AudioObject
    {
        public AudioType type;
        public AudioClip clip;

    }

    private class AudioJob
    {
        public AudioAction action;
        public AudioType type;

        public AudioJob(AudioAction action, AudioType type)
        {
            this.action = action;
            this.type = type;
        }
    }
    private enum AudioAction
    {
        START,
        STOP,
        RESTART
    }

    #region Unity Methods

    private void Awake()
    {
        if (!instance)
            Configrue();
        DontDestroyOnLoad(gameObject);
    }
    private void OnDisable()
    {
        Dispose();
    }

    #endregion

    #region Public Methods

    public void PlayAudio(AudioType type)
    {
        AddJob(new AudioJob(AudioAction.START, type));
    }

    public void StopAudio(AudioType type)
    {
        AddJob(new AudioJob(AudioAction.STOP, type));
    }

    public void RestartAudio(AudioType type)
    {
        AddJob(new AudioJob(AudioAction.RESTART, type));
    }

    #endregion

    #region Private Methods

    private void Configrue()
    {
        instance = this;
        audioTable = new Hashtable();
        jobTable = new Hashtable();
        GenrateAudioTable();
    }

    private void Dispose()
    {
        foreach (DictionaryEntry entry in jobTable)
        {
            IEnumerator job = (IEnumerator) entry.Value;
            StopCoroutine(job);
        }
    }

    private void GenrateAudioTable()
    {
        foreach (AudioTrack track in tracks)
        {
            foreach (AudioObject obj in track.audio)
            {

                if (audioTable.ContainsKey(obj.type))
                {
                    LogWarning("You are trying to register audio [" + obj.type + "] that has already been registered");
                }
                else
                {
                    audioTable.Add(obj.type, track);
                    Log("Registering audio [" + obj.type + "].");
                }
            }

        }
    }

    private IEnumerator RunAudioJob(AudioJob job)
    {
        AudioTrack track = (AudioTrack) audioTable[job.type];
        track.source.clip = GetAudioClipFromAudioTrack(job.type, track);

        switch (job.action)
        {
            case AudioAction.START:
                track.source.Play();
                break;

            case AudioAction.STOP:
                track.source.Stop();
                break;

            case AudioAction.RESTART:
                track.source.Stop();
                track.source.Play();
                break;
        }
        jobTable.Remove(job.type);
        Log("Job Count: " + jobTable.Count);
        yield return null;
    }

    private void AddJob(AudioJob job)
    {
        //Remove the conflict jobs
        RemoveConflictingJobs(job.type);

        //start  job
        IEnumerator jobRunner = RunAudioJob(job);
        jobTable.Add(job.type, jobRunner);

        StartCoroutine(jobRunner);
        Log("Starting job on [" + job.type + "] with operation " + job.action);

    }

    private void RemoveJob(AudioType type)
    {
        if (!jobTable.ContainsKey(type))
        {
            LogWarning("You are trying to stop a job [" + type + "] that is not running");
            return;
        }

        IEnumerator runningJob = (IEnumerator) jobTable[type];
        StopCoroutine(runningJob);
        jobTable.Remove(type);
    }

    private void RemoveConflictingJobs(AudioType type)
    {
        if (jobTable.ContainsKey(type))
            RemoveJob(type);

        AudioType conflictAudio = AudioType.None;
        foreach (DictionaryEntry entry in jobTable)
        {
            AudioType audioType = (AudioType) entry.Key;
            AudioTrack audioTrackInUse = (AudioTrack) audioTable[audioType];
            AudioTrack audioTrackNeeded = (AudioTrack) audioTable[type];
            if (audioTrackNeeded.source == audioTrackInUse.source)
                conflictAudio = audioType;
        }
        if (conflictAudio != AudioType.None)
            RemoveJob(type);
    }

    private AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track)
    {
        foreach (AudioObject obj in track.audio)
        {
            if (obj.type == type)
                return obj.clip;

        }
        return null;
    }
    private void Log(string message)
    {
        if (!debug)
            return;
        Debug.Log("[Audio Controller]: " + message);
    }

    private void LogWarning(string message)
    {
        if (!debug)
            return;
        Debug.LogWarning("[Audio Controller]: " + message);
    }

    #endregion

}