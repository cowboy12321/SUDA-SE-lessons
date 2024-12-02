using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using cn.edu.suda.sumcu.iot.data;
/*
【不动】
 在更改项目时，本文件中的代码不需要做修改
 */
//数据库操作类主要针对iot应用开发，故放在cn.edu.suda.sumcu.iot空间下的util子空间中
namespace cn.edu.suda.sumcu.iot.util
{
    ///---------------------------------------------------------------------
    /// <summary>                                                           
    /// 类          ：SQLCommand                               
    /// 类   功   能：提供基本的sql操作接口
    /// 类中接口包含：
    ///             (1)selectFrames：     选择所有帧
    ///             
    ///             (1)insertFrame：      插入一帧
    ///             
    ///             (3)deletetFrames：    删除所有帧
    /// </summary>                                                                                                       
    /// --------------------------------------------------------------------
    public class SQLCommand
    {
        private String connectString;          //数据库连接字符串
        private string tableName;              //表名

        /// -------------------------------------------------------------------
        /// <summary>        
        ///函数名称：SQLCommand                                                  
        ///功能概要：初始化函数，初始化要访问数据库的连接字符串、表名；
        ///          此函数中提供了三种设置连接字符串的方法，分别为：
        ///          1.相对路径访问数据库
        ///          2.绝对路径访问数据库
        ///          3.通过SQL Server访问数据库
        ///说    明：
        /// </summary>
        /// <param name="connect">数据库连接字符串</param>
        /// <param name="tn">绑定本对象的表名</param>
        /// -------------------------------------------------------------------
        public SQLCommand(string connect, string tn)
        {
            //1.设置表名
            this.tableName = tn;
            //2.获取数据库连接字符串
            string connectionString = connect;
            //3.将相对地址转为绝对地址
            //获取字符串"|DataDirectory|"所在的下标
            int indexStart = connectionString.IndexOf("|DataDirectory|");
            if (indexStart >= 0)    //若存在字符串"|DataDirectory|"
            {
                //获得相对地址的结束下标
                int indexEnd = connectionString.IndexOf(";", indexStart);
                //获得相对地址值
                string addr = connectionString.Substring(indexStart, indexEnd - indexStart);
                //获得程序当前的运行路径
                string ddataDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                ddataDir = ddataDir.Remove(ddataDir.Length - 1);
                //使用当前运行路径替换掉字符串"|DataDirectory|"
                string addrNew = addr.Replace("|DataDirectory|", ddataDir);
                //获得替换后的绝对地址路径
                addrNew = Path.GetFullPath(addrNew);
                //将新的绝对地址替换掉原来的相对地址
                connectionString = connectionString.Replace(addr, addrNew);
            }
            this.connectString = connectionString;
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectFrames
        ///  功能概要：根据帧格式的名称（name）选择表中所有帧
        ///  内部调用：无
        ///  <summary> 
        /// <param name="fd">帧结构变量</param>
        ///  <returns>包含所有帧的数据表</returns>
        /// -------------------------------------------------------------------
        public DataTable selectFrames(FrameData fd)
        {
            string str;
            str = "select ";
            int i;
            //对帧结构内每个字段
            for (i = 0; i < fd.Parameter.Count; i++)
            {
                str += fd.Parameter[i].name + ",";     //向语句中加入字段名   
            }
            str = str.Remove(str.Length - 1);          //去掉结尾的","
            str += " from " + tableName;               //选取表中所有数据的语句
            return this.ExecuteQueryReturnDT(str);     //执行语句，返回结果
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectNeed
        ///  功能概要：选择需要的记录。可根据指定字段的值进行查找。本重载仅支持
        ///            查询一个字段，但是其值可以有多个，使用“，”号分隔，多个
        ///            多个值之间的连接关系为“or（或）”
        ///  <summary> 
        /// <param name="column">字段名</param>
        /// <param name="value">需要查询的字段名对应的值，可用','号隔开</param>
        ///  <returns>包含符合查询条件的数据表</returns>
        /// -------------------------------------------------------------------
        public DataTable selectNeed(string column, string value)
        {
            string str;
            str = "select ";
            int i;
            string[] Value = value.Split(',');
            str = "select * from " + tableName;
            if (value == "*")
                return this.ExecuteQueryReturnDT(str);   //执行语句，返回结果
            str += " where ";
            for (i = 0; i < Value.Count(); i++)
            {
                if (Value[i] == "*") continue;
                str += column + " = '" + Value[i] + "' or ";
            }
            str = str.Remove(str.Length - 4);        //去除最后的“ or ”
            return this.ExecuteQueryReturnDT(str);   //执行语句，返回结果
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：count
        ///  功能概要：获得数据库表中记录的总行数
        ///  <summary> 
        ///  <returns>当前表的总行数，若返回-1说明获取失败</returns>
        /// -------------------------------------------------------------------
        public int count()
        {
            DataTable dd = this.ExecuteQueryReturnDT("select count(*) from " + this.tableName);
            try
            {
                return Convert.ToInt32(dd.Rows[0][0]);
            }
            catch
            {
                return -1;
            }
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：getRowNum
        ///  功能概要：通过ID号获得该记录在表中的行数
        ///  <summary> 
        ///  <returns>行数</returns>
        /// -------------------------------------------------------------------
        public int getRowNum(int ID)
        {
            string str;
            str = "select count(*) from " + this.tableName + " where ID <= " + ID.ToString();
            DataTable dd = this.ExecuteQueryReturnDT(str);//执行语句，返回结果
            if (dd.Rows.Count > 0)
                return Convert.ToInt32(dd.Rows[0][0].ToString());
            else
                return -1;
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectRowByIMSI
        ///  功能概要：通过imsi号获得该记录在表中的10条数据数据
        ///  <summary> 
        ///  <returns>行数</returns>
        /// -------------------------------------------------------------------
        public DataTable selectRowByIMSI(string imsi)
        {

            try
            {
                string str = @"
SELECT *
FROM (
    SELECT top 10 *
    FROM {0}
    WHERE imsi={1}
    ORDER BY currentTime DESC
) temp
ORDER BY currentTime
";
                return this.ExecuteQueryReturnDT(string.Format(str, this.tableName, "'" + imsi + "'"));   //执行语句，返回结果
            }   //"USER_TABLES"
            catch { return null; }
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectRowByIMSIHmonth
        ///  功能概要：通过imsi号获得该记录在表中的近半个月的数据
        ///  <summary> 
        ///  <returns>行数</returns>
        /// -------------------------------------------------------------------
        public DataTable selectRowByIMSIHmonth(string imsi)
        {
            string strCount = @"SELECT * FROM HisTable";
            DataTable DT = this.ExecuteQueryReturnDT(strCount);
            if (DT.Rows.Count < 1)
                return null;
            string strDable = @"SELECT top 1 Name FROM HisTable ORDER BY Time DESC";
            DataTable NT = this.ExecuteQueryReturnDT(strDable);
            string str1 = "";
            for (var i = 0; i < NT.Rows.Count; i++)
            {
                
                str1 += "SELECT * FROM " + NT.Rows[i].ItemArray[0] + " WHERE imsi={0}";
                if (i != NT.Rows.Count - 1)
                    str1 += " union all ";
            }
            try
            {
                return this.ExecuteQueryReturnDT(string.Format(str1, "'" + imsi + "'"));  //执行语句，返回结果
            }
            catch
            {
                return null;
            }
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectRowByIMSIMonth
        ///  功能概要：通过imsi号获得该记录在表中的一个月的数据
        ///  <summary> 
        ///  <returns>行数</returns>
        /// -------------------------------------------------------------------
        public DataTable selectRowByIMSIMonth(string imsi)
        {
            string strTable = "";
            string str1 = "";
            string strCount = @"SELECT * FROM HisTable";
            DataTable DT = this.ExecuteQueryReturnDT(strCount);
            if(DT.Rows.Count<3)
                strTable = @"SELECT Name FROM HisTable ORDER BY Time DESC";
            else 
                strTable = @"SELECT top 3 Name FROM HisTable ORDER BY Time DESC";
            DataTable NT = this.ExecuteQueryReturnDT(strTable);
            for (var i = 0; i < NT.Rows.Count; i++)
            {

                str1 += "SELECT * FROM " + NT.Rows[i].ItemArray[0] + " WHERE imsi={0}";
                if (i != NT.Rows.Count - 1)
                    str1 += " union all ";
            }
            try
            {
                return this.ExecuteQueryReturnDT(string.Format(str1, "'" + imsi + "'"));  //执行语句，返回结果
            }
            catch
            {
                return null;
            }
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectRowByIMSIHyear
        ///  功能概要：通过imsi号获得该记录在表中的近半年的数据
        ///  <summary> 
        ///  <returns>行数</returns>
        /// -------------------------------------------------------------------
        public DataTable selectRowByIMSIHyear(string imsi)
        {
            string strTable = "";
            string str1 = "";
            string strCount = @"SELECT * FROM HisTable";
            DataTable DT = this.ExecuteQueryReturnDT(strCount);
            if (DT.Rows.Count < 19)
                strTable = @"SELECT Name FROM HisTable ORDER BY Time DESC";
            else
                strTable = @"SELECT top 19 Name FROM HisTable ORDER BY Time DESC";

            DataTable NT = this.ExecuteQueryReturnDT(strTable);
            for (var i = 0; i < NT.Rows.Count; i++)
            {
                str1 += "SELECT * FROM " + NT.Rows[i].ItemArray[0] + " WHERE imsi={0}";
                if (i != NT.Rows.Count - 1)
                    str1 += " union all ";
            }
            try
            {
                return this.ExecuteQueryReturnDT(string.Format(str1, "'" + imsi + "'"));  //执行语句，返回结果
            }
            catch
            {
                return null;
            }
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectRowByIMSIYear
        ///  功能概要：通过imsi号获得该记录在表中的近一年的数据
        ///  <summary> 
        ///  <returns>行数</returns>
        /// -------------------------------------------------------------------
        public DataTable selectRowByIMSIYear(string imsi)
        {
            string strTable = "";
            string str1 = "";
            string strCount = @"SELECT * FROM HisTable";
            DataTable DT = this.ExecuteQueryReturnDT(strCount);
            if (DT.Rows.Count < 36)
                strTable = @"SELECT Name FROM HisTable ORDER BY Time DESC";
            else
                strTable = @"SELECT top 36 Name FROM HisTable ORDER BY Time DESC";

            DataTable NT = this.ExecuteQueryReturnDT(strTable);
            for (var i = 0; i < NT.Rows.Count; i++)
            {

                str1 += "SELECT * FROM " + NT.Rows[i].ItemArray[0] + " WHERE imsi={0}";
                if (i != NT.Rows.Count - 1)
                    str1 += " union all ";
            }
            try
            {
                return this.ExecuteQueryReturnDT(string.Format(str1, "'" + imsi + "'"));  //执行语句，返回结果
            }
            catch
            {
                return null;
            }
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectLast
        ///  功能概要：选择数据库表的最后一条记录
        ///  <summary> 
        ///  <returns>据库表的最后一条记录</returns>
        /// -------------------------------------------------------------------
        public DataTable selectLast()
        {
            string str;
            str = "select top 1 * from " + this.tableName + " order by ID desc";
            return this.ExecuteQueryReturnDT(str);   //执行语句，返回结果
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectIMSIlist【20200813】zw
        ///  功能概要：选择数据库表的所有的IMSI号
        ///  <summary> 
        ///  <returns>据库表的所有的IMSI号</returns>
        /// -------------------------------------------------------------------
        public DataTable selectIMSIlist()
        {
            string str;
            str = "select distinct IMSI from Up";
            return this.ExecuteQueryReturnDT(str);   //执行语句，返回结果

        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectIMSI【20200808】zw
        ///  功能概要：选择数据库表的指定IMSI号的记录
        ///  <summary> 
        ///  <returns>据库表的指定IMSI号的记录</returns>
        /// -------------------------------------------------------------------
        public DataTable selectIMSI(String IMSI)
        {
            string str;
            str = "select currentTime,MCUtemp,signalPower,cmd from Up where IMSI =" + IMSI;
            return this.ExecuteQueryReturnDT(str);   //执行语句，返回结果

        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：szh_select
        ///  功能概要：wifi-zigbee项目使用，获取数据库中的gateway，node，sensortype，data
        ///  <summary> 
        ///  <returns>=</returns>
        /// -------------------------------------------------------------------
        public DataTable szh_select(String name)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            str = "select TOP(1) gatewayaddr,gatewayhardaddr,nodetype1,nodeaddr1,nodedata1,upperbound1,lowerbound1,nodetype2,nodeaddr2,nodedata2,nodetype3,nodeaddr3,nodedata3,nodetype4,nodeaddr4,nodedata4,nodetype5,nodeaddr5,nodedata5,nodetype6,nodeaddr6,nodedata6,nodetype7,nodeaddr7,nodedata7,nodetype8,nodeaddr8,nodetype9,nodeaddr9,nodedata9,nodetype10,nodeaddr10,nodedata10,upperbound2,lowerbound2,upperbound3,lowerbound3,upperbound4,lowerbound4,upperbound5,lowerbound5,upperbound6,lowerbound6,upperbound7,lowerbound7,upperbound8,lowerbound8,upperbound9,lowerbound9,upperbound10,lowerbound10 from Up where gatewayhardaddr = '" + name + "'  order by ID desc";

            dataTable = this.ExecuteQueryReturnDT(str);
            return dataTable;

        }


        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：szhstatedt_select
        ///  功能概要：wifi-zigbee项目使用，获取数据库中的gateway，node，sensortype，data
        ///  <summary> 
        ///  <returns>=</returns>
        /// -------------------------------------------------------------------
        public DataTable szhstatedt_select(String name)
        {
            DataTable dataTable = new DataTable();
            string str = "";
            str = "select top(1) gatewayaddr,gatewayhardaddr,node1,node2,node3,node4,node5,node6,node7,node8,node9,node10,iotime from State where gatewayhardaddr = '" + name + "' order by iotime desc";
            dataTable = this.ExecuteQueryReturnDT(str);
            return dataTable;

        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：szh_createtable
        ///  功能概要：wifi-zigbee项目使用，创建State表
        ///  <summary> 
        ///  <returns>执行结果</returns>
        /// -------------------------------------------------------------------
        public int szh_createtable()
        {
            string sqlstr = "CREATE TABLE State(gatewayaddr nvarchar(100),gatewayhardaddr nvarchar(10),node1 nvarchar(10),node2 nvarchar(10),node3 nvarchar(10),node4 nvarchar(10),node5 nvarchar(10),node6 nvarchar(10),node7 nvarchar(10),node8 nvarchar(10),node9 nvarchar(10),node10 nvarchar(10),iotime nvarchar(50),CONSTRAINT onetime UNIQUE (iotime))";
            return ExecuteNonQuery(sqlstr);
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：szh_selectmaxdate
        ///  功能概要：wifi-zigbee项目使用，创建State表
        ///  <summary> 
        ///  <returns>执行结果</returns>
        /// -------------------------------------------------------------------
        public DataTable szh_selectmaxdate()
        {
            string sqlstr = "select max(iotime) from State";
            return ExecuteQueryReturnDT(sqlstr);
        }

        public int szh_insertdate(String str)
        {
            return ExecuteNonQuery(str);
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：del24hourdata
        ///  功能概要：wifi-zigbee项目使用，删除24小时至48小时的正常数据
        ///  参数说明：yesterdaya昨天开始时间,yesterdayb昨天结束时间，格式例子："20191016240000"
        ///  <summary> 
        ///  <returns>执行结果</returns>
        /// -------------------------------------------------------------------
        public void del24hourdata(string yesterdaya, string yesterdayb)
        {
            bool flag = true;
            string sqlstr = "select * from Up where currentTime>=" + yesterdaya + " and currentTime<=" + yesterdayb;
            DataTable temp = this.ExecuteQueryReturnDT(sqlstr);
            string id = "";
            int data = 0, dataup = 0, datalow = 0;
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                flag = true;
                id = temp.Rows[i][19].ToString();
                for (int j = 0; j < 10; j++)
                {
                    data = int.Parse(temp.Rows[i][63 + j * 4].ToString());
                    switch (temp.Rows[i][60 + j * 4].ToString())
                    {
                        case "80":
                            dataup = int.Parse(temp.Rows[i][129 + j * 2].ToString()) * 100;
                            datalow = int.Parse(temp.Rows[i][130 + j * 2].ToString()) * 100;
                            break;
                        case "71":
                            dataup = int.Parse(temp.Rows[i][129 + j * 2].ToString()) * 100;
                            datalow = int.Parse(temp.Rows[i][130 + j * 2].ToString()) * 100;
                            break;
                        case "84":
                            dataup = int.Parse(temp.Rows[i][129 + j * 2].ToString()) * 10;
                            datalow = int.Parse(temp.Rows[i][130 + j * 2].ToString()) * 10;
                            break;
                        default:
                            dataup=999999;
                            datalow = 0;
                            break;
                    }

                    if (dataup != 0 || datalow != 0)
                    {
                        if (data < datalow || data > dataup)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)//数据正常，删除该行
                {
                    sqlstr = "delete from Up where ID=" + id;
                    this.ExecuteNonQuery(sqlstr);
                }
            }
        }



        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectRow
        ///  功能概要：选择数据库表的指定一行记录
        ///  <summary> 
        ///  <returns>据库表的指定一行记录</returns>
        /// -------------------------------------------------------------------
        public DataTable selectRow(int rowcount)
        {
            try
            {
                DataTable temp = new DataTable();
                string str;
                str = "select * from (select * , number = row_number() over(order by ID) from "
                    + this.tableName + ") m where number =" + rowcount.ToString();
                temp = this.ExecuteQueryReturnDT(str);   //执行语句，返回结果
                return temp;
            }
            catch { return null; }
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectID
        ///  功能概要：选择数据库表的指定ID号的记录
        ///  <summary> 
        ///  <returns>据库表的指定ID号的记录</returns>
        /// -------------------------------------------------------------------
        public DataTable selectID(int id)
        {
            string str;
            str = "select * from " + this.tableName + " where ID = " + id.ToString();
            return this.ExecuteQueryReturnDT(str);   //执行语句，返回结果
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：selectNeed
        ///  功能概要：选择需要的记录。字段数组的每个成员表示一个字段，其对应的  
        ///            Value数组的内容为字段的值，多个字段之间的连接关系为    
        ///            “and（与）”，每个字段的值都可包含多个，使用“，”隔开  
        ///            同一字段多个值之间的连接关系为“or（或）”
        ///  <summary> 
        /// <param name="column">字段名数组</param>
        /// <param name="value">需要查询的字段名对应的值，可用','号隔开</param>
        ///  <returns>包含所有帧的数据表</returns>
        /// -------------------------------------------------------------------
        public DataTable selectNeed(string[] column, string[] value)
        {
            string str = "select ";
            if (column.Count() != value.Count()) return null;
            str = "select * from " + tableName + " where ";
            for (int i = 0; i < column.Count(); i++)
            {
                if (value[i] == "*") continue;
                string[] Value = value[i].Split(',');
                string thisValue = "";
                for (int j = 0; j < Value.Count(); j++)
                {
                    thisValue += column[i] + " = '" + Value[j] + "' or ";
                }
                thisValue = thisValue.Remove(thisValue.Length - 4);
                str += "(" + thisValue + ") and ";
            }
            str = str.Remove(str.Length - 5);
            return this.ExecuteQueryReturnDT(str);   //执行语句，返回结果
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：select
        ///  功能概要：将数据库中的表读出
        ///  内部调用：无
        ///  <summary> 
        ///  <returns>表</returns>
        /// -------------------------------------------------------------------
        public DataTable select()
        {
            string str;
            str = "select * from " + tableName;
            return this.ExecuteQueryReturnDT(str);   //执行语句，返回结果
        }


        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：addColumn
        ///  功能概要：在表中新增一个字段，默认类型为varchar（50）
        ///  内部调用：无
        ///  <summary> 
        ///  <returns>表</returns>
        /// -------------------------------------------------------------------
        public int addColumn(string column)
        {
            string strSQL;
            strSQL = "alter table "       //在表中增加字段对应列的语句
                   + tableName + " add " + column + " nvarchar(50)";
            return this.ExecuteNonQuery(strSQL);             //执行语句
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：addColumn
        ///  功能概要：在表中新增一个字段，可自定义字段的数据类型
        ///  内部调用：无
        ///  <summary> 
        ///  <returns>表</returns>
        /// -------------------------------------------------------------------
        //public int addColumn(string column,string type)
        //{
        //    string strSQL;
        //    strSQL = "alter table "       //在表中增加字段对应列的语句
        //           + tableName + " add " + column + " "+type;
        //    return this.ExecuteNonQuery(strSQL);             //执行语句
        //}
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：insertFrame
        ///  功能概要：插入一帧（根据字段name添加value）,并根据帧结构增加表中的字段（即name）
        ///  内部调用：无
        ///  <summary> 
        ///  <param name="fd">包含插入帧中数据的结构体</param>
        ///  <returns>插入后的ID号</returns>
        /// -------------------------------------------------------------------
        public int insertFrame(FrameData fd)
        {
            int i, j;
            string strSQL = "";
            string strName = "";
            string strValue = "";
            int retVal = -1;
            //1.维持数据库表结构与帧结构一致
            //1.1读出表数据
            strSQL = "select top 1 * from " + tableName;  //选取表中一行的语句
            DataTable dt = this.ExecuteQueryReturnDT(strSQL);           //执行语句
            //1.2增加结构体中存在，而表中不存在的字段
            for (i = 0; i < fd.Parameter.Count; i++)
            {
                for (j = 0; j < dt.Columns.Count; j++)
                {
                    if (fd.Parameter[i].name == dt.Columns[j].ColumnName)
                    {
                        break;
                    }
                }
                if (j == dt.Columns.Count)
                {
                    strSQL = "alter table "       //在表中增加字段对应列的语句
                           + tableName + " add " +
                           fd.Parameter[i].name + " varchar(500)";
                    this.ExecuteNonQuery(strSQL);             //执行语句
                }
            }

            //1.3读出删过之后的表数据
            strSQL = "select top 1 * from " + tableName;  //选取表中一行的语句
            dt = this.ExecuteQueryReturnDT(strSQL); //执行语句
            if (dt.Columns.Contains("ID") == false) this.insertID();
            ////1.4删除表中存在，而结构体中不存在的字段。并添加聚焦索引ID
            //bool haveID = false;
            //for (i = 0; i < dt.Columns.Count; i++)
            //{
            //    for (j = 0; j < fd.Parameter.Count; j++)
            //    {
            //        if (dt.Columns[i].ColumnName == fd.Parameter[j].name)
            //        {
            //            break;
            //        }
            //        if (dt.Columns[i].ColumnName == "ID")
            //        {
            //            haveID = true;
            //            break;
            //        }
            //    }
            //    if (j == fd.Parameter.Count)
            //    {
            //        strSQL = "alter table "       //在表中删除字段对应列的语句
            //               + tableName + " drop column "
            //               + dt.Columns[i].ColumnName;
            //        this.ExecuteNonQuery(strSQL);             //执行语句
            //    }
            //    if (!haveID) insertID();
            //}
            //2向数据库中插入一帧数据
            //对帧结构内每个字段
            j = 0;
            for (i = 0; i < fd.Parameter.Count; i++)
            {

                strValue += fd.Parameter[i].value.TrimEnd('\0') + "','";
                strName += fd.Parameter[i].name.ToString() + ",";

                if (fd.Parameter[i].value == "")
                {
                    j++;
                }
            }
            if (j == fd.Parameter.Count)
            {
                return 0;
            }
            strName = strName.Remove(strName.Length - 1);       //去掉结尾的","
            strValue = strValue.Remove(strValue.Length - 2);    //去掉结尾的",'"
            strSQL = "insert into " + tableName + "(";
            strSQL += strName + ") values('";
            strSQL += strValue + ") select count(ID) from " + tableName;
            try
            {
                dt = this.ExecuteQueryReturnDT(strSQL);
                retVal = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch { }
            return retVal;    //执行语句，返回结果
        }
        /// ----------------------------------------------------------------
        /// <summary>
        /// 方法名称：insertColumn
        /// 功能概要：向表中添加一个字段
        /// </summary>
        /// <param name="column">字段名</param>
        /// <param name="type">字段类型</param>
        /// ----------------------------------------------------------------
        public void insertColumn(string column, string type)
        {
            string strSQL = "select   count(*)   from   syscolumns   where   id=object_id('" + this.tableName + "')   and   name='" + column + "'";
            DataTable dd = this.ExecuteQueryReturnDT(strSQL);
            if (dd.Rows[0][0].ToString() == "0")
            {
                strSQL = "alter table " + tableName + " add " + column + " " + type;
                this.ExecuteNoneQuery(strSQL);
            }
        }
        /// ----------------------------------------------------------------
        /// <summary>
        /// 方法名称：dropColumn
        /// 功能概要：从表中删除一个字段
        /// </summary>
        /// <param name="column">字段名</param>
        /// ----------------------------------------------------------------
        public void dropColumn(string column)
        {
            string strSQL = "alter table " + tableName + " drop column " + column;
            this.ExecuteNoneQuery(strSQL);
        }
        /// ----------------------------------------------------------------
        /// <summary>
        /// 方法名称：clear
        /// 功能概要：清空数据库中的所有数据，不删除数据结构
        /// </summary>
        /// ----------------------------------------------------------------
        public void clear()
        {
            string strSQL = "truncate table " + tableName;
            this.ExecuteNoneQuery(strSQL);
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：insert
        ///  功能概要：向数据库的表中插入一条记录
        ///  <summary> 
        ///  <param name="column">字段名，可用“，”分隔开</param>
        ///  <param name="value">对应字段的值，与column一一对应</param>
        ///  <returns>数据库表受改变的行数</returns>
        /// -------------------------------------------------------------------
        public int insert(string column, string value)
        {
            string strSQL = "", strColumn = "", strValue = "";
            string[] Column = column.Split(',');
            string[] Value = value.Split(',');
            if (Column.Count() < Value.Count()) return 1;
            //if (value.Length < Column.Count()) return 2;
            for (int i = 0; i < Column.Count(); i++)
            {
                strColumn += Column[i] + ",";
                if (Value.Count() <= i)
                    strValue += Value[Value.Count() - 1].ToString() + "','";
                else
                    strValue += Value[i].ToString() + "','";
            }
            strColumn = strColumn.Remove(strColumn.Length - 1);  //去掉结尾的","
            strValue = strValue.Remove(strValue.Length - 2);     //去掉结尾的",'"
            strSQL = "insert into " + tableName + "(";
            strSQL += strColumn + ") values('";
            strSQL += strValue + ")";
            return this.ExecuteNonQuery(strSQL);    //执行语句，返回结果
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：update
        ///  功能概要：修改指定行的记录（以ID字段的值为标志），需要表中含有ID
        ///            字段，若无该字段，可以首先使用本构件的insertID函数插入
        ///            自增ID字段
        ///  <summary> 
        ///  <param name="ID">表的ID字段的值</param>
        ///  <param name="column">字段名，可用“，”分隔开</param>
        ///  <param name="value">对应字段的值，与column一一对应</param>
        ///  <returns>数据库表受改变的行数</returns>
        /// -------------------------------------------------------------------
        public int update(int ID, string column, string value)
        {
            string strSQL = "";
            string[] Column = column.Split(',');
            string[] Value = value.Split(',');
            if (Column.Count() != Value.Count()) return 1;
            if (value.Length < Column.Count()) return 2;
            strSQL = "update " + tableName + " set ";
            for (int i = 0; i < Column.Count(); i++)
            {
                strSQL += Column[i] + " = '" + Value[i] + "',";
            }
            strSQL = strSQL.Remove(strSQL.Length - 1);       //去掉结尾的","
            strSQL += " where ID = " + ID.ToString();
            return this.ExecuteNonQuery(strSQL);    //执行语句，返回结果
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：deleteAll
        ///  功能概要：删除所有帧
        ///  内部调用：无
        ///  <summary> 
        ///  <returns>数据库表受改变的行数</returns>
        /// -------------------------------------------------------------------
        public int deleteAll()
        {
            string str;
            str = "truncate table " + tableName;     //删除表中所有数据的语句
            return this.ExecuteNonQuery(str);        //执行语句，返回结果
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：deleteNeed
        ///  功能概要：删除指定的记录。本重载仅支持一个字段，但是字段的值可以有
        ///            多，不同的值使用','隔开，并一一对应。多个值的连接关系为
        ///            “or（或）”
        ///  <summary> 
        /// <param name="column">字段名</param>
        /// <param name="value">需要查询的字段名对应的值，可用','号隔开</param>
        ///  <returns>包含所有帧的数据表</returns>
        /// -------------------------------------------------------------------
        public int deleteNeed(string column, string value)
        {
            string str;
            str = "";
            int i;
            string[] Value = value.Split(',');
            str = "delete from " + tableName;
            if (value == "*")
                return this.ExecuteNonQuery(str);   //执行语句，返回结果
            str += " where ";
            for (i = 0; i < Value.Count(); i++)
            {
                if (Value[i] == "*") continue;
                str += column + " = '" + Value[i] + "' or ";
            }
            str = str.Remove(str.Length - 4);
            return this.ExecuteNonQuery(str);   //执行语句，返回结果
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：deleteRow
        ///  功能概要：删除数据库中的指定行（以ID字段的值为标志）
        ///  内部调用：无
        ///  <summary> 
        ///  <returns>0：失败；1：成功</returns>
        /// -------------------------------------------------------------------
        public int deleteRow(int rowCount)
        {
            string strSQL;
            strSQL = "delete from " + this.tableName + " where ID = " + rowCount.ToString();
            return this.ExecuteNonQuery(strSQL);             //执行语句
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：deleteTopRow
        ///  功能概要：删除数据库中的前rowNum行（以ID字段的值为标志）
        ///  内部调用：无
        ///  <summary> 
        ///  <returns>0：失败；非0：成功</returns>
        /// -------------------------------------------------------------------
        public int deleteTopRow(int rowNum)
        {
            string strSQL;
            strSQL = "delete from " + this.tableName + " where ID in (select top " + rowNum.ToString()
                + " id from " + this.tableName + ")";
            return this.ExecuteNonQuery(strSQL);             //执行语句
        }
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：insertID
        ///  功能概要：插入主键，增加ID字段
        ///  内部调用：无
        ///  <summary> 
        ///  <returns>无</returns>
        /// -------------------------------------------------------------------
        public void insertID()
        {
            string strSQL;

            strSQL = "drop index "                       //删除表中的聚集索引id
                   + tableName + ".id";
            this.ExecuteNonQuery(strSQL);                      //执行语句

            strSQL = "alter table "                          //在表中删除ID字段
                   + tableName + " drop column ID";
            this.ExecuteNonQuery(strSQL);                      //执行语句

            strSQL = "alter table "                    //在表中增加自增型字段ID
                + tableName + " add ID int IDENTITY(1,1)";
            this.ExecuteNonQuery(strSQL);                      //执行语句

            strSQL = "create clustered index id on " + tableName + "(ID)";
            this.ExecuteNonQuery(strSQL);        //将字段ID设置聚焦索引id
        }

        /// ----------------------------------------------------------------
        /// <summary>        
        ///函数名称：ExecuteNonQuery                                                   
        ///功能概要：对数据库进行非查询目录操作
        /// </summary>
        /// <param name="strSql">需要执行的sql语句,例如:增删改</param>
        /// <returns>!= -1，执行sql语句后数据表受影响的行数
        ///           = -1，执行sql语句失败</returns>
        /// ----------------------------------------------------------------
        public int ExecuteNonQuery(string strSql)
        {
            int ret = -1;

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlCommand cmd = new SqlCommand(strSql, connection);
                    ret = cmd.ExecuteNonQuery();
                    connection.Close();

                    return ret;
                }
                catch (Exception)
                {
                    connection.Close();
                    return -1;
                }
            }
        }

        ///以下为内部函数
        /// ----------------------------------------------------------------
        /// <summary>        
        ///函数名称：ExecuteNonQuery                                                   
        ///功能概要：带参数的非查询的Sql语句的执行
        /// </summary>
        /// <param name="strsql">需要执行的sql语句，例如：增删改</param>
        /// <param name="Param">参数</param>
        /// <returns>!= -1，执行sql语句后数据表受影响的行数
        ///           = -1，执行sql语句失败</returns>
        /// ----------------------------------------------------------------
        public int ExecuteNonQuery(string strsql, SqlParameter[] Param)
        {
            int ret = -1;
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlCommand cmd = new SqlCommand(strsql, connection);
                    if (Param != null)
                    {
                        foreach (SqlParameter Para in Param)
                        {
                            cmd.Parameters.Add(Para);
                        }
                    }

                    ret = cmd.ExecuteNonQuery();
                    connection.Close();

                    return ret;
                }
                catch (Exception)
                {
                    connection.Close();
                    return -1;
                }
            }

        }
        //无返回
        public void ExecuteNoneQuery(string strsql)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlCommand cmd = new SqlCommand(strsql, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception)
                {
                    connection.Close();
                }
            }

        }

        /// ----------------------------------------------------------------
        /// <summary>        
        ///函数名称：ExecuteQueryReturnDT                                                   
        ///功能概要：带参数的Sql语句的执行
        /// </summary>
        /// <param name="myCommandString">需要执行的sql语句</param>
        /// <returns>执行sql语句返回的数据表</returns>
        /// ----------------------------------------------------------------
        public DataTable ExecuteQueryReturnDT(string myCommandString)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand myCommand = new SqlCommand(myCommandString, connection);
                    SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
                    myAdapter.Fill(dataTable);
                    connection.Close();

                    return dataTable;
                }
                catch (Exception)
                {
                    connection.Close();
                    return dataTable;
                }
            }
        }

        /// ----------------------------------------------------------------
        /// <summary>        
        ///函数名称：ExecuteQueryReturnDT                                                   
        ///功能概要：带参数的非查询的Sql语句的执行
        /// </summary>
        /// <param name="myCommandString">需要执行的sql语句</param>
        /// <param name="Param"></param>
        /// <returns>执行sql语句返回的数据表</returns>
        /// ----------------------------------------------------------------
        public DataTable ExecuteQueryReturnDT(string myCommandString, SqlParameter[] Param)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();   //打开连接
                    }
                    SqlCommand myCommand = new SqlCommand(myCommandString, connection);
                    if (Param != null)
                    {
                        foreach (SqlParameter Para in Param)
                        {
                            myCommand.Parameters.Add(Para);
                        }
                    }
                    SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
                    myAdapter.Fill(dataTable);
                    connection.Close();

                    return dataTable;
                }
                catch (Exception)
                {
                    connection.Close();
                    return dataTable;
                }
            }
        }

        /// ----------------------------------------------------------------
        /// <summary>        
        ///函数名称：ExecuteQueryReturnDS                                                   
        ///功能概要：Sql查询语句得到DataSet
        /// </summary>
        /// <param name="myCommandString">需要执行的sql语句</param>
        /// <returns>返回值为一个数据集</returns>
        /// ----------------------------------------------------------------
        public DataSet ExecuteQueryReturnDS(string myCommandString)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();   //打开连接
                    }
                    SqlCommand myCommand = new SqlCommand(myCommandString, connection);
                    SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
                    myAdapter.Fill(ds);
                    connection.Close();
                    return ds;
                }
                catch (Exception)
                {
                    connection.Close();
                    return ds;
                }
            }
        }


        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：creat_new_table
        ///  功能概要：把当前Up表名更改为Up-日期,创建新一个Up空表，与原表结构相同
        ///  <summary> 
        ///  <returns>无</returns>
        ///  【20200812】
        /// -------------------------------------------------------------------
        public void BackUP_table()
        {

            //（1）取前一天日期"yyyyMMdd"
            string currentdate = DateTime.Now.AddDays(-1.0).ToString("yyyyMMdd"); //每天建新表
            //（2）把当前Up表名更改为Up-日期
            string sqlstr = "EXEC  sp_rename  'Up' ,  'Up-" + currentdate + "'";
            this.ExecuteNonQuery(sqlstr);

            //（3）创建新一个Up空表，与原表结构相同
            string sqlCopy = "select * into [Up] from [Up-" + currentdate + "] where 1=2"; //创建新表字符串
            this.ExecuteNonQuery(sqlCopy);

        }
    }
}
