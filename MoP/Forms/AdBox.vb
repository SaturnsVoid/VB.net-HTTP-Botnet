Imports System.IO
Imports System.Net
Imports System.Text

Public Class AdBox
    Public AdPicture As String
    Public AdLink As String
    Public UseAdDB As Boolean = False
    Public RepeatAd As Boolean = True
    Public ShowEvery As Integer = 30
    Dim Main As Main
    'Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width, Screen.PrimaryScreen.WorkingArea.Height - Me.Height)
    Private Sub AdBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        If RepeatAd Then : WaitTimer.Interval = ShowEvery * 1000 : WaitTimer.Start() : End If
    End Sub

    Private Sub AdBlock_Click(sender As Object, e As EventArgs) Handles AdBlock.Click
        'Process.Start(AdLink)
        'Main.HTTPRequstWorker("1" & "?HWID=" & Main.GetHWID() & "?CMD=AD?STATE=Opened")
        'Me.Close()
    End Sub

    Private Sub WaitTimer_Tick(sender As Object, e As EventArgs) Handles WaitTimer.Tick

    End Sub

    Private Sub GetAds()
        HTTPRequstWorker("1" & "?HWID==" & Main.GetHWID() & "?CMD=CPA?STATE=Good")
    End Sub

    Public Function HTTPRequstWorker(Data As String) As String
        Try
            Dim result As String = Nothing
            Dim param As Byte() = Encoding.UTF8.GetBytes(Data)
            Dim req As WebRequest = WebRequest.Create(Main.GetPanel)
            req.Method = "POST"
            DirectCast(req, HttpWebRequest).UserAgent = Main.GetUserAgent()
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
End Class