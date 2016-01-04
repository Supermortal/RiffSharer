using System;

using Supermortal.Common.PCL.Enums;

using RiffSharer.Models;

namespace RiffSharer.Services.Abstract
{
    public interface IAudioService
    {
        void SaveAudio(string name, AudioFormat audioFormat, ChannelConfiguration channelConfiguration, int sampleRate, byte[] data);

        void SaveAudio(string name, string localPath);
    }
}

