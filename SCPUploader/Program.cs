using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using WinSCP;


public class Program
{
    public static int Main()
    {
        try
        {
            // Setup session options
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Scp,
                HostName = ConfigurationManager.AppSettings["hostName"],
                PortNumber = Convert.ToInt32(ConfigurationManager.AppSettings["portNumber"]),
                UserName = ConfigurationManager.AppSettings["userName"],
                Password = ConfigurationManager.AppSettings["storedProcedure"],
                SshHostKeyFingerprint = ConfigurationManager.AppSettings["SshHostKeyFingerprint"],
                SshPrivateKeyPath = ConfigurationManager.AppSettings["SshPrivateKeyPath"],
            };

            using (Session session = new Session())
            {
                string fileName = ConfigurationManager.AppSettings["fileName"];
                string localPath = ConfigurationManager.AppSettings["localPath"];
                string remotePath = ConfigurationManager.AppSettings["remotePath"];

                // Connect
                session.Open(sessionOptions);

                Console.WriteLine("## SCP Host Connection Success");

                string getFrom = localPath + fileName;
                string putToo = remotePath;

                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;

                TransferOperationResult transferResult;
                transferResult = session.PutFiles(getFrom, remotePath, false, transferOptions);

                transferResult.Check();

                foreach (TransferEventArgs transfer in transferResult.Transfers)
                {
                    Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    Console.WriteLine("Upload of {0} succeeded", transfer.Destination);
                    Console.WriteLine("Response: ", transfer.Error);
                }

                Console.WriteLine("## SCP Host Transfer Success");
                System.Environment.Exit(0);
            }

            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);
            Console.Read();
            return 1;
        }
    }
}

