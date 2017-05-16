using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SnmpSharpNet;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Threading;
using System.Management;
using System.Data;

namespace PrinterStatus
{
    class Program
    {
        public static volatile bool updatingTable = false;
        public static volatile bool selectionChanged = false;
        //public static bool firstScan = true;
        public static volatile DataTable printerTable;
        public static volatile DataTable printerTableLast;
        //public static volatile BindingSource printerBindingSource;
        public static PrinterStatusForm form;
        public static int printersToScan = 0;
        public static int scannedPrinters = 0;
        public static int tonerWarningThreshold = 20;
        public static int tonerCriticalThreshold = 5;
        public static int scannerTimeout = 10000;
        public static volatile int selectedX = 0;
        public static volatile int selectedY = 0;


        [STAThread]
        static void Main(string[] args)
        {
            form = new PrinterStatusForm();
            printerTable = new DataTable();
            printerTable.Columns.Add(new DataColumn("PrinterName", typeof(string)));
            printerTable.Columns.Add(new DataColumn("PrinterStatus", typeof(string)));
            printerTable.Columns.Add(new DataColumn("PrinterModel", typeof(string)));
            printerTable.Columns.Add(new DataColumn("PrinterIP", typeof(string)));
            printerTable.Columns.Add(new DataColumn("TonerStatus", typeof(string)));
            printerTable.Columns.Add(new DataColumn("PaperStatus", typeof(string)));
            printerTable.Columns.Add(new DataColumn("PrinterDisplay", typeof(string)));
            //printerBindingSource = new BindingSource();
            //printerBindingSource.DataSource = printerTable;
            form = new PrinterStatusForm();
            form.PrinterGridView.AutoGenerateColumns = false;
            //form.PrinterGridView.DataSource = printerBindingSource;
            Thread scanAllPrintersThread = new Thread(Program.ScanAllPrinters);
            scanAllPrintersThread.IsBackground = true;
            scanAllPrintersThread.Start();
            //Walk("172.16.4.156", "1.3.6.1.2.1.43.11.1.1.6");
            Application.Run(form);
        }

        public static void ScanAllPrinters()
        {
            while (!form.IsDisposed)
            {
                printerTableLast = printerTable.Copy();
                form.StopUpdating();

                List<DataRow> printerRows = printerTable.AsEnumerable().ToList();
                foreach (DataRow printerRow in printerRows)
                {
                    printerRow.Delete();
                }
                printersToScan = 0;
                scannedPrinters = 0;
                List<DataRow> printerScanList = new List<DataRow>();
                ManagementObjectSearcher printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
                ManagementObjectCollection printers = printerQuery.Get();
                printersToScan = printers.Count;
                foreach (ManagementBaseObject printer in printers)
                {
                    form.SetStatusText(string.Format("Scanning {0} of {1} Printers", scannedPrinters + 1, printersToScan));
                    string printerName = printer.GetPropertyValue("Name").ToString();
                    string printerStatus = "";
                    string printerModel = "";
                    string printerIP = "";
                    string tonerStatus = "";
                    string paperStatus = "";
                    string printerDisplay = "";

                    bool isDefault = (bool)printer.GetPropertyValue("Default");
                    if (isDefault)
                    {
                        printerName = printerName + " (Default)";
                    }
                    var portName = printer.GetPropertyValue("PortName");
                    var error = printer.GetPropertyValue("ErrorInformation");
                    ManagementObjectSearcher portQuery = new ManagementObjectSearcher("SELECT HostAddress FROM Win32_TCPIPPrinterPort WHERE Name = '" + portName + "'");

                    foreach (ManagementBaseObject port in portQuery.Get())
                    {
                        printerIP = port.GetPropertyValue("HostAddress").ToString();
                    }

                    if (printerIP != "")
                    {
                        string printersWithSameIP = string.Format("PrinterIP = '{0}'", printerIP);
                        DataRow[] existingRows = printerTable.Select(printersWithSameIP);
                        int printersSharingIP = existingRows.Count();
                        if (printersSharingIP == 0)
                        {
                            DataRow printerRow = printerTable.Rows.Add(printerName, printerStatus, printerModel, printerIP, tonerStatus, paperStatus, printerDisplay);
                            printerScanList.Add(printerRow);
                        }
                        else
                        {
                            scannedPrinters++;

                            MatchCollection matches = Regex.Matches(existingRows[0]["PrinterName"].ToString(), @"(.+)\b", RegexOptions.Multiline);
                            bool printerFound = false;
                            foreach(Match match in matches)
                            {
                                if(match.Groups[1].ToString() == printerName)
                                {
                                    printerFound = true;
                                    break;
                                }
                            }
                            if (!printerFound)
                            {
                                existingRows[0]["PrinterName"] += "\r\n" + printerName;
                            }
                        }
                    }
                    else
                    {
                        scannedPrinters++;
                        string printersWithSameName = string.Format("PrinterName = '{0}'", printerName);
                        DataRow[] existingRows = printerTable.Select(printersWithSameName);
                        int printersSharingName = existingRows.Count();
                        if (printersSharingName == 0)
                        {
                            printerTable.Rows.Add(printerName, printerStatus, printerModel, printerIP, tonerStatus, paperStatus, printerDisplay);
                        }
                    }

                }

                foreach (DataRow printerScanRow in printerScanList)
                {
                    Thread printerScannerThread = new Thread(new ParameterizedThreadStart(PrinterScanner));
                    printerScannerThread.IsBackground = true;
                    printerScannerThread.Start(printerScanRow);
                }
                while (scannedPrinters < printersToScan)
                {
                    Thread.Sleep(100);
                }
                form.SetStatusText("Scan Complete");
                form.StartUpdating();
                Thread.Sleep(scannerTimeout);
            }
        }
        public static void PrinterScanner(object printerObj)
        {
            DataRow printerRow = (DataRow)printerObj;
            string printerName = printerRow["PrinterName"].ToString();
            string printerStatus = printerRow["PrinterStatus"].ToString();
            string printerModel = printerRow["PrinterModel"].ToString();
            string printerIP = printerRow["PrinterIP"].ToString();
            string tonerStatus = printerRow["TonerStatus"].ToString();
            string paperStatus = printerRow["PaperStatus"].ToString();
            string printerDisplay = printerRow["PrinterDisplay"].ToString();

            bool online = false;
            Ping pingPrinter = new Ping();
            try
            {
            //Debug.WriteLine("pinging " + printerIP);
                PingReply replyPing = pingPrinter.Send(printerIP, 2000);
                if(replyPing.Status == IPStatus.Success)
                {
                //Debug.WriteLine(printerIP + " online");
                    online = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
            if (online)
            {
                string community = "public";
                SimpleSnmp snmp = new SimpleSnmp(printerIP, community);

                if (!snmp.Valid)
                {
                    Console.WriteLine("SNMP agent host name/ip {0} address is invalid.", printerIP);
                    return;
                }
                Pdu basicInfo = new Pdu();
                basicInfo.Type = PduType.Get;
                Oid deviceStatus = new Oid("1.3.6.1.2.1.25.3.5.1.1.1");
                Oid modelName = new Oid("1.3.6.1.2.1.25.3.2.1.3.1");
                basicInfo.VbList.Add(deviceStatus);
                basicInfo.VbList.Add(modelName);
                Dictionary<Oid, AsnType> basicInfoResult = snmp.Get(SnmpVersion.Ver1, basicInfo);
                //new string[] {
                //"1.3.6.1.2.1.25.3.5.1.1.1", //Device Status
                //"1.3.6.1.2.1.25.3.2.1.3.1", //Model
                //"1.3.6.1.2.1.43.11.1.1.6.1.1", //Toner Name
                //"1.3.6.1.2.1.43.11.1.1.8.1.1", //Toner Max Value
                //"1.3.6.1.2.1.43.11.1.1.9.1.1", //Toner Current Value
                //"1.3.6.1.2.1.43.11.1.1.6.1.2", //Maintenance Kit Name
                //"1.3.6.1.2.1.43.11.1.1.8.1.2", //Maintenacne Kit Max Value
                //"1.3.6.1.2.1.43.11.1.1.9.1.2", //Maintenance Kit Current Value
                //"1.3.6.1.2.1.43.8.2.1.13.1.2", //Tray 2 Name
                //"1.3.6.1.2.1.43.8.2.1.10.1.2", //Tray 2 Status
                //"1.3.6.1.2.1.43.18.1.1.8.1.347", //Alert String
                //"1.3.6.1.2.1.43.16.5.1.2.1.1" //Display String
                //});
                if (basicInfoResult != null)
                {
                    foreach (KeyValuePair<Oid, AsnType> kvp in basicInfoResult)
                    {

                        //Console.WriteLine("{0}: {1} {2}", kvp.Key.ToString(),
                        //                        SnmpConstants.GetTypeName(kvp.Value.Type),
                        //                        kvp.Value.ToString());
                        if (kvp.Key == deviceStatus)
                        {
                            switch (int.Parse(kvp.Value.ToString()))
                            {
                                case 1:
                                    printerStatus += " Other";
                                    break;
                                case 2:
                                    printerStatus += " Processing";
                                    break;
                                case 3:
                                    printerStatus += " Idle";
                                    break;
                                case 4:
                                    printerStatus += " Printing";
                                    break;
                                case 5:
                                    printerStatus += " Warmup";
                                    break;
                            }
                        }
                        if (kvp.Key == modelName)
                        {
                            printerModel = kvp.Value.ToString();
                            printerModel = printerModel.Replace("  ", " ");
                        }
                    }
                }
                List<string> displayMessages = Walk(printerIP, "1.3.6.1.2.1.43.16.5");
                List<string> printerDisplays = new List<string>();
                for (int i = 0; i < displayMessages.Count; i++)
                {
                    string displayMessage = displayMessages[i];
                    displayMessage = Regex.Replace(displayMessage, @"^([0-9A-F]{2}\s){1,}([0-9A-F]{2})$", "");
                    displayMessage = Regex.Replace(displayMessage, @"^\s*$", "");
                    if (displayMessage != "")
                    {
                        printerDisplays.Add(displayMessage);
                    }
                }
                printerDisplay = string.Join("\r\n", printerDisplays.ToArray());

                List<string> toners = Walk(printerIP, "1.3.6.1.2.1.43.11.1.1.6");
                List<string> tonerMaxes = Walk(printerIP, "1.3.6.1.2.1.43.11.1.1.8");
                List<string> tonerCurrents = Walk(printerIP, "1.3.6.1.2.1.43.11.1.1.9");
                List<string> tonerStatuses = new List<string>();
                if (toners.Count == tonerMaxes.Count && toners.Count == tonerCurrents.Count)
                {
                    for (int i = 0; i < toners.Count; i++)
                    {
                        int max = int.Parse(tonerMaxes[i]);
                        int current = int.Parse(tonerCurrents[i]);
                        if (current == -3)
                        {
                            current = max;
                        }
                        decimal percentUsed = decimal.Divide(current, max) * 100;
                        percentUsed = Math.Round(percentUsed, 0);
                        string tonerName = toners[i];
                        int semicolonindex = tonerName.IndexOf(";");
                        if (semicolonindex > 0)
                        {
                            tonerName = tonerName.Substring(0, semicolonindex);
                        }
                        tonerStatuses.Add(percentUsed.ToString() + "% " + tonerName);
                    }
                    tonerStatus = string.Join("\r\n", tonerStatuses.ToArray());
                }

                List<string> trays = Walk(printerIP, "1.3.6.1.2.1.43.8.2.1.18");
                List<string> trayLevels = Walk(printerIP, "1.3.6.1.2.1.43.8.2.1.10");
                List<string> trayTypes = Walk(printerIP, "1.3.6.1.2.1.43.8.2.1.2");
                List<string> trayStatuses = new List<string>();

                if (trays.Count == trayLevels.Count && trays.Count == trayTypes.Count)
                {
                    for (int i = 0; i < trays.Count; i++)
                    {
                        switch (int.Parse(trayLevels[i]))
                        {
                            case 0:
                                if (trayTypes[i] == "4" || trayTypes[i] == "5")
                                {
                                    trayStatuses.Add(trays[i] + " (Feed Tray)");
                                }
                                else
                                {
                                    trayStatuses.Add(trays[i] + " (Empty)");
                                }
                                break;
                            case -1:
                                trayStatuses.Add(trays[i] + " (Status: Unknown)");
                                break;
                            case -2:
                                trayStatuses.Add(trays[i] + " (Tray Open)");
                                break;
                            case -3:
                                trayStatuses.Add(trays[i] + " (Paper Present)");
                                break;
                            default:
                                trayStatuses.Add(string.Format(trays[i] + " ({0} Pages in Tray)", trayLevels[i]));
                                break;

                        }
                    }
                    paperStatus = string.Join("\r\n", trayStatuses.ToArray());
                }

                Dictionary<string,string> jobs = WalkJob(printerIP, "1.3.6.1.4.1.11.2.3.9.4.2.1.1.6.5.1");
                Dictionary<string, string> jobs2 = WalkJob(printerIP, "1.3.6.1.4.1.11.2.3.9.4.2.1.1.6.5.2");
                Dictionary<string, string> jobAttributes = WalkJob(printerIP, "1.3.6.1.4.1.11.2.3.9.4.2.1.1.6.5.23");

                if (jobs.Count == jobs2.Count)
                {
                    List<string> jobNames = new List<string>();
                    for (int i = 0; i < jobs.Count; i++)
                    {
                        string jobOid = jobs.ElementAt(i).Key;
                        Oid jobOidObj = new Oid(jobOid);
                        uint[] jobOidParts = jobOidObj.ToArray();
                        string jobID = jobOidParts.ElementAt(jobOidParts.Length - 2).ToString();
                        string jobRoman8 = jobs.ElementAt(i).Value.Replace(" ", "");
                        jobRoman8 = jobRoman8.Substring(4, jobRoman8.Length - 4);
                        string jobRoman82 = jobs2.ElementAt(i).Value.Replace(" ", "");
                        jobRoman82 = jobRoman82.Substring(4, jobRoman82.Length - 4);

                        string jobName = Roman8.ConvertFromRoman8(jobRoman8 + jobRoman82);
                        List<string> jobAttributeStrings = new List<string>();
                        foreach(KeyValuePair<string,string> jobAttribute in jobAttributes)
                        {
                            Oid jobAttributeOidObj = new Oid(jobAttribute.Key);
                            uint[] jobAttributeoidParts = jobAttributeOidObj.ToArray();
                            string jobAttributeID = jobAttributeoidParts.ElementAt(jobAttributeoidParts.Length - 2).ToString();
                            if (jobAttributeID == jobID)
                            {
                                string jobAttributeString = jobAttribute.Value;
                                jobAttributeString = Regex.Replace(jobAttributeString, "(OS|Render).*=.+", "");
                                jobAttributeString = Regex.Replace(jobAttributeString, "JobAcct1=", "Job User: ");
                                jobAttributeString = Regex.Replace(jobAttributeString, "JobAcct2=", "Job Machine: ");
                                jobAttributeString = Regex.Replace(jobAttributeString,
@"^JobAcct4=(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$",
@"Job Date: $1-$2-$3 $4:$5:$6");
                                jobAttributeString = Regex.Replace(jobAttributeString, "JobAcct5=.*", "");
                                jobAttributeString = Regex.Replace(jobAttributeString, "JobAcct6=", "Job Application: ");
                                jobAttributeString = Regex.Replace(jobAttributeString, "JobAcct7=", "Job Process: ");
                                jobAttributeString = Regex.Replace(jobAttributeString, "^.+=.*", "");
                                //jobAttributeString = Regex.Replace(jobAttributeString,
                                //    @"^[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$",
                                //    "",
                                //    RegexOptions.IgnoreCase);

                                if (jobAttributeString != "")
                                {
                                    jobAttributeStrings.Add(jobAttributeString);
                                }
                            }
                        }
                        jobName += "\r\n" + string.Join("\r\n", jobAttributeStrings.ToArray());

                        //if (i < jobtimes.Count && jobtimes[i] != "")
                        //{
                        //    string year = jobtimes[i].Substring(9, 4);
                        //    string month = jobtimes[i].Substring(13, 2);
                        //    string day = jobtimes[i].Substring(15, 2);
                        //    string hour = jobtimes[i].Substring(17, 2);
                        //    string min = jobtimes[i].Substring(19, 2);
                        //    string sec = jobtimes[i].Substring(21, 2);
                        //    jobNames.Add(string.Format("{0} {1}-{2}-{3} {4}:{5}:{6}",jobName,year,month,day,hour,min,sec));
                        //}
                        jobNames.Add(jobName);

                    }
                    if (jobNames.Count > 0)
                    {
                        printerStatus += "\r\n Last Job: " + jobNames.Last();
                    }
                }

            }
            else
            {
                printerStatus += " Offline";
            }

            try
            {
                //updatingTable = true;
                printerRow.BeginEdit();
                printerRow["PrinterName"] = printerName;
                printerRow["PrinterStatus"] = printerStatus;
                printerRow["PrinterModel"] = printerModel;
                printerRow["TonerStatus"] = tonerStatus;
                printerRow["PaperStatus"] = paperStatus;
                printerRow["PrinterDisplay"] = printerDisplay;
                printerRow.EndEdit();
                //updatingTable = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }

            scannedPrinters++;
            decimal percentDone = Decimal.Divide(scannedPrinters, printersToScan);
            int progressValue = (int)Math.Round(percentDone * 100, 0);
            form.SetProgressBar(progressValue);
            //if (firstScan)
            //{
            //    form.RefreshPrinterGridView();
            //}


            //Debug.WriteLine("PrinterStatus=================");
            //Walk("172.16.4.14", "1.3.6.1.2.1.25.3.5"); //printer status
            //Debug.WriteLine("DeviceStatus=================");
            //Walk("172.16.4.14", "1.3.6.1.2.1.25.3.2"); //printer status
            //Debug.WriteLine("Supplies=================");
            //Walk("172.16.4.1", "1.3.6.1.2.1.43.11.1.1.6"); //toner and kit descriptions
            //Walk("172.16.4.1", "1.3.6.1.2.1.43.11.1.1.8"); //toner and kit max level
            //Walk("172.16.4.1", "1.3.6.1.2.1.43.11.1.1.9"); //toner and kit current level
            //Debug.WriteLine("Trays=================");
            //Walk("172.16.4.14", "1.3.6.1.2.1.43.8.2"); //trays
            //Walk("172.16.4.14", "1.3.6.1.2.1.43.9.2"); //outputs
            //Debug.WriteLine("Life=================");
            //Walk("172.16.4.14", "1.3.6.1.2.1.43.10.2.1.4.1"); //lifetime page count
            //Debug.WriteLine("Alerts=================");
            //Walk("172.16.4.14", "1.3.6.1.2.1.43.18.1.1.8"); //alert status
            //Debug.WriteLine("Display=================");
            //Walk("172.16.4.14", "1.3.6.1.2.1.43.16.5.1.2.1"); //console display
        }
        static List<string> Walk(string host, string oid)
        {
            List<string> values = new List<string>();
            // SNMP community name
            try { 
            OctetString community = new OctetString("public");

            // Define agent parameters class
            AgentParameters param = new AgentParameters(community);
            // Set SNMP version to 1
            param.Version = SnmpVersion.Ver1;
            // Construct the agent address object
            // IpAddress class is easy to use here because
            //  it will try to resolve constructor parameter if it doesn't
            //  parse to an IP address
            IpAddress agent = new IpAddress(host);

            // Construct target
            UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 1);

            // Define Oid that is the root of the MIB
            //  tree you wish to retrieve
            Oid rootOid = new Oid(oid); // ifDescr

            // This Oid represents last Oid returned by
            //  the SNMP agent
            Oid lastOid = (Oid)rootOid.Clone();

            // Pdu class used for all requests
            Pdu pdu = new Pdu(PduType.GetNext);

            // Loop through results
            while (lastOid != null)
            {
                // When Pdu class is first constructed, RequestId is set to a random value
                // that needs to be incremented on subsequent requests made using the
                // same instance of the Pdu class.
                if (pdu.RequestId != 0)
                {
                    pdu.RequestId += 1;
                }
                // Clear Oids from the Pdu class.
                pdu.VbList.Clear();
                // Initialize request PDU with the last retrieved Oid
                pdu.VbList.Add(lastOid);
                // Make SNMP request
                SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);
                // You should catch exceptions in the Request if using in real application.

                // If result is null then agent didn't reply or we couldn't parse the reply.
                if (result != null)
                {
                    // ErrorStatus other then 0 is an error returned by 
                    // the Agent - see SnmpConstants for error definitions
                    if (result.Pdu.ErrorStatus != 0)
                    {
                        // agent reported an error with the request
                        Console.WriteLine("Error in SNMP reply. Error {0} index {1}",
                            result.Pdu.ErrorStatus,
                            result.Pdu.ErrorIndex);
                        lastOid = null;
                        break;
                    }
                    else
                    {
                        // Walk through returned variable bindings
                        foreach (Vb v in result.Pdu.VbList)
                        {
                            // Check that retrieved Oid is "child" of the root OID
                            if (rootOid.IsRootOf(v.Oid))
                            {
                                //Debug.WriteLine("{0} ({1}): {2}",
                                //    v.Oid.ToString(),
                                //    SnmpConstants.GetTypeName(v.Value.Type),
                                //    v.Value.ToString());
                                values.Add(v.Value.ToString());
                                lastOid = v.Oid;
                            }
                            else
                            {
                                // we have reached the end of the requested
                                // MIB tree. Set lastOid to null and exit loop
                                lastOid = null;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No response received from SNMP agent.");
                }
            }
            target.Close();
        }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
            return values;
        }

        static Dictionary<string,string> WalkJob(string host, string oid)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            // SNMP community name
            try
            {
                OctetString community = new OctetString("public");

                // Define agent parameters class
                AgentParameters param = new AgentParameters(community);
                // Set SNMP version to 1
                param.Version = SnmpVersion.Ver1;
                // Construct the agent address object
                // IpAddress class is easy to use here because
                //  it will try to resolve constructor parameter if it doesn't
                //  parse to an IP address
                IpAddress agent = new IpAddress(host);

                // Construct target
                UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 1);

                // Define Oid that is the root of the MIB
                //  tree you wish to retrieve
                Oid rootOid = new Oid(oid); // ifDescr

                // This Oid represents last Oid returned by
                //  the SNMP agent
                Oid lastOid = (Oid)rootOid.Clone();

                // Pdu class used for all requests
                Pdu pdu = new Pdu(PduType.GetNext);

                // Loop through results
                while (lastOid != null)
                {
                    // When Pdu class is first constructed, RequestId is set to a random value
                    // that needs to be incremented on subsequent requests made using the
                    // same instance of the Pdu class.
                    if (pdu.RequestId != 0)
                    {
                        pdu.RequestId += 1;
                    }
                    // Clear Oids from the Pdu class.
                    pdu.VbList.Clear();
                    // Initialize request PDU with the last retrieved Oid
                    pdu.VbList.Add(lastOid);
                    // Make SNMP request
                    SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);
                    // You should catch exceptions in the Request if using in real application.

                    // If result is null then agent didn't reply or we couldn't parse the reply.
                    if (result != null)
                    {
                        // ErrorStatus other then 0 is an error returned by 
                        // the Agent - see SnmpConstants for error definitions
                        if (result.Pdu.ErrorStatus != 0)
                        {
                            // agent reported an error with the request
                            Console.WriteLine("Error in SNMP reply. Error {0} index {1}",
                                result.Pdu.ErrorStatus,
                                result.Pdu.ErrorIndex);
                            lastOid = null;
                            break;
                        }
                        else
                        {
                            // Walk through returned variable bindings
                            foreach (Vb v in result.Pdu.VbList)
                            {
                                // Check that retrieved Oid is "child" of the root OID
                                if (rootOid.IsRootOf(v.Oid))
                                {
                                    //Debug.WriteLine("{0} ({1}): {2}",
                                    //    v.Oid.ToString(),
                                    //    SnmpConstants.GetTypeName(v.Value.Type),
                                    //    v.Value.ToString());
                                    values.Add(v.Oid.ToString(),v.Value.ToString());
                                    lastOid = v.Oid;
                                }
                                else
                                {
                                    // we have reached the end of the requested
                                    // MIB tree. Set lastOid to null and exit loop
                                    lastOid = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No response received from SNMP agent.");
                    }
                }
                target.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
            return values;
        }
    }
}
