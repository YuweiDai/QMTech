using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QMTech.DataImport
{
    class Program
    {
        class LngLat
        {
            public double Lng { get; set; }

            public double Lat { get; set; }
        }

        class Store
        {
            public string id { get; set; }
            public string name { get; set; }

            public string pic { get; set; }

            public double fee { get; set; }

            public string minconsumption { get; set; }

            public string delivery_desc { get; set; }

            public IList<Category> types { get; set; }

            public IList<Product> dishes { get; set; }
        }

        class Category
        {
            public string name { get; set; }
        }

        class Product
        {
            public string name { get; set; }
            public string dishtype { get; set; }
            public string price { get; set; }
            public string unit { get; set; }
            public string desc { get; set; }
            public string pic { get; set; }
        }

        class Near
        {
            public int count { get; set; }

            public IList<Store> stores { get; set; }
        }

        class Stores
        {

            public IList<Category> types { get; set; }

            public IList<Product> dishes { get; set; }
        }

        static void Main(string[] args)
        {

            Near n = null;

            try
            {

                //string url = "http://www.xianmi.me/qzxm/index.php/Wap/Wm/index/memid/511/lng/118.8702400000/lat/28.961739.html";
                //string result = Request(url);

                //int start = result.IndexOf("var near = ");
                //int end = result.IndexOf("</script>", start);

                //string json = result.Substring(start, end).Replace("var near = ", "");

                string json = "{'types':[{'id':'45','name':'衢名小吃','storeids':'42,1072','storenum':2},{'id':'44','name':'营养早餐','storeids':'42,588,1067,1072,1083,1090,1107,1128,1139','storenum':9},{'id':'40','name':'精品炒菜','storeids':'42,869,873,984','storenum':4},{'id':'37','name':'鲜米水果','storeids':'1022','storenum':1},{'id':'20','name':'中式快餐','storeids':'144,499,509,515,873,983,1012','storenum':7},{'id':'21','name':'西式快餐','storeids':'960,985,1013','storenum':3},{'id':'22','name':'特色小吃','storeids':'42,50,54,465,534,582,588,746,749,750,764,825,828,829,860,892,906,940,945,954,972,974,984,1019,1028,1042,1047,1049,1051,1103,1104,1107,1122,1123,1127,1128,1130,1134,1137,1138','storenum':40},{'id':'23','name':'甜品饮料','storeids':'55,283,472,496,528,586,748,756,785,1039,1057,1126','storenum':12},{'id':'24','name':'日韩料理','storeids':'61,944','storenum':2},{'id':'35','name':'西餐','storeids':'507,519','storenum':2}],'count':73,'stores':[{'id':'1130','delivery_mode':'1','name':'绝味鸭脖（坊门街）','pic':'/qzxm/Uploads/2016-07-20/578f5220db9b8.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':17,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'54','delivery_mode':'1','name':'衢老高（原老高粉干）','pic':'/qzxm/Uploads/2015-11-18/564c574103ed2.png','fee':3,'minconsumption':'10','status':1,'ordernum':2478,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1049','delivery_mode':'1','name':'荣华凉皮','pic':'/qzxm/Uploads/2016-05-02/5726ffa333bf8.jpg','fee':8,'minconsumption':'10','status':1,'ordernum':93,'delivery_desc':'10元起送，配送费8元','district':0.5},{'id':'940','delivery_mode':'1','name':'蒸食膳','pic':'/qzxm/Uploads/2015-10-20/56261113deac1.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':340,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'55','delivery_mode':'1','name':'（衢州总店下街店）阿姨奶茶专卖（活动期间满二十送酸梅汤一杯）','pic':'/qzxm/Uploads/2015-01-18/54bb197f05490.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':2779,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'984','delivery_mode':'1','name':'瘸子实惠饭店','pic':'/qzxm/Uploads/2016-01-15/5698697f7a22f.jpeg','fee':3,'minconsumption':'20','status':1,'ordernum':322,'delivery_desc':'20元起送，配送费3元','district':0.5},{'id':'873','delivery_mode':'1','name':'（下午1：30 —— 4：30休息）好食代（原国良鸭头）','pic':'/qzxm/Uploads/2015-08-06/55c319ba31857.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':100,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'42','delivery_mode':'1','name':'余记烤饼（后街巷）','pic':'/qzxm/Uploads/2014-11-05/5459947e14968.gif','fee':3,'minconsumption':'10','status':1,'ordernum':8442,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'892','delivery_mode':'1','name':'斗潭粥铺（下午14：00--16：30休息）','pic':'/qzxm/Uploads/2015-08-28/55e007a9ad5c7.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':616,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1107','delivery_mode':'1','name':'（早餐-7-10点单配送，10-24点可接受预定）（新店上线）胖小妹肉圆，盖浇饭（下单满15送冷饮）','pic':'/qzxm/Uploads/2016-06-29/5773a67c643c1.JPG','fee':5,'minconsumption':'15','status':1,'ordernum':37,'delivery_desc':'15元起送，配送费5元','district':0.5},{'id':'144','delivery_mode':'1','name':'老焙粉干（斗潭东区）','pic':'/qzxm/Uploads/2014-10-11/54390c24bb8e7.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':3503,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'582','delivery_mode':'1','name':'传世老北京卤肉卷','pic':'/qzxm/Uploads/2015-01-20/54be4a3cadedc.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':1745,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'61','delivery_mode':'1','name':'（下街）鲜目录外带寿司','pic':'/qzxm/Uploads/2014-10-11/543907058c6ba.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':1595,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'750','delivery_mode':'1','name':'原新旺煲仔（后街巷）','pic':'/qzxm/Uploads/2015-05-10/554eda86c22f6.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':1361,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'50','delivery_mode':'1','name':'重庆酸辣粉（米兰对面弄堂棋坊巷）','pic':'/qzxm/Uploads/2015-03-26/55139e8245123.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':1285,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'828','delivery_mode':'1','name':'精武鸭脖（十年老店，卖鸭头的小伙子）','pic':'/qzxm/Uploads/2015-07-02/5594f0f23724f.jpg','fee':3,'minconsumption':'20','status':1,'ordernum':1220,'delivery_desc':'20元起送，配送费3元','district':0.5},{'id':'465','delivery_mode':'1','name':'杨家巷烤饼','pic':'/qzxm/Uploads/2015-06-27/558e697fd7eea.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':1193,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'283','delivery_mode':'1','name':'Rand B珍奶会所','pic':'/qzxm/Uploads/2014-07-30/53d8bdb7d7590.png','fee':3,'minconsumption':'10','status':1,'ordernum':1044,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'746','delivery_mode':'1','name':'爱之味铁板烧','pic':'/qzxm/Uploads/2015-05-09/554df6c441ffb.jpg','fee':3,'minconsumption':'20','status':1,'ordernum':1020,'delivery_desc':'20元起送，配送费3元','district':0.5},{'id':'528','delivery_mode':'1','name':'杨小贤','pic':'/qzxm/Uploads/2014-11-06/545b22a4edd30.gif','fee':3,'minconsumption':'10','status':1,'ordernum':996,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'499','delivery_mode':'1','name':'老杜酱果（斗潭东区）（下午14：00--16：00休息）','pic':'/qzxm/Uploads/2014-10-11/54390c683d25b.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':973,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'869','delivery_mode':'1','name':'傅家小厨（炒菜）','pic':'/qzxm/Uploads/2015-08-05/55c1cdd9857d0.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':839,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'748','delivery_mode':'1','name':'黑泷堂','pic':'/qzxm/Uploads/2015-05-09/554dff9b61159.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':793,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'749','delivery_mode':'1','name':'陆老爹','pic':'/qzxm/Uploads/2015-05-10/554f072f1ec8c.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':628,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'906','delivery_mode':'1','name':'拌面粥道','pic':'/qzxm/Uploads/2015-09-05/55ea9a7ef3490.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':492,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'829','delivery_mode':'1','name':'口口乐私房面馆','pic':'/qzxm/Uploads/2015-07-02/559500ae96c76.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':457,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1072','delivery_mode':'1','name':'（早餐-10点-24点接受预定）兰溪煎饺（钟楼底店）','pic':'/qzxm/Uploads/2016-05-22/5741978d44adf.jpg','fee':5,'minconsumption':'8','status':1,'ordernum':421,'delivery_desc':'8元起送，配送费5元','district':0.5},{'id':'588','delivery_mode':'1','name':'煎饼哥','pic':'/qzxm/Uploads/2015-06-23/55892fe4c890f.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':379,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1019','delivery_mode':'1','name':'馋嘴花甲（新桥街店）','pic':'/qzxm/Uploads/2016-03-09/56df977335d93.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':309,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'785','delivery_mode':'1','name':'甘茶度','pic':'/qzxm/Uploads/2015-06-19/5583db79ef2ea.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':304,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'825','delivery_mode':'1','name':'衢福记','pic':'/qzxm/Uploads/2015-07-02/5594e49e2421b.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':275,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'496','delivery_mode':'1','name':'muse手工烘焙','pic':'/qzxm/Uploads/2014-10-14/543cc8a9b38b2.gif','fee':3,'minconsumption':'10','status':1,'ordernum':258,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'534','delivery_mode':'1','name':'瘸子馄饨','pic':'/qzxm/Uploads/2014-11-20/546de100b1837.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':254,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'974','delivery_mode':'1','name':'（新店上线）骨汤麻辣烫（推荐）','pic':'/qzxm/Uploads/2015-12-14/566e65211d9b7.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':245,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'509','delivery_mode':'1','name':'飞鸿面馆','pic':'/qzxm/Uploads/2014-10-11/543906241486b.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':239,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'945','delivery_mode':'1','name':'武汉黑鸭','pic':'/qzxm/Uploads/2015-10-26/562de8074522f.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':197,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1039','delivery_mode':'1','name':'（新店上线）50岚（鲜茶专卖连锁）','pic':'/qzxm/Uploads/2016-04-26/571f363e53076.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':194,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'954','delivery_mode':'1','name':'渝口福重庆小面','pic':'/qzxm/Uploads/2015-11-09/56406d788433e.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':192,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'472','delivery_mode':'1','name':'柒寿司（果沫奶茶）','pic':'/qzxm/Uploads/2014-10-11/54390ad75571f.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':187,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'764','delivery_mode':'1','name':'重庆麻辣烫（十三香龙虾）','pic':'/qzxm/Uploads/2015-05-22/555edb2411222.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':102,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1090','delivery_mode':'1','name':'（早餐-7-10点单配送，10-24点可接受预定）乐行食','pic':'/qzxm/Uploads/2016-05-28/57491f81adc9b.jpg','fee':5,'minconsumption':'10','status':1,'ordernum':91,'delivery_desc':'10元起送，配送费5元','district':0.5},{'id':'586','delivery_mode':'1','name':'三公主奶茶','pic':'/qzxm/Uploads/2015-01-24/54c36fae07edb.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':90,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1051','delivery_mode':'1','name':'方记粥铺','pic':'/qzxm/Uploads/2016-05-08/572f09299d329.png','fee':3,'minconsumption':'10','status':1,'ordernum':89,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'756','delivery_mode':'1','name':'罗蒂爸爸','pic':'/qzxm/Uploads/2015-05-15/55558f82c68cd.jpg','fee':3,'minconsumption':'20','status':1,'ordernum':85,'delivery_desc':'20元起送，配送费3元','district':0.5},{'id':'507','delivery_mode':'1','name':'拉芳舍','pic':'/qzxm/Uploads/2014-10-11/5439066225598.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':70,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'519','delivery_mode':'1','name':'英爵咖啡','pic':'/qzxm/Uploads/2014-11-06/545b332c65ffb.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':69,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'944','delivery_mode':'1','name':'阿美丽韩式料理','pic':'/qzxm/Uploads/2015-10-25/562c782e47177.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':54,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'985','delivery_mode':'1','name':'小西门咖啡','pic':'/qzxm/Uploads/2016-01-21/56a0b97c547ab.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':54,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1042','delivery_mode':'1','name':'豪大大鸡排','pic':'/qzxm/Uploads/2016-04-30/5724698be55ff.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':54,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1057','delivery_mode':'1','name':'大麦Bake（衢州店）','pic':'/qzxm/Uploads/2016-05-22/574162e6c3ce9.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':48,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1083','delivery_mode':'1','name':'（早餐-7-10点单配送，10-24点可接受预定）红樱桃美食','pic':'/qzxm/Uploads/2016-05-25/574530b1ed1e1.png','fee':5,'minconsumption':'5','status':1,'ordernum':46,'delivery_desc':'5元起送，配送费5元','district':0.5},{'id':'1103','delivery_mode':'1','name':'重庆小面（中河沿）','pic':'/qzxm/Uploads/2016-06-19/57663a630b1de.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':46,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1012','delivery_mode':'1','name':'三毛煲店','pic':'/qzxm/Uploads/2016-03-07/56dd1964c245c.jpg','fee':3,'minconsumption':'68','status':1,'ordernum':39,'delivery_desc':'68元起送，配送费3元','district':0.5},{'id':'1067','delivery_mode':'1','name':'（早餐-10点-24点接受预定）肯德基早餐（香溢）','pic':'/qzxm/Uploads/2016-05-22/574187da2a8e8.jpg','fee':10,'minconsumption':'0','status':1,'ordernum':35,'delivery_desc':'0元起送，配送费10元','district':0.5},{'id':'1013','delivery_mode':'1','name':'肯德基（香溢店）','pic':'/qzxm/Uploads/2016-03-07/56dd1a7c46da9.jpg','fee':10,'minconsumption':'10','status':1,'ordernum':34,'delivery_desc':'10元起送，配送费10元','district':0.5},{'id':'515','delivery_mode':'1','name':'香雪面馆','pic':'/qzxm/Uploads/2014-10-14/543ccaaab3e47.gif','fee':3,'minconsumption':'10','status':1,'ordernum':26,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1022','delivery_mode':'1','name':'果缤纷','pic':'/qzxm/Uploads/2016-03-23/56f24791169dc.jpg','fee':3,'minconsumption':'15','status':1,'ordernum':21,'delivery_desc':'15元起送，配送费3元','district':0.5},{'id':'1104','delivery_mode':'1','name':'重庆小面（后街巷）','pic':'/qzxm/Uploads/2016-06-19/57663a7944055.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':21,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1127','delivery_mode':'1','name':'（新店上线）来自明洞（韩国炸鸡）','pic':'/qzxm/Uploads/2016-07-21/5790229ed9c48.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':19,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1047','delivery_mode':'1','name':'全旺山粉饺（裱背巷店）','pic':'/qzxm/Uploads/2016-05-02/57270025619db.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':17,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1122','delivery_mode':'1','name':'椿林麻辣烫（满十五送饮料一份）（另加打包费1元）','pic':'/qzxm/Uploads/2016-07-14/57870aaf7ac6d.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':11,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'983','delivery_mode':'1','name':'爱吧餐厅（正常）','pic':'/qzxm/Uploads/2016-01-06/568ccbee44d5a.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':7,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1126','delivery_mode':'1','name':'（新店上线）享廿茶','pic':'/qzxm/Uploads/2016-07-20/578ee8249d8e3.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':4,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1138','delivery_mode':'1','name':'（新店上线）遵义虾子张六羊肉粉','pic':'/qzxm/Uploads/2016-08-19/57b68fa70cc02.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':4,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1123','delivery_mode':'1','name':'重庆小面（日新巷）','pic':'/qzxm/Uploads/2016-07-14/57870ae0c1689.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':3,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1134','delivery_mode':'1','name':'（新店上线）开心花甲粉','pic':'/qzxm/Uploads/2016-08-06/57a5d0d2b66d1.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':1,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1139','delivery_mode':'1','name':'（早餐-7-10点单配送，10-24点可接受预定）遵义虾子张六羊肉粉','pic':'/qzxm/Uploads/2016-08-19/57b68f8b0e704.JPG','fee':5,'minconsumption':'10','status':1,'ordernum':1,'delivery_desc':'10元起送，配送费5元','district':0.5},{'id':'960','delivery_mode':'1','name':'大茴香咖啡','pic':'/qzxm/Uploads/2015-11-14/5646ff5461019.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':0,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1028','delivery_mode':'1','name':'虞家面食','pic':'/qzxm/Uploads/2016-04-02/56ff66934014f.jpg','fee':3,'minconsumption':'10','status':1,'ordernum':0,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1137','delivery_mode':'1','name':'（新店上线）蜀锦香厕所串串（另加1元打包费）（下午5:00——9:00休息）','pic':'/qzxm/Uploads/2016-08-19/57b69020a7d29.JPG','fee':3,'minconsumption':'10','status':1,'ordernum':0,'delivery_desc':'10元起送，配送费3元','district':0.5},{'id':'1128','delivery_mode':'1','name':'（新店上线）玲珑生煎馆','pic':'/qzxm/Uploads/2016-07-20/578f1e1cd3947.JPG','fee':5,'minconsumption':'10','status':0,'ordernum':95,'delivery_desc':'10元起送，配送费5元','district':0.5},{'id':'860','delivery_mode':'1','name':'虾大大（龙虾每份两斤起做）（龙虾3斤以内打包费3元。主食打包每份1元，凉菜打包每份1元）（下午4：00-凌晨1：00营业）','pic':'/qzxm/Uploads/2015-07-30/55b9e33042e5b.jpg','fee':5,'minconsumption':'30','status':0,'ordernum':75,'delivery_desc':'30元起送，配送费5元','district':0.5},{'id':'972','delivery_mode':'1','name':'金氏精武鸭脖（四季青旁边）','pic':'/qzxm/Uploads/2015-12-05/56629f5a8587c.jpg','fee':3,'minconsumption':'10','status':0,'ordernum':134,'delivery_desc':'10元起送，配送费3元','district':0.5}],'tag':1}";
                n = JsonConvert.DeserializeObject<Near>(json);

                foreach (var store in n.stores)
                {
                    string url = "http://www.xianmi.me/qzxm/index.php/Wap/Wm/getStore/memid/1/lng/138.854582/lat/28.968332.html?store_id=" + store.id;
                    string result = Request(url);

                    var sss = JsonConvert.DeserializeObject<Stores>(result);

                    store.dishes = sss.dishes;
                    store.types = sss.types;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(n.count);

            Console.ReadLine();
        }

        static string Request(string url)
        {
            StreamReader reader = null;
            HttpWebRequest hwr = WebRequest.Create(string.Format(url)) as HttpWebRequest;
            hwr.Method = "GET";
            var webResponse = hwr.GetResponse() as HttpWebResponse;
            if (webResponse.StatusCode == HttpStatusCode.OK && webResponse.ContentLength < 1024 * 1024)
            {
                Stream stream = webResponse.GetResponseStream();
                stream.ReadTimeout = 30000;
                if (webResponse.ContentEncoding == "gzip")
                {
                    reader = new StreamReader(stream, Encoding.Default);
                }
                else
                {
                    reader = new StreamReader(stream, Encoding.Default);
                }
                string html = reader.ReadToEnd();
                return HttpUtility.HtmlDecode(html);
            }
            return "";
        }

    }
}
