﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ThaiDust.Core.Model;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.Service
{
    public class XmlParser
    {
        public static IList<StationParam> ParseParameter(string xml)
        {
            var x = new XmlDocument();
            x.LoadXml(xml);
            List<StationParam> @params = x.GetElementsByTagName("option").OfType<XmlNode>().SelectMany(e =>
                e.Attributes.OfType<XmlAttribute>().Where(a => a.Name == "value").Select(a => new StationParam
                { Param = (string)a.Value, Name = (string)a.Value })).ToList();
            if (@params.All(p => p.Param != "O3"))
                @params.Add(new StationParam
                { Param = "O3", Name = "O3" });
            return @params;
        }

        public static Record[] ParseData(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return new Record[0];
            var x = new XmlDocument();
            try
            {
                x.LoadXml(xml);
            }
            catch (Exception)
            {
                return new Record[0];
            }

            var gregorianCalendar = new GregorianCalendar();
            var dataTable = x.GetElementsByTagName("tr").OfType<XmlNode>().ToArray();
            return dataTable.Skip(1).TakeWhile((_, i) => i + 1 < dataTable.Length - 5).Select(e =>
                {
                    // No Enumerable.SkipLast in net Standard 2.0
                    var dateNodeValue = (string)e.FirstChild.InnerText;
                    int[] dateSplit = dateNodeValue.Split(',').Select(int.Parse).ToArray();
                    var date = new DateTime(dateSplit[0], dateSplit[1], dateSplit[2], dateSplit[3], dateSplit[4], dateSplit[5], gregorianCalendar);
                    return double.TryParse(e.LastChild?.InnerText, out double value) ?
                        new Record { DateTime = date, Value = value }
                        : new Record { DateTime = date, Value = null };
                }).ToArray();
        }
    }
}
