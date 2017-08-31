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

            Bag.Write(@"Test.bag", Directory.GetFiles(@".\in"), new TimeProvider());

            var records = Bag.Read(@"Test.bag");

            using (var in_stream = File.OpenRead(@"Test.bag"))
            {
                foreach (var file_record in records)
                {
                    file_record.Save(in_stream, $@".\out\{file_record.Name}");
                    Console.WriteLine(file_record);
                }
            }
        }
    }
}
