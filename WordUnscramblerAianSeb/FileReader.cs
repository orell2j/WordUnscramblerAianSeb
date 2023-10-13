using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordUnscramblerAianSeb
{
    class FileReader
    {
        public string[] Read(string filename)
        {
          
            try
            {
                return File.ReadAllLines(filename);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                return null;
            }
        }
    }
}
