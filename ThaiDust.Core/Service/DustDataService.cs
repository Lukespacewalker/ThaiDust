using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Splat;
using ThaiDust.Core.Model;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.Service
{
    internal class DustDataService
    {
        private readonly DustContext _dustContext;

        public DustDataService(DustContext dustContext)
        {
            _dustContext = dustContext ?? Locator.Current.GetService<DustContext>();
        }

        public Station LoadDustData(string stationCode)
        {
            var station = _dustContext.Stations.Include(s=>s.Records).SingleOrDefault(s=>s.Code.ToLower() == stationCode.ToLower());
            return station;
        }

        public void InsertOrUpdateDustData(string stationCode, IEnumerable<Record> records, string customStationName = "")
        {
            var station = _dustContext.Stations.Find(stationCode);
            if (station == null)
            {
                // Add new station
                var knownStationName = Stations.All.SingleOrDefault(s =>
                    s.Code.ToLower() == stationCode.ToLower())?.Name;
                station = new Station{Code = stationCode, Name = knownStationName ?? customStationName , Records = new List<Record>(records)};
                _dustContext.Stations.Add(station);
                _dustContext.SaveChanges();
            }
            else
            {
                // Add data to existing station
                station.Records.AddRange(records);
                _dustContext.SaveChanges();
            }
        }
    }
}
