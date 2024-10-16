using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    // AudioClip을 WAV 파일 포맷으로 변환하는 함수
    public static byte[] FromAudioClip(AudioClip clip)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            WriteWavHeader(stream, clip);
            WriteWavData(stream, clip);
            return stream.ToArray();
        }
    }

    // WAV 파일의 헤더 작성
    private static void WriteWavHeader(Stream stream, AudioClip clip)
    {
        int frequency = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        stream.Position = 0;

        // RIFF 헤더
        stream.Write(new byte[4] { 82, 73, 70, 70 }, 0, 4);
        stream.Write(BitConverter.GetBytes(samples * channels * 2 + 36), 0, 4);
        stream.Write(new byte[4] { 87, 65, 86, 69 }, 0, 4);

        // fmt 서브 청크
        stream.Write(new byte[4] { 102, 109, 116, 32 }, 0, 4);
        stream.Write(BitConverter.GetBytes(16), 0, 4);
        stream.Write(BitConverter.GetBytes((short)1), 0, 2);
        stream.Write(BitConverter.GetBytes((short)channels), 0, 2);
        stream.Write(BitConverter.GetBytes(frequency), 0, 4);
        stream.Write(BitConverter.GetBytes(frequency * channels * 2), 0, 4);
        stream.Write(BitConverter.GetBytes((short)(channels * 2)), 0, 2);
        stream.Write(BitConverter.GetBytes((short)16), 0, 2);

        // data 서브 청크
        stream.Write(new byte[4] { 100, 97, 116, 97 }, 0, 4);
        stream.Write(BitConverter.GetBytes(samples * channels * 2), 0, 4);
    }

    // WAV 파일에 실제 오디오 데이터를 작성
    private static void WriteWavData(Stream stream, AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        foreach (var sample in samples)
        {
            short sampleInt = (short)(sample * short.MaxValue);
            stream.Write(BitConverter.GetBytes(sampleInt), 0, 2);
        }
    }
}
