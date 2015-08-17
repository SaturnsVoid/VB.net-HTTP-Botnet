Imports System.Windows.Forms

Public Class CommandButton
    Inherits Button
    Public Sub New()
        FlatStyle = FlatStyle.System
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cParams As CreateParams = MyBase.CreateParams
            If Environment.OSVersion.Version.Major >= 6 Then
                cParams.Style = cParams.Style Or 14
            End If
            Return cParams
        End Get
    End Property
End Class
