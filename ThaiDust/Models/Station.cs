using System.Collections.Generic;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.Models
{
    public class Station : ReactiveObject
    {
        [Reactive] public string Id { get; set; }
        [Reactive] public string Name { get; set; }
    }

    public static class Stations
    {
        public static IList<Station> All = new List<Station>
        {
            new Station {Id = "02t", Name = "แขวงหิรัญรูจี เขตธนบุรี กรุงเทพ"},
            new Station {Id = "03t", Name = "ริมถนนกาญจนาภิเษก เขตบางขุนเทียน กรุงเทพ"},
            new Station {Id = "05t", Name = "แขวงบางนา เขตบางนา กรุงเทพ"},
            new Station {Id = "08t", Name = "ต.ทรงคนอง อ.พระประแดง จ.สมุทรปราการ"},
            new Station {Id = "10t", Name = "แขวงคลองจั่น เขตบางกะปิ กรุงเทพ"},
            new Station {Id = "11t", Name = "แขวงดินแดง เขตดินแดง กรุงเทพ"},
            new Station {Id = "12t", Name = "แขวงช่องนนทรี เขตยานนาวา กรุงเทพ"},
            new Station {Id = "13t", Name = "ต.บางกรวย อ.บางกรวย จ.นนทบุรี"},
            new Station {Id = "14t", Name = "ต.อ้อมน้อย อ.กระทุ่มแบน จ.สมุทรสาคร"},
            new Station {Id = "16t", Name = "ต.บางโปรง อ.เมือง จ.สมุทรปราการ"},
            new Station {Id = "17t", Name = "ต.ตลาด อ.พระประแดง จ.สมุทรปราการ"},
            new Station {Id = "18t", Name = "ต.ปากน้ำ อ.เมือง จ.สมุทรปราการ"},
            new Station {Id = "19t", Name = "ต.บางเสาธง อ.บางเสาธง จ.สมุทรปราการ"},
            new Station {Id = "20t", Name = "ต.คลองหนึ่ง อ.คลองหลวง จ.ปทุมธานี"},
            new Station {Id = "21t", Name = "ต.ประตูชัย อ.พระนครศรีอยุธยา จ.พระนครศรีอยุธยา"},
            new Station {Id = "22t", Name = "ต.บางพูด อ.ปากเกร็ด จ.นนทบุรี"},
            new Station {Id = "24t", Name = "ต.หน้าพระลาน อ.เฉลิมพระเกียรติ จ.สระบุรี"},
            new Station {Id = "25t", Name = "ต.ปากเพรียว อ.เมือง จ.สระบุรี"},
            new Station {Id = "26t", Name = "ต.หน้าเมือง อ.เมือง จ.ราชบุรี"},
            new Station {Id = "27t", Name = "ต.มหาชัย อ.เมือง จ.สมุทรสาคร"},
            new Station {Id = "28t", Name = "ต.ปลวกแดง, อ.ปลวกแดง, จ.ระยอง"},
            new Station {Id = "29t", Name = "ต.มาบตาพุด, อ.เมือง, จ.ระยอง"},
            new Station {Id = "30t", Name = "ต.ท่าประดู่, อ.เมือง, จ.ระยอง"},
            new Station {Id = "31t", Name = "ต.ห้วยโป่ง อ.เมือง จ.ระยอง"},
            new Station {Id = "32t", Name = "ต.ทุ่งสุขลา อ.ศรีราชา จ.ชลบุรี"},
            new Station {Id = "33t", Name = "ต.บ่อวิน อ.ศรีราชา จ.ชลบุรี"},
            new Station {Id = "34t", Name = "ต.บ้านสวน อ.เมือง จ.ชลบุรี"},
            new Station {Id = "35t", Name = "ต.ช้างเผือก อ.เมือง จ.เชียงใหม่"},
            new Station {Id = "36t", Name = "ต.ศรีภูมิ อ.เมือง จ.เชียงใหม่"},
            new Station {Id = "37t", Name = "ต.พระบาท อ.เมือง จ.ลำปาง"},
            new Station {Id = "38t", Name = "ต.สบป้าด อ.แม่เมาะ จ.ลำปาง"},
            new Station {Id = "39t", Name = "ต.บ้านดง อ.แม่เมาะ จ.ลำปาง"},
            new Station {Id = "40t", Name = "ต.แม่เมาะ อ.แม่เมาะ จ.ลำปาง"},
            new Station {Id = "41t", Name = "ต.ปากน้ำโพ อ.เมือง จ.นครสวรรค์"},
            new Station {Id = "42t", Name = "ต.มะขามเตี้ย อ.เมือง จ.สุราษฎร์ธานี"},
            new Station {Id = "43t", Name = "ต.ตลาดใหญ่ อ.เมือง จ.ภูเก็ต"},
            new Station {Id = "44t", Name = "ต.หาดใหญ่ อ.หาดใหญ่ จ.สงขลา"},
            new Station {Id = "46t", Name = "ต.ในเมือง อ.เมือง จ.ขอนแก่น"},
            new Station {Id = "47t", Name = "ต.ในเมือง อ.เมือง จ.นครราชสีมา"},
            new Station {Id = "50t", Name = "ริมถนนพระรามสี่, เขตปทุมวัน, กรุงเทพ"},
            new Station {Id = "52t", Name = "ริมถนนอินทรพิทักษ์ เขตธนบุรี กรุงเทพ"},
            new Station {Id = "53t", Name = "ริมถนนลาดพร้าว เขตวังทองหลาง กรุงเทพ"},
            new Station {Id = "54t", Name = "ริมถนนดินแดง เขตดินแดง กรุงเทพ"},
            new Station {Id = "57t", Name = "ต.เวียง อ.เมือง จ.เชียงราย"},
            new Station {Id = "58t", Name = "ต.จองคำ อ.เมือง จ.แม่ฮ่องสอน"},
            new Station {Id = "59t", Name = "แขวงพญาไท เขตพญาไท กรุงเทพ"},
            new Station {Id = "60t", Name = "ต.วังเย็น อ.แปลงยาว จ.ฉะเชิงเทรา"},
            new Station {Id = "61t", Name = "แขวงพลับพลา เขตวังทองหลาง กรุงเทพ"},
            new Station {Id = "62t", Name = "ต.บางนาค อ.เมือง จ.นราธิวาส"},
            new Station {Id = "63t", Name = "ต.สะเตง อ.เมือง จ.ยะลา"},
            new Station {Id = "67t", Name = "ต.ในเวียง อ.เมือง จ.น่าน"},
            new Station {Id = "68t", Name = "ต.บ้านกลาง อ.เมือง จ.ลำพูน"},
            new Station {Id = "69t", Name = "ต.นาจักร อ.เมือง จ.แพร่"},
            new Station {Id = "70t", Name = "ต.บ้านต๋อม อ.เมืองพะเยา จ.พะเยา"},
            new Station {Id = "71t", Name = "ต.อรัญประเทศ อ.อรัญประเทศ จ.สระแก้ว"},
            new Station {Id = "72t", Name = "ต.นาอาน อ.เมือง จ.เลย"},
            new Station {Id = "73t", Name = "ต.เวียงพางคำ, อ.แม่สาย, จ.เชียงราย"},
            new Station {Id = "74t", Name = "ต.เนินพระ อ.เมือง จ.ระยอง"},
            new Station {Id = "75t", Name = "ต.ห้วยโก๋น อ.เฉลิมพระเกียรติ จ.น่าน"},
            new Station {Id = "76t", Name = "ต.แม่ปะ อ.แม่สอด จ.ตาก"},
            new Station {Id = "77t", Name = "ต.ท่าตูม อ.ศรีมหาโพธิ จ.ปราจีนบุรี"},
            new Station {Id = "78t", Name = "ต.เบตง อ.เบตง จ.ยะลา"},
            new Station {Id = "79t", Name = "ต.บ้านเหนือ อ.เมือง จ.กาญจนบุรี"},
            new Station {Id = "80t", Name = "ต.พิมาน อ.เมือง จ.สตูล"},
            new Station {Id = "81t", Name = "ต.นครปฐม อ.เมือง จ.นครปฐม"},
            new Station {Id = "m8", Name = "หน่วยตรวจวัดเคลื่อนที่ 8 ค่ายมหาสุรสิงหนาท จ.ระยอง"}
        };
    }
}