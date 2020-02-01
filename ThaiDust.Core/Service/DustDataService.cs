using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Splat;
using ThaiDust.Core.Model;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.Service
{
    public class DustDataService
    {
        private readonly DustContext _dustContext;

        public DustDataService(DustContext dustContext = null)
        {
            _dustContext = dustContext ?? Locator.Current.GetService<DustContext>();
        }

        public IList<Station> GetAllStations()
        {
            return _dustContext.Stations.ToImmutableList();
        }

        public Station GetStation(string stationCode)
        {
            return _dustContext.Stations.FirstOrDefault(s => s.Code.ToLower() == stationCode.ToLower());
        }

        public void AddOrUpdateStations(params Station[] stations)
        {
            _dustContext.Stations.UpdateRange(stations);
        }

        public void RemoveStation(Station station)
        {
            _dustContext.Stations.Remove(station);
        }

        public void RemoveStation(string stationCode)
        {
            Station exitingStation = _dustContext.Stations.Find(stationCode);
            if (exitingStation != null) _dustContext.Stations.Remove(exitingStation);
        }

        public int Commit()
        {
            return _dustContext.SaveChanges();
        }

        public void InsertOrUpdateDustData(string stationCode, IEnumerable<Record> dustApiRecords, string customStationName = "")
        {
            var databaseStation = _dustContext.Stations.FirstOrDefault(s => s.Code.ToLower() == stationCode.ToLower());
            if (databaseStation == null)
            {
                // Get Known Station Name
                var knownStationName = Stations.All.FirstOrDefault(s =>
                    s.Code.ToLower() == stationCode.ToLower())?.Name;
                // Create New Station Data
                databaseStation = new Station { Code = stationCode, Name = knownStationName ?? customStationName, Records = new List<Record>(dustApiRecords) };
                _dustContext.Stations.Update(databaseStation);
                _dustContext.SaveChanges();
            }
            else
            {
                // Update ForeignKey
                foreach (Record record in dustApiRecords)
                {
                    record.StationCode = stationCode;
                }
                // Add data to existing station
                _dustContext.Records.UpdateRange(dustApiRecords);
                _dustContext.SaveChanges();
            }
        }
    }
}
