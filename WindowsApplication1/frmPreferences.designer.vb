<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPreferences
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
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPreferences))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.imgList32 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.intTagFontSize = New DevComponents.Editors.IntegerInput()
        Me.ColorPickerButton1 = New DevComponents.DotNetBar.ColorPickerButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grpMeasure = New System.Windows.Forms.GroupBox()
        Me.rbCent = New System.Windows.Forms.RadioButton()
        Me.rbMeters = New System.Windows.Forms.RadioButton()
        Me.rbFeet = New System.Windows.Forms.RadioButton()
        Me.rbInches = New System.Windows.Forms.RadioButton()
        Me.ColorPickerBase = New DevComponents.DotNetBar.ColorPickerButton()
        Me.ColorPickerOverllay3 = New DevComponents.DotNetBar.ColorPickerButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ColorPickerOverlay = New DevComponents.DotNetBar.ColorPickerDropDown()
        Me.ColorPickerDropDown1 = New DevComponents.DotNetBar.ColorPickerDropDown()
        Me.ColorPickerDropDown2 = New DevComponents.DotNetBar.ColorPickerDropDown()
        Me.ColorPickerDropDown3 = New DevComponents.DotNetBar.ColorPickerDropDown()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblSensitivity = New System.Windows.Forms.Label()
        Me.sldSensitivity2 = New System.Windows.Forms.TrackBar()
        Me.lblThreshold = New System.Windows.Forms.Label()
        Me.rbFixed = New System.Windows.Forms.RadioButton()
        Me.sldThreshold2 = New System.Windows.Forms.TrackBar()
        Me.rbAdaptive = New System.Windows.Forms.RadioButton()
        Me.sldThreshold = New DevComponents.DotNetBar.SliderItem()
        Me.sldSensitivity = New DevComponents.DotNetBar.SliderItem()
        Me.SliderItem1 = New DevComponents.DotNetBar.SliderItem()
        Me.SliderItem2 = New DevComponents.DotNetBar.SliderItem()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkInvert = New System.Windows.Forms.CheckBox()
        Me.chkExtract = New System.Windows.Forms.CheckBox()
        CType(Me.intTagFontSize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpMeasure.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.sldSensitivity2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sldThreshold2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.PowderBlue
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(466, 439)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(100, 36)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
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
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(132, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(102, 22)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Tag Font Size:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(116, 130)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(118, 18)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Measure Color:"
        '
        'intTagFontSize
        '
        '
        '
        '
        Me.intTagFontSize.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.intTagFontSize.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.intTagFontSize.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.intTagFontSize.Location = New System.Drawing.Point(244, 89)
        Me.intTagFontSize.MaxValue = 64
        Me.intTagFontSize.MinValue = 8
        Me.intTagFontSize.Name = "intTagFontSize"
        Me.intTagFontSize.ShowUpDown = True
        Me.intTagFontSize.Size = New System.Drawing.Size(58, 26)
        Me.intTagFontSize.TabIndex = 13
        Me.intTagFontSize.Value = 22
        '
        'ColorPickerButton1
        '
        Me.ColorPickerButton1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ColorPickerButton1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ColorPickerButton1.Image = CType(resources.GetObject("ColorPickerButton1.Image"), System.Drawing.Image)
        Me.ColorPickerButton1.Location = New System.Drawing.Point(246, 130)
        Me.ColorPickerButton1.Name = "ColorPickerButton1"
        Me.ColorPickerButton1.SelectedColorImageRectangle = New System.Drawing.Rectangle(2, 2, 12, 12)
        Me.ColorPickerButton1.Size = New System.Drawing.Size(56, 18)
        Me.ColorPickerButton1.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.CadetBlue
        Me.Label3.Location = New System.Drawing.Point(12, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(123, 22)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Preferences"
        '
        'grpMeasure
        '
        Me.grpMeasure.BackColor = System.Drawing.Color.Transparent
        Me.grpMeasure.Controls.Add(Me.rbCent)
        Me.grpMeasure.Controls.Add(Me.rbMeters)
        Me.grpMeasure.Controls.Add(Me.rbFeet)
        Me.grpMeasure.Controls.Add(Me.rbInches)
        Me.grpMeasure.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpMeasure.Location = New System.Drawing.Point(334, 77)
        Me.grpMeasure.Name = "grpMeasure"
        Me.grpMeasure.Size = New System.Drawing.Size(146, 127)
        Me.grpMeasure.TabIndex = 17
        Me.grpMeasure.TabStop = False
        Me.grpMeasure.Text = "Measure Unit"
        '
        'rbCent
        '
        Me.rbCent.AutoSize = True
        Me.rbCent.BackColor = System.Drawing.Color.Transparent
        Me.rbCent.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbCent.Location = New System.Drawing.Point(21, 93)
        Me.rbCent.Name = "rbCent"
        Me.rbCent.Size = New System.Drawing.Size(96, 20)
        Me.rbCent.TabIndex = 20
        Me.rbCent.TabStop = True
        Me.rbCent.Text = "Centimeters"
        Me.rbCent.UseVisualStyleBackColor = False
        '
        'rbMeters
        '
        Me.rbMeters.AutoSize = True
        Me.rbMeters.BackColor = System.Drawing.Color.Transparent
        Me.rbMeters.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbMeters.Location = New System.Drawing.Point(21, 69)
        Me.rbMeters.Name = "rbMeters"
        Me.rbMeters.Size = New System.Drawing.Size(66, 20)
        Me.rbMeters.TabIndex = 19
        Me.rbMeters.TabStop = True
        Me.rbMeters.Text = "Meters"
        Me.rbMeters.UseVisualStyleBackColor = False
        '
        'rbFeet
        '
        Me.rbFeet.AutoSize = True
        Me.rbFeet.BackColor = System.Drawing.Color.Transparent
        Me.rbFeet.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbFeet.Location = New System.Drawing.Point(21, 45)
        Me.rbFeet.Name = "rbFeet"
        Me.rbFeet.Size = New System.Drawing.Size(52, 20)
        Me.rbFeet.TabIndex = 18
        Me.rbFeet.TabStop = True
        Me.rbFeet.Text = "Feet"
        Me.rbFeet.UseVisualStyleBackColor = False
        '
        'rbInches
        '
        Me.rbInches.AutoSize = True
        Me.rbInches.BackColor = System.Drawing.Color.Transparent
        Me.rbInches.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbInches.Location = New System.Drawing.Point(21, 21)
        Me.rbInches.Name = "rbInches"
        Me.rbInches.Size = New System.Drawing.Size(64, 20)
        Me.rbInches.TabIndex = 17
        Me.rbInches.TabStop = True
        Me.rbInches.Text = "Inches"
        Me.rbInches.UseVisualStyleBackColor = False
        '
        'ColorPickerBase
        '
        Me.ColorPickerBase.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ColorPickerBase.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ColorPickerBase.ForeColor = System.Drawing.Color.Maroon
        Me.ColorPickerBase.Image = CType(resources.GetObject("ColorPickerBase.Image"), System.Drawing.Image)
        Me.ColorPickerBase.Location = New System.Drawing.Point(246, 168)
        Me.ColorPickerBase.Name = "ColorPickerBase"
        Me.ColorPickerBase.SelectedColorImageRectangle = New System.Drawing.Rectangle(2, 2, 12, 12)
        Me.ColorPickerBase.Size = New System.Drawing.Size(56, 18)
        Me.ColorPickerBase.TabIndex = 18
        '
        'ColorPickerOverllay3
        '
        Me.ColorPickerOverllay3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ColorPickerOverllay3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ColorPickerOverllay3.Image = CType(resources.GetObject("ColorPickerOverllay3.Image"), System.Drawing.Image)
        Me.ColorPickerOverllay3.Location = New System.Drawing.Point(246, 208)
        Me.ColorPickerOverllay3.Name = "ColorPickerOverllay3"
        Me.ColorPickerOverllay3.SelectedColorImageRectangle = New System.Drawing.Rectangle(2, 2, 12, 12)
        Me.ColorPickerOverllay3.Size = New System.Drawing.Size(56, 18)
        Me.ColorPickerOverllay3.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(138, 170)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(90, 16)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Base Color:"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(132, 208)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(114, 18)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Overlay Color:"
        '
        'ColorPickerOverlay
        '
        Me.ColorPickerOverlay.AutoExpandOnClick = True
        Me.ColorPickerOverlay.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.ColorPickerOverlay.DisplayThemeColors = False
        Me.ColorPickerOverlay.FontBold = True
        Me.ColorPickerOverlay.Name = "ColorPickerOverlay"
        Me.ColorPickerOverlay.PersonalizedMenus = DevComponents.DotNetBar.ePersonalizedMenus.Both
        Me.ColorPickerOverlay.PopupSide = DevComponents.DotNetBar.ePopupSide.Right
        Me.ColorPickerOverlay.PopupWidth = 250
        Me.ColorPickerOverlay.Shape = New DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2)
        Me.ColorPickerOverlay.Text = "Overlay Color"
        '
        'ColorPickerDropDown1
        '
        Me.ColorPickerDropDown1.AutoExpandOnClick = True
        Me.ColorPickerDropDown1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.ColorPickerDropDown1.DisplayThemeColors = False
        Me.ColorPickerDropDown1.FontBold = True
        Me.ColorPickerDropDown1.Name = "ColorPickerDropDown1"
        Me.ColorPickerDropDown1.PopupSide = DevComponents.DotNetBar.ePopupSide.Right
        Me.ColorPickerDropDown1.Shape = New DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2)
        Me.ColorPickerDropDown1.Text = "Base Color"
        '
        'ColorPickerDropDown2
        '
        Me.ColorPickerDropDown2.AutoExpandOnClick = True
        Me.ColorPickerDropDown2.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.ColorPickerDropDown2.DisplayThemeColors = False
        Me.ColorPickerDropDown2.FontBold = True
        Me.ColorPickerDropDown2.Name = "ColorPickerDropDown2"
        Me.ColorPickerDropDown2.PersonalizedMenus = DevComponents.DotNetBar.ePersonalizedMenus.Both
        Me.ColorPickerDropDown2.PopupSide = DevComponents.DotNetBar.ePopupSide.Right
        Me.ColorPickerDropDown2.PopupWidth = 250
        Me.ColorPickerDropDown2.Shape = New DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2)
        Me.ColorPickerDropDown2.Text = "Overlay Color"
        '
        'ColorPickerDropDown3
        '
        Me.ColorPickerDropDown3.AutoExpandOnClick = True
        Me.ColorPickerDropDown3.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.ColorPickerDropDown3.DisplayThemeColors = False
        Me.ColorPickerDropDown3.FontBold = True
        Me.ColorPickerDropDown3.Name = "ColorPickerDropDown3"
        Me.ColorPickerDropDown3.PopupSide = DevComponents.DotNetBar.ePopupSide.Right
        Me.ColorPickerDropDown3.Shape = New DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2)
        Me.ColorPickerDropDown3.Text = "Base Color"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.lblSensitivity)
        Me.GroupBox1.Controls.Add(Me.sldSensitivity2)
        Me.GroupBox1.Controls.Add(Me.lblThreshold)
        Me.GroupBox1.Controls.Add(Me.rbFixed)
        Me.GroupBox1.Controls.Add(Me.sldThreshold2)
        Me.GroupBox1.Controls.Add(Me.rbAdaptive)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(110, 332)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(295, 131)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Thresholding"
        '
        'lblSensitivity
        '
        Me.lblSensitivity.BackColor = System.Drawing.Color.Transparent
        Me.lblSensitivity.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSensitivity.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSensitivity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSensitivity.Location = New System.Drawing.Point(6, 54)
        Me.lblSensitivity.Name = "lblSensitivity"
        Me.lblSensitivity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSensitivity.Size = New System.Drawing.Size(102, 22)
        Me.lblSensitivity.TabIndex = 27
        Me.lblSensitivity.Text = "Sensitivity"
        '
        'sldSensitivity2
        '
        Me.sldSensitivity2.Location = New System.Drawing.Point(9, 79)
        Me.sldSensitivity2.Maximum = 100
        Me.sldSensitivity2.Name = "sldSensitivity2"
        Me.sldSensitivity2.Size = New System.Drawing.Size(138, 45)
        Me.sldSensitivity2.TabIndex = 25
        Me.sldSensitivity2.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.sldSensitivity2.Value = 20
        '
        'lblThreshold
        '
        Me.lblThreshold.BackColor = System.Drawing.Color.Transparent
        Me.lblThreshold.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThreshold.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThreshold.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThreshold.Location = New System.Drawing.Point(151, 54)
        Me.lblThreshold.Name = "lblThreshold"
        Me.lblThreshold.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThreshold.Size = New System.Drawing.Size(102, 22)
        Me.lblThreshold.TabIndex = 28
        Me.lblThreshold.Text = "Threshold:"
        '
        'rbFixed
        '
        Me.rbFixed.AutoSize = True
        Me.rbFixed.BackColor = System.Drawing.Color.Transparent
        Me.rbFixed.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbFixed.Location = New System.Drawing.Point(154, 25)
        Me.rbFixed.Name = "rbFixed"
        Me.rbFixed.Size = New System.Drawing.Size(58, 20)
        Me.rbFixed.TabIndex = 18
        Me.rbFixed.Text = "Fixed"
        Me.rbFixed.UseVisualStyleBackColor = False
        '
        'sldThreshold2
        '
        Me.sldThreshold2.Location = New System.Drawing.Point(154, 79)
        Me.sldThreshold2.Maximum = 255
        Me.sldThreshold2.Name = "sldThreshold2"
        Me.sldThreshold2.Size = New System.Drawing.Size(138, 45)
        Me.sldThreshold2.TabIndex = 26
        Me.sldThreshold2.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.sldThreshold2.Value = 50
        '
        'rbAdaptive
        '
        Me.rbAdaptive.AutoSize = True
        Me.rbAdaptive.BackColor = System.Drawing.Color.Transparent
        Me.rbAdaptive.Checked = True
        Me.rbAdaptive.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbAdaptive.Location = New System.Drawing.Point(21, 25)
        Me.rbAdaptive.Name = "rbAdaptive"
        Me.rbAdaptive.Size = New System.Drawing.Size(75, 20)
        Me.rbAdaptive.TabIndex = 17
        Me.rbAdaptive.TabStop = True
        Me.rbAdaptive.Text = "Adaptive"
        Me.rbAdaptive.UseVisualStyleBackColor = False
        '
        'sldThreshold
        '
        Me.sldThreshold.Maximum = 255
        Me.sldThreshold.Name = "sldThreshold"
        Me.sldThreshold.Text = "Thresh"
        Me.sldThreshold.Value = 0
        '
        'sldSensitivity
        '
        Me.sldSensitivity.Name = "sldSensitivity"
        Me.sldSensitivity.Text = "Radius"
        Me.sldSensitivity.Value = 0
        '
        'SliderItem1
        '
        Me.SliderItem1.Maximum = 255
        Me.SliderItem1.Name = "SliderItem1"
        Me.SliderItem1.Text = "Thresh"
        Me.SliderItem1.Value = 0
        '
        'SliderItem2
        '
        Me.SliderItem2.Name = "SliderItem2"
        Me.SliderItem2.Text = "Radius"
        Me.SliderItem2.Value = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.chkInvert)
        Me.GroupBox2.Controls.Add(Me.chkExtract)
        Me.GroupBox2.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(158, 255)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(429, 60)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "PDF to TIF"
        '
        'chkInvert
        '
        Me.chkInvert.AutoSize = True
        Me.chkInvert.Location = New System.Drawing.Point(197, 25)
        Me.chkInvert.Name = "chkInvert"
        Me.chkInvert.Size = New System.Drawing.Size(127, 23)
        Me.chkInvert.TabIndex = 1
        Me.chkInvert.Text = "Invert Output"
        Me.chkInvert.UseVisualStyleBackColor = True
        '
        'chkExtract
        '
        Me.chkExtract.AutoSize = True
        Me.chkExtract.Location = New System.Drawing.Point(24, 25)
        Me.chkExtract.Name = "chkExtract"
        Me.chkExtract.Size = New System.Drawing.Size(110, 23)
        Me.chkExtract.TabIndex = 0
        Me.chkExtract.Text = "Extract TIF"
        Me.chkExtract.UseVisualStyleBackColor = True
        '
        'frmPreferences
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CancelButton = Me.cmdOK
        Me.ClientSize = New System.Drawing.Size(599, 487)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ColorPickerOverllay3)
        Me.Controls.Add(Me.ColorPickerBase)
        Me.Controls.Add(Me.grpMeasure)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ColorPickerButton1)
        Me.Controls.Add(Me.intTagFontSize)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(52, 101)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPreferences"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "ComparA Preferences"
        CType(Me.intTagFontSize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpMeasure.ResumeLayout(False)
        Me.grpMeasure.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.sldSensitivity2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sldThreshold2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents imgList32 As System.Windows.Forms.ImageList
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents intTagFontSize As DevComponents.Editors.IntegerInput
    Friend WithEvents ColorPickerButton1 As DevComponents.DotNetBar.ColorPickerButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents grpMeasure As System.Windows.Forms.GroupBox
    Friend WithEvents rbCent As System.Windows.Forms.RadioButton
    Friend WithEvents rbMeters As System.Windows.Forms.RadioButton
    Friend WithEvents rbFeet As System.Windows.Forms.RadioButton
    Friend WithEvents rbInches As System.Windows.Forms.RadioButton
    Friend WithEvents ColorPickerBase As DevComponents.DotNetBar.ColorPickerButton
    Friend WithEvents ColorPickerOverllay3 As DevComponents.DotNetBar.ColorPickerButton
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ColorPickerOverlay As DevComponents.DotNetBar.ColorPickerDropDown
    Friend WithEvents ColorPickerDropDown1 As DevComponents.DotNetBar.ColorPickerDropDown
    Friend WithEvents ColorPickerDropDown2 As DevComponents.DotNetBar.ColorPickerDropDown
    Friend WithEvents ColorPickerDropDown3 As DevComponents.DotNetBar.ColorPickerDropDown
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbFixed As System.Windows.Forms.RadioButton
    Friend WithEvents rbAdaptive As System.Windows.Forms.RadioButton
    Friend WithEvents sldThreshold As DevComponents.DotNetBar.SliderItem
    Friend WithEvents sldSensitivity As DevComponents.DotNetBar.SliderItem
    Friend WithEvents SliderItem1 As DevComponents.DotNetBar.SliderItem
    Friend WithEvents SliderItem2 As DevComponents.DotNetBar.SliderItem
    Friend WithEvents sldSensitivity2 As System.Windows.Forms.TrackBar
    Friend WithEvents sldThreshold2 As System.Windows.Forms.TrackBar
    Public WithEvents lblSensitivity As System.Windows.Forms.Label
    Public WithEvents lblThreshold As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkInvert As System.Windows.Forms.CheckBox
    Friend WithEvents chkExtract As System.Windows.Forms.CheckBox
#End Region
End Class