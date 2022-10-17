<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAboutPDFTrans
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdAuthorize As System.Windows.Forms.Button
    Public WithEvents lblSponsorWeb As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblTitle As System.Windows.Forms.Label
    Public WithEvents lblVersion As System.Windows.Forms.Label

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAboutPDFTrans))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdAuthorize = New System.Windows.Forms.Button()
        Me.lblSponsorWeb = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.pictStatus = New System.Windows.Forms.PictureBox()
        Me.imgList32 = New System.Windows.Forms.ImageList(Me.components)
        Me.btnClearLicenses = New System.Windows.Forms.Button()
        Me.lblComputer = New System.Windows.Forms.Label()
        Me.chkTag = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkCompare = New System.Windows.Forms.CheckBox()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkPublish = New System.Windows.Forms.CheckBox()
        Me.chkPDFTrans = New System.Windows.Forms.CheckBox()
        Me.chkMultipage = New System.Windows.Forms.CheckBox()
        Me.chkMeasure = New System.Windows.Forms.CheckBox()
        CType(Me.pictStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.PowderBlue
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(436, 42)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(100, 36)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdAuthorize
        '
        Me.cmdAuthorize.BackColor = System.Drawing.Color.PowderBlue
        Me.cmdAuthorize.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAuthorize.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAuthorize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAuthorize.Location = New System.Drawing.Point(8, 11)
        Me.cmdAuthorize.Name = "cmdAuthorize"
        Me.cmdAuthorize.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAuthorize.Size = New System.Drawing.Size(97, 38)
        Me.cmdAuthorize.TabIndex = 4
        Me.cmdAuthorize.Text = "Buy Online"
        Me.cmdAuthorize.UseVisualStyleBackColor = False
        '
        'lblSponsorWeb
        '
        Me.lblSponsorWeb.BackColor = System.Drawing.Color.Transparent
        Me.lblSponsorWeb.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSponsorWeb.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSponsorWeb.ForeColor = System.Drawing.Color.Black
        Me.lblSponsorWeb.Location = New System.Drawing.Point(176, 223)
        Me.lblSponsorWeb.Name = "lblSponsorWeb"
        Me.lblSponsorWeb.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSponsorWeb.Size = New System.Drawing.Size(341, 24)
        Me.lblSponsorWeb.TabIndex = 8
        Me.lblSponsorWeb.Text = "For more information or help, visit us at:"
        Me.lblSponsorWeb.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.Color.Transparent
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.Color.Black
        Me.lblDescription.Location = New System.Drawing.Point(212, 81)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(307, 87)
        Me.lblDescription.TabIndex = 0
        Me.lblDescription.Text = "DocuFi PDFTrans is a tool for converting single or multi-page PDF and TIF files. " & _
            " Use the free version on your single Page documents or the paid version to conve" & _
            "rt and edit multi-page drawing sets."
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.Black
        Me.lblTitle.Location = New System.Drawing.Point(146, 62)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(195, 32)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Application Title"
        '
        'lblVersion
        '
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVersion.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVersion.Location = New System.Drawing.Point(89, 99)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVersion.Size = New System.Drawing.Size(147, 37)
        Me.lblVersion.TabIndex = 2
        Me.lblVersion.Text = "Version"
        '
        'lblResult
        '
        Me.lblResult.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblResult.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblResult.Location = New System.Drawing.Point(213, 172)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(335, 39)
        Me.lblResult.TabIndex = 30
        '
        'pictStatus
        '
        Me.pictStatus.BackColor = System.Drawing.Color.Transparent
        Me.pictStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.pictStatus.Location = New System.Drawing.Point(166, 179)
        Me.pictStatus.Name = "pictStatus"
        Me.pictStatus.Size = New System.Drawing.Size(31, 32)
        Me.pictStatus.TabIndex = 31
        Me.pictStatus.TabStop = False
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
        'btnClearLicenses
        '
        Me.btnClearLicenses.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearLicenses.BackColor = System.Drawing.Color.PowderBlue
        Me.btnClearLicenses.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearLicenses.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnClearLicenses.Location = New System.Drawing.Point(494, 312)
        Me.btnClearLicenses.Name = "btnClearLicenses"
        Me.btnClearLicenses.Size = New System.Drawing.Size(64, 36)
        Me.btnClearLicenses.TabIndex = 40
        Me.btnClearLicenses.Text = "Clear licenses"
        Me.btnClearLicenses.UseVisualStyleBackColor = False
        '
        'lblComputer
        '
        Me.lblComputer.BackColor = System.Drawing.Color.Transparent
        Me.lblComputer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComputer.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComputer.ForeColor = System.Drawing.Color.Black
        Me.lblComputer.Location = New System.Drawing.Point(293, 312)
        Me.lblComputer.Name = "lblComputer"
        Me.lblComputer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComputer.Size = New System.Drawing.Size(111, 22)
        Me.lblComputer.TabIndex = 41
        Me.lblComputer.Text = "Computer"
        Me.lblComputer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkTag
        '
        Me.chkTag.AutoSize = True
        Me.chkTag.BackColor = System.Drawing.Color.Transparent
        Me.chkTag.Enabled = False
        Me.chkTag.Location = New System.Drawing.Point(17, 303)
        Me.chkTag.Name = "chkTag"
        Me.chkTag.Size = New System.Drawing.Size(103, 18)
        Me.chkTag.TabIndex = 50
        Me.chkTag.Text = "Tag Differences"
        Me.chkTag.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(5, 261)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 16)
        Me.Label3.TabIndex = 48
        Me.Label3.Text = "Features"
        '
        'chkCompare
        '
        Me.chkCompare.AutoSize = True
        Me.chkCompare.BackColor = System.Drawing.Color.Transparent
        Me.chkCompare.Enabled = False
        Me.chkCompare.Location = New System.Drawing.Point(17, 279)
        Me.chkCompare.Name = "chkCompare"
        Me.chkCompare.Size = New System.Drawing.Size(115, 18)
        Me.chkCompare.TabIndex = 46
        Me.chkCompare.Text = "Compare Versions"
        Me.chkCompare.UseVisualStyleBackColor = False
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel2.Location = New System.Drawing.Point(247, 243)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(101, 16)
        Me.LinkLabel2.TabIndex = 52
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "www.docufi.com"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(364, 243)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(213, 24)
        Me.Label1.TabIndex = 53
        Me.Label1.Text = "For Company information."
        '
        'chkPublish
        '
        Me.chkPublish.AutoSize = True
        Me.chkPublish.BackColor = System.Drawing.Color.Transparent
        Me.chkPublish.Enabled = False
        Me.chkPublish.Location = New System.Drawing.Point(17, 328)
        Me.chkPublish.Name = "chkPublish"
        Me.chkPublish.Size = New System.Drawing.Size(94, 18)
        Me.chkPublish.TabIndex = 55
        Me.chkPublish.Text = "Publish to PDF"
        Me.chkPublish.UseVisualStyleBackColor = False
        '
        'chkPDFTrans
        '
        Me.chkPDFTrans.AutoSize = True
        Me.chkPDFTrans.BackColor = System.Drawing.Color.Transparent
        Me.chkPDFTrans.Enabled = False
        Me.chkPDFTrans.Location = New System.Drawing.Point(138, 279)
        Me.chkPDFTrans.Name = "chkPDFTrans"
        Me.chkPDFTrans.Size = New System.Drawing.Size(123, 18)
        Me.chkPDFTrans.TabIndex = 56
        Me.chkPDFTrans.Text = "PDF2TIF Conversion"
        Me.chkPDFTrans.UseVisualStyleBackColor = False
        '
        'chkMultipage
        '
        Me.chkMultipage.AutoSize = True
        Me.chkMultipage.BackColor = System.Drawing.Color.Transparent
        Me.chkMultipage.Enabled = False
        Me.chkMultipage.Location = New System.Drawing.Point(138, 303)
        Me.chkMultipage.Name = "chkMultipage"
        Me.chkMultipage.Size = New System.Drawing.Size(112, 18)
        Me.chkMultipage.TabIndex = 57
        Me.chkMultipage.Text = "Multipage Support"
        Me.chkMultipage.UseVisualStyleBackColor = False
        '
        'chkMeasure
        '
        Me.chkMeasure.AutoSize = True
        Me.chkMeasure.BackColor = System.Drawing.Color.Transparent
        Me.chkMeasure.Enabled = False
        Me.chkMeasure.Location = New System.Drawing.Point(138, 328)
        Me.chkMeasure.Name = "chkMeasure"
        Me.chkMeasure.Size = New System.Drawing.Size(68, 18)
        Me.chkMeasure.TabIndex = 58
        Me.chkMeasure.Text = "Measure"
        Me.chkMeasure.UseVisualStyleBackColor = False
        '
        'frmAboutPDFTrans
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CancelButton = Me.cmdOK
        Me.ClientSize = New System.Drawing.Size(560, 352)
        Me.Controls.Add(Me.chkMeasure)
        Me.Controls.Add(Me.chkMultipage)
        Me.Controls.Add(Me.chkPDFTrans)
        Me.Controls.Add(Me.chkPublish)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.chkTag)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.chkCompare)
        Me.Controls.Add(Me.lblComputer)
        Me.Controls.Add(Me.btnClearLicenses)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.pictStatus)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdAuthorize)
        Me.Controls.Add(Me.lblSponsorWeb)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblVersion)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(52, 101)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAboutPDFTrans"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Authorize and Info About PDFTrans"
        CType(Me.pictStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents lblResult As System.Windows.Forms.Label
    Private WithEvents pictStatus As System.Windows.Forms.PictureBox
    Private WithEvents imgList32 As System.Windows.Forms.ImageList
    Private WithEvents btnClearLicenses As System.Windows.Forms.Button
    Public WithEvents lblComputer As System.Windows.Forms.Label
    Friend WithEvents chkTag As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkCompare As System.Windows.Forms.CheckBox
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkPublish As System.Windows.Forms.CheckBox
    Friend WithEvents chkPDFTrans As System.Windows.Forms.CheckBox
    Friend WithEvents chkMultipage As System.Windows.Forms.CheckBox
    Friend WithEvents chkMeasure As System.Windows.Forms.CheckBox
#End Region
End Class