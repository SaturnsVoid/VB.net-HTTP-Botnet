Imports System.Threading
Public Class Startup
    Private Sub Startup_Load(sender As Object, e As EventArgs) Handles Me.Load
        ParseCommandLineArgs()
    End Sub
    Dim MoP As New Main
    Sub BootStrapper()
        MoP.Begin()
    End Sub
    Private Sub ParseCommandLineArgs()
        Dim Argument As String = "/remove="
        Dim file As String = ""
        Thread.Sleep(2000) '2 second sleep to make sure old closes
        For Each s As String In My.Application.CommandLineArgs
            If s.ToLower.StartsWith(Argument) Then
                file = s.Remove(0, Argument.Length)
            End If
        Next
        If file = "" Then
            Dim mainThread As New Thread(AddressOf BootStrapper)
            mainThread.Start()
        Else
            Dim si As New ProcessStartInfo()
            si.FileName = "cmd.exe"
            si.Arguments = "/C ping 1.1.1.1 -n 1 -w 4000 > Nul & Del """ & file & """"
            si.CreateNoWindow = True
            si.WindowStyle = ProcessWindowStyle.Hidden
            Process.Start(si)
            Dim mainThread As New Thread(AddressOf BootStrapper)
            mainThread.Start()
        End If
    End Sub
End Class