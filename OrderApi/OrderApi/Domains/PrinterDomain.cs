using DataModels;
using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Domains
{
    public class PrinterDomain
    {
        public readonly string URL = "http://api.feieyun.cn/Api/Open/";
        public readonly string USER = "996736094@qq.com";
        public readonly string UKEY = "H6wDMjHPSKEuGyYm";
        private static PrinterDomain _current;
        public static PrinterDomain Current = _current ?? new PrinterDomain();


        public string addprinter(string snslist)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URL);
            req.Method = "POST";

            UTF8Encoding encoding = new UTF8Encoding();

            string postData = "printerContent=" + snslist;

            int itime = DateTimeToStamp(System.DateTime.Now);//时间戳秒数
            string stime = itime.ToString();
            string sig = sha1(USER, UKEY, stime);

            //公共参数
            postData += ("&user=" + USER);
            postData += ("&stime=" + stime);
            postData += ("&sig=" + sig);
            postData += ("&apiname=" + "Open_printerAddlist");

            byte[] data = encoding.GetBytes(postData);

            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            Stream resStream = req.GetRequestStream();

            resStream.Write(data, 0, data.Length);
            resStream.Close();

            HttpWebResponse response;
            string strResult;
            try
            {
                response = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                strResult = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                strResult = response.StatusCode.ToString();//错误信息
            }

            response.Close();
            req.Abort();

            return strResult;
        }

        public string clearPrintStatus(string account)
        {
            string sn = "";
            using(var db = new OrderDB())
            {
                sn = (from p in db.Shops
                         where p.ACCOUNT == account && p.STATE == 'A'
                         select p).FirstOrDefault()?.PrinterCode;
            }
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("当前用户未维护打印机");
            }
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URL);
            req.Method = "POST";

            UTF8Encoding encoding = new UTF8Encoding();

            string postData = "sn=" + sn;

            int itime = DateTimeToStamp(System.DateTime.Now);//时间戳秒数
            string stime = itime.ToString();
            string sig = sha1(USER, UKEY, stime);

            //公共参数
            postData += ("&user=" + USER);
            postData += ("&stime=" + stime);
            postData += ("&sig=" + sig);
            postData += ("&apiname=" + "Open_delPrinterSqs");

            byte[] data = encoding.GetBytes(postData);

            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            Stream resStream = req.GetRequestStream();

            resStream.Write(data, 0, data.Length);
            resStream.Close();

            HttpWebResponse response;
            string strResult;
            try
            {
                response = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                strResult = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                strResult = response.StatusCode.ToString();//错误信息
            }

            response.Close();
            req.Abort();

            return strResult;
        }


        public int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); return (int)(time - startTime).TotalSeconds;
        }

        //签名USER,UKEY,STIME
        public string sha1(string user, string ukey, string stime)
        {
            var buffer = Encoding.UTF8.GetBytes(user + ukey + stime);
            var data = SHA1.Create().ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString().ToLower();

        }


        //方法1
        public string print(PlaceAnOrder order, bool addDish)
        {
            var SN = "";
            var name = "";
            var capitation = "";
            var cost = 0M;
            using (var db = new OrderDB())
            {
                var sn = (from p in db.Shops where p.ACCOUNT == order.Account select p).FirstOrDefault();
                SN = sn.PrinterCode;
                name = sn.NAME;
                capitation = sn.CAPITATION;
                cost = sn.COST;
            }
            //标签说明：
            //单标签: 
            //"<BR>"为换行,"<CUT>"为切刀指令(主动切纸,仅限切刀打印机使用才有效果)
            //"<LOGO>"为打印LOGO指令(前提是预先在机器内置LOGO图片),"<PLUGIN>"为钱箱或者外置音响指令
            //成对标签：
            //"<CB></CB>"为居中放大一倍,"<B></B>"为放大一倍,"<C></C>"为居中,<L></L>字体变高一倍
            //<W></W>字体变宽一倍,"<QR></QR>"为二维码,"<BOLD></BOLD>"为字体加粗,"<RIGHT></RIGHT>"为右对齐

            //拼凑订单内容时可参考如下格式
            string orderInfo;
            orderInfo = $"<CB>{name}</CB>    {(addDish ? "加菜" : "")}<BR>";//标题字体如需居中放大,就需要用标签套上
            orderInfo += $"桌号:{order.DescNum}    用餐人数：{order.PersonNum}<BR>";
            orderInfo += "名称　　　　　 单价  数量 金额<BR>";
            orderInfo += "--------------------------------<BR>";
            var total = 0M;
            foreach (var ds in order.Foods)
            {
                using (var db = new OrderDB())
                {
                    var dtlName = (from p in db.FoodDetails where p.ID == ds.DETAIL_ID select p).FirstOrDefault();
                    if(dtlName is null)
                    {
                        throw new Exception("明细不存在！");
                    }
                    var food = (from p in db.Foods where p.ID == dtlName.FoodId select p).FirstOrDefault();
                    var nm = $"{food.NAME}";
                    if (!string.IsNullOrEmpty(dtlName.NAME)) nm += $"({dtlName.NAME})";

                    orderInfo += $"{GetNameWithSameLenght(nm, 14)}￥{decimal.Round(dtlName.PRICE.Value),-6}{ds.NUM,-3}￥{decimal.Round(dtlName.PRICE.Value) * ds.NUM}<BR>";
                    total += decimal.Round(dtlName.PRICE.Value * ds.NUM);
                }

            }
            if (!string.IsNullOrEmpty(capitation) && !addDish)
            {
                orderInfo += $"{GetNameWithSameLenght(capitation, 14)}￥{decimal.Round(cost),-6}{order.PersonNum,-3}￥{cost * order.PersonNum}<BR>";
                total += cost * order.PersonNum;
            }
            
            orderInfo += "--------------------------------<BR>";
            orderInfo += $"合计：{total}元<BR>";
            orderInfo += $"订餐时间：{DateTime.Now}<BR>";
            orderInfo += $"订单号：{order.OrderId}";
            //orderInfo += "----------请扫描二维码----------";
            //orderInfo += "<QR>http://www.dzist.com</QR>";//把二维码字符串用标签套上即可自动生成二维码
            orderInfo += "<BR>";
            orderInfo = Uri.EscapeDataString(orderInfo);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URL);
            req.Method = "POST";
            UTF8Encoding encoding = new UTF8Encoding();

            string postData = "sn=" + SN;
            postData += ("&content=" + orderInfo);
            postData += ("&times=" + "1");//默认1联

            int itime = DateTimeToStamp(System.DateTime.Now);//时间戳秒数
            string stime = itime.ToString();
            string sig = sha1(USER, UKEY, stime);

            //公共参数
            postData += ("&user=" + USER);
            postData += ("&stime=" + stime);
            postData += ("&sig=" + sig);
            postData += ("&apiname=" + "Open_printMsg");

            byte[] data = encoding.GetBytes(postData);

            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            Stream resStream = req.GetRequestStream();

            resStream.Write(data, 0, data.Length);
            resStream.Close();

            HttpWebResponse response;
            string strResult;
            try
            {
                response = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                strResult = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                strResult = response.StatusCode.ToString();//错误信息
            }

            response.Close();
            req.Abort();
            //服务器返回的JSON字符串，建议要当做日志记录起来
            return strResult;

        }

        public string reprint(PlaceAnOrder order)
        {
            var SN = "";
            var name = "";
            var capitation = "";
            var cost = 0M;
            using (var db = new OrderDB())
            {
                var sn = (from p in db.Shops where p.ACCOUNT == order.Account select p).FirstOrDefault();
                SN = sn.PrinterCode;
                name = sn.NAME;
                capitation = sn.CAPITATION;
                cost = sn.COST;
            }
            //标签说明：
            //单标签: 
            //"<BR>"为换行,"<CUT>"为切刀指令(主动切纸,仅限切刀打印机使用才有效果)
            //"<LOGO>"为打印LOGO指令(前提是预先在机器内置LOGO图片),"<PLUGIN>"为钱箱或者外置音响指令
            //成对标签：
            //"<CB></CB>"为居中放大一倍,"<B></B>"为放大一倍,"<C></C>"为居中,<L></L>字体变高一倍
            //<W></W>字体变宽一倍,"<QR></QR>"为二维码,"<BOLD></BOLD>"为字体加粗,"<RIGHT></RIGHT>"为右对齐

            //拼凑订单内容时可参考如下格式
            string orderInfo;
            orderInfo = $"<CB>{name}</CB><BR>";//标题字体如需居中放大,就需要用标签套上
            orderInfo += $"桌号:{order.DescNum}    用餐人数：{order.PersonNum}<BR>";
            orderInfo += "名称　　　　　 单价  数量 金额<BR>";
            orderInfo += "--------------------------------<BR>";
            var total = 0M;
            foreach (var ds in order.Foods)
            {
                using (var db = new OrderDB())
                {
                    orderInfo += $"{GetNameWithSameLenght(ds.DETAIL_NAME, 14)}￥{decimal.Round(ds.PRICE ?? 0),-6}{ds.NUM,-3}￥{decimal.Round(ds.PRICE.Value) * ds.NUM}<BR>";
                    total += decimal.Round(ds.PRICE.Value * ds.NUM);
                }

            }
            if (!string.IsNullOrEmpty(capitation))
            {
                orderInfo += $"{GetNameWithSameLenght(capitation, 14)}￥{decimal.Round(cost),-6}{order.PersonNum,-3}￥{cost * order.PersonNum}<BR>";
                total += cost * order.PersonNum;
            }
            orderInfo += "--------------------------------<BR>";
            orderInfo += $"合计：{total}元<BR>";
            orderInfo += $"订餐时间：{DateTime.Now}<BR>";
            orderInfo += $"订单号：{order.OrderId}";
            //orderInfo += "----------请扫描二维码----------";
            //orderInfo += "<QR>http://www.dzist.com</QR>";//把二维码字符串用标签套上即可自动生成二维码
            orderInfo += "<BR>";
            orderInfo = Uri.EscapeDataString(orderInfo);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URL);
            req.Method = "POST";
            UTF8Encoding encoding = new UTF8Encoding();

            string postData = "sn=" + SN;
            postData += ("&content=" + orderInfo);
            postData += ("&times=" + "1");//默认1联

            int itime = DateTimeToStamp(System.DateTime.Now);//时间戳秒数
            string stime = itime.ToString();
            string sig = sha1(USER, UKEY, stime);

            //公共参数
            postData += ("&user=" + USER);
            postData += ("&stime=" + stime);
            postData += ("&sig=" + sig);
            postData += ("&apiname=" + "Open_printMsg");

            byte[] data = encoding.GetBytes(postData);

            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            Stream resStream = req.GetRequestStream();

            resStream.Write(data, 0, data.Length);
            resStream.Close();

            HttpWebResponse response;
            string strResult;
            try
            {
                response = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                strResult = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                strResult = response.StatusCode.ToString();//错误信息
            }

            response.Close();
            req.Abort();
            //服务器返回的JSON字符串，建议要当做日志记录起来
            return strResult;

        }

        private string GetNameWithSameLenght(string str, int c)
        {
            int t = 0;
            foreach(var s in str)
            {
                if (char.IsControl(s) || char.IsDigit(s) || s == '(' || s == ')' || s >= 'A' && s <= 'z') t += 1;
                else t += 2;
            }
            while(t <= c)
            {
                str += " ";
                t += 1;
            }
            return str;
        }

    }
}
