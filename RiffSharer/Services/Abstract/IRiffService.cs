using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Supermortal.Common.PCL.Enums;

using RiffSharer.Models;

namespace RiffSharer.Services.Abstract
{
    public interface IRiffService
    {
        RiffDTO SaveRiff(string name, AudioFormat audioFormat, ChannelConfiguration channelConfiguration, int sampleRate, byte[] data, string userId);

        Audio SaveAudio(string name, string localPath);

        RiffDTO GetRiff(string riffId);

        Task<IEnumerable<RiffDTO>> GetRiffsForUser(string userId, int page, int pageSize);

        Task<int> GetRiffCountForUser(string userId);

        void AddRiffComment(string riffId, string userId, string text);

        void AddRiffRating(string riffId, string userId, float value);
    }
}

