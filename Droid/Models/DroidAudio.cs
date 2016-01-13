using System;

using Supermortal.Common.PCL.Enums;

using RiffSharer.Models;

namespace RiffSharer.Droid.Models
{
    public class DroidAudio : Java.Lang.Object
    {
        public string AudioID { get; set; }

        public string Name { get; set; }

        public string LocalPath { get; set; }

        public DateTime DateCreated { get; set; }

        public int AudioFormatInt { get; private set; }

        public int ChannelConfigurationInt { get; private set; }

        public int SampleRate { get; set; }

        public byte[] Data { get; set; }

        public string UserID { get; set; }

        public DroidAudio(Audio a)
        {
            UpdateModel(a);
        }

        #region Methods

        public void UpdateModel(Audio a)
        {
            LocalPath = a.LocalPath;
            Name = a.Name;
            AudioFormatInt = a.AudioFormatInt;
            ChannelConfigurationInt = a.ChannelConfigurationInt;
            SampleRate = a.SampleRate;
            Data = a.Data;
        }

        public AudioFormat GetAudioFormat()
        {
            if (AudioFormatInt == (int)AudioFormat.PCM16Bit)
                return AudioFormat.PCM16Bit;
            if (AudioFormatInt == (int)AudioFormat.PCM8Bit)
                return AudioFormat.PCM8Bit;

            return AudioFormat.Unknown;
        }

        public ChannelConfiguration GetChannelConfiguration()
        {
            if (ChannelConfigurationInt == (int)ChannelConfiguration.Mono)
                return ChannelConfiguration.Mono;
            if (ChannelConfigurationInt == (int)ChannelConfiguration.Stereo)
                return ChannelConfiguration.Stereo;

            return ChannelConfiguration.Unknown;
        }

        public void SetAudioFormat(AudioFormat af)
        {
            AudioFormatInt = (int)af;
        }

        public void SetChannelConfiguration(ChannelConfiguration cc)
        {
            ChannelConfigurationInt = (int)cc;
        }

        public Audio ToAudio()
        {
            var a = new Audio();

            a.LocalPath = LocalPath;
            a.Name = Name;
            a.SetAudioFormat(GetAudioFormat());
            a.SetChannelConfiguration(GetChannelConfiguration());
            a.SampleRate = SampleRate;
            a.Data = Data;

            return a;
        }

        #endregion
    }
}

