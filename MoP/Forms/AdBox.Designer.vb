<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdBox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.AdBlock = New System.Windows.Forms.PictureBox()
        Me.WaitTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.AdBlock, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AdBlock
        '
        Me.AdBlock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.AdBlock.Cursor = System.Windows.Forms.Cursors.Hand
        Me.AdBlock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AdBlock.Location = New System.Drawing.Point(0, 0)
        Me.AdBlock.Name = "AdBlock"
        Me.AdBlock.Size = New System.Drawing.Size(334, 111)
        Me.AdBlock.TabIndex = 0
        Me.AdBlock.TabStop = False
        '
        'WaitTimer
        '
        '
        'AdBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 111)
        Me.ControlBox = False
        Me.Controls.Add(Me.AdBlock)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AdBox"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Advertisment"
        Me.TopMost = True
        CType(Me.AdBlock, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AdBlock As Windows.Forms.PictureBox
    Friend WithEvents WaitTimer As Windows.Forms.Timer
End Class
