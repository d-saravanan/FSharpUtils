#undef TRACE
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleTester
{
    class Program
    {
        static int sum(int a)
        {
            return 1 + a;
        }

        static object sumo(object a)
        {
            return null;
        }

        private static bool Test(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; }
        static void Main(string[] args)
        {

            string content = File.ReadAllText(@"C:\Users\Saran\Desktop\Utils\Data\samplemetadata.xml");
            var md = MetadataParser.DeconstructFromContent(content);

            Console.WriteLine(md);

            DiagnosticUtility.Log("test", "hello");
            DiagnosticUtility.LogWithFormat("test", "hello {0}", "Saran");
            Console.ReadLine();
        }
    }
}
