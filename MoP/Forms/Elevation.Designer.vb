
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ElevationFrm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblText = New System.Windows.Forms.Label()
        Me.lblHead = New System.Windows.Forms.Label()
        Me.panelBot = New System.Windows.Forms.Panel()
        Me.linkError = New System.Windows.Forms.LinkLabel()
        Me.btnRestore = New CommandButton()
        Me.btnRestoreAndCheck = New CommandButton()
        Me.picError = New System.Windows.Forms.PictureBox()
        Me.picInfo = New System.Windows.Forms.PictureBox()
        Me.panelBot.SuspendLayout()
        CType(Me.picError, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblText
        '
        Me.lblText.AutoSize = True
        Me.lblText.Location = New System.Drawing.Point(61, 42)
        Me.lblText.Name = "lblText"
        Me.lblText.Size = New System.Drawing.Size(47, 13)
        Me.lblText.TabIndex = 9
        Me.lblText.Text = "%TEXT%"
        '
        'lblHead
        '
        Me.lblHead.AutoSize = True
        Me.lblHead.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHead.ForeColor = System.Drawing.Color.MediumBlue
        Me.lblHead.Location = New System.Drawing.Point(60, 6)
        Me.lblHead.Name = "lblHead"
        Me.lblHead.Size = New System.Drawing.Size(79, 20)
        Me.lblHead.TabIndex = 6
        Me.lblHead.Text = "%ERROR%"
        '
        'panelBot
        '
        Me.panelBot.BackColor = System.Drawing.Color.WhiteSmoke
        Me.panelBot.Controls.Add(Me.linkError)
        Me.panelBot.Controls.Add(Me.picInfo)
        Me.panelBot.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBot.Location = New System.Drawing.Point(0, 245)
        Me.panelBot.Name = "panelBot"
        Me.panelBot.Size = New System.Drawing.Size(542, 38)
        Me.panelBot.TabIndex = 8
        '
        'linkError
        '
        Me.linkError.AutoSize = True
        Me.linkError.Location = New System.Drawing.Point(37, 11)
        Me.linkError.Name = "linkError"
        Me.linkError.Size = New System.Drawing.Size(87, 13)
        Me.linkError.TabIndex = 1
        Me.linkError.TabStop = True
        Me.linkError.Text = "%MOREDETAILS"
        '
        'btnRestore
        '
        Me.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnRestore.Location = New System.Drawing.Point(12, 140)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(518, 42)
        Me.btnRestore.TabIndex = 10
        Me.btnRestore.Text = "CommandButton1"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnRestoreAndCheck
        '
        Me.btnRestoreAndCheck.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnRestoreAndCheck.Location = New System.Drawing.Point(12, 190)
        Me.btnRestoreAndCheck.Name = "btnRestoreAndCheck"
        Me.btnRestoreAndCheck.Size = New System.Drawing.Size(518, 42)
        Me.btnRestoreAndCheck.TabIndex = 1
        Me.btnRestoreAndCheck.Text = "CommandButton1"
        Me.btnRestoreAndCheck.UseVisualStyleBackColor = True
        '
        'picError
        '
        Me.picError.Location = New System.Drawing.Point(12, 6)
        Me.picError.Name = "picError"
        Me.picError.Size = New System.Drawing.Size(42, 42)
        Me.picError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picError.TabIndex = 7
        Me.picError.TabStop = False
        '
        'picInfo
        '
        Me.picInfo.Image = Global.MoP.My.Resources.Resources.information
        Me.picInfo.Location = New System.Drawing.Point(12, 10)
        Me.picInfo.Name = "picInfo"
        Me.picInfo.Size = New System.Drawing.Size(16, 16)
        Me.picInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picInfo.TabIndex = 0
        Me.picInfo.TabStop = False
        '
        'ElevationFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(542, 283)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnRestore)
        Me.Controls.Add(Me.btnRestoreAndCheck)
        Me.Controls.Add(Me.lblText)
        Me.Controls.Add(Me.picError)
        Me.Controls.Add(Me.lblHead)
        Me.Controls.Add(Me.panelBot)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ElevationFrm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "%TITLE%"
        Me.TopMost = True
        Me.panelBot.ResumeLayout(False)
        Me.panelBot.PerformLayout()
        CType(Me.picError, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picInfo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents lblText As Windows.Forms.Label
    Private WithEvents picError As Windows.Forms.PictureBox
    Private WithEvents lblHead As Windows.Forms.Label
    Private WithEvents panelBot As Windows.Forms.Panel
    Private WithEvents linkError As Windows.Forms.LinkLabel
    Private WithEvents picInfo As Windows.Forms.PictureBox
    Friend WithEvents btnRestoreAndCheck As CommandButton
    Friend WithEvents btnRestore As CommandButton
End Class