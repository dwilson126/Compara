<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmLicense
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    '<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose2(ByVal Disposing As Boolean)
    '    If Disposing Then
    '        If Not components Is Nothing Then
    '            components.Dispose()
    '        End If
    '    End If
    '    MyBase.Dispose(Disposing)
    'End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLicense))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.imgList32 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.pictStatus = New System.Windows.Forms.PictureBox()
        Me.txtLicenseKey = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.cmdAuthorize = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnManual = New System.Windows.Forms.Button()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.txtCompany = New System.Windows.Forms.TextBox()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.btnOnline = New System.Windows.Forms.Button()
        CType(Me.pictStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'imgList32
        '
        Me.imgList32.ImageStream = CType(resources.GetObject("imgList32.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgList32.TransparentColor = System.Drawing.Color.Transparent
        Me.imgList32.Images.SetKeyName(0, "certificate.ico")
        Me.imgList32.Images.SetKeyName(1, "certificate_add.ico")
        Me.imgList32.Images.SetKeyName(2, "certificate_broken.ico")
        Me.imgList32.Images.SetKeyName(3, "certificate_delete.ico")
        Me.imgList32.Images.SetKeyName(4, "certificate_error.ico")
        Me.imgList32.Images.SetKeyName(5, "certificate_information.ico")
        Me.imgList32.Images.SetKeyName(6, "certificate_new.ico")
        Me.imgList32.Images.SetKeyName(7, "certificate_ok.ico")
        Me.imgList32.Images.SetKeyName(8, "certificate_preferences.ico")
        Me.imgList32.Images.SetKeyName(9, "certificate_refresh.ico")
        Me.imgList32.Images.SetKeyName(10, "certificate_view.ico")
        Me.imgList32.Images.SetKeyName(11, "certificate_warning.ico")
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.Color.Transparent
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblMessage.Location = New System.Drawing.Point(214, 61)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(380, 74)
        Me.lblMessage.TabIndex = 25
        Me.lblMessage.Text = "To register, please enter  your license key in the box below.  A valid entry will" & _
    " initiate the license on your system." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "A 20 day Trial License is provided.  Clic" & _
    "k on Activate then click Continue."
        '
        'lblResult
        '
        Me.lblResult.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblResult.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblResult.Location = New System.Drawing.Point(192, 234)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(434, 22)
        Me.lblResult.TabIndex = 24
        '
        'pictStatus
        '
        Me.pictStatus.BackColor = System.Drawing.Color.Transparent
        Me.pictStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.pictStatus.Location = New System.Drawing.Point(155, 224)
        Me.pictStatus.Name = "pictStatus"
        Me.pictStatus.Size = New System.Drawing.Size(31, 32)
        Me.pictStatus.TabIndex = 29
        Me.pictStatus.TabStop = False
        '
        'txtLicenseKey
        '
        Me.txtLicenseKey.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicenseKey.Location = New System.Drawing.Point(206, 203)
        Me.txtLicenseKey.Name = "txtLicenseKey"
        Me.txtLicenseKey.Size = New System.Drawing.Size(414, 20)
        Me.txtLicenseKey.TabIndex = 28
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.BackColor = System.Drawing.Color.Transparent
        Me.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.label1.Location = New System.Drawing.Point(223, 186)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(70, 14)
        Me.label1.TabIndex = 26
        Me.label1.Text = "&License Key:"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblStatus.Location = New System.Drawing.Point(192, 266)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(434, 69)
        Me.lblStatus.TabIndex = 27
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.PowderBlue
        Me.btnClose.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnClose.Location = New System.Drawing.Point(483, 338)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(136, 38)
        Me.btnClose.TabIndex = 31
        Me.btnClose.Text = "&Continue Trial"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'cmdAuthorize
        '
        Me.cmdAuthorize.BackColor = System.Drawing.Color.PowderBlue
        Me.cmdAuthorize.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAuthorize.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAuthorize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAuthorize.Location = New System.Drawing.Point(12, 22)
        Me.cmdAuthorize.Name = "cmdAuthorize"
        Me.cmdAuthorize.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAuthorize.Size = New System.Drawing.Size(103, 47)
        Me.cmdAuthorize.TabIndex = 32
        Me.cmdAuthorize.Text = "&Buy Online"
        Me.cmdAuthorize.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(-1, 267)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(50, 14)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "Features"
        Me.Label3.Visible = False
        '
        'btnManual
        '
        Me.btnManual.BackColor = System.Drawing.Color.PowderBlue
        Me.btnManual.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnManual.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnManual.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnManual.Location = New System.Drawing.Point(101, 328)
        Me.btnManual.Name = "btnManual"
        Me.btnManual.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnManual.Size = New System.Drawing.Size(85, 38)
        Me.btnManual.TabIndex = 37
        Me.btnManual.Text = "&Manual Activation"
        Me.btnManual.UseVisualStyleBackColor = False
        Me.btnManual.Visible = False
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.BackColor = System.Drawing.Color.Transparent
        Me.lblEmail.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblEmail.Location = New System.Drawing.Point(-1, 132)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmail.Size = New System.Drawing.Size(34, 14)
        Me.lblEmail.TabIndex = 42
        Me.lblEmail.Text = "&Email:"
        Me.lblEmail.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblEmail.Visible = False
        '
        'txtCompany
        '
        Me.txtCompany.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCompany.Location = New System.Drawing.Point(12, 87)
        Me.txtCompany.Name = "txtCompany"
        Me.txtCompany.Size = New System.Drawing.Size(198, 20)
        Me.txtCompany.TabIndex = 41
        Me.txtCompany.Visible = False
        '
        'lblCompany
        '
        Me.lblCompany.AutoSize = True
        Me.lblCompany.BackColor = System.Drawing.Color.Transparent
        Me.lblCompany.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblCompany.Location = New System.Drawing.Point(5, 104)
        Me.lblCompany.Name = "lblCompany"
        Me.lblCompany.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCompany.Size = New System.Drawing.Size(55, 14)
        Me.lblCompany.TabIndex = 40
        Me.lblCompany.Text = "&Company:"
        Me.lblCompany.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblCompany.Visible = False
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.BackColor = System.Drawing.Color.White
        Me.txtName.Location = New System.Drawing.Point(17, 61)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(185, 20)
        Me.txtName.TabIndex = 39
        Me.txtName.Visible = False
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblName.Location = New System.Drawing.Point(31, 78)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(37, 14)
        Me.lblName.TabIndex = 38
        Me.lblName.Text = "&Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblName.Visible = False
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.Location = New System.Drawing.Point(37, 129)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(221, 20)
        Me.txtEmail.TabIndex = 43
        Me.txtEmail.Visible = False
        '
        'btnOnline
        '
        Me.btnOnline.BackColor = System.Drawing.Color.PowderBlue
        Me.btnOnline.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnOnline.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOnline.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnOnline.Location = New System.Drawing.Point(294, 338)
        Me.btnOnline.Name = "btnOnline"
        Me.btnOnline.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnOnline.Size = New System.Drawing.Size(95, 38)
        Me.btnOnline.TabIndex = 44
        Me.btnOnline.Text = "Activate"
        Me.btnOnline.UseVisualStyleBackColor = False
        '
        'frmLicense
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(631, 378)
        Me.Controls.Add(Me.btnOnline)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.txtCompany)
        Me.Controls.Add(Me.lblCompany)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.btnManual)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmdAuthorize)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.pictStatus)
        Me.Controls.Add(Me.txtLicenseKey)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.lblStatus)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(52, 101)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLicense"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "DocuFi ComparA License Management"
        CType(Me.pictStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents imgList32 As System.Windows.Forms.ImageList
    Private WithEvents lblMessage As System.Windows.Forms.Label
    Private WithEvents lblResult As System.Windows.Forms.Label
    Private WithEvents pictStatus As System.Windows.Forms.PictureBox
    Private WithEvents txtLicenseKey As System.Windows.Forms.TextBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents lblStatus As System.Windows.Forms.Label
    Private WithEvents btnClose As System.Windows.Forms.Button
    Public WithEvents cmdAuthorize As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents btnManual As System.Windows.Forms.Button
    Private WithEvents lblEmail As System.Windows.Forms.Label
    Private WithEvents txtCompany As System.Windows.Forms.TextBox
    Private WithEvents lblCompany As System.Windows.Forms.Label
    Private WithEvents txtName As System.Windows.Forms.TextBox
    Private WithEvents lblName As System.Windows.Forms.Label
    Private WithEvents txtEmail As System.Windows.Forms.TextBox
    Public WithEvents btnOnline As System.Windows.Forms.Button
#End Region
End Class