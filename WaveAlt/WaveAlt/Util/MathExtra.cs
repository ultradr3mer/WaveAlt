using System;
using System.Linq;
using System.Threading.Tasks;

namespace WaveAlt.Util
{
  internal class MathExtra
  {
    #region Methods

    public static float Clip(float value, float min, float max)
    {
      return Math.Max(Math.Min(value, max), min);
    }

    public static double Gaus(double x, double a, double b, double c)
    {
      var v1 = (x - b);
      var v2 = (v1 * v1) / (2 * (c * c));
      var v3 = a * Math.Exp(-v2);
      return v3;
    }

    public static float Gaus(float x, float a, float b, float c)
    {
      var v1 = (x - b);
      var v2 = (v1 * v1) / (2 * (c * c));
      var v3 = a * Math.Exp(-v2);
      return (float)v3;
    }

    internal static float GetMaxAmplitude(float[] array)
    {
      var maxAmplitude = array.Max();
      var minAmplitude = array.Min();

      return Math.Max(Math.Abs(maxAmplitude), Math.Abs(minAmplitude));
    }

    internal static float[][] AbsArray(float[][] input)
    {
      int bands = input.Length;
      int samples = input[0].Length;
      var result = new float[bands][];
      Parallel.For(0, bands, currentBandNumber =>
      {
        var inputBand = input[currentBandNumber];
        var currentBand = new float[samples];
        for (int i = 0; i < samples; i++)
        {
          currentBand[i] = Math.Abs(inputBand[i]);
        }

        result[currentBandNumber] = currentBand;
      });

      return result;
    }

    internal static float[][] SubArray(float[][] input, int startIndex, int length)
    {
      int bands = input.Length;
      int samples = input[0].Length;
      var result = new float[bands][];

      Parallel.For(0, bands, currentBandNumber =>
      {
        var inputBand = input[currentBandNumber];
        var currentBand = new float[length];
        for (int i = 0; i < length; i++)
        {
          currentBand[i] = inputBand[startIndex+i];
        }

        result[currentBandNumber] = currentBand;
      });

      return result;
    }

    internal static float[][] ShrinkArray(float[][] input, int factor)
    {
      int bands = input.Length;
      int inputSamples = input[0].Length;
      int targetSamples = inputSamples / factor;
      var result = new float[bands][];
      Parallel.For(0, bands, currentBandNumber =>
      {
        var inputBand = input[currentBandNumber];
        var currentBand = new float[targetSamples];
        for (int i = 0; i < targetSamples; i ++)
        {
          float currentSampleValue = 0;
          float currentSampleCount = 0;
          for (int shift = 0; shift < factor; shift++)
          {
            int readPos = i * factor + shift;
            if(readPos >= inputSamples)
            {
              break;
            }

            currentSampleValue += inputBand[readPos];
            currentSampleCount++;
          }

          currentBand[i] = currentSampleValue / currentSampleCount;
        }

        result[currentBandNumber] = currentBand;
      });

      return result;
    }

    #endregion Methods
  }
}