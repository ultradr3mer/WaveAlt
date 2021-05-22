using System;
using System.Drawing;
using System.Threading.Tasks;
using WaveAlt.Util;

namespace WaveAlt.Services
{
  internal class Analyzer
  {
    #region Methods

    public Bitmap DrawBitmap(float[][] data, float v1, float v2)
    {
      int height = data.Length;
      int width = data[0].Length;
      var bitmap = new Bitmap(width, height);
      for (int y = 0; y < height; y++)
      {
        var currentLine = data[y];
        for (int x = 0; x < width; x++)
        {
          var value = (int)Math.Round(MathExtra.Clip((currentLine[x] - v1) / (v2 - v1) * 255, 0, 255));
          var color = Color.FromArgb(255, value, value, value);
          bitmap.SetPixel(x, y, color);
        }
      }
      return bitmap;
    }

    internal AnalyzerResult Start(float[] samples)
    {
      int bands = 512;
      float[][] blured = PerformBlur(bands, samples);

      float length = blured[0].Length;
      var diff = new float[bands - 1][];
      Parallel.For(0, bands - 1, currentBandNumber =>
      {
        var bluredA = blured[currentBandNumber];
        var bluredB = blured[currentBandNumber+1];
        var currentBand = new float[bluredA.Length];
        for (int i = 0; i < currentBand.Length; i++)
        {
          currentBand[i] = bluredA[i] - bluredB[i];
        }

        diff[currentBandNumber] = currentBand;
      });

      return new AnalyzerResult() { Blured = blured, Diff = diff };
    }

    private static float[][] PerformBlur(int bands, float[] originalSamples)
    {
      int originalSampleCount = originalSamples.Length;
      var blured = new float[bands][];

      var firstBand = new float[originalSampleCount];
      originalSamples.CopyTo(firstBand, 0);
      blured[0] = firstBand;

      var result = Parallel.For(1, bands, currentBandNumber =>
      {
        float x = currentBandNumber;
        float blurValue = (float)(1.0 / Math.Pow(2.0, (double)(x / (bands / 8.0))));

        var currentBand = new float[originalSampleCount];

        float currentValue = originalSamples[0];

        for (int i = 0; i < currentBand.Length; i++)
        {
          currentValue = (originalSamples[i] * blurValue) + (currentValue * (1 - blurValue));
          currentBand[i] = currentValue;

          if (float.IsNaN(currentValue))
          {
            throw new Exception();
          }
        }

        for (int i = currentBand.Length - 1; i >= 0; i--)
        {
          currentValue = (currentBand[i] * blurValue) + (currentValue * (1 - blurValue));
          currentBand[i] = currentValue;

          if(float.IsNaN(currentValue))
          {
            throw new Exception();
          }
        }

        blured[currentBandNumber] = currentBand;
      });

      return blured;
    }

    #endregion Methods
  }

  internal class AnalyzerResult
  {
    #region Properties

    public float[][] Blured { get; internal set; }
    public float[][] Diff { get; internal set; }

    #endregion Properties
  }
}