
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.Service;

namespace ThaiDust.Test
{
    [TestClass]
    public class UnitTest1
    {
        private DustContext SetUpDatabase()
        {
            var context = new DustContext(":memory:");
            context.Database.Migrate();
            return context;
        }

        [TestMethod]
        public async Task LoadEmptyData()
        {
            using var dustContext = SetUpDatabase();
            var dustService = new DustDataService(dustContext);
            Station stationWithData = await dustService.GetStationAsync("10E");
            stationWithData.Should().BeNull();
        }

        [TestMethod]
        public void AddDustData()
        {
            using var dustContext = SetUpDatabase();
            var dustService = new DustDataService(dustContext);
            var records = new Record[]
            {
                new Record
                {
                    DateTime = DateTime.Now.AddHours(-2),
                    Type = RecordType.PM25,
                    Value = 15
                },
                new Record
                {
                    DateTime = DateTime.Now.AddHours(-1),
                    Type = RecordType.PM25,
                    Value = 10
                },
                new Record
                {
                    DateTime = DateTime.Now,
                    Type = RecordType.PM25,
                    Value = 20
                }
            };
            //dustService.AddOrUpdateRecords("25t",records);
        }

        [TestMethod]
        public void UpdateDustData()
        {

        }

        [TestMethod]
        public void DeleteDustData()
        {

        }
    }
}
