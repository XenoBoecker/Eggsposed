using UnityEngine;

public static class AudioLoudnessDetection
{
    private static int sampleWindow = 64;

    static AudioClip micClip;

    static void MicrophoneToAudioClip()
    {
        string micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);
    }

    public static float GetLoudnessFromMicrophone()
    {
        if (micClip == null) MicrophoneToAudioClip();

        float maxLoudness = 0;

        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            float micLoudness = GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[i]), micClip);

            if(micLoudness > maxLoudness) maxLoudness = micLoudness;
        }

        return maxLoudness;// GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), micClip);
    }

    public static float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0) return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}