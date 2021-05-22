using NAudio.Wave;

namespace WaveAlt.Audio
{
  internal class Io
  {
    #region Methods

    public static float[] ReadFile(string file)
    {
      WaveStream mainOutputStream = new AudioFileReader(file);
      var sampleProvider = mainOutputStream.ToSampleProvider();
      int originalSampleCount = (int)(mainOutputStream.Length / mainOutputStream.BlockAlign);
      var samples = new float[originalSampleCount];
      sampleProvider.Read(samples, 0, samples.Length);
      return samples;
    }

    public static void SaveAsWav(float[] data, string fileName)
    {
      WaveFormat waveFormat = new WaveFormat(44100, 1);
      //WaveFormat waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);
      using (WaveFileWriter writer = new WaveFileWriter(fileName, waveFormat))
      {
        writer.WriteSamples(data, 0, data.Length);
      }
    }

    #endregion Methods
  }
}