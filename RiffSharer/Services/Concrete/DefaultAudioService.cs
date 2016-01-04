using System;

using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Enums;

using RiffSharer.Services.Abstract;
using RiffSharer.Repositories.Abstract;
using RiffSharer.Models;

namespace RiffSharer.Services.Concrete
{
    public class DefaultAudioService : IAudioService
    {

        private readonly AAudioRepository _ar;

        public DefaultAudioService()
            : this(IoCHelper.Instance.GetService<AAudioRepository>())
        {
        }

        public DefaultAudioService(AAudioRepository ar)
        {
            _ar = ar;
        }

        public void SaveAudio(string name, AudioFormat audioFormat, ChannelConfiguration channelConfiguration, int sampleRate, byte[] data)
        {
            var audio = new Audio();

            audio.Name = name;
            audio.SetAudioFormat(audioFormat);
            audio.SetChannelConfiguration(channelConfiguration);
            audio.SampleRate = sampleRate;
            audio.Data = data;

            _ar.Insert(audio);
        }

        public void SaveAudio(string name, string localPath)
        {
            throw new NotImplementedException();
        }
    }
}

