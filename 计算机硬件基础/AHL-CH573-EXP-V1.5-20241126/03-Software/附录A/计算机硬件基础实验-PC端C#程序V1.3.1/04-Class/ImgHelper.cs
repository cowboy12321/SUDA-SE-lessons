using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace GEC_LAB._04_Class
{
    public class ImgHelper
    {
        //======================================================================
        //函数名称：BitmapToBitmapImage
        //函数返回：BitmapSource
        //参数说明：Bitmap
        //功能概要：将bitmap转化成bitmapimage以用于image控件中
        //======================================================================
        public static BitmapSource? BitmapToBitmapImage(Bitmap? bitmap)
        {
            if (bitmap == null) return null;
            BitmapSource? bf = null;
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                bf = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
            return bf;
        }

        public static BitmapSource? byteToBitmapSource(byte[] bytes) {

            return BitmapToBitmapImage(ByteToBitmap(bytes));
        }

        public static Bitmap? ByteToBitmap(byte[] bytes)
        {
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                return new Bitmap(new MemoryStream(bytes));
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            finally
            {
                stream.Close();
            }
        }
        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            BitmapData bitmapDat = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            // 获取bmpData的内存起始位置
            IntPtr intPtr = bitmapDat.Scan0;
            byte[] image = new byte[bitmap.Width * bitmap.Height];//原始数据
                                                                  // 将数据复制到byte数组中，
            Marshal.Copy(intPtr, image, 0, bitmap.Width * bitmap.Height);
            //解锁内存区域  
            bitmap.UnlockBits(bitmapDat);
            return image;
        }

        public static BitmapSource? FileToBitmapSourece(string fileName)
        {
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open,
            FileAccess.Read, FileShare.Read);

            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            return byteToBitmapSource(bytes);

        }
        public static Bitmap? FileToBitmap(string fileName)
        {
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open,
            FileAccess.Read, FileShare.Read);

            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                return new Bitmap(new MemoryStream(bytes));
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            finally
            {
                stream.Close();
            }

        }
    }
}
