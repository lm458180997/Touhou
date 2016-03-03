using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FastLoopExample
{


    public class FileData
    {
        //int32 占4个字节
        public const int INT32_LEN = 4;
        //头 占4个字节 [标记总长]
        public const int headerLength = 4;
        //名 占4个字节 [标记名字]
        public const int fileNameLength = 4;
        //名字
        public string Name;
        //文件名长
        public int nameLength;
        //实际文件长度
        public int bodyLength;
        //文件数据
        public byte[] filedata;
        public FileData(string filename, byte[] datas)
        {
            Name = filename;
            nameLength = System.Text.Encoding.UTF8.GetByteCount(filename);
            bodyLength = datas.Length;
            filedata = datas;
        }
        //总长
        public int Length
        {
            get { return headerLength + fileNameLength + nameLength + bodyLength; }
        }
    }

    public class FileManager
    {
        //储存总长
        public int totolLenth = 0;
        public const int INT32_LEN = 4;
        //文件数据
        public List<FileData> FilesDatas;
        public FileManager()
        {
            FilesDatas = new List<FileData>();
        }
        public void AddFile(String name, byte[] data)
        {
            FileData dt = new FileData(name, data);
            FilesDatas.Add(dt);
            totolLenth += dt.Length;
        }

        public void AddFile(String path)
        {
            StreamReader rd = new StreamReader(path);
            byte[] bs = StreamToBytes(rd.BaseStream);
            rd.Close();
            rd.Dispose();
            AddFile(Path.GetFileName(path), bs);
        }


        public void CreateFileClip(string filepath)
        {


            //--PackDatas
            byte[] t_datas = new byte[totolLenth + INT32_LEN];  //总数据
            int read = 0;            //已阅读长度
            int Lenth = 0;           //单个文件总长
            int nameLenth = 0;       //名字总长
            string name = "";          //名字  
            byte[] bs;

            //写入所有数据的总和长度
            bs = System.BitConverter.GetBytes(totolLenth);
            for (int i = 0; i < INT32_LEN; i++)
            {
                t_datas[read + i] = bs[i];
            }
            read += INT32_LEN;
            //分别写入每一个数据
            foreach (FileData dt in FilesDatas)
            {
                Lenth = dt.Length;
                nameLenth = dt.nameLength;
                name = dt.Name;
                //写入文件总长
                bs = System.BitConverter.GetBytes(Lenth);
                for (int i = 0; i < INT32_LEN; i++)
                {
                    t_datas[read + i] = bs[i];
                }
                read += INT32_LEN;
                //写入名字总长
                bs = System.BitConverter.GetBytes(nameLenth);
                for (int i = 0; i < INT32_LEN; i++)
                {
                    t_datas[read + i] = bs[i];
                }
                read += INT32_LEN;
                //写入名字
                System.Text.Encoding.UTF8.GetBytes(name, 0, name.Length,
                    t_datas, read);
                read += nameLenth;
                //写入文件本体
                bs = dt.filedata;
                bs.CopyTo(t_datas, read);
                read += dt.bodyLength;
            }
            Stream stream = BytesToStream(t_datas);
            StreamToFile(stream, filepath);

            //--PackDatas
        }

        public void ReadFileClip(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new Exception("不存在 " + filepath);
            }
            StreamReader rd = new StreamReader(filepath);
            
            byte[] bs = StreamToBytes(rd.BaseStream);
            if (bs.Length <= 4)
            {
                throw new Exception(filepath + "中数据为空");
            }
            rd.Close();
            rd.Dispose();
            byte[] datas;              //单个文件的总数据
            int read = 0;              //已阅读长度
            int filesTotolLenth = 0;   //文件所包含的总长度
            int INT32_LEN = 4;         //int32长度
            int Lenth = 0;             //单个文件总长
            int nameLenth = 0;         //名字总长
            int bodylenth = 0;         //体长
            string name = "";          //名字  
            bool havefile = true;      //是否有文件没有读取
            //读取文件所包含的所有子文件的长度
            byte[] numbs = new byte[4];
            for (int i = 0; i < 4; i++)
                numbs[i] = bs[read + i];
            read += INT32_LEN;
            filesTotolLenth = System.BitConverter.ToInt32(numbs, 0);
            totolLenth += filesTotolLenth;         //这里只加正文长不加头长
            filesTotolLenth += INT32_LEN;
            //分别读取每一个文件数据
            while (havefile)
            {
                //存储长度数据
                for (int i = 0; i < 4; i++)
                    numbs[i] = bs[read + i];
                read += INT32_LEN;
                Lenth = System.BitConverter.ToInt32(numbs, 0);
                //获取名字长度
                for (int i = 0; i < 4; i++)
                    numbs[i] = bs[read + i];
                nameLenth = System.BitConverter.ToInt32(numbs, 0);
                read += INT32_LEN;
                //获得名字
                byte[] namedts = new byte[nameLenth];
                for (int i = 0; i < nameLenth; i++)
                {
                    namedts[i] = bs[read + i];
                }
                name = Encoding.UTF8.GetString(namedts, 0, nameLenth);
                read += nameLenth;
                //获得文件数据
                bodylenth = Lenth - FileData.headerLength - FileData.fileNameLength - nameLenth;
                datas = new byte[bodylenth];
                for (int i = 0; i < bodylenth; i++)
                {
                    datas[i] = bs[read + i];
                }
                FileData fdata = new FileData(name, datas);
                read += bodylenth;
                //文件信息载入
                FilesDatas.Add(fdata);
                //没有下一个文件，循环跳出
                if (read >= filesTotolLenth)
                    havefile = false;
            }
        }

        public void ClearFiles()
        {
            FilesDatas.Clear();
            totolLenth = 0;
        }


        /* - - - - - - - - - - - - - - - - - - - - - - - -  
* Stream 和 byte[] 之间的转换 
* - - - - - - - - - - - - - - - - - - - - - - - */
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /* - - - - - - - - - - - - - - - - - - - - - - - -  
         * Stream 和 文件之间的转换 
         * - - - - - - - - - - - - - - - - - - - - - - - */
        /// <summary> 
        /// 将 Stream 写入文件 
        /// </summary> 
        public void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        /// <summary> 
        /// 从文件读取 Stream 
        /// </summary> 
        public Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }


}
