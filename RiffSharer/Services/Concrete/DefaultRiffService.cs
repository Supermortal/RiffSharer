using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Enums;

using RiffSharer.Services.Abstract;
using RiffSharer.Repositories.Abstract;
using RiffSharer.Models;

namespace RiffSharer.Services.Concrete
{
    public class DefaultRiffService : IRiffService
    {

        private readonly AAudioRepository _ar;
        private readonly ARiffRepository _rr;
        private readonly ACommentRepository _cr;
        private readonly ARatingRepository _rar;

        public DefaultRiffService()
            : this(IoCHelper.Instance.GetService<AAudioRepository>(), 
                   IoCHelper.Instance.GetService<ARiffRepository>(), 
                   IoCHelper.Instance.GetService<ACommentRepository>(),
                   IoCHelper.Instance.GetService<ARatingRepository>())
        {
        }

        public DefaultRiffService(AAudioRepository ar, 
                                  ARiffRepository rr,
                                  ACommentRepository cr,
                                  ARatingRepository rar)
        {
            _ar = ar;
            _rr = rr;
            _cr = cr;
            _rar = rar;
        }

        public RiffDTO SaveRiff(string name, AudioFormat audioFormat, ChannelConfiguration channelConfiguration, int sampleRate, byte[] data, string userId)
        {
            var audio = new Audio();

            audio.SetAudioFormat(audioFormat);
            audio.SetChannelConfiguration(channelConfiguration);
            audio.SampleRate = sampleRate;
            audio.Data = data;

            audio = _ar.Insert(audio);

            var riff = new Riff();

            riff.AudioID = audio.AudioID;
            riff.Name = name;
            riff.UserID = userId;

            riff = _rr.Insert(riff);

            return new RiffDTO(riff, audio);
        }

        public Audio SaveAudio(string name, string localPath)
        {
            throw new NotImplementedException();
        }

        public RiffDTO GetRiff(string riffId)
        {
            var riff = _rr.Get(riffId);
            var audio = _ar.Get(riff.AudioID);

            return new RiffDTO(riff, audio);
        }

        public async Task<IEnumerable<RiffDTO>> GetRiffsForUser(string userId, int page, int pageSize)
        {
            var riffs = await _rr.GetAllForUserPaged(userId, page, pageSize);
            return riffs.Select(i => new RiffDTO(i));
        }

        public async Task<int> GetRiffCountForUser(string userId)
        {
            return await _rr.GetCountForUser(userId).ConfigureAwait(false);
        }

        public void AddRiffComment(string riffId, string userId, string text)
        {
            Comment c = new Comment();

            c.RiffID = riffId;
            c.UserID = userId;
            c.Text = text;

            _cr.Insert(c);
        }

        public void AddRiffRating(string riffId, string userId, float value)
        {
            var r = new RiffSharer.Models.Rating();

            r.RiffID = riffId;
            r.UserID = userId;
            r.Value = value;

            _rar.Insert(r);
        }
    }
}

