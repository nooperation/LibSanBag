using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            using (var outStream = File.OpenWrite(@"Test.bag"))
            {
                Bag.Write(outStream, Directory.GetFiles(@".\in"), new TimeProvider());
            }

            using (var inStream = File.OpenRead(@"Test.bag"))
            {
                var records = Bag.Read(inStream);

                foreach (var fileRecord in records)
                {
                    using (var outStream = File.OpenWrite($@".\out\{fileRecord.Name}"))
                    {
                        fileRecord.Save(inStream, outStream);
                        Console.WriteLine(fileRecord);
                    }
                }
            }
        }
    }
}
