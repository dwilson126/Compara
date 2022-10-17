<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmProperties
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
    Public WithEvents lblTitle As System.Windows.Forms.Label
    Public WithEvents lblDocument As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProperties))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblDocument = New System.Windows.Forms.Label()
        Me.imgList32 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblProperties = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.PowderBlue
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(436, 41)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(100, 36)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.Black
        Me.lblTitle.Location = New System.Drawing.Point(90, 24)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(195, 32)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Application Title"
        '
        'lblDocument
        '
        Me.lblDocument.BackColor = System.Drawing.Color.Transparent
        Me.lblDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocument.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocument.Location = New System.Drawing.Point(133, 105)
        Me.lblDocument.Name = "lblDocument"
        Me.lblDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocument.Size = New System.Drawing.Size(329, 46)
        Me.lblDocument.TabIndex = 2
        Me.lblDocument.Text = "Document:"
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
        'lblProperties
        '
        Me.lblProperties.BackColor = System.Drawing.Color.Transparent
        Me.lblProperties.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProperties.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProperties.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProperties.Location = New System.Drawing.Point(133, 177)
        Me.lblProperties.Name = "lblProperties"
        Me.lblProperties.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProperties.Size = New System.Drawing.Size(403, 115)
        Me.lblProperties.TabIndex = 10
        Me.lblProperties.Text = "Properties:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(104, 83)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(90, 22)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Document:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(104, 153)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(90, 16)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Properties:"
        '
        'frmProperties
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CancelButton = Me.cmdOK
        Me.ClientSize = New System.Drawing.Size(566, 301)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblProperties)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblDocument)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(52, 101)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProperties"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Document Properties"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents imgList32 As System.Windows.Forms.ImageList
    Public WithEvents lblProperties As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
#End Region
End Class