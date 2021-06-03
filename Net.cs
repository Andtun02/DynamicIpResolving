using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace DynamicIpResolving
{
    class Net
    {
        /*
         *  获取Token值 并 输出到根目录文件 "token.json" 
         */
        internal static void getToken()
        {
            string HuaweiID = AllVoid.readConfig("华为云账号");
            string HuaweiPw = AllVoid.readConfig("华为云密码");
            //定义变量
            string url = "https://iam.cn-east-2.myhuaweicloud.com/v3/auth/tokens";
            //Json参数
            string json = "{" +
                "    \"auth\": {" +
                "        \"identity\": {" +
                "            \"methods\": [" +
                "                \"password\"" +
                "            ]," +
                "            \"password\": {" +
                "                \"user\": {" +
                "                    \"domain\": {" +
                "                        \"name\": \""+HuaweiID+"\"" +
                "                    }," +
                "                    \"name\": \""+HuaweiID+"\"," +
                "                    \"password\": \""+HuaweiPw+"\"" +
                "                }" +
                "            }" +
                "        }," +
                "        \"scope\": {" +
                "            \"domain\": {" +
                "                \"name\": \""+HuaweiID+ "\"" +
                "}}}}";
            //建立Post
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf8";
            byte[] byteData = Encoding.UTF8.GetBytes(json);
            int length = byteData.Length;
            request.ContentLength = length;
            Stream writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            writer.Close();
            var response = (HttpWebResponse)request.GetResponse();
            var responseHeader = response.Headers["X-Subject-Token"];
            //写入token.js
            AllVoid.writeToken("getTime", DateTime.Now.ToString());
            AllVoid.writeToken("token", responseHeader.ToString());
        }


        /*
         * 更新域名解析
         */
        internal static void UpdateRecordSet(string reIP)
        {
            //定义变量
            string token = AllVoid.readToken("token");
            string zoneId = AllVoid.readConfig("zone_id");
            string recordsetId = AllVoid.readConfig("recordset_id");
            string url = "https://dns.cn-east-2.myhuaweicloud.com/v2/zones/" + zoneId + "/recordsets/" + recordsetId;
            //Json参数
            string json = "{" +
                "    \"records\": [" +
                "        \"" + reIP + "\"" +
                "    ]" +
                "}";
            //建立Put
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Headers.Add("X-Auth-Token", token);
            request.ContentType = "application/json";
            byte[] byteData = Encoding.UTF8.GetBytes(json);
            int length = byteData.Length;
            request.ContentLength = length;
            Stream writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            writer.Close();
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
            File.Delete("./DynamicIpResolving/UpdateRecordSet.json");
            File.WriteAllText("./DynamicIpResolving/UpdateRecordSet.json", responseString.ToString(), System.Text.Encoding.UTF8);//将内容写进jon文件中
        }

         /*
         * 获取本地IP外网地址
         */
        internal static string getLocalIP()
        {
            //获取外部IPURL
            string strUrl = "http://www.net.cn/static/customercare/yourip.asp";
            //string strUrl = "http://www.ip.cn/getip.php?action=getip&ip_url=&from=web";
            //string strUrl = "http://216.157.85.151/getip.php?action=getip&ip_url=&from=web";  
            Uri uri = new Uri(strUrl);
            WebRequest webreq = WebRequest.Create(uri);
            Stream s = webreq.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd();
            all = Regex.Replace(all, @"(\d+)", "000$1");
            all = Regex.Replace(all, @"0+(\d{3})", "$1");
            string reg = @"(\d{3}\.\d{3}\.\d{3}\.\d{3})";
            Regex regex = new Regex(reg);
            Match match = regex.Match(all);
            string ip = match.Groups[1].Value;
            AllVoid.writeConfig( "LocalIP", Regex.Replace(ip, @"0*(\d+)", "$1"));
            return Regex.Replace(ip, @"0*(\d+)", "$1");
        }

        /*
         * 发送邮件
         */
        internal static void sendEmail()
        {
            try
            {
                //实例化一个发送邮件类。
                MailMessage mailMessage = new MailMessage();
                //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
                mailMessage.From = new MailAddress("发件人邮箱");
                //收件人邮箱地址。
                mailMessage.To.Add(new MailAddress("邮箱"));
                if (AllVoid.readConfig("mail")!="null")
                {
                    mailMessage.To.Add(new MailAddress(AllVoid.readConfig("mail")));
                }
                //邮件标题。
                mailMessage.Subject = "DynamicIpResolving解析变更通知";
                //邮件内容。
                mailMessage.Body = "域名："+ AllVoid.readUpdateRecordSet("name") +"\n更新时间："+ DateTime.Now.ToString() + "\n本地IP由"+ AllVoid.getDomainIP() + "变更为：" + AllVoid.readConfig( "LocalIP");

                //实例化一个SmtpClient类。
                SmtpClient client = new SmtpClient();
                //在这里我使用的是yeah邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
                client.Host = "smtp.qq.com";
                //使用安全加密连接。
                client.EnableSsl = true;
                //不和请求一块发送。
                client.UseDefaultCredentials = false;
                //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
                client.Credentials = new NetworkCredential("账号", "密码");//前缀是指@之前的字符
                //发送
                client.Send(mailMessage);
                AllVoid.print("\t┠━ 邮件 -> 发送成功", "灰色", true);
            }
            catch (Exception ex)
            {
                AllVoid.print(ex.ToString(), "灰色", true);
            }
        }
    }
}
