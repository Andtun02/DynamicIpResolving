using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicIpResolving
{
    class AllVoid
    {
        //全局变量
        //控制台名称
        public static string title = "HuaweiDNS-DynamicIpResolving";
        public static string tokenJsonPath = "./DynamicIpResolving/token.json";
        public static string configJsonPath = "./DynamicIpResolving/config.json";
        public static string updateRecordSetPath = "./DynamicIpResolving/UpdateRecordSet.json";

        /*
         * 打印文字并记录log
         */
        static string timeNow;      //时间 && 文件名
        internal static void print(string context, string color, bool tab)
        {
            timeNow = DateTime.Now.ToString();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n[" + timeNow + "] ");
            Console.ForegroundColor = color switch
            {
                "" => ConsoleColor.White,
                "红色" => ConsoleColor.Red,
                "黑色" => ConsoleColor.Black,
                "蓝色" => ConsoleColor.Blue,
                "青色" => ConsoleColor.Cyan,
                "绿色" => ConsoleColor.Green,
                "黄色" => ConsoleColor.Yellow,
                "灰色" => ConsoleColor.Gray,
                "暗灰色" => ConsoleColor.DarkGray,
                "洋红色" => ConsoleColor.Magenta,
                "暗红色" => ConsoleColor.DarkRed,
                "暗黄色" => ConsoleColor.DarkYellow,
                _ => ConsoleColor.White,
            };
            Console.Write(context);

            if (!Directory.Exists("./DynamicIpResolving/logs"))
                Directory.CreateDirectory("./DynamicIpResolving/logs");
            StreamWriter sw = new StreamWriter("./DynamicIpResolving/logs/" + DateTime.Now.ToString("yyyy_MM_dd") + ".log", true);
            if (tab)
            {
                sw.WriteLine("[" + timeNow + "] " + context);
            }
            else
            {
                sw.Write("[" + timeNow + "] " + context);
            }
            sw.Flush();
            sw.Close();
        }

        /*
         * 展示LOGO
         */
        internal static void ShowLogo()
        {
            //设置控制台标题为 变量title
            Console.Title = title;
            Console.WindowHeight = 40;
            Console.WindowWidth = 80;
            //打印LOGO
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("   _             _ _                _              \n  /_\\  _ __   __| | |_ _   _ _ __  | |_ ___  _ __  \n //_\\| '_ \\ / _` | __| | | | '_ \\ | __/ _ \\| '_ \\ \n/  _  \\ | | | (_| | |_| |_| | | | || || (_) | |_) |\n\\_/ \\_/_| |_|\\__,_|\\__|\\__,_|_| |_(_)__\\___/| .__/ \n                                            |_|    ");
            AllVoid.print("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "暗灰色", true);
            AllVoid.print("\t\t华为云域名动态IP解析 V1.0", "青色", true);
            AllVoid.print("\t\tHuaweiDNS-DynamicIpResolving", "青色", true);
            AllVoid.print("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "暗灰色", true);
        }

        /*
         * 读取token.json
         */
        internal static string readToken(string b)
        {
            StreamReader file = File.OpenText(tokenJsonPath);
            JsonTextReader reader = new JsonTextReader(file);
            JObject jsonObject = (JObject)JToken.ReadFrom(reader);
            file.Close();
            string result = jsonObject["DynamicIpResolving"][0][b].ToString();
            return result;
        }

        /*
         * 写入token.json
         */
        internal static void writeToken(string b, string c)
        {
            string jsonString = File.ReadAllText(tokenJsonPath, System.Text.Encoding.UTF8);//读取文件
            JObject jobject = JObject.Parse(jsonString);//解析成json
            jobject["DynamicIpResolving"][0][b] = c;//替换需要的文件
            string convertString = Convert.ToString(jobject);//将json装换为string
            File.WriteAllText(tokenJsonPath, convertString, System.Text.Encoding.UTF8);//将内容写进jon文件中
        }

        /*
         * 创建token.json
         */
        internal static void createToken()
        {
            FileStream tokenJson = new FileStream(tokenJsonPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            tokenJson.Close();
            string initial = "{\n    \"DynamicIpResolving\":\n    [\n      {\n        \"getTime\": \"null\",\n      }\n    ]\n}";
            File.WriteAllText(tokenJsonPath, initial, System.Text.Encoding.UTF8);//将内容写进jon文件中
            AllVoid.writeToken( "getTime", DateTime.Now.ToString());
            AllVoid.writeToken("token", "null");
        }

        /*
        * 读取config.json
        */
        internal static string readConfig(string b)
        {
            StreamReader file = File.OpenText(configJsonPath);
            JsonTextReader reader = new JsonTextReader(file);
            JObject jsonObject = (JObject)JToken.ReadFrom(reader);
            string result = jsonObject["DynamicIpResolving"][0][b].ToString();
            file.Close();
            return result;
        }

        /*
         * 写入config.json
         */
        internal static void writeConfig(string b, string c)
        {
            string jsonString = File.ReadAllText(configJsonPath, System.Text.Encoding.UTF8);//读取文件
            JObject jobject = JObject.Parse(jsonString);//解析成json
            jobject["DynamicIpResolving"][0][b] = c;//替换需要的文件
            string convertString = Convert.ToString(jobject);//将json装换为string
            File.WriteAllText(configJsonPath, convertString, System.Text.Encoding.UTF8);//将内容写进jon文件中
        }

        /*
         * 创建config.json
         */
        internal static void createConfig()
        {
            FileStream tokenJson = new FileStream(configJsonPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            tokenJson.Close();
            string initial = "{\n    \"DynamicIpResolving\":\n    [\n      {\n        \"getTime\": \"null\",\n      }\n    ]\n}";
            File.WriteAllText(configJsonPath, initial, System.Text.Encoding.UTF8);//将内容写进jon文件中
            AllVoid.writeConfig( "getTime", DateTime.Now.ToString());
            AllVoid.writeConfig("recordset_id", "null");
            AllVoid.writeConfig( "zone_id", "null");
            AllVoid.writeConfig("华为云账号", "null");
            AllVoid.writeConfig("华为云密码", "null");
            AllVoid.writeConfig("mail", "null");
        }

        /*
         * 读取UpdateRecordSet.json
         */
        internal static string readUpdateRecordSet(string a)
        {
            using System.IO.StreamReader file = System.IO.File.OpenText(updateRecordSetPath);
            using JsonTextReader reader = new JsonTextReader(file);
            JObject o = (JObject)JToken.ReadFrom(reader);
            var value = o[a].ToString();
            return value;
        }


        /*
         * 创建UpdateRecordSet.json
         */
        internal static void createUpdateRecordSet()
        {
            FileStream updateRecordSetJson = new FileStream(updateRecordSetPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            updateRecordSetJson.Close();
            string initial = "{\n    \"DynamicIpResolving\":\n    [\n      {\n        \"getTime\": \"null\",\n      }\n    ]\n}";
            File.WriteAllText(updateRecordSetPath, initial, System.Text.Encoding.UTF8);//将内容写进jon文件中

        }


        /*
         * 获取域名IP地址
         */
        internal static string getDomainIP()
        {
            try
            {
                StreamReader file = File.OpenText(updateRecordSetPath);
                JsonTextReader reader = new JsonTextReader(file);
                JObject jsonObject = (JObject)JToken.ReadFrom(reader);
                file.Close();
                string result = jsonObject["records"][0].ToString();
                return result;
            }
            catch
            {
                return "error";
            }
        }
    }
}

