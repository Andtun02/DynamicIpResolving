
using System;
using System.Threading;

namespace DynamicIpResolving
{
    class MainDIR
    {
        static void Main()
        {
            AllVoid.ShowLogo();
            AllVoid.print("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "暗黄色", true);
            AllVoid.print("· 运行 Boot", "绿色", true);
            if (Boot.run() == true)
            {
              AllVoid.print("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "暗黄色", true);
            //AllVoid.print(AllVoid.getDomainIP(), "红色", true);
            //AllVoid.print(AllVoid.getLocalIP(), "红色", true);
            AllVoid.print("· 运行 主程序", "绿色", true);
             Start();
            //Net.UpdateRecordSet("125.122.103.240");
            //AllVoid.print(AllVoid.readUpdateRecordSet("records"), "绿色", true);
            }
            AllVoid.print("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "暗黄色", true);
            Boot.end();
        }

        private static void Start()
        {
            AllVoid.print("\t初始化 信息：", "", true);
            AllVoid.print("\t┠━ 本地IP -> " + AllVoid.readConfig("LocalIP"), "灰色", true);
            AllVoid.print("\t┠━ 域名   -> " + AllVoid.readUpdateRecordSet("name"), "灰色", true);
            AllVoid.print("\t┠━ 域名IP -> " + AllVoid.getDomainIP(), "灰色", true);
            AllVoid.print("\t┠━ Token获取时间 -> " + AllVoid.readToken("getTime"), "灰色", true);
            AllVoid.print("\t┠━ 系统当前时间  -> " + DateTime.Now.ToString(), "灰色", true);
            AllVoid.print("\t┖━ [完成]", "", true);


            do
            {
                string nowLocalIP = Net.getLocalIP();
                string nowDomainIP = AllVoid.getDomainIP();
                string nowTokenTime = AllVoid.readToken("getTime");
                string sysTime = DateTime.Now.ToString();
                TimeSpan nowChaTime = DateTime.Parse(sysTime) - DateTime.Parse(nowTokenTime);
                AllVoid.print("\t循环任务 | 解析：", "", true);
                if (nowDomainIP != nowLocalIP)
                {
                    AllVoid.print("\t┠━ 本地IP -> " + nowLocalIP, "灰色", true);
                    AllVoid.print("\t┠━ 域名IP -> " + nowDomainIP, "灰色", true);
                    if (nowChaTime.Days >= 1)
                    {
                        AllVoid.print("\t┠━ Token逾期", "红色", true);
                        Net.getToken();
                        AllVoid.print("\t┠━ Token获取时间 -> " + nowTokenTime, "灰色", true);
                        AllVoid.print("\t┠━ Token值 -> " + AllVoid.readToken("token"), "灰色", true);
                    }
                    else
                    {
                        AllVoid.print("\t┠━ 本地IP变动", "红色", true);
                        Net.sendEmail();
                        AllVoid.print("\t┠━ 本地IP获取时间 -> " + sysTime, "灰色", true);
                        AllVoid.print("\t┠━ 更新解析地址 -> " + nowLocalIP, "灰色", true);
                        Net.UpdateRecordSet(nowLocalIP);
                    }
                }
                else
                {
                    AllVoid.print("\t┠━ 正常 | 等待进入下一次循环[3分钟]", "绿色", true);
                    Thread.Sleep(60000);
                    AllVoid.print("\t┠━ 待机 | 等待进入下一次循环[2分钟]", "灰色", true);
                    Thread.Sleep(60000);
                    AllVoid.print("\t┠━ 待机 | 等待进入下一次循环[1分钟]", "灰色", true);
                    Thread.Sleep(60000);
                    AllVoid.print("\t┠━ 待机 | 即将进入下一次循环..", "灰色", true);
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("   _             _ _                _              \n  /_\\  _ __   __| | |_ _   _ _ __  | |_ ___  _ __  \n //_\\| '_ \\ / _` | __| | | | '_ \\ | __/ _ \\| '_ \\ \n/  _  \\ | | | (_| | |_| |_| | | | || || (_) | |_) |\n\\_/ \\_/_| |_|\\__,_|\\__|\\__,_|_| |_(_)__\\___/| .__/ \n                                            |_|    ");
                    
                }
            } while (true);
            
        }
    }
}
