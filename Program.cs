using System;
using System.IO;
using System.Linq;
using System.Text;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace TicketPrinterTest {
    class Program {
        static void Main(string[] args) {
            TestPrint();
        }

        private static void TestPrint() {
            var connection = new TcpConnection("192.168.2.3", 9100);

            try {
                connection.Open();
                var zebraPrinter = ZebraPrinterFactory.GetInstance(connection);
                

                var path = $"{Directory.GetCurrentDirectory()}\\zpl\\biostoom.prn";
                var status = zebraPrinter?.GetCurrentStatus();
                if (status != null && status.isReadyToPrint) {
                    var zpl = FormatZpl(path);
                    connection.Write(Encoding.UTF8.GetBytes(zpl));
                }
                else {
                    // Do additional checks and log
                    // log status.isPaperOut
                }
            }
            catch (ConnectionException e) {
                Console.WriteLine(e.ToString());
            }
            finally {
                connection.Close();
            }
        }

        private static string FormatZpl(string path) {
            using var sr = new StreamReader(path ?? throw new ArgumentNullException(nameof(path)));
            var contents = sr.ReadToEnd();
            contents = contents.Replace("date", DateTime.Now.ToString("g"));
            contents = contents.Replace("gateNumber", "27");
            contents = contents.Replace("barcode", "111111111");
            
            return contents;
        }
    }
}