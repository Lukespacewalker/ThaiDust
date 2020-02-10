using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IObservable<Station> GetStationAsync(string stationCode)
        {
            return Observable.FromAsync(cts =>
            {
                return _dustContext.Stations.FirstOrDefaultAsync(s => s.Code == stationCode, cts);
            }).Select(async station =>
            {
                if(station!=null) await _dustContext.Entry(station).Collection(c=>c.Records).LoadAsync();
                return station;
            }).Switch();
            //if (station != null)
            //    _dustContext.Entry(station).Collection(s => s.Records).Load();
        }

        public async Task AddOrUpdateStations(params Station[] stations)
        {
            // Because "Station" entity already has a "Primary" key such as "26T"
            // We need to check weather the entity is already existed first
            foreach (Station station in stations)
            {
                var dbStation = await GetStationAsync(station.Code);
                if (dbStation == null)
                {
                    _dustContext.Stations.Add(station);
                }
                else
                {
                    _dustContext.Stations.Update(station);
                    //_dustContext.Entry(dbStation).CurrentValues.SetValues(station);
                    //// The database records should be populated by EF Core proxy
                    //foreach (Record dbRecord in dbStation.Records)
                    //{
                    //    foreach (Record stationRecord in station.Records)
                    //    {
                    //        dbRecord.
                    //    }
                    //}
                    ////IEnumerable<Record> diff = dbStation.Records.Except(station.Records);
                    //// Add only different records
                    //dbStation.Records.AddRange(diff);
                    //// Update exiting records
                    //foreach (Record dbRecord in dbStation.Records)
                    //{
                    //    .Records.Exists()
                    //}
                }
            }
        }

        public void RemoveStations(params Station[] stations)
        {
            foreach (var station in stations)
            {
                RemoveStation(station.Code);
            }
        }

        public void RemoveStation(string stationCode)
        {
            Station exitingStation = _dustContext.Stations.First(s => s.Code == stationCode);
            if (exitingStation != null) _dustContext.Stations.Remove(exitingStation);
        }

        public Task<int> CommitAsync()
        {
            _dustContext.ChangeTracker.DetectChanges();
            return _dustContext.SaveChangesAsync();
        }

        //public void AddOrUpdateRecords(string stationCode, IEnumerable<Record> dustApiRecords, string customStationName = "")
        //{
        //    var databaseStation = _dustContext.Stations.FirstOrDefault(s => s.Code.ToLower() == stationCode.ToLower());
        //    if (databaseStation == null)
        //    {
        //        // Get Known Station Name
        //        var knownStationName = Stations.All.FirstOrDefault(s =>
        //            s.Code.ToLower() == stationCode.ToLower())?.Name;
        //        // Create New Station Data
        //        databaseStation = new Station { Code = stationCode, Name = knownStationName ?? customStationName, Records = new List<Record>(dustApiRecords) };
        //        _dustContext.Stations.Update(databaseStation);
        //        _dustContext.SaveChanges();
        //    }
        //    else
        //    {
        //        // Update ForeignKey
        //        foreach (Record record in dustApiRecords)
        //        {
        //            record.StationCode = stationCode;
        //        }
        //        // Add data to existing station
        //        _dustContext.Records.UpdateRange(dustApiRecords);
        //        _dustContext.SaveChanges();
        //    }
        //}
    }
}
