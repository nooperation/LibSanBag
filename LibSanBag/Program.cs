using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibSanBag
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Directory.CreateDirectory(@".\in");
            Directory.CreateDirectory(@".\out");

            //using (var outStream = File.OpenWrite(@"Test.bag"))
            //{
            //    Bag.Write(outStream, Directory.GetFiles(@".\in"), new TimeProvider());
            //}
            var pattern_record = new Regex(@"(?<hash>[a-z0-9]{32})\.(?<image_name>.*\.png)|(?<file_type>[^\.]+)\.v(?<version_hash>([a-z0-9]{16})\.(?<type>[^\.]+)\.v(?<version_number>[0-9]+)\.(?<unknown_type>[a-zA-Z0-9]+)");

            using (var inStream = File.OpenRead(@"C:\Users\Mario\AppData\Local\LindenLab\SansarClient\ClientAssetCacheLarge.bag"))
            {
                var records = Bag.Read(inStream).OrderBy(n => n.Name); ;
                foreach (var fileRecord in records)
                {
                    //using (var outStream = File.OpenWrite($@".\out\{fileRecord.Name}"))
                    {
                        var matches = pattern_record.Matches(fileRecord.Name);
                        //fileRecord.Save(inStream, outStream);
                        Console.WriteLine(fileRecord);
                    }
                }
            }
        }
    }
}
