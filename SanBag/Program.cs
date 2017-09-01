using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanBag
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

            using (var out_Stream = File.OpenWrite(@"Test.bag"))
            {
                Bag.Write(out_Stream, Directory.GetFiles(@".\in"), new TimeProvider());
            }

            using (var in_stream = File.OpenRead(@"Test.bag"))
            {
                var records = Bag.Read(in_stream);

                foreach (var file_record in records)
                {
                    using (var out_stream = File.OpenWrite($@".\out\{file_record.Name}"))
                    {
                        file_record.Save(in_stream, out_stream);
                        Console.WriteLine(file_record);
                    }
                }
            }
        }
    }
}
