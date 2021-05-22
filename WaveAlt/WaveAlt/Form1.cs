using System;
using System.Linq;
using System.Windows.Forms;
using WaveAlt.Audio;
using WaveAlt.Services;
using WaveAlt.Util;

namespace WaveAlt
{
  public partial class Form1 : Form
  {
    #region Constructors

    public Form1()
    {
      InitializeComponent();
    }

    #endregion Constructors

    #region Methods

    private void Form1_Load(object sender, EventArgs e)
    {
      var a = new Analyzer();
      var samples = Io.ReadFile("3Notes.wav");
      var result = a.Start(samples);

      this.SaveBandToWav(result.Diff, 32 * 1);
      this.SaveBandToWav(result.Diff, 32 * 2);
      this.SaveBandToWav(result.Diff, 32 * 3);
      this.SaveBandToWav(result.Diff, 32 * 4);
      this.SaveBandToWav(result.Diff, 32 * 5);
      this.SaveBandToWav(result.Diff, 32 * 6);
      this.SaveBandToWav(result.Diff, 32 * 7);
      this.SaveBandToWav(result.Diff, 32 * 8);

      var bitmapData = MathExtra.AbsArray(result.Diff);
      //bitmapData = MathExtra.ShrinkArray(bitmapData, 40);
      bitmapData = MathExtra.SubArray(bitmapData, 3358 * 10, 3358);

      this.pictureBox1.Image = a.DrawBitmap(bitmapData, 0, 0.0005f);
    }

    private void SaveBandToWav(float[][] diff, int bandNr)
    {
      var band = diff[bandNr];
      var amp = 0.5f / MathExtra.GetMaxAmplitude(band);
      var amplifiedBand = band.Select(o => o * amp).ToArray();
      Io.SaveAsWav(amplifiedBand, $"Band{bandNr}.wav");
    }

    #endregion Methods
  }
}