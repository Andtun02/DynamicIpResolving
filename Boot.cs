using System;
using System.IO;

namespace DynamicIpResolving
{
    class Boot
    {
        /*
         *  软件  运行
         */
        internal static bool run()
        {
            //检测是否首次运行
            if (first() == true)
            {
                if (File.Exists("./DynamicIpResolving/UpdateRecordSet.json") == false)
                {
                    AllVoid.print("┎━ 创建配置文件：", "", true);
                    createFile();
                }
                //此处应判断是否config填写信息否则不执行解析
                if (AllVoid.readConfig("recordset_id") !="null" && AllVoid.readConfig("zone_id")!="null" && AllVoid.readConfig("华为云账号") != "null" && AllVoid.readConfig("华为云密码") != "null")
                {
                    AllVoid.print("\t┠━ 获取 token", "灰色", true);
                    Net.getToken();
                    AllVoid.print("\t┠━ 获取 本地IP", "灰色", true);
                    Net.getLocalIP();
                    string ip = AllVoid.readConfig( "LocalIP");
                    AllVoid.print("\t┠━ 更新解析 指向本地 -> " + ip, "灰色", true);
                    Net.UpdateRecordSet(ip);
                    AllVoid.print("\t┖━ [完成]", "", true);
                    return true;
                }
                else
                {
                    AllVoid.print("\t┖━ [错误] 请检查并更改 config.json 中的null项", "红色", true);
                    return false;
                }
            }
            return true;
        }
        /*
         *  软件  结束
         */
        internal static void end()
        {
            AllVoid.print("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "暗灰色", true);
            AllVoid.print("\t\t软件已经关闭", "暗红色", true);
            AllVoid.print("\t\t按任意键关闭窗口", "暗红色", true);
            AllVoid.print("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "暗灰色", true);
            Console.ReadKey();
        }


        /*
         *  是否首次运行 （是==true || 否==false）
         */
        internal static bool first()
        {
            AllVoid.print("\t检测 首次运行：","", true);
            if (File.Exists("./DynamicIpResolving/token.json") == true && AllVoid.readToken("token") != "null")
            {
                    AllVoid.print("\t┖━ [否]", "灰色", true);
                    return false;
            }
            else
            {
                AllVoid.print("\t┠━ [是]", "灰色", true);
                return true;
            }
        }

        /*
         *  创建配置文件
         */
        internal static void createFile()
        {
            AllVoid.print("┠━ 创建文件夹 DynamicIpResolving", "灰色", true);
            Directory.CreateDirectory("./DynamicIpResolving");
            AllVoid.print("┠━ 创建文件 token.json", "灰色", true);
            AllVoid.createToken();
            AllVoid.print("┠━ 创建文件 config.json", "灰色", true);
            AllVoid.createConfig();
            AllVoid.print("┠━ 创建文件 UpdateRecordSet.json", "灰色", true);
            AllVoid.createUpdateRecordSet();
        }
    }
}
