using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Windows.Foundation;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.Popups;
using Windows.Web.Http;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using ThaiDust.Dto;
using ThaiDust.Models;

namespace ThaiDust
{
    public class MainPageViewModel : ReactiveObject, IActivatableViewModel
    {
        private HttpClient _client;

        public IList<Station> Stations = new List<Station>
        {
            new Station {Id = "02t", Name = "02t แขวงหิรัญรูจี เขตธนบุรี กรุงเทพ"},
            new Station {Id = "03t", Name = "03t ริมถนนกาญจนาภิเษก เขตบางขุนเทียน กรุงเทพ"},
            new Station {Id = "05t", Name = "05t แขวงบางนา เขตบางนา กรุงเทพ"},
            new Station {Id = "08t", Name = "08t ต.ทรงคนอง อ.พระประแดง จ.สมุทรปราการ"},
            new Station {Id = "10t", Name = "10t แขวงคลองจั่น เขตบางกะปิ กรุงเทพ"},
            new Station {Id = "11t", Name = "11t แขวงดินแดง เขตดินแดง กรุงเทพ"},
            new Station {Id = "12t", Name = "12t แขวงช่องนนทรี เขตยานนาวา กรุงเทพ"},
            new Station {Id = "13t", Name = "13t ต.บางกรวย อ.บางกรวย จ.นนทบุรี"},
            new Station {Id = "14t", Name = "14t ต.อ้อมน้อย อ.กระทุ่มแบน จ.สมุทรสาคร"},
            new Station {Id = "16t", Name = "16t ต.บางโปรง อ.เมือง จ.สมุทรปราการ"},
            new Station {Id = "17t", Name = "17t ต.ตลาด อ.พระประแดง จ.สมุทรปราการ"},
            new Station {Id = "18t", Name = "18t ต.ปากน้ำ อ.เมือง จ.สมุทรปราการ"},
            new Station {Id = "19t", Name = "19t ต.บางเสาธง อ.บางเสาธง จ.สมุทรปราการ"},
            new Station {Id = "20t", Name = "20t ต.คลองหนึ่ง อ.คลองหลวง จ.ปทุมธานี"},
            new Station {Id = "21t", Name = "21t ต.ประตูชัย อ.พระนครศรีอยุธยา จ.พระนครศรีอยุธยา"},
            new Station {Id = "22t", Name = "22t ต.บางพูด อ.ปากเกร็ด จ.นนทบุรี"},
            new Station {Id = "24t", Name = "24t ต.หน้าพระลาน อ.เฉลิมพระเกียรติ จ.สระบุรี"},
            new Station {Id = "25t", Name = "25t ต.ปากเพรียว อ.เมือง จ.สระบุรี"},
            new Station {Id = "26t", Name = "26t ต.หน้าเมือง อ.เมือง จ.ราชบุรี"},
            new Station {Id = "27t", Name = "27t ต.มหาชัย อ.เมือง จ.สมุทรสาคร"},
            new Station {Id = "28t", Name = "28t ต.ปลวกแดง, อ.ปลวกแดง, จ.ระยอง"},
            new Station {Id = "29t", Name = "29t ต.มาบตาพุด, อ.เมือง, จ.ระยอง"},
            new Station {Id = "30t", Name = "30t ต.ท่าประดู่, อ.เมือง, จ.ระยอง"},
            new Station {Id = "31t", Name = "31t ต.ห้วยโป่ง อ.เมือง จ.ระยอง"},
            new Station {Id = "32t", Name = "32t ต.ทุ่งสุขลา อ.ศรีราชา จ.ชลบุรี"},
            new Station {Id = "33t", Name = "33t ต.บ่อวิน อ.ศรีราชา จ.ชลบุรี"},
            new Station {Id = "34t", Name = "34t ต.บ้านสวน อ.เมือง จ.ชลบุรี"},
            new Station {Id = "35t", Name = "35t ต.ช้างเผือก อ.เมือง จ.เชียงใหม่"},
            new Station {Id = "36t", Name = "36t ต.ศรีภูมิ อ.เมือง จ.เชียงใหม่"},
            new Station {Id = "37t", Name = "37t ต.พระบาท อ.เมือง จ.ลำปาง"},
            new Station {Id = "38t", Name = "38t ต.สบป้าด อ.แม่เมาะ จ.ลำปาง"},
            new Station {Id = "39t", Name = "39t ต.บ้านดง อ.แม่เมาะ จ.ลำปาง"},
            new Station {Id = "40t", Name = "40t ต.แม่เมาะ อ.แม่เมาะ จ.ลำปาง"},
            new Station {Id = "41t", Name = "41t ต.ปากน้ำโพ อ.เมือง จ.นครสวรรค์"},
            new Station {Id = "42t", Name = "42t ต.มะขามเตี้ย อ.เมือง จ.สุราษฎร์ธานี"},
            new Station {Id = "43t", Name = "43t ต.ตลาดใหญ่ อ.เมือง จ.ภูเก็ต"},
            new Station {Id = "44t", Name = "44t ต.หาดใหญ่ อ.หาดใหญ่ จ.สงขลา"},
            new Station {Id = "46t", Name = "46t ต.ในเมือง อ.เมือง จ.ขอนแก่น"},
            new Station {Id = "47t", Name = "47t ต.ในเมือง อ.เมือง จ.นครราชสีมา"},
            new Station {Id = "50t", Name = "50t ริมถนนพระรามสี่, เขตปทุมวัน, กรุงเทพ"},
            new Station {Id = "52t", Name = "52t ริมถนนอินทรพิทักษ์ เขตธนบุรี กรุงเทพ"},
            new Station {Id = "53t", Name = "53t ริมถนนลาดพร้าว เขตวังทองหลาง กรุงเทพ"},
            new Station {Id = "54t", Name = "54t ริมถนนดินแดง เขตดินแดง กรุงเทพ"},
            new Station {Id = "57t", Name = "57t ต.เวียง อ.เมือง จ.เชียงราย"},
            new Station {Id = "58t", Name = "58t ต.จองคำ อ.เมือง จ.แม่ฮ่องสอน"},
            new Station {Id = "59t", Name = "59t แขวงพญาไท เขตพญาไท กรุงเทพ"},
            new Station {Id = "60t", Name = "60t ต.วังเย็น อ.แปลงยาว จ.ฉะเชิงเทรา"},
            new Station {Id = "61t", Name = "61t แขวงพลับพลา เขตวังทองหลาง กรุงเทพ"},
            new Station {Id = "62t", Name = "62t ต.บางนาค อ.เมือง จ.นราธิวาส"},
            new Station {Id = "63t", Name = "63t ต.สะเตง อ.เมือง จ.ยะลา"},
            new Station {Id = "67t", Name = "67t ต.ในเวียง อ.เมือง จ.น่าน"},
            new Station {Id = "68t", Name = "68t ต.บ้านกลาง อ.เมือง จ.ลำพูน"},
            new Station {Id = "69t", Name = "69t ต.นาจักร อ.เมือง จ.แพร่"},
            new Station {Id = "70t", Name = "70t ต.บ้านต๋อม อ.เมืองพะเยา จ.พะเยา"},
            new Station {Id = "71t", Name = "71t ต.อรัญประเทศ อ.อรัญประเทศ จ.สระแก้ว"},
            new Station {Id = "72t", Name = "72t ต.นาอาน อ.เมือง จ.เลย"},
            new Station {Id = "73t", Name = "73t ต.เวียงพางคำ, อ.แม่สาย, จ.เชียงราย"},
            new Station {Id = "74t", Name = "74t ต.เนินพระ อ.เมือง จ.ระยอง"},
            new Station {Id = "75t", Name = "75t ต.ห้วยโก๋น อ.เฉลิมพระเกียรติ จ.น่าน"},
            new Station {Id = "76t", Name = "76t ต.แม่ปะ อ.แม่สอด จ.ตาก"},
            new Station {Id = "77t", Name = "77t ต.ท่าตูม อ.ศรีมหาโพธิ จ.ปราจีนบุรี"},
            new Station {Id = "78t", Name = "78t ต.เบตง อ.เบตง จ.ยะลา"},
            new Station {Id = "79t", Name = "79t ต.บ้านเหนือ อ.เมือง จ.กาญจนบุรี"},
            new Station {Id = "80t", Name = "80t ต.พิมาน อ.เมือง จ.สตูล"},
            new Station {Id = "81t", Name = "81t ต.นครปฐม อ.เมือง จ.นครปฐม"},
            new Station {Id = "m8", Name = "m8 หน่วยตรวจวัดเคลื่อนที่ 8 ค่ายมหาสุรสิงหนาท จ.ระยอง"}
        };

        [Reactive] public Station SelectedStation { get; set; }
        [Reactive] public StationParam SelectedParameter { get; set; }
        private SourceList<StationParam> _paramsList = new SourceList<StationParam>();
        public ObservableCollectionExtended<StationParam> StationParams = new ObservableCollectionExtended<StationParam>();

        public MainPageViewModel(HttpClient httpClient = null)
        {
            _client = httpClient ?? Locator.Current.GetService<HttpClient>();

            this.WhenActivated(cleanup =>
            {
                _paramsList.Connect().Bind(StationParams).Subscribe().DisposeWith(cleanup);

                var LoadParameterCommand = ReactiveCommand.CreateFromObservable<Station, IEnumerable<StationParam>>(
                    station =>
                    {
                        var dto = new GetParamListDto { StationId = station.Id };
                        return _client.TryPostAsync(new Uri("http://aqmthai.com/includes/getManReport.php"),
                                dto.GenerateFormUrlEncodedContent())
                            .ToObservable()
                            .Where(r => r.Succeeded)
                            .Select(async r => await r.ResponseMessage.Content.ReadAsStringAsync())
                            .Switch().Select(ParseParameter);
                    }).DisposeWith(cleanup);
                LoadParameterCommand.ThrownExceptions.Subscribe(ex =>
                {
                    var dialog = new MessageDialog(ex.Message, "Error");
                    dialog.ShowAsync();
                });
                LoadParameterCommand.Subscribe(r =>
                {
                    _paramsList.Clear();
                    _paramsList.AddRange(r);
                });
                this.WhenAnyValue(p => p.SelectedStation).Where(p => p != null).InvokeCommand(LoadParameterCommand);
            });
        }

        private IEnumerable<StationParam> ParseParameter(string xml)
        {
            var x = new XmlDocument();
            x.LoadXml(xml);
            return x.GetElementsByTagName("option").SelectMany(e =>
                e.Attributes.Where(a => a.NodeName == "value").Select(a => new StationParam
                { Param = (string)a.NodeValue, Name = (string)a.NodeValue }));
        }

        private IEnumerable<StationParam> ParseData(string xml)
        {
            var x = new XmlDocument();
            x.LoadXml(xml);
            return x.GetElementsByTagName("tr").SkipLast(5).Select(e =>
            {
                e.FirstChild.NodeValue;
                e.LastChild.NodeValue;
            });
        }
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
