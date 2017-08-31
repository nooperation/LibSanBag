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

            //Bag.CreateNewBag(@"Test.bag", new List<string>()
            //{
            //    @"r:\csharp\SanBag\SanBag.Tests\In\TestFile1.txt"
            //});

            var bag_contents = Bag.ReadBag(@"C:\Users\Mario\AppData\Local\LindenLab\SansarClient\ClientAssetCacheSmall--.bag");
            foreach (var item in bag_contents)
            {
                var file_record = item.Value;
                {
                    //file_record.Save($@".\out\{file_record.Name}");
                    Console.WriteLine(file_record);
                }
            }
        }
    }
}
