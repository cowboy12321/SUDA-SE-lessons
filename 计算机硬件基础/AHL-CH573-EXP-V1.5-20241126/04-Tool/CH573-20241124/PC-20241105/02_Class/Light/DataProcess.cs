using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Light
{
    class DataProcess
    {
        static int MB = 1024 * 1024;
        static int KB = 1024;
        static int half = 512;

        /// <summary>
        /// 判断时间格式是否正确的函数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool judgeTimeFormat(string time)
        {

            return true;
        }

        /// <summary>
        /// 提取某一个灯的数据，将其写入一个文件
        /// 这里是直接写入字节数据，没有转为16进制数形式的字符串输出
        /// </summary>
        /// <param name="lightIndex"></param>
        /// <param name="filePath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public int extractData(int lightIndex, string filePath, string outputPath)
        {
            //lightIndex从1-128
            if (lightIndex < 1 || lightIndex > 128)
            {
                return 0;
            }
            int beginIndex = 4 * (lightIndex - 1);

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //StreamWriter sw = new StreamWriter(outputPath,true);
            FileStream fsWrite = new FileStream(outputPath, FileMode.OpenOrCreate);
            BinaryReader br = new BinaryReader(fs);

            long fileLength = fs.Length;
            long frameCount;//有多少帧，多少个512字节
            int count;

            if (fileLength % 512 != 0)
            {
                //判断数据格式是否合理
                return 0;
            }
            else
            {
                frameCount = fileLength / 512;
            }
            while (frameCount > 0)
            {
                fs.Seek(beginIndex, SeekOrigin.Begin);
                count = 4;
                while (count > 0)
                {
                    //sw.Write(br.ReadByte());
                    fsWrite.WriteByte(br.ReadByte());
                    count -= 1;
                }

                frameCount -= 1;
                beginIndex += 512;
            }
            fs.Close();
            br.Close();
            fsWrite.Close();
            //sw.Close();

            return 0;
        }

        /// <summary>
        /// 从DataProcess2中拿过来测试，生成的All.text是否和预期符合
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public int readAll(string filePath, string outputPath)
        {
            int i;
            //数据开始位置4d 52 4b 4a
            int startPostion = 2 * MB;
            //56个数据开始的位置
            int dataStartPosition = startPostion + half;
            //灯光数据开始的位置
            int ligthStartPosition = dataStartPosition + 56;

            int result = readData(filePath, outputPath, startPostion, 4);

            while (result == 1)
            {
                //需要加一层for循环，每一段0x600长度中有8段数据
                //但最后有可能存在小于8段的，需要判断一下当前的位置和文件总长度，不越界就行，或者trycatch
                for (i = 0; i < 8; i++)
                {
                    //从灯光开始的位置读512字节
                    //修改为1360字节
                    //readData(filePath, outputPath, ligthStartPosition, 1360);

                    //ligthStartPosition += (6 + 1360);

                    //readData(filePath, outputPath, ligthStartPosition, 92);

                    ////移到下一部分的开头
                    //ligthStartPosition += 170;
                    readData(filePath, outputPath, ligthStartPosition, 512);
                    ligthStartPosition += 0x600;

                }
                //一个部分读取完成，下一个部分
                startPostion += 0x3200;
                dataStartPosition = startPostion + half;
                ligthStartPosition = dataStartPosition + 56;

                result = readData(filePath, outputPath, startPostion, 4);
            }
            return 0;
        }

        public int readAll2(string filePath,string outputPath)
        {
            //数据开始位置4d 52 4b 4a
            int startPostion = 2 * MB;
            //56个数据开始的位置
            int dataStartPosition = startPostion + half;
            //灯光数据开始的位置
            int ligthStartPosition = dataStartPosition + 56;

            int result = readData(filePath, outputPath, startPostion, 4);

            while (result == 1)
            {
                //从灯光开始的位置读512字节
                readData(filePath, outputPath, ligthStartPosition, half);

                //这里还不能确定，只能暂时这么写
                ligthStartPosition += (6 + 512);
                readData(filePath, outputPath, ligthStartPosition, half);

                startPostion += (2 * KB);
                dataStartPosition = startPostion + half;
                ligthStartPosition = dataStartPosition + 56;
                result = readData(filePath, outputPath, startPostion, 4);
            }
            return 0;
        }

        /// <summary>
        /// 从文件的某一个位置开始读固定长度的数据
        /// 并且按字节的格式写入
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outputPath"></param>
        /// <param name="seekBegin"></param>
        /// <param name="readLength"></param>
        /// <returns></returns>
        public int readData(string filePath, string outputPath, int seekBegin, int readLength)
        {
            byte[] dataArray = new byte[readLength];

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            fs.Seek(seekBegin, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(fs);
            //读readLength个字节
            int count = 0;
            while (count < readLength)
            {
                dataArray[count++] = br.ReadByte();

            }

            //这里根据readLength的长度，来判断调用这个函数的目的，是读512，还是判断4d 52 4b 4a
            //并且如果是判断，返回不同的返回值
            //写入文件
            //和之前函数不同的地方，
            //StreamWriter sw = new StreamWriter(outputPath, true);
            FileStream fsWrite = new FileStream(outputPath, FileMode.OpenOrCreate);
            if (readLength == 1360 || readLength == 92 || readLength == 512)
            {
                fsWrite.Position = fsWrite.Length;
                fsWrite.Write(dataArray, 0, readLength);
                //foreach (byte data in dataArray)
                //{
                //    //sw.Write(data.ToString("X2"));
                //    fs.Write(data);

                //}
            }
            else if (readLength == 4)
            {
                string tempStr = "";
                tempStr = string.Join("", dataArray);
                //77827574==4d524b4a,10进制和16进制
                if (tempStr == "77827574")
                {
                    fsWrite.Close();
                    br.Close();
                    fs.Close();
                    //sw.Close();
                    return 1;
                }
            }
            fsWrite.Close();
            fs.Close();
            br.Close();
            //sw.Close();

            return 0;
        }
    }
}
