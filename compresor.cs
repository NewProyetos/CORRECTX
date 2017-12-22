using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Sinconizacion_EXactus
{
    class compresor
    {

          

          public static  byte[] comprimir(byte[] gzip)
        {




            var output = new MemoryStream();
            using (var data = new GZipStream(output, CompressionMode.Compress, true))
            {
                data.Write(gzip, 0, gzip.Length);
                data.Close();
            }
            return output.ToArray();

            //using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Compress,false))
            //    {
            //        //const int size = 4096;
            //        //byte[] buffer = new byte[size];
            //        //using (MemoryStream memory = new MemoryStream())
            //        //{
            //        //    int count = 0;
            //        //    do
            //        //    {
            //        //        count = stream.Read(buffer, 0, size);
            //        // //   stream.Read()
            //        //        if (count > 0)
            //        //        {
            //        //            memory.Write(buffer, 0, count);
            //        //        }
            //        //    }
            //        //    while (count > 0);
            //        //    return memory.ToArray();
            //        //}


            //    }
        }

            public static byte[] descompirmir(byte[] gzip)
            {
            //var output = new MemoryStream();
            //var input = new MemoryStream();
            //input.Write(gzip, 0, gzip.Length);
            //input.Position = 0;

            //using (var data = new GZipStream(input, CompressionMode.Decompress, true))
            //{
            //    var buff = new byte[64];
            //    var read = data.Read(buff, 0, buff.Length);

            //    while (read > 0)
            //    {
            //        output.Write(buff, 0, read);
            //        read = data.Read(buff, 0, buff.Length);
            //    }

            //    data.Close();
            //}
            //return output.ToArray();






            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }

        }
        
        }




    
}
