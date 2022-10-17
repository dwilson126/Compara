
Option Strict Off
Option Explicit On
Imports GdPicture14
Imports System
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices


Public Class frmMain
    Protected license As QLM.LicenseValidator
    Private computerID As String
    Private blnStartupCompleted As Boolean = False
    Private TIFOutFile As String
    Private blnTIFSaved As Boolean = False
    Private blnJPGSaved As Boolean = False
    Private OutputFilePath As String
    Private OutputJPGPath As String
    Private SourceExtents As String
    Private OverlayExtents As String
    Private mvarTempDoc As String = User_Path() & "\TempDoc.tif"
    Dim sourceID As Integer
    Dim overlayID As Integer
    Dim mergedID As Integer
    Dim commonID As Integer 'hold te common pixels
    Dim TiffID As Integer = 0  'the multipage TIFF document 
    Dim mvarSourcePage As Integer
    Dim mvarOverlayPage As Integer
    Private mvarSourcePageCount As Integer
    Private mvarOverlayPageCount As Integer
    Private mvarSourceDocumentFormat As GdPicture14.DocumentFormat
    Private mvarOverlayDocumentFormat As GdPicture14.DocumentFormat
    Private mvarisGTX As Boolean = False
    Private mvarConvertMode As Boolean
    Dim txtDestFile As String
    Dim mvarRenderDPI As Integer = 150
    Private intThumbStart As Integer
    Private intThumbEnd As Integer
    Private blnPrompted As Boolean = False
    Private ClipCount As Integer = 1
    Private isThresholded As Boolean
    Private blnNewFile As Boolean
    Dim FirstPoint As Point
    Dim SecondPoint As Point
    Private blncopyregion As Boolean = False
    Private mvarFileName As String
    Private mvarNoiseSize As Integer = 5
    Private mvarLastAnnotation As Integer
    Private mvarIsViewer1 As Boolean
    Private mvarMeasureScale As Double = 1.0
    Private mvarActiveChanges As Boolean = False
    Private mvarMono As Integer = 1
    Private gSourceDir As String
    Private fileCounter As Integer
    Private groupCounter As Integer = 1
    Private ocrtimercount As Integer = 1
    Dim strcounter As String
    Dim hitchar As String
    Private mvarIsSketch As Boolean = False
    Private m_StrokingColor As Color = Color.BurlyWood
    Private m_FillingColor As Color = Color.Red
    Dim myLeft As Short
    Dim myTop As Short
    Dim myWidth As Short
    Dim myHeight As Short

    'gd picture handles
    Dim BasePDF As New GdPicture14.GdPicturePDF
    Dim OverlayPDF As New GdPicture14.GdPicturePDF
    Private myGD As New GdPicture14.GdPictureImaging
    Dim mvarSourceGDHandle As Integer
    Private SourceCloneID As Integer
    Private OverlayCloneId As Integer
    Dim mvarSelectedItem As Integer
    Private mvarMask As String = "_"
    Private GroupList As New Collection
    Dim CurrentSelection As Rectangle

    Private Nudgeleft, NudgeTop As Integer
    Private lt, tp, rt, bt As Integer
    Private mvarTempImage As String
    Private mvarZoomMode As Boolean = True
    Private mvarOCRDone As Boolean
    Private strOCRResults As String

    Private clipLeft As Integer
    Private clipRight As Integer
    Private clipTop As Integer
    Private clipBottom As Integer
    Private myApp As String = APP_Path()
    Dim worldWidth As Double
    Dim worldHeight As Double
    Dim cropx2 As Double
    Dim cropx1 As Double
    Dim cropy1 As Double
    Dim cropy2 As Double
    Private mvarCircleSize As Double

    'now the CompareTag data type
    Structure CompareTag
        Dim Name As String
        Dim Left As Integer
        Dim top As Integer
        Dim width As Integer
        Dim height As Integer
        Dim sourcePage As Integer
        Dim OverlayPage As Integer
        Dim ComparePage As Integer
        Dim Mergedimage As Image
        Dim sourceImage As Image
        Dim DestImage As Image
    End Structure
    Private mvarCompareCount As Integer
    Private CompareTags(25) As CompareTag

    Private Sub frmMain_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Try
            'Application.Exit()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Form_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs)

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then

            Dim Files() As String
            Files = e.Data.GetData(DataFormats.FileDrop)
            mvarFileName = Files(0)

            OpenFile(Files(0), True, DocuFiSession.IsPDFTrans)

        End If

    End Sub
    Private Sub DisplayLicenseForm(ByVal computerID As String)

        Dim licenseFrm As frmLicense
        licenseFrm = New frmLicense(license, computerID)

        licenseFrm.ShowDialog()

    End Sub

    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.Delete Then
            If mvarIsViewer1 Then
                GdViewerBase.DeleteAnnotation(mvarLastAnnotation)
            Else
                GdViewerOverlay.DeleteAnnotation(mvarLastAnnotation)
            End If

        End If
    End Sub
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                'components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub Form1_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Me.KeyPreview = True 'allow entry of the delete key
            Try
                Dim oLicenseManager As New GdPicture14.LicenseManager

                oLicenseManager.RegisterKEY("21187998990978994171811632377917792928")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            'This call is required by the Windows Form Designer.- PDF

            Me.AllowDrop = True
            Dim featureset As Integer
            Me.BackgroundImageLayout = ImageLayout.Stretch
            'Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None

            'AddHandler Me.DragDrop, AddressOf Form_DragDrop
            'comboResolution.SelectedItem = 1 'set the default dpi to 200 for rendering
            comboResolution.SelectedIndex = 2
            Dim txtLicenseFile As String = ""
            Dim needsActivation As Boolean
            Dim errorMessage As String
            needsActivation = False
            errorMessage = String.Empty
            ' Set the computerID to validate computerBound Keys
            computerID = String.Empty
            DocuFiSession.OEM = String.Empty
            'handle the licensing for accela
            'added true for accela build
            If IO.File.Exists(APP_Path() & "\AccelaLicense.lic") Then
                DocuFiSession.Authorized = True
                DocuFiSession.isMeasure = True
                DocuFiSession.IsPublish = True
                DocuFiSession.isCompare = True
                DocuFiSession.IsMultipage = True 'added 3.3
                DocuFiSession.IsPDFTrans = False
                DocuFiSession.OEM = "Accela"

                'remove the higher resolution renderings from the accela project
                comboResolution.Items.Remove(Combo400)
                comboResolution.Items.Remove(Combo300)
                Me.WindowState = FormWindowState.Maximized
                'handle licensing for PDF Trans

            Else 'use the qlm licensing system 
                Me.WindowState = FormWindowState.Normal
                '3.32 added check for comparalicense and pdftrans license files

            End If

            If license.ValidateLicenseAtStartup(Environment.MachineName, needsActivation, errorMessage) Then
                DocuFiSession.Authorized = True

                featureset = license.FeatureSet
                If license.EvaluationExpired Then
                    If DocuFiSession.isCompare Then
                        PopupBox("DocuFi License Manager", "Your License has expired.  Thank you for considering purchasing ComPara. We will now load a demo file for you to continue evaluating ComPara.", 400)
                    Else
                        PopupBox("DocuFi License Manager", "Your License has expired.  Thank you for using PDFTrans,  you are being redirected to the DocuFi PDFTrans license request page.", 400)
                        System.Diagnostics.Process.Start("http://www.docufi.com/products/edit-compare-convert-pdf-tif/pdftrans-annual-license-renewal")
                    End If
                    DocuFiSession.Authorized = False

                    'what to turn on here
                    DocuFiSession.IsPDFTrans = True
                    DocuFiSession.isCompare = True
                    DocuFiSession.IsPublish = True
                    DocuFiSession.IsMultipage = True
                    DocuFiSession.isMeasure = True
                    DisplayLicenseForm(computerID)
                ElseIf license.IsEvaluation Then
                    PopupBox("DocuFi License Manager", "Your License expires in " & license.EvaluationRemainingDays.ToString & " days", 400)

                    DocuFiSession.isCompare = (featureset And FeatCompare)
                    DocuFiSession.isMeasure = (featureset And FeatMeasure)
                    DocuFiSession.IsPublish = (featureset And FeatPublish) 'for multipage, tag, and publish
                    DocuFiSession.IsPDFTrans = (featureset And FeatPDFTrans)
                    DocuFiSession.IsMultipage = (featureset And FeatMultiPage)

                Else
                    DocuFiSession.Authorized = True
                    DocuFiSession.isCompare = (featureset And FeatCompare)
                    DocuFiSession.isMeasure = (featureset And FeatMeasure)
                    DocuFiSession.IsPublish = (featureset And FeatPublish) 'for multipage, tag, and publish
                    DocuFiSession.IsPDFTrans = (featureset And FeatPDFTrans)
                    DocuFiSession.IsMultipage = (featureset And FeatMultiPage)
                End If
            Else


                If IO.File.Exists(APP_Path() & "ComparaLicense.lic") Then
                    'read the line of text
                    DocuFiSession.isCompare = True
                    txtLicenseFile = "ComparaLicense.lic"
                ElseIf IO.File.Exists(APP_Path() & "PDFTransLicense.lic") Then 'gtx handling
                    DocuFiSession.IsPDFTrans = True
                    txtLicenseFile = "PDFTransLicense.lic"
                    RibbonPDFTrans.Select()
                    RibbonOpen.Width = 300
                End If

                Dim myLicense As String
                If txtLicenseFile <> "" Then
                    myLicense = IO.File.ReadAllText(APP_Path() & txtLicenseFile)
                    myLicense = Strings.Replace(myLicense, " ", "") 'get rid of spaces
                    myLicense = myLicense.Trim
                    DocuFiSession.LicenseKey = myLicense
                End If

                If license.ValidateLicense(Trim(myLicense), "", "", needsActivation, errorMessage) Then
                    featureset = license.FeatureSet

                    If license.EvaluationExpired Then
                        DocuFiSession.Authorized = False
                        DocuFiSession.IsEvaluation = True
                    Else
                        DocuFiSession.Authorized = True
                    End If
                    DocuFiSession.isCompare = (featureset And FeatCompare)
                    DocuFiSession.isMeasure = (featureset And FeatMeasure)
                    DocuFiSession.IsPublish = (featureset And FeatPublish) 'for multipage, tag, and publish
                    DocuFiSession.IsPDFTrans = (featureset And FeatPDFTrans)
                    DocuFiSession.IsMultipage = (featureset And FeatMultiPage)
                    DocuFiSession.IsEvaluation = license.IsEvaluation
                Else
                    If license.EvaluationExpired Then
                        DocuFiSession.Authorized = False
                        DocuFiSession.IsEvaluation = True
                    End If

                    DocuFiSession.needsActivation = needsActivation
                    DisplayLicenseForm(computerID)
                End If

            End If

            myApp = Strings.Replace(myApp, "\", "\\")
            'LoadSettings("default")
            fileCounter = 1
            Me.AllowDrop = True

            GdViewerBase.Width = Me.Width / 2 - 5
            If GdViewerOverlay.Visible Then GdViewerOverlay.Width = GdViewerBase.Width

            'now see if we are in 
            If DocuFiSession.isCompare Then
                Me.Text = "ComPare, Convert, and Organize"
                RibbonCompare.Select()
                OverlayPanel.Text = "Overlay Pages"
                btnDelPage.Enabled = DocuFiSession.IsMultipage
                btnInsertAfter.Enabled = DocuFiSession.IsMultipage
                BtnInsertBefore.Enabled = DocuFiSession.IsMultipage
                Application.DoEvents()
            End If


            'now display evaluation or expiration notifications if it is not an oem branded version
            If DocuFiSession.OEM <> "Accela" Then
                If license.IsEvaluation Then
                    mvarRemainingDays = license.EvaluationRemainingDays
                    Me.Text += " (Expires in " & license.EvaluationRemainingDays.ToString & " days)"
                ElseIf license.EvaluationExpired Then
                    Me.Text += " (Evaluation Expired, Limited Functionality)"
                End If
            End If
            'now deal with command line argments sent to the session object
            If DocuFiSession.isNoOpen Then
                MenuOpenCompare.Visible = False
                btnOpenFile.Visible = False 'new gui icon
            End If
            If DocuFiSession.openBaseFile Then ' we got a command line parsed file
                OpenFile(mvarCurrentFile, False, False)
            End If

            'Now deal with free trial version. turn everything on but single page only
            If DocuFiSession.Authorized = False Then
                Me.Text = Me.Text & " Trial Version"
                Me.WindowState = FormWindowState.Normal
                'OpenFile(APP_Path() & "samples\comparebefore.pdf", False, False)
                DocuFiSession.IsMultipage = True
                DocuFiSession.IsPDFTrans = False
                DocuFiSession.isCompare = True
                DocuFiSession.isMeasure = True
                DocuFiSession.IsPublish = True
                DocuFiSession.Authorized = False
            End If
            'now handle gui considerations based on the licensed features
            btnPublishTagstoPDF.Visible = DocuFiSession.IsPublish
            RibbonTabMeasure.Visible = DocuFiSession.isMeasure
            RibbonCompare.Visible = DocuFiSession.isCompare
            RibbonPDFTrans.Visible = False
            RibbonEdit.Visible = False
            RibbonTabDraw.Visible = False
            RibbonEnhance.Visible = False

            'now deal with the thumbnail panels
            OverlayPanel.Expanded = False
            BasePanel.Expanded = False
            thumbBase.Visible = False

            If GdViewerOverlay.Visible Then
                GdViewerBase.Width = Me.Width / 2 - 5
                GdViewerOverlay.Width = GdViewerBase.Width
                OverlayPanel.Left = Me.Width - GdViewerOverlay.Width
            End If

            btnDeletions.Visible = DocuFiSession.isCompare
            btnAdditions.Visible = DocuFiSession.isCompare
            btnCommon.Visible = DocuFiSession.isCompare
            btnDifferences.Visible = DocuFiSession.isCompare


            'now load the mru list
            lblMRU1.Tag = GetSetting("ComPara", "Recent Files", "MRU1")
            If lblMRU1.Tag <> "" Then
                lblMRU1.Text = IO.Path.GetFileName(lblMRU1.Tag)
            End If
            lblMRU2.Tag = GetSetting("ComPara", "Recent Files", "MRU2")
            If lblMRU2.Tag <> "" Then
                lblMRU2.Text = IO.Path.GetFileName(lblMRU2.Tag)
            End If
            lblMRU3.Tag = GetSetting("ComPara", "Recent Files", "MRU3")
            If lblMRU3.Tag <> "" Then
                lblMRU3.Text = IO.Path.GetFileName(lblMRU3.Tag)
            End If
            lblMRU4.Tag = GetSetting("ComPara", "Recent Files", "MRU4")
            If lblMRU4.Tag <> "" Then
                lblMRU4.Text = IO.Path.GetFileName(lblMRU4.Tag)
            End If
            lblMRU5.Tag = GetSetting("ComPara", "Recent Files", "MRU5")
            If lblMRU5.Tag <> "" Then
                lblMRU5.Text = IO.Path.GetFileName(lblMRU5.Tag)
            End If

            '----xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            ' mvarDrawColor = Color.Red

            'xxxxxxxxxxxxxxxxx
            'LoadPreferences()

            ''display the default units
            'Select Case mvarUnits
            '    Case 1
            '        lblUnits.Text = "Inches"
            '    Case 2
            '        lblUnits.Text = "Feet"
            '    Case 3
            '        lblUnits.Text = "Meters"
            '    Case 4
            '        lblUnits.Text = "Centimeters"
            'End Select

        Catch
            MsgBox(Err.Description)
        End Try

    End Sub
    Private Sub LoadPreferences()

        Try


            Dim tmpString As String
            tmpString = GetSetting("Compara", "Preferences", "TagFontSize")
            If IsNumeric(tmpString) Then
                mvarTagFontSize = CInt(tmpString)
            End If
            tmpString = GetSetting("Compara", "Preferences", "MeasureColor")
            If IsNumeric(tmpString) Then
                mvarDrawColor = Color.FromArgb(CInt(tmpString))
            End If
            tmpString = GetSetting("Compara", "Preferences", "BaseColor")
            If IsNumeric(tmpString) Then
                mvarBaseColor = Color.FromArgb(CInt(tmpString))
            End If
            tmpString = GetSetting("Compara", "Preferences", "OverlayColor")
            If IsNumeric(tmpString) Then
                mvarOverlayColor = Color.FromArgb(CInt(tmpString))
            End If
            tmpString = GetSetting("Compara", "Preferences", "Units")
            If IsNumeric(tmpString) Then
                mvarUnits = CInt(tmpString)
            End If

            tmpString = CStr(GetSetting("Compara", "Preferences", "Sensitivity"))
            If IsNumeric(tmpString) Then SensitivityValue = CLng(tmpString)

            tmpString = CStr(GetSetting("Compara", "Preferences", "Threshold"))
            If IsNumeric(tmpString) Then ThresholdValue = CLng(tmpString)

            tmpString = CStr(GetSetting("Compara", "Preferences", "ThresholdMode"))
            If IsNumeric(tmpString) Then DocuFiSession.ThreshMode = CLng(tmpString)

            Dim tmpBool As Boolean

            tmpBool = CBool(GetSetting("Compara", "Preferences", "ExtractTIF"))
            If tmpBool Then DocuFiSession.ExtractTIF = tmpBool

            tmpBool = CBool(GetSetting("Compara", "Preferences", "InvertTIF"))
            If tmpBool Then DocuFiSession.InvertExtraction = tmpBool

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ZoomInTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ZoomIn()

    End Sub

    Private Sub ZoomOutTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ZoomOut()
    End Sub
    Private Sub RotateRIghtTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Rotate90()
    End Sub

    Private Sub PanWIndowTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ShowPanWindow()
    End Sub

    Private Sub RotateLeftTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Rotate270()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        frmAbout.Show()
    End Sub


    Private Sub PBLMouse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Pan()
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#Region "Display"
    Public Sub Rotate90()

    End Sub
    Public Sub Rotate180()

    End Sub
    Public Sub Rotate270()

    End Sub
    Public Sub GotoPage(ByVal inPage As Integer)

    End Sub
    Public Sub ZoomOut()


    End Sub
    Public Sub ZoomIn()


    End Sub
    Public Sub ZoomExtents()

    End Sub
    Public Sub ZoomFit()

    End Sub
    Public Sub Pan()

    End Sub
    Public Sub ZoomWindow()

        mvarZoomMode = True

    End Sub
    Public Sub OCR()

        mvarZoomMode = False

    End Sub
    Public Sub ShowPanWindow()
        'set some properties for the PanWindow


    End Sub
#End Region
#Region "IO"

    Public Function OpenFileAsImageID(ByVal inFile As String, ByVal ForEDIT As Boolean) As Integer
        mvarCurrentFile = inFile

        'make sure the original file exists
        If IO.File.Exists(inFile) = False Then
            MsgBox("The file " & inFile & " does not exist")
            Exit Function
        End If


        Dim fileExt As String
        Dim GotIt As Boolean = False
        Me.Cursor = Cursors.WaitCursor
        GdPictureDocumentUtilities.ReleaseAllGdPictureImages()
        'now handle if they have authorized the product
        If DocuFiSession.Authorized Then
            Me.Text = "ComPara Viewer"
            btnProperties.Enabled = True
        End If

        If DocuFiSession.IsPDFTrans Then
            Me.Text = "PDFTrans Viewer"
        End If
        'clear out the bubble bar
        mvarCompareCount = 0
        bubbleBarTab1.Buttons.Clear()
        'see if the file name is in the Most Recent Used List
        If lblMRU1.Tag = inFile Then
            GotIt = True
        ElseIf lblMRU2.Tag = inFile Then
            GotIt = True
        ElseIf lblMRU3.Tag = inFile Then
            GotIt = True
        ElseIf lblMRU4.Tag = inFile Then
            GotIt = True
        ElseIf lblMRU5.Tag = inFile Then
            GotIt = True
        End If

        If GotIt = False Then

            lblMRU5.Tag = lblMRU4.Tag
            lblMRU4.Tag = lblMRU3.Tag
            lblMRU3.Tag = lblMRU2.Tag
            lblMRU2.Tag = lblMRU1.Tag
            lblMRU1.Tag = inFile

            lblMRU5.Text = lblMRU4.Text
            lblMRU4.Text = lblMRU3.Text
            lblMRU3.Text = lblMRU2.Text
            lblMRU2.Text = lblMRU1.Text
            lblMRU1.Text = IO.Path.GetFileName(inFile)
        End If

        mvarFileName = inFile


        ' mvarFileName = "C:\Program Files\Docufi\RevisA\Samples\comparebefore.tif"


        'Delete the temporary holding bin for the active document

        fileExt = IO.Path.GetExtension(mvarFileName).ToLower

        'if it is a pdf file, check to see if it has keyword flags indicating it is a merged file
        If fileExt = ".pdf" Then
            Dim myPDF2 As New GdPicturePDF
            Dim appendID As Integer

            myPDF2.LoadFromFile(mvarFileName, False)
            myPDF2.SelectPage(1)
            GdViewerBase.DisplayFromGdPicturePDF(myPDF2)
            thumbBase.LoadFromGdViewer(GdViewerBase)
            BasePanel.Expanded = False
            thumbBase.Visible = True
            BasePanel.Visible = True
            If False Then


                If myPDF2.GetPageCount > 1 Then
                    appendID = myPDF2.RenderPageToGdPictureImage(200, False)
                    sourceID = myGD.TiffCreateMultiPageFromGdPictureImage(appendID)
                    For i As Integer = 2 To myPDF2.GetPageCount()
                        myPDF2.SelectPage(i)
                        appendID = myPDF2.RenderPageToGdPictureImage(200, False)
                        myGD.TiffAppendPageFromGdPictureImage(sourceID, appendID)
                        myGD.ReleaseGdPictureImage(appendID)
                    Next
                    myGD.TiffCloseMultiPageFile(sourceID)
                    thumbBase.Visible = True
                    BasePanel.Visible = True
                    thumbBase.ClearAllItems()
                    GdViewerBase.DisplayFromGdPictureImage(sourceID)
                    GdViewerBase.SetZoomWidthViewer()
                    thumbBase.LoadFromGdViewer(GdViewerBase)
                    BasePanel.Expanded = False
                Else
                    sourceID = myPDF2.RenderPageToGdPictureImage(200, False)
                    BasePanel.Visible = False
                    GdViewerBase.DisplayFromGdPictureImage(sourceID)
                    GdViewerBase.SetZoomWidthViewer()
                End If

            End If

            myGD.SetContrast(sourceID, 100)
            myGD.ConvertTo1Bpp(sourceID)
            Dim mykeywords As String = myPDF2.GetKeywords
            If Strings.Left(mykeywords, 7).ToLower = "compara" Then

                ParseKeywords(mykeywords)
                LoadThumbsFromPDF(myPDF2)
                'now save the page to tif 

                myPDF2.CloseDocument()
                GdViewerOverlay.Visible = False
                GdViewerBase.Width = Me.Width

                btnCompareSidebySide.Visible = False
                btnCompareOverlay.Visible = False
                btnOpenFile.Visible = False
                btnCreateTags.Enabled = False
                'SliderTransparency.Visible = False
                btnTagComparisons.Visible = False
                RibbonCompNudge.Visible = False

                Exit Function

            End If
        Else
            sourceID = myGD.CreateGdPictureImageFromFile(mvarFileName)
            If DocuFiSession.Authorized = False Then ' stamp it
                myGD.DrawText(sourceID, "Trial License", 50, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(sourceID, "Trial License", 50, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(sourceID, "Trial License", myGD.GetWidth(sourceID) - 1500, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(sourceID, "Trial License", myGD.GetWidth(sourceID) - 1500, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(sourceID, "Trial License", myGD.GetWidth(sourceID) / 2, myGD.GetHeight(sourceID) / 2, 75, FontStyle.Bold, Color.Black, "Arial", False)
            End If
        End If

        bubbleBar1.Visible = False
        'now display the source (base) file
        GdViewerBase.DisplayFromGdPictureImage(sourceID)


        GdViewerOverlay.Visible = True
        GdViewerBase.Width = GdViewerOverlay.Width
        GdViewerBase.SetZoomWidthViewer()
        'now open the overay file
        Me.Cursor = Cursors.Default
        If DocuFiSession.Authorized = False Then
            OpenFileDialog1.FileName = APP_Path() & "samples\compareafter.pdf"
        Else

            OpenFileDialog1.Title = "Open Comparison File"
            OpenFileDialog1.RestoreDirectory = True
            OpenFileDialog1.FileName = mvarFileName
            OpenFileDialog1.Filter = "Compare File|*" & fileExt
            Dim res As Windows.Forms.DialogResult

            res = OpenFileDialog1.ShowDialog()

            If res = Windows.Forms.DialogResult.Cancel Then Exit Function

        End If
        'now turn on the GUI for having a second file
        btnCompareSidebySide.Visible = True
        btnCompareOverlay.Visible = True
        'btnCompareDiff.Visible = True
        'SliderTransparency.Visible = True
        btnCreateTags.Enabled = True
        btnTagComparisons.Visible = True
        RibbonCompNudge.Visible = True


        If OpenFileDialog1.FileName <> "" Then
            mvarOverlayFile = (OpenFileDialog1.FileName)

            ' Me.Text = "Compara Viewer by DocuFi"

            fileExt = IO.Path.GetExtension(mvarFileName).ToLower
            Me.Cursor = Cursors.WaitCursor
            'if it is a pdf file, render it to an image
            If fileExt = ".pdf" Then
                Dim myPDF2 As New GdPicturePDF
                Dim appendID As Integer
                myPDF2.LoadFromFile(mvarOverlayFile, False)
                myPDF2.SelectPage(1)
                If myPDF2.GetPageCount > 1 Then
                    appendID = myPDF2.RenderPageToGdPictureImage(200, False)
                    overlayID = myGD.TiffCreateMultiPageFromGdPictureImage(appendID)
                    For i As Integer = 2 To myPDF2.GetPageCount()
                        myPDF2.SelectPage(i)
                        appendID = myPDF2.RenderPageToGdPictureImage(200, False)
                        myGD.TiffAppendPageFromGdPictureImage(overlayID, appendID)
                        myGD.ReleaseGdPictureImage(appendID)
                    Next
                    myGD.TiffCloseMultiPageFile(overlayID)
                    thumbOverlay.ClearAllItems()
                    GdViewerOverlay.DisplayFromGdPictureImage(overlayID)
                    GdViewerOverlay.SetZoomWidthViewer()
                    thumbOverlay.LoadFromGdViewer(GdViewerOverlay)
                    OverlayPanel.Expanded = False
                    thumbOverlay.Visible = True
                    OverlayPanel.Visible = True
                Else

                    overlayID = myPDF2.RenderPageToGdPictureImage(200, False)
                    OverlayPanel.Visible = False
                    GdViewerOverlay.DisplayFromGdPictureImage(overlayID)
                    GdViewerOverlay.SetZoomWidthViewer()
                End If

                myGD.SetContrast(overlayID, 100)
                myGD.ConvertTo1Bpp(overlayID)
            Else
                overlayID = myGD.CreateGdPictureImageFromFile(mvarOverlayFile)
            End If

            If DocuFiSession.Authorized = False Then ' stamp it
                myGD.DrawText(overlayID, "Trial License", 50, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(overlayID, "Trial License", 50, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(overlayID, "Trial License", myGD.GetWidth(sourceID) - 1500, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(overlayID, "Trial License", myGD.GetWidth(sourceID) - 1500, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(overlayID, "Trial License", myGD.GetWidth(sourceID) / 2, myGD.GetHeight(sourceID) / 2, 75, FontStyle.Bold, Color.Black, "Arial", False)
            End If


            Me.Cursor = Cursors.Default
        End If


        lblBase2.Text = "  Source: " & IO.Path.GetFileName(mvarFileName) & "  Overlay: " & IO.Path.GetFileName(mvarOverlayFile)
        barStatus.Refresh()
        GdViewerBase.Width = Me.Width / 2 - 5
        If GdViewerOverlay.Visible Then GdViewerOverlay.Width = GdViewerBase.Width
        If GdViewerBase.PrintGetPaperHeight <> GdViewerOverlay.PrintGetPaperHeight Then

            PopupBox("File Open Warning", "The docuent widths do not match")
        End If
    End Function
    Public Function OpenFile(ByVal inFile As String, ByVal ForEDIT As Boolean, ByVal ISPDFTrans As Boolean) As Integer
        Try

            mvarCurrentFile = inFile

            'make sure the original file exists
            If IO.File.Exists(inFile) = False Then
                MsgBox("The file " & inFile & " does not exist")
                Exit Function
            End If


            Dim fileExt As String
            Dim GotIt As Boolean = False
            Me.Cursor = Cursors.WaitCursor

            'now handle if they have authorized the product


            'clear out the bubble bar
            mvarCompareCount = 0
            mvarSourcePageCount = 0
            gSourceisPDF = False
            mvarSourceGDHandle = 0

            bubbleBarTab1.Buttons.Clear()
            'see if the file name is in the Most Recent Used List
            If lblMRU1.Tag = inFile Then
                GotIt = True
            ElseIf lblMRU2.Tag = inFile Then
                GotIt = True
            ElseIf lblMRU3.Tag = inFile Then
                GotIt = True
            ElseIf lblMRU4.Tag = inFile Then
                GotIt = True
            ElseIf lblMRU5.Tag = inFile Then
                GotIt = True
            End If

            If GotIt = False Then

                lblMRU5.Tag = lblMRU4.Tag
                lblMRU4.Tag = lblMRU3.Tag
                lblMRU3.Tag = lblMRU2.Tag
                lblMRU2.Tag = lblMRU1.Tag
                lblMRU1.Tag = inFile

                lblMRU5.Text = lblMRU4.Text
                lblMRU4.Text = lblMRU3.Text
                lblMRU3.Text = lblMRU2.Text
                lblMRU2.Text = lblMRU1.Text
                lblMRU1.Text = IO.Path.GetFileName(inFile)
            End If

            mvarFileName = inFile


            ' mvarFileName = "C:\Program Files\Docufi\RevisA\Samples\comparebefore.tif"

            'Delete the temporary holding bin for the active document

            fileExt = IO.Path.GetExtension(mvarFileName).ToLower
            myGD.ReleaseGdPictureImage(overlayID)
            myGD.ReleaseGdPictureImage(sourceID)
            myGD.ReleaseGdPictureImage(commonID)

            GdViewerBase.CloseDocument()
            GdViewerOverlay.CloseDocument()
            GdViewerMerged.CloseDocument()


            'if it is a pdf file, check to see if it has keyword flags indicating it is a merged file
            BasePanel.Visible = True
            If fileExt = ".pdf" Then
                BasePDF.CloseDocument() 'close out anything still outstanding

                BasePDF.Dispose()



                BasePDF.LoadFromFile(mvarFileName, False)

                If BasePDF.IsEncrypted() Then
                    If BasePDF.SetPassword("") <> GdPictureStatus.OK Then
                        If BasePDF.SetPassword(InputBox("Password: ", "Password protected document")) <> GdPictureStatus.OK Then
                            MsgBox("Can not uncrypt document")
                            BasePDF.CloseDocument()
                            Me.Cursor = Cursors.Default
                            Exit Function
                        End If
                    End If
                End If



                If BasePDF.GetPageCount = 0 Then
                    PopupBox("Warning", "The base file was not loaded properly.  Please reload")
                    OverlayPDF.CloseDocument()
                    BasePDF.CloseDocument()
                    Me.Cursor = Cursors.Default
                    Exit Function
                End If

                If Not DocuFiSession.Authorized Then
                    Dim font_res_name As String = BasePDF.AddTrueTypeFont("Arial", False, False, False)
                    BasePDF.SetTextMode(PdfTextMode.PdfTextModeFill)
                    For j As Integer = 1 To BasePDF.GetPageCount
                        BasePDF.SelectPage(j)
                        ' BasePDF.AddStampAnnotation(BasePDF.GetPageWidth / 10, BasePDF.GetPageHeight / 2, BasePDF.GetPageWidth / 4, BasePDF.GetPageHeight / 6, "Trial", "Trial File", PdfRubberStampAnnotationIcon.NotApproved, 50, 255, 50, 50)


                        BasePDF.SetTextSize(BasePDF.GetPageHeight / 20)
                        BasePDF.SetFillColor(255, 125, 125, 0) 'Using Cyan colour
                        BasePDF.SetOrigin(PdfOrigin.PdfOriginTopLeft)
                        BasePDF.SetMeasurementUnit(PdfMeasurementUnit.PdfMeasurementUnitPoint)
                        BasePDF.DrawText(font_res_name, BasePDF.GetPageWidth / 10 * 8, BasePDF.GetPageHeight / 5, "Trial Version")
                        BasePDF.DrawText(font_res_name, BasePDF.GetPageWidth / 10 * 8, BasePDF.GetPageHeight / 5 * 4, "Trial Version")
                        BasePDF.DrawText(font_res_name, BasePDF.GetPageWidth / 10, BasePDF.GetPageHeight / 5, "Trial Version")
                        BasePDF.DrawText(font_res_name, BasePDF.GetPageWidth / 10, BasePDF.GetPageHeight / 5 * 4, "Trial Version")

                    Next
                End If
                mvarSourcePageCount = BasePDF.GetPageCount
                GdViewerBase.Clear()
                GdViewerBase.CloseDocument()
                gSourceisPDF = True
                SourceExtents = BasePDF.GetPageWidth() & " by " & BasePDF.GetPageHeight

                thumbBase.ClearAllItems()
                GdViewerBase.DisplayFromGdPicturePDF(BasePDF)

                myGD.ReleaseGdPictureImage(sourceID)

                thumbBase.LoadFromGdViewer(GdViewerBase)
                BasePanel.Expanded = False
                thumbBase.Visible = True
                BasePanel.Visible = True
                GdViewerMerged.Visible = False
                lblRenderResolution.Visible = True
                comboResolution.Visible = True
                'moved here as it caused errors when located 6 lines up
                GdViewerBase.Visible = True

                mvarOverlayPage = 1
                mvarSourcePage = 1
                Dim mykeywords As String = BasePDF.GetKeywords
                If Strings.Left(mykeywords, 7).ToLower = "compara" Then

                    ParseKeywords(mykeywords)
                    LoadThumbsFromPDF(BasePDF)
                    'now save the page to tif 

                    BasePDF.CloseDocument()
                    GdViewerOverlay.Visible = False
                    GdViewerBase.Width = Me.Width

                    btnCompareSidebySide.Visible = False
                    btnCompareOverlay.Visible = False
                    btnOpenFile.Visible = False
                    btnCreateTags.Enabled = False
                    'SliderTransparency.Visible = False
                    btnTagComparisons.Visible = False
                    RibbonCompNudge.Visible = False
                    Me.Cursor = Cursors.Default
                    Exit Function

                End If
            ElseIf fileExt = ".jpg" Or fileExt = ".jpeg" Then
                GdPictureDocumentUtilities.ReleaseAllGdPictureImages()
                mvarSourceGDHandle = myGD.CreateGdPictureImageFromFile(mvarFileName)
                mvarSourcePageCount = 1

                Dim mywidth, myheight As Integer
                mywidth = myGD.GetWidth(mvarSourceGDHandle)
                myheight = myGD.GetHeight(mvarSourceGDHandle)
                Dim myfont As Integer = myheight / 25
                myGD.DrawTextBox(mvarSourceGDHandle, "Trial Version", mywidth / 5, myheight / 5,
                                 mywidth / 2, myheight / 3, myfont, GdPicture14.TextAlignment.TextAlignmentNear, FontStyle.Bold, Color.Black, "Times New Roman", True, True)
                myGD.DrawTextBox(mvarSourceGDHandle, "Trial Version", mywidth / 5, myheight / 5 * 4,
                                mywidth / 2, myheight / 3, myfont, GdPicture14.TextAlignment.TextAlignmentNear, FontStyle.Bold, Color.Black, "Times New Roman", True, True)


                GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)

                lblRenderResolution.Visible = False
                comboResolution.Visible = False
            Else

                GdPictureDocumentUtilities.ReleaseAllGdPictureImages()

                mvarSourceGDHandle = myGD.TiffCreateMultiPageFromFile(mvarFileName)
                sourceID = mvarSourceGDHandle
                mvarSourcePageCount = myGD.TiffGetPageCount(mvarSourceGDHandle)

                If mvarSourcePageCount = 0 Then mvarSourcePageCount = 1

                If Not DocuFiSession.Authorized Then
                    For j As Integer = 1 To mvarSourcePageCount
                        myGD.TiffSelectPage(mvarSourceGDHandle, j)
                        Dim mywidth, myheight As Integer
                        mywidth = myGD.GetWidth(mvarSourceGDHandle)
                        myheight = myGD.GetHeight(mvarSourceGDHandle)
                        Dim myfont As Integer = myheight / 25
                        myGD.DrawTextBox(mvarSourceGDHandle, "Trial Version", mywidth / 5, myheight / 5,
                                         mywidth / 2, myheight / 3, myfont, GdPicture14.TextAlignment.TextAlignmentNear, FontStyle.Bold, Color.Black, "Times New Roman", True, True)
                        myGD.DrawTextBox(mvarSourceGDHandle, "Trial Version", mywidth / 5, myheight / 5 * 4,
                                        mywidth / 2, myheight / 3, myfont, GdPicture14.TextAlignment.TextAlignmentNear, FontStyle.Bold, Color.Black, "Times New Roman", True, True)
                    Next
                    'For j As Integer = 1 To mvarSourcePageCount

                    '    myGD.TiffSelectPage(mvarSourceGDHandle, j)
                    '    'GDImaging.FxBitonalDespeckleMore(m_CurrentImage, True)
                    '    'dblBlankThreshold = GDImaging.GetWidth(m_CurrentImage) * GDImaging.GetHeight(m_CurrentImage) * GDImaging.GetHorizontalResolution(m_CurrentImage)

                    '    myGD.AutoDeskew(mvarSourceGDHandle)
                    '    myGD.CropBorders(mvarSourceGDHandle, ImagingContext.ContextDocument)

                    'Next
                End If
                GdViewerBase.DisplayFromFile(mvarFileName)
                ' myGD.TiffOpenMultiPageForWrite(True)

                'user interface considerations for TIF
                lblRenderResolution.Visible = False
                comboResolution.Visible = False

                If DocuFiSession.Authorized = False Then ' stamp it
                    myGD.DrawText(sourceID, "Trial License", 50, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                    myGD.DrawText(sourceID, "Trial License", 50, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                    myGD.DrawText(sourceID, "Trial License", myGD.GetWidth(sourceID) - 1500, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                    myGD.DrawText(sourceID, "Trial License", myGD.GetWidth(sourceID) - 1500, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                    myGD.DrawText(sourceID, "Trial License", myGD.GetWidth(sourceID) / 2, myGD.GetHeight(sourceID) / 2, 75, FontStyle.Bold, Color.Black, "Arial", False)

                End If

                mvarOverlayPage = 1
                mvarSourcePage = 1
                ' myGD.TiffCloseMultiPageFile(sourceID)

                SourceExtents = myGD.GetWidth(mvarSourceGDHandle) & " by " & myGD.GetHeight(mvarSourceGDHandle) & " at " & myGD.GetHorizontalResolution(mvarSourceGDHandle) & " dpi"

                ' GdViewerBase.DisplayFromGdPictureImage(sourceID)

                GdViewerBase.Focus()
                thumbBase.LoadFromGdViewer(GdViewerBase)
                GdViewerBase.SetZoomFitViewer()
            End If

            bubbleBar1.Visible = False


            'display the thumbnails if greater than 1 page
            BasePanel.Visible = True
            OverlayPanel.Visible = True
            If mvarSourcePageCount <= 1 Then

                'BasePanel.Visible = False
                'OverlayPanel.Visible = False
            End If
            GdViewerOverlay.Clear()
            GdViewerOverlay.CloseDocument()

            If ISPDFTrans Then ' it's pdftrans, exit handling the overlay image
                lblBase2.Text = "  Source:  " & IO.Path.GetFileName(mvarFileName)
                Me.Cursor = Cursors.Default
                GdViewerOverlay.CloseDocument()
                thumbOverlay.ClearAllItems()
                Me.Cursor = Cursors.Default
                Exit Function
            End If

            'GdViewerBase.ZoomMode = ViewerZoomMode.ZoomModeFitToViewer
            'now open the overay file
            Me.Cursor = Cursors.Default
            'If DocuFiSession.Authorized = False Then
            '    OpenFileDialog1.FileName = APP_Path() & "samples\compareafter.pdf"
            'Else

            OpenFileDialog1.Title = "Open Comparison File"
            OpenFileDialog1.RestoreDirectory = True
            OpenFileDialog1.FileName = mvarFileName
            OpenFileDialog1.Filter = "Compare File|*" & fileExt
            Dim res As Windows.Forms.DialogResult

            If DocuFiSession.isSingle Then
                OpenFileDialog1.FileName = mvarFileName
            ElseIf mvarOverlayFile <> "" Then
                OpenFileDialog1.FileName = mvarOverlayFile
            Else

                res = OpenFileDialog1.ShowDialog()

                If res = Windows.Forms.DialogResult.Cancel Then Exit Function
            End If


            'End If
            'now turn on the GUI for having a second file
            btnCompareSidebySide.Visible = True
            btnCompareOverlay.Visible = True
            'btnCompareDiff.Visible = True
            'SliderTransparency.Visible = True
            btnCreateTags.Enabled = True
            btnTagComparisons.Visible = True
            RibbonCompNudge.Visible = True
            grpPageTools.Visible = True
            btnNextPage.Visible = True
            btnPreviousPage.Visible = True
            grpOverlayTools.Visible = False
            lblCompareTools.Visible = False
            barStatus.Refresh()

            If OpenFileDialog1.FileName <> "" Then
                mvarOverlayFile = (OpenFileDialog1.FileName)

                fileExt = IO.Path.GetExtension(mvarFileName).ToLower
                Me.Cursor = Cursors.WaitCursor
                'if it is a pdf file, render it to an image
                If fileExt = ".pdf" Then
                    OverlayPDF.CloseDocument()
                    OverlayPDF.Dispose()
                    OverlayPDF.LoadFromFile(mvarOverlayFile, False)

                    If OverlayPDF.IsEncrypted() Then
                        If OverlayPDF.SetPassword("") <> GdPictureStatus.OK Then
                            If OverlayPDF.SetPassword(InputBox("Password: ", "Password protected document")) <> GdPictureStatus.OK Then
                                MsgBox("Can not uncrypt document")
                                OverlayPDF.CloseDocument()
                                Exit Function
                            End If
                        End If
                    End If
                    OverlayPDF.SelectPage(1)

                    'GdViewerOverlay.ZoomMode = ViewerZoomMode.ZoomModeFitToViewer
                    OverlayExtents = OverlayPDF.GetPageWidth.ToString & " by " & OverlayPDF.GetPageHeight.ToString
                    OverlayPanel.Expanded = False
                    mvarOverlayPageCount = OverlayPDF.GetPageCount
                    If Not DocuFiSession.Authorized Then
                        For j As Integer = 1 To OverlayPDF.GetPageCount
                            OverlayPDF.SelectPage(j)

                            Dim font_res_name As String = OverlayPDF.AddTrueTypeFont("Arial", False, False, False)
                            OverlayPDF.SetTextMode(PdfTextMode.PdfTextModeFill)
                            OverlayPDF.SetTextSize(OverlayPDF.GetPageHeight / 20)
                            OverlayPDF.SetFillColor(255, 125, 125, 0) 'Using Cyan colour
                            OverlayPDF.SetOrigin(PdfOrigin.PdfOriginTopLeft)
                            OverlayPDF.SetMeasurementUnit(PdfMeasurementUnit.PdfMeasurementUnitPoint)
                            OverlayPDF.DrawText(font_res_name, OverlayPDF.GetPageWidth / 10 * 8, OverlayPDF.GetPageHeight / 5, "Trial Version")
                            OverlayPDF.DrawText(font_res_name, OverlayPDF.GetPageWidth / 10 * 8, OverlayPDF.GetPageHeight / 5 * 4, "Trial Version")
                            OverlayPDF.DrawText(font_res_name, OverlayPDF.GetPageWidth / 10, OverlayPDF.GetPageHeight / 5, "Trial Version")
                            OverlayPDF.DrawText(font_res_name, OverlayPDF.GetPageWidth / 10, OverlayPDF.GetPageHeight / 5 * 4, "Trial Version")

                        Next
                    End If
                    GdViewerOverlay.DisplayFromGdPicturePDF(OverlayPDF)
                    GdViewerOverlay.Visible = True
                    GdViewerBase.Width = GdViewerOverlay.Width
                    thumbOverlay.LoadFromGdViewer(GdViewerOverlay)
                Else
                    overlayID = myGD.CreateGdPictureImageFromFile(mvarOverlayFile)
                    GdViewerOverlay.DisplayFromFile(mvarOverlayFile)
                    OverlayExtents = myGD.GetWidth(overlayID).ToString & " by " & myGD.GetHeight(overlayID).ToString
                    thumbOverlay.LoadFromGdViewer(GdViewerOverlay)
                    mvarOverlayPageCount = myGD.TiffGetPageCount(overlayID)

                    If mvarOverlayPageCount = 0 Then mvarOverlayPageCount = 1
                    Dim itest As Integer = myGD.GetHeight(overlayID)
                    GdViewerOverlay.Visible = True

                    If DocuFiSession.Authorized = False Then ' stamp it
                        myGD.DrawText(overlayID, "Trial License", 50, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                        myGD.DrawText(overlayID, "Trial License", 50, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                        myGD.DrawText(overlayID, "Trial License", myGD.GetWidth(sourceID) - 1500, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                        myGD.DrawText(overlayID, "Trial License", myGD.GetWidth(sourceID) - 1500, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                        myGD.DrawText(overlayID, "Trial License", myGD.GetWidth(sourceID) / 2, myGD.GetHeight(sourceID) / 2, 75, FontStyle.Bold, Color.Black, "Arial", False)
                    End If
                End If

                Me.Cursor = Cursors.Default
            End If

            GdViewerBase.Width = Me.Width / 2 - 5
            If GdViewerOverlay.Visible Then GdViewerOverlay.Width = GdViewerBase.Width

            'GdViewerBase.ZoomMode = ViewerZoomMode.ZoomModeFitToViewer
            'GdViewerOverlay.ZoomMode = ViewerZoomMode.ZoomModeFitToViewer

            lblBase2.Text = "  Source:  " & IO.Path.GetFileName(mvarFileName) & "    Overlay:  " & IO.Path.GetFileName(mvarOverlayFile) & " "

            If GdViewerBase.PrintGetPaperHeight <> GdViewerOverlay.PrintGetPaperHeight Then

                PopupBox("File Open Warning", "The docuent widths do not match")
            End If



        Catch ex As Exception
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Public Sub SavePage(ByVal TargetFile As String, Optional ByVal PDFGreyscale As Boolean = False)
        On Error GoTo errorhandle
        Dim pgCount As Integer
        Dim tmpHandle As Long
        Dim bReturn As Boolean


        If PDFGreyscale Then
        Else
        End If

        If bReturn = False Then
            Exit Sub
        End If

        Exit Sub
errorhandle:

    End Sub


    Public Sub CloseFile()

    End Sub
    Public Sub AppendPage(ByVal TargetFile As String, Optional ByVal PDFGreyscale As Boolean = False)


        Exit Sub
errorhandle:

    End Sub

#End Region

#Region "ImageProcessing"


#End Region


    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        CurrentSelection.Location = New Point(e.X, e.Y)

    End Sub
    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        CurrentSelection = New Rectangle(0, 0, 0, 0)
        Me.Invalidate()
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            CurrentSelection.Size = New Point(e.X, e.Y) - CurrentSelection.Location
            Me.Invalidate()
        End If
    End Sub

    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim rect As Rectangle = CurrentSelection
        If rect.Width < 0 Then
            rect.X += rect.Width
            rect.Width = -rect.Width
        End If
        If rect.Height < 0 Then
            rect.Y += rect.Height
            rect.Height = -rect.Height
        End If
        e.Graphics.DrawRectangle(SystemPens.Highlight, rect)
    End Sub
    Private Sub ToolStripZoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'PictureBox1.Image = My.Resources.zoomleft100
        Me.ZoomWindow()

        mvarZoomMode = True
        ' PictureBox1.Tag = "zoom"
    End Sub

    Private Sub SketchHide(ByVal irlFile As String, ByVal markupID As String)
        Try
            'open the markup file
            Dim contents As String = File.ReadAllText(irlFile)
            Dim contentsout As String
            ' Format the data with new line so that on the web page it appears in the new line
            Dim xpos As Integer
            Dim objectpos As Integer
            Dim newxml As String
            Dim leftstring As String
            Dim rightstring As String

            'find the markupid for this object
            objectpos = InStr(contents, markupID)
            leftstring = Strings.Left(contents, objectpos)
            rightstring = Strings.Right(contents, contents.Length - objectpos)
            'now find the drawstyle and change it

            xpos = InStr(rightstring, "color")
            newxml = leftstring & Strings.Left(rightstring, xpos + 6) & "255|255|255"""
            xpos = InStr(rightstring, " ")
            contentsout = newxml & Strings.Right(rightstring, rightstring.Length - xpos + 1)
            File.WriteAllText(irlFile, contentsout)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub ToolZoomExtents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ZoomExtents()

    End Sub

    Private Sub RotateRightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Rotate90()
    End Sub

    Private Sub RotateLeftToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Rotate270()
    End Sub


    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click


        mvarActiveChanges = False
        mvarPublishFile = ""
        mvarOverlayFile = ""
        mvarFileName = ""

        If GdViewerBase.Visible Then GdViewerBase.CloseDocument()
        If GdViewerOverlay.Visible Then GdViewerOverlay.CloseDocument()
        If GdViewerMerged.Visible Then GdViewerMerged.CloseDocument()

        GdPictureDocumentUtilities.ReleaseAllGdPictureImages()
        Nudgeleft = 0
        NudgeTop = 0
        bubbleBar1.Visible = False
        mvarCompareCount = 0
        bubbleBarTab1.Buttons.Clear()

        mvarOverlayFile = ""
        mvarOverlayPage = 1
        mvarSourcePage = 1
        mvarFileName = ""
        'clear out ui considerations
        btnTagComparisons.Enabled = False
        btnCreateTags.Enabled = False
        btnShowDifferences.Enabled = False
        btnHideTags.Enabled = False
        btnTagComparisons.Enabled = False
    End Sub

    Private Sub ButtonItem72_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFitAll.Click
        GdViewerBase.ZoomMode = GdPicture14.ViewerZoomMode.ZoomModeFitToViewer
        If GdViewerOverlay.Visible Then GdViewerOverlay.ZoomMode = GdPicture14.ViewerZoomMode.ZoomModeFitToViewer
    End Sub

    Private Sub sldThreshold_MouseUp_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)

    End Sub

    Private Sub lblMRU1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMRU1.Click
        mvarOverlayFile = ""
        OpenFile(lblMRU1.Tag, True, DocuFiSession.IsPDFTrans)
        blnNewFile = False
    End Sub
    Private Sub lblMRU2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMRU2.Click
        mvarOverlayFile = ""
        OpenFile(lblMRU2.Tag, True, DocuFiSession.IsPDFTrans)
        blnNewFile = False
    End Sub

    Private Sub lblMRU3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMRU3.Click
        mvarOverlayFile = ""
        OpenFile(lblMRU3.Tag, True, DocuFiSession.IsPDFTrans)
        blnNewFile = False
    End Sub

    Private Sub lblMRU4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMRU4.Click
        mvarOverlayFile = ""
        OpenFile(lblMRU4.Tag, True, DocuFiSession.IsPDFTrans)
        blnNewFile = False
    End Sub

    Private Sub lblMRU5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMRU5.Click
        mvarOverlayFile = ""
        OpenFile(lblMRU5.Tag, True, DocuFiSession.IsPDFTrans)
        blnNewFile = False
    End Sub

    'Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    '    Try
    '        'Exit Sub
    '        'If lblMRU1.Tag <> "" Then SaveSetting("Compara", "Recent Files", "MRU1", lblMRU1.Tag)
    '        'If lblMRU2.Tag <> "" Then SaveSetting("Compara", "Recent Files", "MRU2", lblMRU2.Tag)
    '        'If lblMRU3.Tag <> "" Then SaveSetting("Compara", "Recent Files", "MRU3", lblMRU3.Tag)
    '        'If lblMRU4.Tag <> "" Then SaveSetting("Compara", "Recent Files", "MRU4", lblMRU4.Tag)
    '        'If lblMRU5.Tag <> "" Then SaveSetting("Compara", "Recent Files", "MRU5", lblMRU5.Tag)
    '        ''now save the preferences
    '        'SaveSetting("Compara", "Preferences", "TagFontSize", mvarTagFontSize.ToString)
    '        'SaveSetting("Compara", "Preferences", "MeasureColor", mvarDrawColor.ToArgb.ToString)
    '        'SaveSetting("Compara", "Preferences", "BaseColor", mvarBaseColor.ToArgb.ToString)
    '        'SaveSetting("Compara", "Preferences", "OverlayColor", mvarOverlayColor.ToArgb.ToString)

    '        'SaveSetting("Compara", "Preferences", "Units", mvarUnits.ToString)

    '        'SaveSetting("Compara", "Preferences", "Threshold", ThresholdValue.ToString)
    '        'SaveSetting("Compara", "Preferences", "Sensitivity", SensitivityValue.ToString)
    '        'SaveSetting("Compara", "Preferences", "ThresholdMode", DocuFiSession.ThreshMode.ToString)
    '        'SaveSetting("Compara", "Preferences", "ExtractTIF", DocuFiSession.ExtractTIF.ToString)
    '        'SaveSetting("Compara", "Preferences", "InvertTIF", DocuFiSession.InvertExtraction.ToString)


    '    Catch ex As Exception

    '    End Try
    'End Sub
    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        frmAbout.Show()
    End Sub


    Private Sub buttonOpenConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenConvert.Click, btnOpenPages.Click, btnOpenConvert2.Click
        Try

            If DocuFiSession.Authorized = False Then

                PopupBox("License Manager", "Your License has expired, a trial stamp will be added to your files")
                Application.DoEvents()
            End If

            RibbonPDFTrans.Select()

            With OpenFileDialog1

                If DocuFiSession.IsPDFTrans Then
                    '    .Filter = "All|*.pdf"
                    'Else
                    .Filter = "All|*.tif;*.tiff;*.pdf;*.jpg;*.jpeg"
                End If

                .FilterIndex = 1
                .Title = "Open File Dialog"
            End With
            If mvarActiveChanges Then
                MsgBox("There are active changes from a previous document, please close or save before proceeding")
                Exit Sub
            End If

            'null out the initial dialogue
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Title = "Open a TIF or PDF file"
            Dim res As Windows.Forms.DialogResult

            res = OpenFileDialog1.ShowDialog()

            If res = Windows.Forms.DialogResult.Cancel Then Exit Sub
            mvarOverlayFile = ""
            mvarFileName = ""
            GdViewerBase.Visible = True
            GdViewerOverlay.Visible = True
            If OpenFileDialog1.FileName <> "" Then

                'determine the file type for the conversion command name and store the source format
                If IO.Path.GetExtension(OpenFileDialog1.FileName).ToLower = ".pdf" Then
                    ' btnConverttoTIF.Text = "Convert to TIF"
                    btnSaveAllPages.Tag = "pdf"
                Else
                    ' btnConverttoTIF.Text = "Convert to PDF"
                    btnSaveAllPages.Tag = "tif"
                End If
                RibbonOpen.Refresh()
                mvarConvertMode = True
                OpenFile(OpenFileDialog1.FileName, True, True)


            End If
            btnSaveAllPages.Enabled = True
            btnSavePage.Enabled = True
            ' If Not mvarisGTX Then
            btnThreshold.Enabled = True
            OverlayPanel.TitleText = "Converted Pages"
            OverlayPanel.Refresh()
            ' btnThresholdArea.Enabled = True
            ' End If

        Catch
            MsgBox("Error opening file " & Err.Description)
        End Try
    End Sub
    Private Sub MenuOpenConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenCompare2.Click, MenuOpenCompare.Click, btnOpenFile.Click
        Try
            btnSaveAllPages.Enabled = False
            btnSavePage.Enabled = False
            btnThreshold.Enabled = False

            RibbonCompare.Select()
            GdPictureDocumentUtilities.ReleaseAllGdPictureImages()
            If DocuFiSession.Authorized = False Then

                PopupBox("ComPara License Manager", "Your License has expired, a demo stamp will is added")

            End If
            With OpenFileDialog1

                'If DocuFiSession.IsPDFTrans Then
                '    .Filter = "All|*.pdf"
                'Else
                .Filter = "All|*.tif;*.tiff;*.pdf"
                'End If

                .FilterIndex = 1
                .Title = "ComPara Open File Dialog"
            End With
            If mvarActiveChanges Then
                MsgBox("There are active changes from a previous document, please close or save before proceeding")
                Exit Sub
            End If

            Dim res As Windows.Forms.DialogResult

            res = OpenFileDialog1.ShowDialog()

            If res = Windows.Forms.DialogResult.Cancel Then Exit Sub
            mvarOverlayFile = ""
            mvarFileName = ""
            If OpenFileDialog1.FileName <> "" Then
                'determine the file type for the conversion command name and store the source format
                If IO.Path.GetExtension(OpenFileDialog1.FileName).ToLower = ".pdf" Then
                    'btnSaveAllPages.Text = "Convert to TIF"
                    btnSaveAllPages.Tag = "pdf"
                Else
                    ' btnSaveAllPages.Text = "Convert to PDF"
                    btnSaveAllPages.Tag = "tif"
                End If
                OpenFile(OpenFileDialog1.FileName, True, False)
                mvarConvertMode = False
            End If
            'make sure we have images
            thumbOverlay.Visible = True
            thumbBase.Visible = True
            If OverlayPDF.GetPageCount = 0 Then
                PopupBox("Warning", "The overlay file was not loaded properly.  Please reload")
            End If
            If BasePDF.GetPageCount = 0 Then
                PopupBox("Warning", "The Base file was not loaded properly.  Please reload")
            End If
        Catch
            MsgBox("Error opening file")
        End Try
    End Sub
    Private Sub btnClose_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        GdViewerBase.CloseDocument()
        GdViewerOverlay.CloseDocument()
        GdViewerMerged.CloseDocument()
        thumbBase.ClearAllItems()
        thumbOverlay.ClearAllItems()
        OverlayPanel.Visible = False
        BasePanel.Visible = False
        mvarCompareCount = 0
        SliderTransparency.Visible = False
        SliderSensitivity.Visible = False
        lblBase2.Text = ""
        btnSaveAllPages.Enabled = False
        btnSavePage.Enabled = False
        btnThreshold.Enabled = False
        '  btnThresholdArea.Enabled = False
    End Sub

    Private Sub btnExit_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, btnExit2.Click
        Try


            If blnPrompted = False Or True Then
                Dim myResult As MsgBoxResult

                myResult = MsgBox("Are you sure you want to exit", MsgBoxStyle.YesNo)
                If myResult = MsgBoxResult.Yes Then
                    blnPrompted = True

                    'GdViewerBase.Dispose()
                    'GdViewerMerged.Dispose()
                    'GdViewerOverlay.Dispose()
                    'GdPictureDocumentUtilities.ReleaseAllGdPictureImages()
                    'BasePDF.Dispose()
                    'OverlayPDF.Dispose()
                    'myGD.Dispose()

                    If System.Windows.Forms.Application.MessageLoop Then
                        'System.Windows.Forms.Application.Exit()
                        Application.Exit()
                    Else
                        System.Environment.Exit(1)
                    End If

                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnNudgeLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNudgeLeft.Click
        Nudgeleft -= 1
        nudgeOverlay2()
    End Sub
    Private Sub btnNudgeDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNudgeDown.Click
        NudgeTop += 1
        nudgeOverlay2()
    End Sub

    Private Sub btnNudgeUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNudgeUp.Click
        NudgeTop -= 1
        nudgeOverlay2()
    End Sub
    Private Sub btnNudgeRight_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNudgeRight.Click
        Nudgeleft += 1
        nudgeOverlay2()
    End Sub

    Private Sub btnProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProperties.Click, btnProperties2.Click

        mvarPDFProperties = "Base File " & SourceExtents & " with " & mvarSourcePageCount.ToString & " pages"
        If IO.File.Exists(mvarOverlayFile) Then
            mvarPDFProperties += vbCrLf & "Compare File  " & OverlayExtents & " with " & mvarOverlayPageCount.ToString & " pages"
        End If
        frmProperties.Show()

    End Sub

    Private Sub btnMeasureLine2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMeasureLine2.Click
        GdViewerBase.AddRulerAnnotInteractive(m_FillingColor, 1, 1, GdPicture14.Annotations.Annotation.UnitMode.Inch)
    End Sub
    'Private Sub ButtonbtnMeasureArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonbtnMeasureArea.Click
    '    GdViewerBase.AddRectangleAnnotInteractive(False, True, m_FillingColor, m_StrokingColor, 0.01, 0)
    'End Sub

    Private Sub GdViewer1_AnnotationAddedByUser(ByVal AnnotationIdx As Integer) Handles GdViewerBase.AnnotationAddedByUser, GdViewerMerged.AnnotationAddedByUser

        Dim myAnnotation As GdPicture14.Annotations.Annotation

        mvarLastAnnotation = AnnotationIdx
        mvarIsViewer1 = True
        mvarActiveChanges = True
        If GdViewerBase.Visible Then
            myAnnotation = GdViewerBase.GetAnnotationFromIdx(AnnotationIdx)
        ElseIf GdViewerMerged.Visible Then
            myAnnotation = GdViewerMerged.GetAnnotationFromIdx(AnnotationIdx)
        End If

        'if it is calibration, don't show the popup
        If DocuFiSession.MeasureMode = 4 Then
            If GdViewerBase.Visible Then
                GdViewerBase.DeleteAnnotation(AnnotationIdx)
            ElseIf GdViewerMerged.Visible Then
                GdViewerMerged.DeleteAnnotation(AnnotationIdx)
            End If

            DocuFiSession.MeasureMode = 0
            Exit Sub

        End If

        If DocuFiSession.MeasureMode > 0 Then
            'if it is the circe object, select it, otherwise delete the other measurement tools
            If myAnnotation.GetAnnotationType = Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeEllipse Then
                GdViewerBase.SelectAnnotation(AnnotationIdx)
            Else
                'GdViewer1.DeleteAnnotation(AnnotationIdx)
            End If
            'DocuFiSession.MeasureMode = 0
        End If

        If myAnnotation.GetAnnotationType = Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeEllipse Then

            myAnnotation.Height = CDec(txtCircleSize.ControlText) / mvarMeasureScale
            If chkRadius.Checked Then myAnnotation.Height = myAnnotation.Height * 2
            myAnnotation.Width = myAnnotation.Height

            myAnnotation.CanResize = False
            myAnnotation.CanRotate = False
        End If

        GdViewerBase.MouseMode = ViewerMouseMode.MouseModePan
        GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
        GdViewerMerged.MouseMode = GdViewerBase.MouseMode
    End Sub
    Private Sub GdViewer2_AnnotationAddedByUser(ByVal AnnotationIdx As Integer) Handles GdViewerOverlay.AnnotationAddedByUser
        Dim myAnnotation As GdPicture14.Annotations.Annotation
        mvarLastAnnotation = AnnotationIdx
        mvarIsViewer1 = False
        mvarActiveChanges = True
        myAnnotation = GdViewerOverlay.GetAnnotationFromIdx(AnnotationIdx)

        'if it is calibration, don't show the popup
        If DocuFiSession.MeasureMode = 4 Then
            GdViewerOverlay.DeleteAnnotation(AnnotationIdx)
            DocuFiSession.MeasureMode = 0
            Exit Sub

        End If

        If DocuFiSession.MeasureMode > 0 Then
            'if it is the circe object, select it, otherwise delete the other measurement tools
            If myAnnotation.GetAnnotationType = GdPicture14.Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeEllipse Then
                GdViewerOverlay.SelectAnnotation(AnnotationIdx)
            Else
                'GdViewer1.DeleteAnnotation(AnnotationIdx)
            End If
            DocuFiSession.MeasureMode = 0
        End If

        If myAnnotation.GetAnnotationType = GdPicture14.Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeEllipse Then

            myAnnotation.Height = CDec(txtCircleSize.ControlText) / mvarMeasureScale
            If chkRadius.Checked Then myAnnotation.Height = myAnnotation.Height * 2
            myAnnotation.Width = myAnnotation.Height

            myAnnotation.CanResize = False
            myAnnotation.CanRotate = False
        End If


    End Sub
    Private Sub GdViewer1_AnnotationMoved(ByVal AnnotationIdx As Integer) Handles GdViewerBase.AnnotationMoved
        Dim len As Double
        Dim myAnnotation As GdPicture14.Annotations.Annotation

        myAnnotation = GdViewerBase.GetAnnotationFromIdx(AnnotationIdx)
        If myAnnotation.GetAnnotationType = GdPicture14.Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeLine Then

            len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
            len = Math.Round(len * mvarMeasureScale, 2)
            PopupBox("Line Length", len.ToString & GetUnits())

        End If
    End Sub
    Private Sub GdViewer2_AnnotationMoved(ByVal AnnotationIdx As Integer) Handles GdViewerOverlay.AnnotationMoved
        Dim len As Double
        Dim myAnnotation As GdPicture14.Annotations.Annotation

        myAnnotation = GdViewerOverlay.GetAnnotationFromIdx(AnnotationIdx)
        If myAnnotation.GetAnnotationType = GdPicture14.Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeLine Then

            len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
            len = Math.Round(len * mvarMeasureScale, 2)
            PopupBox("Line Length", len.ToString & GetUnits())
        End If
    End Sub
    Private Sub GdViewer1_AnnotationResized(ByVal AnnotationIdx As Integer) Handles GdViewerBase.AnnotationResized, GdViewerMerged.AnnotationResized
        Dim len As Double
        Dim myAnnotation As GdPicture14.Annotations.Annotation

        If AnnotationIdx < 0 Then Exit Sub
        If GdViewerBase.Visible Then
            myAnnotation = GdViewerBase.GetAnnotationFromIdx(AnnotationIdx)
        Else
            myAnnotation = GdViewerMerged.GetAnnotationFromIdx(AnnotationIdx)
        End If

        'rt = myAnnotation.Left +
        'lt = myAnnotation.Left
        'bt = myAnnotation.Top + myAnnotation.Height
        'tp = myAnnotation.Top
        If myAnnotation.GetAnnotationType = Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeLine Then

            len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
            len = Math.Round(len * mvarMeasureScale, 2)
            PopupBox("Line Length", len.ToString & GetUnits())
        ElseIf myAnnotation.GetAnnotationType = Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeRectangle Then 'rectangle
            len = (myAnnotation.Width * mvarMeasureScale) * (myAnnotation.Height * mvarMeasureScale)
            len = Math.Round(len, 2)
            PopupBox("Area Size", len.ToString & " Square " & GetUnits())
        ElseIf myAnnotation.GetAnnotationType = Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeEllipse Then 'circle
            len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
            len = Math.Round(len * mvarMeasureScale, 2)

        ElseIf DocuFiSession.MeasureMode = 6 Then 'circle
            len = (rt - lt)
            len = Math.Round(len * mvarMeasureScale, 2)
            PopupBox("Circle Size", len.ToString)
        End If
    End Sub
    Private Sub GdViewer2_AnnotationResized(ByVal AnnotationIdx As Integer) Handles GdViewerOverlay.AnnotationResized
        Dim len As Double
        Dim myAnnotation As GdPicture14.Annotations.Annotation

        myAnnotation = GdViewerOverlay.GetAnnotationFromIdx(AnnotationIdx)
        'rt = myAnnotation.Left +
        'lt = myAnnotation.Left
        'bt = myAnnotation.Top + myAnnotation.Height
        'tp = myAnnotation.Top
        If myAnnotation.GetAnnotationType = Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeLine Then

            len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
            len = Math.Round(len * mvarMeasureScale, 2)
            PopupBox("Line Length", len.ToString & GetUnits())
        ElseIf myAnnotation.GetAnnotationType = Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeRectangle Then 'rectangle
            len = (myAnnotation.Width * mvarMeasureScale) * (myAnnotation.Height * mvarMeasureScale)
            len = Math.Round(len, 2)
            PopupBox("Area Size", len.ToString & " Square " & GetUnits())
        ElseIf myAnnotation.GetAnnotationType = Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeEllipse Then 'circle
            len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
            len = Math.Round(len * mvarMeasureScale, 2)

        ElseIf DocuFiSession.MeasureMode = 6 Then 'circle
            len = (rt - lt)
            len = Math.Round(len * mvarMeasureScale, 2)
            PopupBox("Circle Size", len.ToString)
        End If
    End Sub
    Private Sub GdViewer1_AnnotationSelected(ByVal AnnotationIdx As Integer) Handles GdViewerBase.AnnotationSelected, GdViewerMerged.AnnotationSelected
        mvarLastAnnotation = AnnotationIdx
        mvarIsViewer1 = True
    End Sub
    Private Sub GdViewer2_AnnotationSelected(ByVal AnnotationIdx As Integer) Handles GdViewerOverlay.AnnotationSelected
        mvarLastAnnotation = AnnotationIdx
        mvarIsViewer1 = False
    End Sub
    Private Sub GdViewer1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GdViewerBase.MouseDown
        'store the left mouse click
        lt = e.X
        tp = e.Y
        mvarIsViewer1 = True
    End Sub
    Private Sub GdVieweroverlay_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GdViewerOverlay.MouseDown
        'store the left mouse click
        lt = e.X
        tp = e.Y
        mvarIsViewer1 = False


    End Sub
    Private Sub GdViewermerged_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GdViewerMerged.MouseDown
        'store the left mouse click
        GdViewerMerged.GetMouseLeftInDocument()
        Dim PixelColor As Color

        Dim pixelint As Integer

        lt = e.X
        tp = e.Y
        mvarIsViewer1 = False


        If e.Button = Windows.Forms.MouseButtons.Right Then

            Try


                If mergedID <> 0 Then
                    PixelColor = myGD.PixelGetColor(mergedID, GdViewerMerged.GetMouseLeftInDocument, GdViewerMerged.GetMouseTopInDocument)
                    PopupBox("Color", PixelColor.ToString)
                    ' myGD.SwapColor(mergedID, PixelColor, Color.Gray)
                    'GdViewerMerged.DisplayFromGdPictureImage(mergedID)
                    ' GdViewerMerged.Refresh()
                End If
            Catch ex As Exception

            End Try
        End If

    End Sub

    'syncronize the pan and zoom events within the two windows.
    Private Sub GdViewer1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GdViewerBase.MouseUp, GdViewerMerged.MouseUp

        Try

            If DocuFiSession.MeasureMode = 0 And GdViewerBase.MouseMode = GdPicture14.ViewerMouseMode.MouseModeAreaZooming Then
                'GdViewerBase.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
                'If GdViewerOverlay.Visible Then
                '    GdViewerOverlay.ZoomArea(myLeft, myTop, myWidth, myHeight)
                'End If

                If GdViewerOverlay.Visible And DocuFiSession.IsPDFTrans = False Then
                    GdViewerBase.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
                    'GdViewerOverlay.ZoomArea(myLeft, myTop, myWidth, myHeight)
                    GdViewerOverlay.Zoom = GdViewerBase.Zoom
                    GdViewerOverlay.SetRectCoordinatesOnDocument(myLeft, myTop, myWidth, myHeight)
                    GdViewerOverlay.ZoomRect()

                End If

            End If


            If GdViewerBase.MouseMode = GdPicture14.ViewerMouseMode.MouseModeAreaZooming Then
                GdViewerBase.MouseMode = GdPicture14.ViewerMouseMode.MouseModePan
                GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
                GdViewerMerged.MouseMode = GdViewerBase.MouseMode
                GdViewerBase.Cursor = Cursors.Hand
                GdViewerOverlay.Cursor = GdViewerBase.Cursor
                GdViewerMerged.Cursor = GdViewerBase.Cursor
            End If

            rt = e.X
            bt = e.Y

            Dim len As Double
            Dim myAnnotation As GdPicture14.Annotations.Annotation

            If GdViewerBase.Visible Then
                If mvarIsViewer1 Then
                    myAnnotation = GdViewerBase.GetAnnotationFromIdx(mvarLastAnnotation)
                Else
                    myAnnotation = GdViewerOverlay.GetAnnotationFromIdx(mvarLastAnnotation)
                End If

            ElseIf GdViewerMerged.Visible Then
                myAnnotation = GdViewerMerged.GetAnnotationFromIdx(mvarLastAnnotation)
            End If

            If DocuFiSession.MeasureMode = 1 Then 'line measurement
                If myAnnotation.GetAnnotationType = GdPicture14.Annotations.Annotation.GdPictureAnnotationType.AnnotationTypeLine Then
                    len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
                    len = Math.Round(len * mvarMeasureScale, 2)
                    'PopupBox("Line Length", len.ToString & GetUnits())
                End If

            ElseIf DocuFiSession.MeasureMode = 2 Then 'rectangle
                len = ((myAnnotation.Width * mvarMeasureScale) * (myAnnotation.Height * mvarMeasureScale))
                len = Math.Round(len, 2)
                'PopupBox("Area Size", len.ToString & GetUnits())
            ElseIf DocuFiSession.MeasureMode = 6 Then 'circle
                'If sourceID = 0 Then sourceID = myGD.CreateGdPictureImageFromFile(mvarFileName)
                'myGD.DrawCircle(sourceID, lt, tp, 22, mvarDrawColor, 2, False)
                'DocuFiSession.MeasureMode = 0
                'GdViewer1.DisplayFromGdPictureImage(sourceID)
                len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
                len = Math.Round(len / mvarMeasureScale, 2)

                'PopupBox("Circle Size", len.ToString)

            ElseIf DocuFiSession.MeasureMode = 4 Then 'calibration
                len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
                len = Math.Round(len, 2)
                Dim newlength As String
                newlength = InputBox("Measured as " & len.ToString & " please enter the actual length")
                While IsNumeric(newlength) = False And (newlength.Length) <> 0
                    PopupBox("Measurement warning", "Please enter a valid calibration value (number).")
                    newlength = InputBox("Measured as " & len.ToString & " please enter the actual length")
                End While
                If newlength = "" Then Exit Sub
                Dim floatlength As Double

                floatlength = System.Convert.ToDecimal(newlength)

                mvarMeasureScale = floatlength / len

            ElseIf DocuFiSession.MeasureMode = 5 Then 'comparatag Mode

                If GdViewerBase.Visible Then
                    GdViewerBase.GetRectCoordinatesOnDocument(myLeft, myTop, myWidth, myHeight)
                ElseIf GdViewerMerged.Visible Then
                    GdViewerMerged.GetRectCoordinatesOnDocument(myLeft, myTop, myWidth, myHeight)
                End If

                'if the width and height are too smal, do not store it
                If myWidth < 5 Or myHeight < 5 Then Exit Sub

                Dim markLabel As String
                markLabel = InputBox("ComparaTag", "Please enter a name for this comparison tag")
                If (markLabel.Length) <> 0 Then
                    PopupBox("Compara Tag ", "Comparison Region Tagged as " & markLabel.ToString, 100)
                    CompareTags(mvarCompareCount).Name = markLabel


                    CompareTags(mvarCompareCount).Left = myLeft
                    CompareTags(mvarCompareCount).top = myTop
                    CompareTags(mvarCompareCount).width = myWidth
                    CompareTags(mvarCompareCount).height = myHeight
                    CompareTags(mvarCompareCount).sourcePage = mvarSourcePage
                    CompareTags(mvarCompareCount).OverlayPage = mvarOverlayPage
                    If GdViewerBase.Visible Then
                        GdViewerBase.CopyRegionToClipboard(myLeft, myTop, myWidth, myHeight)
                    ElseIf GdViewerMerged.Visible Then
                        GdViewerMerged.CopyRegionToClipboard(myLeft, myTop, myWidth, myHeight)
                    End If

                    CompareTags(mvarCompareCount).Mergedimage = Clipboard.GetImage
                    myGD.DrawRectangle(mergedID, myLeft, myTop, myWidth, myHeight, 5, Color.Red, False)
                    GdViewerMerged.DisplayFromGdPictureImage(mergedID)

                    myGD.CopyRegionToClipboard(sourceID, myLeft, myTop, myWidth, myHeight)

                    GdViewerBase.CopyRegionToClipboard(myLeft, myTop, myWidth, myHeight)
                    CompareTags(mvarCompareCount).sourceImage = Clipboard.GetImage

                    myGD.CopyRegionToClipboard(overlayID, myLeft, myTop, myWidth, myHeight)
                    CompareTags(mvarCompareCount).DestImage = Clipboard.GetImage

                    'turned these off to stay in tagging mode
                    'DocuFiSession.MeasureMode = 0
                    'GdViewerBase.MouseMode = ViewerMouseMode.MouseModePan
                    'GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
                    'GdViewerMerged.MouseMode = GdViewerBase.MouseMode
                    GdViewerBase.Cursor = Cursors.Hand
                    GdViewerOverlay.Cursor = GdViewerBase.Cursor
                    GdViewerMerged.Cursor = GdViewerBase.Cursor

                    btnCreateTags.Enabled = True
                    Dim bmp As System.Drawing.Bitmap
                    Dim bButton As DevComponents.DotNetBar.BubbleButton
                    Dim myTag As Integer
                    Dim myfont As System.Drawing.Font
                    myfont = New System.Drawing.Font("Arial", mvarTagFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

                    bubbleBar1.TooltipFont = myfont

                    myTag = myGD.CreateGdPictureImageFromClipboard

                    myGD.DrawRectangle(myTag, 2, 2, myGD.GetWidth(myTag) - 3, myGD.GetHeight(myTag) - 4, 2, Color.Black, False)
                    myGD.CopyToClipboard(myTag)
                    bmp = Clipboard.GetImage
                    bButton = New DevComponents.DotNetBar.BubbleButton

                    bButton.Name = CompareTags(mvarCompareCount).Name
                    bButton.TooltipText = CompareTags(mvarCompareCount).Name
                    bButton.Tag = "Tag " & mvarCompareCount.ToString
                    bButton.ImageLarge = bmp
                    bButton.Image = bmp
                    bubbleBar1.Visible = True
                    bubbleBarTab1.Buttons.Add(bButton)
                    AddHandler bButton.Click, AddressOf bButton_Click
                    bButton.Dispose()
                    bmp = Nothing
                    myGD.ReleaseGdPictureImage(myTag)
                    bubbleBar1.Refresh()
                    mvarCompareCount += 1 'increment for the next one
                    btnHideTags.Enabled = True
                    btnShowDifferences.Enabled = True
                Else
                    ' DocuFiSession.MeasureMode = 0
                    Exit Sub
                End If
            ElseIf DocuFiSession.MeasureMode = 7 Then 'Align overlays
                Dim outLeft1 As Integer
                Dim outTop1 As Integer
                Dim outLeft2 As Integer
                Dim outTop2 As Integer

                GdViewerMerged.CoordViewerToDocument(lt, tp, outLeft1, outTop1)
                GdViewerMerged.CoordViewerToDocument(rt, bt, outLeft2, outTop2)
                Nudgeleft = Nudgeleft + (outLeft2 - outLeft1) ' / GdViewerMerged.Zoom
                NudgeTop = NudgeTop + (outTop2 - outTop1) '/ GdViewerMerged.Zoom

                DocuFiSession.MeasureMode = 0
                GdViewerBase.MouseMode = GdPicture14.ViewerMouseMode.MouseModePan
                GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
                GdViewerMerged.MouseMode = GdViewerBase.MouseMode
                GdViewerBase.Cursor = Cursors.Hand
                GdViewerOverlay.Cursor = GdViewerBase.Cursor
                GdViewerMerged.Cursor = GdViewerBase.Cursor
                GdViewerBase.DeleteAnnotation(mvarLastAnnotation)
                GdViewerMerged.DeleteAnnotation(mvarLastAnnotation)
                'now redisplay the change
                nudgeOverlay2()

            ElseIf DocuFiSession.MeasureMode = 10 Then 'compare viewport

                If GdViewerBase.Visible Then
                    GdViewerBase.GetRectCoordinatesOnDocument(myLeft, myTop, myWidth, myHeight)
                End If
                GdViewerOverlay.SetRectCoordinatesOnDocument(myLeft, myTop, myWidth, myHeight)
            End If

            'now clear out the second viewer annotation mode
            If mvarIsViewer1 And GdViewerOverlay.Visible Then GdViewerOverlay.CancelLastAnnotInteractiveAdd()
            If Not GdViewerMerged.Visible Then
                GdViewerMerged.CancelLastAnnotInteractiveAdd()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub GdViewer2_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GdViewerOverlay.MouseUp

        Try


            If DocuFiSession.MeasureMode = 0 And GdViewerBase.MouseMode = GdPicture14.ViewerMouseMode.MouseModeAreaZooming Then

                If GdViewerBase.Visible Then
                    GdViewerOverlay.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
                    'GdViewerOverlay.ZoomArea(myLeft, myTop, myWidth, myHeight)
                    GdViewerBase.Zoom = GdViewerOverlay.Zoom
                    GdViewerBase.SetRectCoordinatesOnDocument(myLeft, myTop, myWidth, myHeight)
                    GdViewerBase.ZoomRect()

                End If


            End If


            If GdViewerBase.MouseMode = GdPicture14.ViewerMouseMode.MouseModeAreaZooming Then
                GdViewerBase.MouseMode = GdPicture14.ViewerMouseMode.MouseModePan
                GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
                GdViewerBase.Cursor = Cursors.Hand
                GdViewerOverlay.Cursor = GdViewerBase.Cursor
            End If

            rt = e.X
            bt = e.Y

            Dim len As Double
            Dim myAnnotation As GdPicture14.Annotations.Annotation

            myAnnotation = GdViewerOverlay.GetAnnotationFromIdx(mvarLastAnnotation)
            If DocuFiSession.MeasureMode = 1 Then 'line measurement
                len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
                len = Math.Round(len * mvarMeasureScale, 2)
                PopupBox("Line Length", len.ToString & GetUnits())
            ElseIf DocuFiSession.MeasureMode = 2 Then 'rectangle
                len = ((myAnnotation.Width * mvarMeasureScale) * (myAnnotation.Height * mvarMeasureScale))
                len = Math.Round(len, 2)
                PopupBox("Area Size", len.ToString & GetUnits())
            ElseIf DocuFiSession.MeasureMode = 6 Then 'circle
                'If sourceID = 0 Then sourceID = myGD.CreateGdPictureImageFromFile(mvarFileName)
                'myGD.DrawCircle(sourceID, lt, tp, 22, mvarDrawColor, 2, False)
                'DocuFiSession.MeasureMode = 0
                'GdViewer1.DisplayFromGdPictureImage(sourceID)
                len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
                len = Math.Round(len / mvarMeasureScale, 2)

                'PopupBox("Circle Size", len.ToString)
            ElseIf DocuFiSession.MeasureMode = 4 Then 'calibration
                len = Math.Sqrt(((myAnnotation.Width) * (myAnnotation.Width)) + ((myAnnotation.Height) * (myAnnotation.Height)))
                len = Math.Round(len, 2)
                Dim newlength As String
                newlength = InputBox("Measured as " & len.ToString & " please enter the actual length")
                While IsNumeric(newlength) = False And (newlength.Length) <> 0
                    PopupBox("Measurement warning", "Please enter a valid calibration value (number).")
                    newlength = InputBox("Measured as " & len.ToString & " please enter the actual length")
                End While
                If newlength = "" Then Exit Sub
                Dim floatlength As Double

                floatlength = System.Convert.ToDecimal(newlength)
                mvarMeasureScale = floatlength / len

            ElseIf DocuFiSession.MeasureMode = 5 Then 'Markup Mode

                Dim markLabel As String
                markLabel = InputBox("ComparaTag", "Please enter a name for this comparison tag")
                If (markLabel.Length) <> 0 Then
                    PopupBox("Compara Tag ", "Comparison Region Tagged as " & markLabel.ToString, 100)
                    CompareTags(mvarCompareCount).Name = markLabel
                    GdViewerOverlay.GetRectCoordinatesOnDocument(myLeft, myTop, myWidth, myHeight)
                    CompareTags(mvarCompareCount).Left = myLeft
                    CompareTags(mvarCompareCount).top = myTop
                    CompareTags(mvarCompareCount).width = myWidth
                    CompareTags(mvarCompareCount).height = myHeight
                    CompareTags(mvarCompareCount).sourcePage = mvarSourcePage
                    CompareTags(mvarCompareCount).OverlayPage = mvarOverlayPage
                    GdViewerOverlay.CopyRegionToClipboard(myLeft, myTop, myWidth, myHeight)
                    CompareTags(mvarCompareCount).Mergedimage = Clipboard.GetImage


                    DocuFiSession.MeasureMode = 0
                    GdViewerBase.MouseMode = ViewerMouseMode.MouseModePan
                    GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
                    GdViewerBase.Cursor = Cursors.Hand
                    GdViewerOverlay.Cursor = GdViewerBase.Cursor
                    DocuFiSession.MeasureMode = 0
                    btnCreateTags.Enabled = True
                    Dim bmp As System.Drawing.Bitmap
                    Dim bButton As DevComponents.DotNetBar.BubbleButton
                    Dim myTag As Integer
                    Dim myfont As System.Drawing.Font
                    myfont = New System.Drawing.Font("Arial", mvarTagFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

                    bubbleBar1.TooltipFont = myfont

                    bmp = CompareTags(mvarCompareCount).Mergedimage
                    bButton = New DevComponents.DotNetBar.BubbleButton

                    bButton.Name = CompareTags(mvarCompareCount).Name
                    bButton.TooltipText = CompareTags(mvarCompareCount).Name
                    bButton.Tag = "Tag " & mvarCompareCount.ToString
                    bButton.ImageLarge = bmp
                    bButton.Image = bmp
                    bubbleBar1.Visible = True
                    bubbleBarTab1.Buttons.Add(bButton)
                    AddHandler bButton.Click, AddressOf bButton_Click
                    bButton.Dispose()
                    bmp = Nothing
                    myGD.ReleaseGdPictureImage(myTag)
                    bubbleBar1.Refresh()
                    mvarCompareCount += 1 'increment for the next one
                    btnHideTags.Enabled = True
                    btnShowDifferences.Enabled = True
                Else
                    Exit Sub
                End If
            ElseIf DocuFiSession.MeasureMode = 7 Then 'Align overlays

                Nudgeleft = rt - lt
                NudgeTop = bt - tp

                DocuFiSession.MeasureMode = 0
                GdViewerBase.MouseMode = ViewerMouseMode.MouseModePan
                GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
                GdViewerBase.Cursor = Cursors.Hand
                GdViewerOverlay.Cursor = GdViewerBase.Cursor
                GdViewerOverlay.DeleteAnnotation(mvarLastAnnotation)
                'now redisplay the change
                NudgeOverlay()
            End If

            'now clear out the first viewer annotation mode
            GdViewerBase.CancelLastAnnotInteractiveAdd()
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetUnits() As String
        If mvarUnits = 1 Then
            Return " Inches"
        ElseIf mvarUnits = 2 Then
            Return " Feet"
        ElseIf mvarUnits = 3 Then
            Return " Meters"
        ElseIf mvarUnits = 4 Then
            Return (" Centimeters")
        Else
            Return " units"
        End If
    End Function
    Private Sub RegenOverlay()
        Try
            Dim destwidth As Integer
            Dim destHeight As Integer
            Me.Cursor = Cursors.WaitCursor

            GdViewerMerged.CloseDocument()

            If DocuFiSession.IsMultipage = False Then
                PopupBox("License Restriction    .", "Comparing multipage files is not allowed with this license, comparing page 1")
                Application.DoEvents()
                System.Threading.Thread.Sleep(3000)
                mvarSourcePage = 1
                mvarOverlayPage = 1

            End If
            ProgressBarX1.Visible = True
            ProgressBarX1.Maximum = 8
            ProgressBarX1.Value = 0

            If sourceID > 0 And False Then ' we have an existing image
                GdViewerMerged.DisplayFromGdPictureImage(mergedID)
                ' Exit Sub
            Else


                If mvarCurrentFile.ToLower.Contains(".pdf") Then
                    BasePDF.SelectPage(mvarSourcePage)
                    OverlayPDF.SelectPage(mvarOverlayPage)
                    sourceID = BasePDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                Else
                    sourceID = myGD.CreateGdPictureImageFromFile(mvarFileName)
                    myGD.TiffSelectPage(sourceID, mvarSourcePage)
                End If

                ProgressBarX1.Value = 1
                If DocuFiSession.ThreshMode = 1 Then
                    myGD.ConvertTo1BppSauvola(sourceID, SensitivityValue / 100.0F, ThresholdValue, 3)

                Else
                    myGD.ConvertTo1Bpp(sourceID)
                End If


                myGD.ConvertTo16BppRGB555(sourceID)
                myGD.SwapColor(sourceID, Color.Black, Color.Red)

                myGD.ReleaseGdPictureImage(overlayID)
                ProgressBarX1.Value = 2
                If mvarCurrentFile.ToLower.Contains(".pdf") Then
                    overlayID = OverlayPDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                Else
                    overlayID = myGD.CreateGdPictureImageFromFile(mvarOverlayFile)
                    myGD.TiffSelectPage(overlayID, mvarOverlayPage)
                End If

                ProgressBarX1.Value = 3
                If DocuFiSession.ThreshMode = 1 Then
                    myGD.ConvertTo1BppSauvola(overlayID, SensitivityValue / 100.0F, ThresholdValue, 3)
                Else
                    myGD.ConvertTo1Bpp(overlayID)
                End If

                myGD.ConvertTo16BppRGB555(overlayID)

                myGD.SwapColor(overlayID, Color.Black, Color.Green) 'added 6/4

                ProgressBarX1.Value = 4
                destwidth = myGD.GetWidth(overlayID)
                destHeight = myGD.GetHeight(overlayID)
                ProgressBarX1.Value = 5
                mergedID = myGD.CreateClonedGdPictureImage(overlayID)
                ProgressBarX1.Value = 6

                myGD.DrawGdPictureImageTransparency(sourceID, mergedID, 128, 0, 0, destwidth, destHeight, Drawing2D.InterpolationMode.NearestNeighbor)


                Dim PixelColor As Color

                If mergedID <> 0 Then
                    myGD.SwapColor(mergedID, myGD.ARGB(255, 188, 92, 0), Color.LightGray, 95)
                    myGD.SwapColor(mergedID, myGD.ARGB(255, 188, 205, 188), mvarOverlayColor, 95)
                    myGD.SwapColor(mergedID, myGD.ARGB(255, 254, 186, 186), mvarBaseColor, 95)
                End If

                GoTo done

                myGD.ConvertTo1Bpp(overlayID)
                myGD.FxBitonalRemoveIsolatedDots4(overlayID)
                ProgressBarX1.Value = 4
                myGD.ReleaseGdPictureImage(commonID)
                commonID = myGD.CreateClonedGdPictureImage(overlayID)   ' Operators.OperatorPlus, InterpolationMode.HighQualityBicubic)


                Dim myMessage As String
                myMessage = "Source (" & mvarSourcePage & "):" & myGD.GetWidth(sourceID).ToString & " by " & myGD.GetHeight(sourceID).ToString
                myMessage = myMessage & vbCrLf & "Overlay (" & mvarOverlayPage & "):" & myGD.GetWidth(overlayID).ToString & " by " & myGD.GetHeight(overlayID).ToString

                If myGD.GetWidth(overlayID) <> myGD.GetWidth(sourceID) And myGD.GetHeight(overlayID) <> myGD.GetHeight(sourceID) Then
                    PopupBox("Overlay Comparison", "These images have mismatching extents")
                    Application.DoEvents()
                    WriteLog("Mismatching extents: " & myMessage)
                Else
                    WriteLog(myMessage)
                End If

                'now address nudging temp
                If Nudgeleft > 0 Then 'the overlay is 
                    myGD.CropLeft(commonID, Nudgeleft)
                    destwidth = myGD.GetWidth(sourceID)
                ElseIf Nudgeleft < 0 Then
                    'myGD.CropRight(commonID, Math.Abs(Nudgeleft))
                    destwidth = myGD.GetWidth(overlayID) + Nudgeleft
                Else
                    destwidth = myGD.GetWidth(overlayID)
                End If

                If NudgeTop > 0 Then 'the overlay is 
                    myGD.CropTop(overlayID, NudgeTop)
                    destHeight = myGD.GetHeight(overlayID) - NudgeTop
                ElseIf NudgeTop < 0 Then
                    myGD.CropTop(overlayID, Math.Abs(NudgeTop))
                    destHeight = myGD.GetHeight(overlayID)
                Else
                    destHeight = myGD.GetHeight(overlayID)
                End If

                myGD.DrawGdPictureImageTransparency(sourceID, commonID, 125, 0, 0, destwidth, destHeight, Drawing2D.InterpolationMode.High)
                myGD.ConvertTo1BppAT(commonID, 50)
                ProgressBarX1.Value = 5

            End If

            'first get the deletions to red
            myGD.ReleaseGdPictureImage(mergedID)
            mergedID = myGD.CreateClonedGdPictureImage(sourceID)


            myGD.DrawGdPictureImageOP(commonID, mergedID, Nudgeleft, NudgeTop, myGD.GetWidth(mergedID), myGD.GetHeight(mergedID), Operators.OperatorLess, Drawing2D.InterpolationMode.High)

            ProgressBarX1.Value = 6
            myGD.FxNegative(mergedID)
            myGD.SwapColor(mergedID, Color.Black, mvarBaseColor)
            ' myGD.CopyToClipboard(mergedID)
            Dim addedID As Integer = myGD.CreateClonedGdPictureImage(overlayID)

            'now get the additions

            ' myGD.ConvertTo24BppRGB(addedID)

            myGD.DrawGdPictureImageOP(commonID, addedID, 0, 0, myGD.GetWidth(addedID), myGD.GetHeight(addedID), Operators.OperatorLess, Drawing2D.InterpolationMode.HighQualityBicubic)
            ProgressBarX1.Value = 7
            myGD.FxNegative(addedID)
            myGD.SwapColor(addedID, Color.Black, mvarOverlayColor)
            myGD.DrawGdPictureImageOP(commonID, addedID, 0, 0, myGD.GetWidth(addedID), myGD.GetHeight(addedID), Operators.OperatorAnd, Drawing2D.InterpolationMode.HighQualityBicubic)
            myGD.DrawGdPictureImageOP(addedID, mergedID, 0, 0, myGD.GetWidth(mergedID), myGD.GetHeight(mergedID), Operators.OperatorAnd, Drawing2D.InterpolationMode.HighQualityBicubic)

done:
            ProgressBarX1.Value = 7
            ProgressBarX1.Visible = False
            GdViewerMerged.DisplayFromGdPictureImage(mergedID)
            myGD.ReleaseGdPictureImage(addedID)
            ProgressBarX1.Value = 8
            grpSliderTools.Visible = False
            barStatus.Refresh()
            GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
            myGD.ReleaseGdPictureImage(mergedID)
            mergedID = myGD.CreateClonedGdPictureImage(overlayID)
            ProgressBarX1.Value = 6

            'get the common area as black

            myGD.DrawGdPictureImageOP(sourceID, mergedID, Nudgeleft, NudgeTop, myGD.GetWidth(overlayID), myGD.GetHeight(overlayID), Operators.OperatorMultiply, Drawing2D.InterpolationMode.HighQualityBicubic)

            GdViewerMerged.DisplayFromGdPictureImage(mergedID)

            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub OverlayDust()
        Try
            Dim destwidth As Integer
            Dim destHeight As Integer
            Me.Cursor = Cursors.WaitCursor

            GdViewerMerged.CloseDocument()

            If DocuFiSession.IsMultipage = False Then
                PopupBox("License Restriction    .", "Comparing multipage files is not allowed with this license, comparing page 1")
                Application.DoEvents()
                System.Threading.Thread.Sleep(3000)
                mvarSourcePage = 1
                mvarOverlayPage = 1
            End If
            ProgressBarX1.Visible = True
            ProgressBarX1.Maximum = 8
            ProgressBarX1.Value = 0


            GdPictureDocumentUtilities.ReleaseAllGdPictureImages()
            ProgressBarX1.Value = 1

            If mvarSourceDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF Then
                BasePDF.SelectPage(mvarSourcePage)
                OverlayPDF.SelectPage(mvarOverlayPage)
                sourceID = BasePDF.ExtractPageImage(1)
            Else
                sourceID = myGD.CreateGdPictureImageFromFile(mvarFileName)
                myGD.TiffSelectPage(sourceID, mvarSourcePage)
            End If

            'first convert the original to greyscale

            myGD.ReleaseGdPictureImage(overlayID)
            ProgressBarX1.Value = 2
            If mvarSourceDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF Then
                overlayID = OverlayPDF.ExtractPageImage(1)
            Else
                overlayID = myGD.CreateGdPictureImageFromFile(mvarOverlayFile)
                myGD.TiffSelectPage(overlayID, mvarOverlayPage)
            End If

            ProgressBarX1.Value = 3
            myGD.ConvertTo8BppGrayScale(sourceID)
            If DocuFiSession.ThreshMode = 1 Then
                'myGD.ConvertTo1BppSauvola(overlayID, SensitivityValue / 100.0F, ThresholdValue, 3)
            Else
                ' myGD.ConvertTo1Bpp(overlayID)
            End If

            ' myGD.ConvertTo16BppRGB555(overlayID)

            'myGD.SwapColor(overlayID, Color.Black, Color.Red) 'added 6/4

            ProgressBarX1.Value = 4
            destwidth = myGD.GetWidth(overlayID)
            destHeight = myGD.GetHeight(overlayID)
            ProgressBarX1.Value = 5
            mergedID = myGD.CreateClonedGdPictureImage(overlayID)
            ProgressBarX1.Value = 6
            myGD.DrawGdPictureImageTransparency(sourceID, mergedID, 125, 0, 0, destwidth, destHeight, Drawing2D.InterpolationMode.NearestNeighbor)

            ProgressBarX1.Value = 7
            ProgressBarX1.Visible = False
            GdViewerMerged.DisplayFromGdPictureImage(mergedID)

            ProgressBarX1.Value = 8
            grpSliderTools.Visible = False
            barStatus.Refresh()
            GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)


            myGD.CountColor(mergedID, Color.Aqua)


            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
        End Try

    End Sub



    Private Sub btnCompareDiff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            RibbonCompNudge.Visible = True
            btnTagComparisons.Enabled = True
            ContainerTags.Enabled = True
            bubbleBar1.Visible = False
            If mvarFileName = "" Or mvarOverlayFile = "" Then
                PopupBox("File Compare Warning", "Please open a file before proceeding")
                Exit Sub
            End If
            PopupBox("Overlay Comparison", "Please wait while Compara identifies the differences")
            Application.DoEvents()

            GdViewerBase.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
            Me.Cursor = Cursors.WaitCursor

            OverlayPanel.Visible = False
            BasePanel.Visible = False
            GdViewerBase.Visible = False
            GdViewerOverlay.Visible = False
            GdViewerMerged.Visible = True

            myGD.ReleaseGdPictureImage(sourceID)
            myGD.ReleaseGdPictureImage(mergedID)
            myGD.ReleaseGdPictureImage(OverlayCloneId)
            If Path.GetExtension(mvarFileName).ToLower = ".pdf" Then
                sourceID = BasePDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                mergedID = OverlayPDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                OverlayCloneId = myGD.CreateClonedGdPictureImage(mergedID) ' first create a new image of the deleted data
            Else

            End If
            myGD.SetContrast(sourceID, 800)
            myGD.SetContrast(mergedID, 800)
            'now show the overlay tools
            'SliderTransparency.Enabled = True

            lblBase2.Enabled = True

            If True Then

                myGD.ConvertTo24BppRGB(sourceID)

                myGD.ConvertTo24BppRGB(mergedID)
                'first get just the common pixels stored in the merged id
                myGD.DrawGdPictureImageOP(sourceID, mergedID, Nudgeleft, NudgeTop, myGD.GetWidth(sourceID), myGD.GetHeight(sourceID), Operators.OperatorPlus, Drawing2D.InterpolationMode.HighQualityBicubic)
                ' myGD.CopyToClipboard(mergedID)

                myGD.DrawGdPictureImageOP(mergedID, sourceID, Nudgeleft, NudgeTop, myGD.GetWidth(sourceID), myGD.GetHeight(sourceID), Operators.OperatorLess, Drawing2D.InterpolationMode.HighQualityBicubic)
                'myGD.CopyToClipboard(sourceID)
                myGD.FxNegative(sourceID)
                ' myGD.CopyToClipboard(sourceID)
                myGD.ConvertTo24BppRGB(sourceID)
                myGD.SwapColor(sourceID, Color.Black, Color.Red)
                ' now create a new Image ID of the added data


                myGD.SetContrast(OverlayCloneId, 100)
                ' myGD.CopyToClipboard(OverlayCloneId)
                myGD.DrawGdPictureImageOP(mergedID, OverlayCloneId, Nudgeleft, NudgeTop, myGD.GetWidth(overlayID), myGD.GetHeight(overlayID), Operators.OperatorLess, Drawing2D.InterpolationMode.HighQualityBicubic)
                myGD.CopyToClipboard(OverlayCloneId)
                'myGD.FxNegative(OverlayCloneId)

                myGD.ConvertTo24BppRGB(OverlayCloneId)
                myGD.SwapColor(OverlayCloneId, Color.Black, Color.Green)

                'now or the resulting source and overlay
                myGD.DrawGdPictureImageOP(sourceID, OverlayCloneId, Nudgeleft, NudgeTop, myGD.GetWidth(sourceID), myGD.GetHeight(sourceID), Operators.OperatorAnd, Drawing2D.InterpolationMode.HighQualityBicubic)

                ' myGD.SetContrast(SourceCloneID, 100)
                'myGD.ConvertTo1Bpp(mergedID)

                myGD.ReleaseGdPictureImage(SourceCloneID)
                SourceCloneID = 0
                myGD.ReleaseGdPictureImage(mergedID)
                mergedID = 0
            Else

                myGD.ConvertTo24BppRGB(sourceID)

                myGD.SwapColor(sourceID, Color.Black, mvarBaseColor) 'Color.Red)

                myGD.ConvertTo24BppRGB(overlayID)
                myGD.SwapColor(overlayID, Color.Black, mvarOverlayColor) 'Color.Green)

                myGD.DrawGdPictureImageTransparency(sourceID, mergedID, SliderTransparency.Value, Nudgeleft, NudgeTop, myGD.GetWidth(sourceID), myGD.GetHeight(sourceID), Drawing2D.InterpolationMode.HighQualityBicubic)

            End If

            If DocuFiSession.Authorized = False Then ' stamp it
                myGD.DrawText(mergedID, "Trial License", 50, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(mergedID, "Trial License", 50, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(mergedID, "Trial License", myGD.GetWidth(sourceID) - 1500, 50, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(mergedID, "Trial License", myGD.GetWidth(sourceID) - 1500, myGD.GetHeight(sourceID) - 250, 75, FontStyle.Bold, Color.Black, "Arial", False)
                myGD.DrawText(mergedID, "Trial License", myGD.GetWidth(sourceID) / 2, myGD.GetHeight(sourceID) / 2, 75, FontStyle.Bold, Color.Black, "Arial", False)
            End If
            GdViewerMerged.DisplayFromGdPictureImage(OverlayCloneId)

            GdViewerMerged.SetZoomFitViewer()
            'set up ui considerations
            btnTagComparisons.Enabled = True
            btnShowDifferences.Enabled = False
            btnHideTags.Enabled = False
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub btnCompareOverlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompareOverlay.Click


        Try

            If mvarFileName = "" Or mvarOverlayFile = "" Then
                PopupBox("File Compare Warning", "Please open a file before proceeding")
                Exit Sub
            End If

            If mvarFileName = "" Or mvarOverlayFile = "" Then
                PopupBox("File Compare Warning", "Please open a file before proceeding")
                OverlayPDF.CloseDocument()
                BasePDF.CloseDocument()
                Exit Sub
            End If

            If OverlayPDF.GetPageCount = 0 Then
                PopupBox("Warning", "The overlay file was not loaded properly.  Please reload")
                OverlayPDF.CloseDocument()
                BasePDF.CloseDocument()
                Exit Sub
            End If
            SliderTransparency.Visible = True

            grpSliderTools.Visible = True
            grpOverlayTools.Visible = True
            GdViewerOverlay.Visible = False
            GdViewerBase.Visible = False
            GdViewerMerged.Clear()
            GdViewerMerged.CloseDocument()


            GdViewerMerged.Width = Me.Width
            Me.Cursor = Cursors.WaitCursor

            'GdViewerBase.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
            BasePanel.Visible = False
            OverlayPanel.Visible = False

            'PopupBox("Overlay Comparison", "Please wait while Compara displays the overlay")
            ' Application.DoEvents()
            RegenOverlay()
            ' OverlayDust()
            'GdViewer1.ZoomArea(myLeft, myTop, myWidth, myHeight)
            GdViewerMerged.Visible = True

            RibbonCompNudge.Visible = True
            btnTagComparisons.Enabled = True
            ContainerTags.Enabled = True


            'set up ui considerations 
            grpOverlayTools.Visible = True
            lblCompareTools.Visible = True
            grpPageTools.Visible = True
            ' btnNextPage.Visible = False
            ' btnPreviousPage.Visible = False

            barStatus.Refresh()

            '
            'btnShowDifferences.Enabled = False
            'btnHideTags.Enabled = False
            Me.Cursor = Cursors.Default
            Me.Refresh()

        Catch ex As Exception
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCompareSidebySide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompareSidebySide.Click
        Try

            grpSliderTools.Visible = False
            SliderSensitivity.Visible = False
            SliderTransparency.Visible = False
            'ui considerations
            RibbonCompNudge.Visible = False
            'btnTagComparisons.Enabled = False
            ContainerTags.Enabled = False

            If mvarFileName = "" Or mvarOverlayFile = "" Then
                PopupBox("File Compare Warning", "Please open a file before proceeding")
                OverlayPDF.CloseDocument()
                BasePDF.CloseDocument()
                Exit Sub
            End If

            If OverlayPDF.GetPageCount = 0 Then
                PopupBox("Warning", "The overlay file was not loaded properly.  Please reload")
                OverlayPDF.CloseDocument()
                BasePDF.CloseDocument()
                Exit Sub
            End If
            If BasePDF.GetPageCount = 0 Then
                PopupBox("Warning", "The Base file was not loaded properly.  Please reload")
                Exit Sub
            End If
            If GdViewerMerged.Visible Then
                GdViewerMerged.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
            Else
                GdViewerBase.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
            End If


            'now hide the viewing objects before loading the files, then make them visible
            GdViewerBase.Visible = False
            GdViewerOverlay.Visible = False
            GdViewerMerged.Visible = False

            ' GdViewer1.DisplayFromFile(mvarFileName)
            ' GdViewer1.DisplayFromGdPictureImage(sourceID)
            If GdViewerBase.PageCount > 1 Then
                'display the thumbnail panels
                OverlayPanel.Visible = True
                BasePanel.Visible = True
                'reload the base thumbs after differences have been rendered
                'thumbBase.LoadFromGdViewer(GdViewer1)
            End If

            If mvarSourceDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF Then
                GdViewerBase.DisplayFromGdPicturePDF(BasePDF)
                GdViewerOverlay.DisplayFromGdPicturePDF(OverlayPDF)
            Else
                GdViewerBase.DisplayFromFile(mvarFileName)
                GdViewerOverlay.DisplayFromFile(mvarOverlayFile)
            End If

            'GdViewer2.DisplayFromFile(mvarOverlayFile)

            GdViewerBase.Width = Me.Width / 2 - 5
            GdViewerOverlay.Width = GdViewerBase.Width
            GdViewerBase.SetZoomWidthViewer()
            GdViewerOverlay.SetZoomWidthViewer()
            ' GdViewerBase.ZoomArea(myLeft, myTop, myWidth, myHeight)
            GdViewerBase.Visible = True
            ' GdViewer2.ZoomArea(myLeft, myTop, myWidth, myHeight)
            GdViewerOverlay.Visible = True

            'User interface considerations
            'SliderTransparency.Visible = False
            'ColorPickerBase.Visible = False
            'ColorPickerOverlay.Visible = False
            grpPageTools.Visible = True
            grpOverlayTools.Visible = False
            lblCompareTools.Visible = False
            btnNextPage.Visible = True
            btnPreviousPage.Visible = True
            btnTagComparisons.Enabled = False
            barStatus.Refresh()


            OverlayPanel.Visible = True
            BasePanel.Visible = True

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnPan_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPan.Click, btnPan2.Click, btnPan3.Click, btnPanPage.Click
        Me.Pan()
        mvarZoomMode = False
        GdViewerBase.MouseMode = ViewerMouseMode.MouseModePan
        GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
        GdViewerMerged.MouseMode = GdViewerBase.MouseMode
        GdViewerBase.Cursor = Cursors.Hand
        GdViewerOverlay.Cursor = GdViewerBase.Cursor
        GdViewerMerged.Cursor = GdViewerBase.Cursor
        DocuFiSession.MeasureMode = 0
    End Sub

    Private Sub btnZoomIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomIn.Click, btnZoomIn2.Click, btnZoomIn3.Click, btnZmInPage.Click
        Try


            If GdViewerBase.Visible Then GdViewerBase.ZoomIN()

            If GdViewerOverlay.Visible Then GdViewerOverlay.ZoomIN()


            If GdViewerMerged.Visible Then GdViewerMerged.ZoomIN()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnZoomOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomOut.Click, btnZoomOut2.Click, btnZoomOut3.Click, btnZmOutPage.Click
        If GdViewerBase.Visible Then GdViewerBase.ZoomOUT()
        If GdViewerOverlay.Visible Then GdViewerOverlay.ZoomOUT()
        If GdViewerMerged.Visible Then GdViewerMerged.ZoomOUT()
    End Sub
    Private Sub btnFitWidth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFitWidth.Click, btnFitWidth2.Click, btnFitWidth3.Click, btnZmExtPage.Click


        If GdViewerBase.Visible Then GdViewerBase.ZoomMode = ViewerZoomMode.ZoomModeFitToViewer
        If GdViewerOverlay.Visible Then GdViewerOverlay.ZoomMode = ViewerZoomMode.ZoomModeFitToViewer
        If GdViewerMerged.Visible Then GdViewerMerged.ZoomMode = ViewerZoomMode.ZoomModeFitToViewer
    End Sub

    Private Sub btnZoomWindow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomWindow.Click, btnZoomWindow2.Click, btnZoomWin3.Click, btnZmWinPage.Click
        DocuFiSession.MeasureMode = 0 'clear this out so draw actions are not set
        GdViewerBase.MouseMode = ViewerMouseMode.MouseModeAreaZooming
        GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
        GdViewerBase.Cursor = Cursors.Cross
        GdViewerOverlay.Cursor = GdViewerBase.Cursor
        GdViewerMerged.MouseMode = GdViewerBase.MouseMode
        GdViewerMerged.Cursor = GdViewerBase.Cursor
    End Sub

    Private Sub btnMagnifier_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMagnifier.Click
        'if the magnifier is on, disable it
        If GdViewerBase.MouseMode = ViewerMouseMode.MouseModeMagnifier Then
            GdViewerBase.MouseMode = ViewerMouseMode.MouseModePan
            GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
            GdViewerBase.Cursor = Cursors.Hand
            GdViewerOverlay.Cursor = GdViewerBase.Cursor
        Else
            GdViewerBase.MouseMode = ViewerMouseMode.MouseModeMagnifier
        End If
        GdViewerOverlay.MouseMode = GdViewerBase.MouseMode
        GdViewerMerged.MouseMode = GdViewerBase.MouseMode
    End Sub

    Private Sub RibbonMeasure_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub nudgeOverlay2()
        Try


            GdViewerMerged.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)

            myGD.ReleaseGdPictureImage(mergedID)
            mergedID = myGD.CreateClonedGdPictureImage(overlayID)
            Dim offsetleft As Integer = Nudgeleft
            Dim offsettop As Integer = NudgeTop

            If Nudgeleft < 0 Then
                offsetleft = Nudgeleft * -1
                myGD.Crop(mergedID, 0, 0, myGD.GetWidth(mergedID) - offsetleft, myGD.GetHeight(mergedID))
            Else
                If NudgeTop < 0 Then
                    offsettop = NudgeTop * -1
                    myGD.Crop(mergedID, Nudgeleft, 0, myGD.GetWidth(mergedID) - offsetleft, myGD.GetHeight(mergedID) - offsettop)
                Else
                    myGD.Crop(mergedID, Nudgeleft, NudgeTop, myGD.GetWidth(mergedID) - offsetleft, myGD.GetHeight(mergedID) - offsettop)
                End If

            End If

            If NudgeTop < 0 Then

            Else

            End If
            ProgressBarX1.Value = 6

            'get the common area as black

            If SliderTransparency.Value < 2 Then
                Dim sourceGreen As Integer
                sourceGreen = myGD.CreateClonedGdPictureImage(sourceID)
                myGD.ConvertTo1Bpp(sourceGreen)
                myGD.ConvertTo16BppRGB565(sourceGreen)
                myGD.CopyToClipboard(sourceGreen)
                myGD.SwapColor(sourceGreen, Color.Black, Color.Green)
                myGD.CopyToClipboard(sourceGreen)

                Dim DestRed As Integer
                DestRed = myGD.CreateClonedGdPictureImage(overlayID)
                myGD.ConvertTo1Bpp(DestRed)
                myGD.CopyToClipboard(DestRed)
                myGD.ConvertTo16BppRGB565(DestRed)
                myGD.SwapColor(DestRed, Color.Black, Color.Red)
                myGD.CopyToClipboard(DestRed)
                myGD.DrawGdPictureImageTransparency(sourceGreen, DestRed, SliderTransparency.Value, 0, 0, myGD.GetWidth(mergedID), myGD.GetHeight(mergedID), Drawing2D.InterpolationMode.HighQualityBicubic)
                GdViewerMerged.DisplayFromGdPictureImage(DestRed)
                myGD.ReleaseGdPictureImage(sourceGreen)

            Else
                'myGD.CopyToClipboard(mergedID)
                myGD.DrawGdPictureImageTransparency(sourceID, mergedID, SliderTransparency.Value, 0, 0, myGD.GetWidth(mergedID), myGD.GetHeight(mergedID), Drawing2D.InterpolationMode.HighQualityBicubic)
                'myGD.DrawGdPictureImageOP(sourceID, mergedID, 0, 0, myGD.GetWidth(mergedID), myGD.GetHeight(mergedID), Operators.OperatorMultiply, Drawing2D.InterpolationMode.HighQualityBicubic)
                GdViewerMerged.DisplayFromGdPictureImage(mergedID)

            End If

            Exit Sub

            '--------------------------------------------------
            Dim destWidth, destHeight As Integer


            'first get the deletions to red
            myGD.ReleaseGdPictureImage(mergedID)
            mergedID = myGD.CreateClonedGdPictureImage(sourceID)


            myGD.DrawGdPictureImageOP(commonID, mergedID, Nudgeleft, NudgeTop, myGD.GetWidth(mergedID), myGD.GetHeight(mergedID), Operators.OperatorLess, Drawing2D.InterpolationMode.High)

            ProgressBarX1.Value = 6
            myGD.FxNegative(mergedID)
            myGD.SwapColor(mergedID, Color.Black, mvarBaseColor)
            ' myGD.CopyToClipboard(mergedID)
            Dim addedID As Integer = myGD.CreateClonedGdPictureImage(overlayID)

            'now get the additions

            ' myGD.ConvertTo24BppRGB(addedID)

            myGD.DrawGdPictureImageOP(commonID, addedID, 0, 0, myGD.GetWidth(addedID), myGD.GetHeight(addedID), Operators.OperatorLess, Drawing2D.InterpolationMode.HighQualityBicubic)
            ProgressBarX1.Value = 7
            myGD.FxNegative(addedID)
            myGD.SwapColor(addedID, Color.Black, mvarOverlayColor)
            myGD.DrawGdPictureImageOP(commonID, addedID, Nudgeleft, NudgeTop, myGD.GetWidth(addedID), myGD.GetHeight(addedID), Operators.OperatorAnd, Drawing2D.InterpolationMode.HighQualityBicubic)
            myGD.DrawGdPictureImageOP(addedID, mergedID, Nudgeleft, NudgeTop, myGD.GetWidth(mergedID), myGD.GetHeight(mergedID), Operators.OperatorAnd, Drawing2D.InterpolationMode.HighQualityBicubic)

done:
            ProgressBarX1.Value = 7
            ProgressBarX1.Visible = False
            GdViewerMerged.DisplayFromGdPictureImage(mergedID)
            myGD.ReleaseGdPictureImage(addedID)
            ProgressBarX1.Value = 8
            grpSliderTools.Visible = False
            barStatus.Refresh()
            GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
            myGD.ReleaseGdPictureImage(mergedID)
            mergedID = myGD.CreateClonedGdPictureImage(overlayID)
            ProgressBarX1.Value = 6

            'get the common area as black

            myGD.DrawGdPictureImageOP(sourceID, mergedID, 0, 0, myGD.GetWidth(overlayID), myGD.GetHeight(overlayID), Operators.OperatorMultiply, Drawing2D.InterpolationMode.HighQualityBicubic)

            GdViewerMerged.DisplayFromGdPictureImage(mergedID)
            Exit Sub
            Me.Cursor = Cursors.Default
            myGD.ReleaseGdPictureImage(mergedID)
            mergedID = myGD.CreateClonedGdPictureImage(overlayID)
            destWidth = myGD.GetWidth(overlayID)
            destHeight = myGD.GetHeight(overlayID)
            ProgressBarX1.Value = 4
            mergedID = myGD.CreateClonedGdPictureImage(overlayID)
            myGD.DrawGdPictureImageTransparency(sourceID, mergedID, SliderTransparency.Value, Nudgeleft, NudgeTop, destWidth, destHeight, Drawing2D.InterpolationMode.NearestNeighbor)
            ProgressBarX1.Value = 8
            ProgressBarX1.Visible = False

            If mergedID <> 0 Then
                myGD.SwapColor(mergedID, myGD.ARGB(255, 188, 92, 0), Color.Black, 95)
                myGD.SwapColor(mergedID, myGD.ARGB(255, 188, 205, 188), mvarOverlayColor, 95)
                myGD.SwapColor(mergedID, myGD.ARGB(255, 254, 186, 186), mvarBaseColor, 95)
            End If
            GdViewerMerged.DisplayFromGdPictureImage(mergedID)

            'grpSliderTools.Visible = False
            barStatus.Refresh()
            GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub NudgeOverlay()
        Try

            If DocuFiSession.Authorized = False Then
                PopupBox("License Notification       ", "Document Registration is not supported in the trial version")
                Application.DoEvents()
                System.Threading.Thread.Sleep(3000)
                Exit Sub
            End If

            PopupBox("Overlay Comparison", "Please wait while Compara aligns the overlay data")
            Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor

            GdViewerMerged.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)

            If sourceID = 0 Then
                MsgBox("The SourceID is empty, please regnerate the Overlay")
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
            Dim sourcecopyID As Integer = myGD.CreateClonedGdPictureImage(sourceID)
            myGD.ConvertTo8BppQ(sourcecopyID)
            myGD.SwapColor(sourcecopyID, Color.Black, mvarBaseColor)
            myGD.ReleaseGdPictureImage(mergedID)
            mergedID = myGD.CreateClonedGdPictureImage(overlayID)
            myGD.ConvertTo8BppQ(mergedID)
            myGD.SwapColor(mergedID, Color.Black, mvarOverlayColor)
            myGD.DrawGdPictureImageTransparency(sourcecopyID, mergedID, SliderTransparency.Value, Nudgeleft, NudgeTop, myGD.GetWidth(mergedID), myGD.GetHeight(mergedID), Drawing2D.InterpolationMode.NearestNeighbor)

            'myGD.DrawGdPictureImageOP(sourceID, mergedID, Nudgeleft, NudgeTop, myGD.GetWidth(commonID), myGD.GetHeight(commonID), Operators.OperatorOr, InterpolationMode.Default)

            GdViewerMerged.DisplayFromGdPictureImage(mergedID)
            GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
            myGD.ReleaseGdPictureImage(sourcecopyID)
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Try


            If GdViewerOverlay.Visible Then
                GdViewerOverlay.Left = Me.Width / 2
                GdViewerBase.Width = Me.Width / 2 - 5
                GdViewerOverlay.Width = GdViewerBase.Width
            Else
                If GdViewerBase.Visible Then GdViewerBase.Width = Me.Width - 20
            End If
            If GdViewerMerged.Visible Then
                GdViewerMerged.Height = Me.Height - barStatus.Height - RibbonPanel1.Height
            End If
            If BasePanel.Expanded Then
                BasePanel.Height = GdViewerBase.Height - barStatus.Height
                OverlayPanel.Height = BasePanel.Height
            End If

            OverlayPanel.Left = Me.Width - GdViewerOverlay.Width
        Catch ex As Exception

        End Try
    End Sub


    Private Sub ButtonMeasurePoly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GdViewerBase.AddPolygonAnnotInteractive(Color.Black, 0.03, m_StrokingColor, 1)
        DocuFiSession.MeasureMode = 3
    End Sub

    Private Sub ButtonAdditions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdditions.Click

        Me.Cursor = Cursors.WaitCursor
        Dim destwidth As Integer
        Dim destheight As Integer
        Dim NewID As Integer
        Dim commonID As Integer
        GdViewerMerged.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
        destwidth = myGD.GetWidth(sourceID)
        destheight = myGD.GetHeight(sourceID)
        ProgressBarX1.Value = 5
        myGD.ReleaseGdPictureImage(commonID)

        commonID = myGD.CreateClonedGdPictureImage(sourceID)
        ProgressBarX1.Value = 6
        'get the common area as black
        myGD.DrawGdPictureImageOP(overlayID, commonID, 0, 0, destwidth, destheight, Operators.OperatorOr, Drawing2D.InterpolationMode.HighQualityBicubic)
        myGD.ConvertTo1Bpp(commonID)
        myGD.ConvertTo24BppRGB(commonID)
        ' myGD.SwapColor(commonID, Color.Black, Color.Purple)
        NewID = myGD.CreateClonedGdPictureImage(overlayID)
        myGD.ConvertTo1Bpp(NewID)
        myGD.ConvertTo24BppRGB(NewID)
        myGD.SwapColor(NewID, Color.Black, Color.Red)

        'myGD.DrawGdPictureImageOP(commonID, NewID, 0, 0, myGD.GetWidth(commonID), myGD.GetHeight(commonID), Operators.OperatorLess, Drawing2D.InterpolationMode.NearestNeighbor)
        myGD.DrawGdPictureImageTransparency(commonID, NewID, 126, 0, 0, myGD.GetWidth(commonID), myGD.GetHeight(commonID), Drawing2D.InterpolationMode.Default)

        myGD.SwapColor(NewID, myGD.ARGB(255, 254, 90, 211), Color.Black)
        myGD.SwapColor(NewID, myGD.ARGB(255, 187, 0, 0), Color.Black)
        myGD.SwapColor(NewID, myGD.ARGB(255, 255, 188, 188), Color.Green)
        myGD.SwapColor(NewID, myGD.ARGB(255, 255, 188, 187), Color.Green)


        GdViewerMerged.DisplayFromGdPictureImage(NewID)
        GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
        grpSliderTools.Visible = False
        barStatus.Refresh()
        GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btDeletions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeletions.Click
        Me.Cursor = Cursors.WaitCursor
        Dim destwidth As Integer
        Dim destheight As Integer
        Dim NewID As Integer
        Dim commonID As Integer
        GdViewerMerged.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
        destwidth = myGD.GetWidth(overlayID)
        destheight = myGD.GetHeight(overlayID)
        ProgressBarX1.Value = 5
        myGD.ReleaseGdPictureImage(mergedID)

        commonID = myGD.CreateClonedGdPictureImage(overlayID)
        ProgressBarX1.Value = 6
        'get the common area as black
        myGD.DrawGdPictureImageOP(sourceID, commonID, 0, 0, myGD.GetWidth(overlayID), myGD.GetHeight(overlayID), Operators.OperatorOr, Drawing2D.InterpolationMode.HighQualityBicubic)
        myGD.ConvertTo1Bpp(commonID)
        myGD.ConvertTo24BppRGB(commonID)
        ' myGD.SwapColor(commonID, Color.Black, Color.Purple)
        NewID = myGD.CreateClonedGdPictureImage(sourceID)
        myGD.ConvertTo1Bpp(NewID)
        myGD.ConvertTo24BppRGB(NewID)
        myGD.SwapColor(NewID, Color.Black, Color.Red)

        'myGD.DrawGdPictureImageOP(commonID, NewID, 0, 0, myGD.GetWidth(commonID), myGD.GetHeight(commonID), Operators.OperatorLess, Drawing2D.InterpolationMode.NearestNeighbor)
        myGD.DrawGdPictureImageTransparency(commonID, NewID, 126, 0, 0, myGD.GetWidth(commonID), myGD.GetHeight(commonID), Drawing2D.InterpolationMode.Default)

        myGD.SwapColor(NewID, myGD.ARGB(255, 254, 90, 211), Color.Black)
        myGD.SwapColor(NewID, myGD.ARGB(255, 187, 0, 0), Color.Black)
        myGD.SwapColor(NewID, myGD.ARGB(255, 255, 188, 188), Color.Red)
        myGD.SwapColor(NewID, myGD.ARGB(255, 255, 188, 188), Color.Red)


        GdViewerMerged.DisplayFromGdPictureImage(NewID)

        grpSliderTools.Visible = False
        barStatus.Refresh()
        GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnCommon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommon.Click
        Me.Cursor = Cursors.WaitCursor
        Dim destwidth As Integer
        Dim destheight As Integer
        GdViewerMerged.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
        destwidth = myGD.GetWidth(overlayID)
        destheight = myGD.GetHeight(overlayID)
        ProgressBarX1.Value = 5

        myGD.ReleaseGdPictureImage(mergedID)
        mergedID = myGD.CreateClonedGdPictureImage(overlayID)
        ProgressBarX1.Value = 6
        ' myGD.DrawGdPictureImageTransparency(sourceID, mergedID, 125, 0, 0, destwidth, destheight, Drawing2D.InterpolationMode.NearestNeighbor)
        'operatoror or shows common
        'operatorand shows merged
        myGD.DrawGdPictureImageOP(sourceID, mergedID, 0, 0, myGD.GetWidth(overlayID), myGD.GetHeight(overlayID), Operators.OperatorOr, Drawing2D.InterpolationMode.HighQualityBicubic)


        myGD.ConvertTo1Bpp(mergedID)
        myGD.ConvertTo24BppRGB(mergedID)

        GdViewerMerged.DisplayFromGdPictureImage(mergedID)

        grpSliderTools.Visible = False
        barStatus.Refresh()
        GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
        Me.Cursor = Cursors.Default
    End Sub


    Private Sub ButtonMeasureCalibrate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMeasureCalibrate.Click
        If DocuFiSession.Authorized = False Then
            PopupBox("License Notification       ", "Calibration is not supported in the Trial Version")
            Application.DoEvents()
            System.Threading.Thread.Sleep(3000)
            Exit Sub
        End If
        GdViewerBase.AddLineAnnotInteractive(Color.BurlyWood, 0.02, Drawing2D.LineCap.ArrowAnchor, Drawing2D.LineCap.ArrowAnchor, 1)
        GdViewerOverlay.AddLineAnnotInteractive(Color.BurlyWood, 0.02, Drawing2D.LineCap.ArrowAnchor, Drawing2D.LineCap.ArrowAnchor, 1)
        GdViewerMerged.AddLineAnnotInteractive(Color.BurlyWood, 0.02, Drawing2D.LineCap.ArrowAnchor, Drawing2D.LineCap.ArrowAnchor, 1)
        DocuFiSession.MeasureMode = 4
    End Sub

    Private Sub btnDifferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDifferences.Click


        Me.Cursor = Cursors.WaitCursor
        GdViewerMerged.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)

        myGD.ReleaseGdPictureImage(mergedID)
        mergedID = myGD.CreateClonedGdPictureImage(overlayID)
        ProgressBarX1.Value = 6

        'get the common area as black

        myGD.DrawGdPictureImageOP(sourceID, mergedID, Nudgeleft, NudgeTop, myGD.GetWidth(overlayID) - Nudgeleft, myGD.GetHeight(overlayID) - NudgeTop, Operators.OperatorMultiply, Drawing2D.InterpolationMode.HighQualityBicubic)

        GdViewerMerged.DisplayFromGdPictureImage(mergedID)
        ' myGD.ReleaseGdPictureImage(addedID)
        grpSliderTools.Visible = False
        barStatus.Refresh()
        GdViewerMerged.ZoomArea(myLeft, myTop, myWidth, myHeight)
        Me.Cursor = Cursors.Default
    End Sub


    Private Sub ButtonTollerance_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTollerance.Click
        If Not DocuFiSession.Authorized Then
            PopupBox("License Notification       ", "Measurements are not supported in the trial version")
            Application.DoEvents()
            System.Threading.Thread.Sleep(3000)
            Exit Sub
        End If
        GdViewerBase.AddEllipseAnnotInteractive(False, True, Color.RosyBrown, mvarDrawColor, 0.01, 1)
        GdViewerOverlay.AddEllipseAnnotInteractive(False, True, Color.RosyBrown, mvarDrawColor, 0.01, 1)
        GdViewerMerged.AddEllipseAnnotInteractive(False, True, Color.RosyBrown, mvarDrawColor, 0.01, 1)
        DocuFiSession.MeasureMode = 6
    End Sub


    Private Sub buttonMeasureLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonMeasureLine.Click
        If Not DocuFiSession.Authorized Then
            PopupBox("License Notification       ", "Measurements are not supported in the trial version")
            Application.DoEvents()
            System.Threading.Thread.Sleep(3000)
            Exit Sub
        End If
        GdViewerBase.AddLineAnnotInteractive(mvarDrawColor, 0.02, Drawing2D.LineCap.ArrowAnchor, Drawing2D.LineCap.ArrowAnchor, 1)
        GdViewerOverlay.AddLineAnnotInteractive(mvarDrawColor, 0.02, Drawing2D.LineCap.ArrowAnchor, Drawing2D.LineCap.ArrowAnchor, 1)
        GdViewerMerged.AddLineAnnotInteractive(mvarDrawColor, 0.02, Drawing2D.LineCap.ArrowAnchor, Drawing2D.LineCap.ArrowAnchor, 1)
        DocuFiSession.MeasureMode = 1
    End Sub

    Private Sub buttonMeasureArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonMeasureArea.Click
        If Not DocuFiSession.Authorized Then
            PopupBox("License Notification       ", "Measurements are not supported in the trial version")
            Application.DoEvents()
            System.Threading.Thread.Sleep(3000)
            Exit Sub
        End If
        GdViewerBase.AddRectangleAnnotInteractive(False, True, Color.RosyBrown, mvarDrawColor, 0.01, 1)
        GdViewerOverlay.AddRectangleAnnotInteractive(False, True, Color.RosyBrown, mvarDrawColor, 0.01, 1)
        GdViewerMerged.AddRectangleAnnotInteractive(False, True, Color.RosyBrown, mvarDrawColor, 0.01, 1)
        DocuFiSession.MeasureMode = 2
    End Sub

    Private Sub btnClearMeasurements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearMeasurements.Click
        Try

            Dim i As Integer

            For i = 1 To GdViewerBase.GetAnnotationCount
                GdViewerBase.DeleteAnnotation(0)
            Next

            'now deal with the second view
            For i = 1 To GdViewerOverlay.GetAnnotationCount
                GdViewerOverlay.DeleteAnnotation(0)
            Next

            'now deal with the merged view
            For i = 1 To GdViewerMerged.GetAnnotationCount
                GdViewerMerged.DeleteAnnotation(0)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnMagIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMagIn.Click
        If GdViewerMerged.Visible Then
            GdViewerMerged.MagnifierZoomX = GdViewerMerged.MagnifierZoomX * 1.5
        Else
            GdViewerBase.MagnifierZoomX = GdViewerBase.MagnifierZoomX * 1.5
            GdViewerBase.MagnifierZoomY = GdViewerBase.MagnifierZoomX
        End If

    End Sub

    Private Sub btnMagOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMagOut.Click
        If GdViewerMerged.Visible Then
            GdViewerMerged.MagnifierZoomX = GdViewerMerged.MagnifierZoomX / 1.5
        Else
            GdViewerBase.MagnifierZoomX = GdViewerBase.MagnifierZoomX / 1.5
            GdViewerBase.MagnifierZoomY = GdViewerBase.MagnifierZoomX
        End If

    End Sub
    Private Sub PopupBox(ByVal inTitle As String, ByVal inMessage As String, Optional ByVal duration As Integer = 200)

        Try

            lblBalloon.Visible = False
            lblBalloon.Left = Me.Width / 2
            lblBalloon.Top = 5
            lblBalloon.Text = inMessage
            BalloonTip1.SetBalloonCaption(lblBalloon, inTitle)
            BalloonTip1.Style = DevComponents.DotNetBar.eBallonStyle.Office2007Alert
            BalloonTip1.AlertAnimationDuration = duration
            BalloonTip1.InitialDelay = 2
            BalloonTip1.SetBalloonText(lblBalloon, inMessage)
            BalloonTip1.ShowBalloon(lblBalloon)
            BalloonTip1.Remove(lblBalloon)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub GdViewer2_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GdViewerOverlay.MouseWheel


        If GdViewerBase.Visible Then
            GdViewerBase.Zoom = GdViewerOverlay.Zoom
            'If e.Delta = 120 Then
            '    GdViewerBase.ZoomIN()
            'ElseIf e.Delta = -120 Then
            '    GdViewerBase.ZoomOUT()
            'ElseIf e.Delta = 240 Then
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            'ElseIf e.Delta = -240 Then
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            'ElseIf e.Delta = 360 Then
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            'ElseIf e.Delta = -360 Then
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            'ElseIf e.Delta = 480 Then
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            'ElseIf e.Delta = -480 Then
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            'ElseIf e.Delta = 600 Then
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            '    GdViewerBase.ZoomIN()
            'ElseIf e.Delta = -600 Then
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            '    GdViewerBase.ZoomOUT()
            'Else
            '    MsgBox("Mouse wheel " & e.Delta)
            'End If
        End If
    End Sub

    Private Sub GdViewer1_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GdViewerBase.MouseWheel


        If GdViewerOverlay.Visible Then
            GdViewerOverlay.Zoom = GdViewerBase.Zoom
            'If e.Delta = 120 Then
            '    GdViewerOverlay.ZoomIN()
            'ElseIf e.Delta = -120 Then
            '    GdViewerOverlay.ZoomOUT()
            'ElseIf e.Delta = 240 Then
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            'ElseIf e.Delta = -240 Then
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            'ElseIf e.Delta = 360 Then
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            'ElseIf e.Delta = -360 Then
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            'ElseIf e.Delta = 480 Then
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            'ElseIf e.Delta = -480 Then
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            'ElseIf e.Delta = 600 Then
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            '    GdViewerOverlay.ZoomIN()
            'ElseIf e.Delta = -600 Then
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            '    GdViewerOverlay.ZoomOUT()
            'Else
            '    MsgBox("Mouse wheel " & e.Delta)
            'End If
            'If e.Delta >= 120 Then
            '    For i As Integer = 1 To e.Delta / 120
            '        GdViewerOverlay.ZoomIN()
            '    Next
            'ElseIf e.Delta <= -120 Then
            '    For i As Integer = e.Delta / 120 To 1 Step -1
            '        GdViewerOverlay.ZoomOUT()
            '    Next
            'End If

        End If
    End Sub


    Private Sub btnTagComparisons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTagComparisons.Click
        GdViewerBase.MouseMode = ViewerMouseMode.MouseModeAreaSelection
        GdViewerOverlay.MouseMode = ViewerMouseMode.MouseModeAreaSelection
        GdViewerMerged.MouseMode = ViewerMouseMode.MouseModeAreaSelection
        GdViewerMerged.Cursor = Cursors.Cross

        DocuFiSession.MeasureMode = 5
    End Sub

    Private Sub LoadThumbsBubbleBar()

        Try

            Dim i As Integer
            Dim bmp As System.Drawing.Bitmap
            Dim bButton As DevComponents.DotNetBar.BubbleButton
            Dim myTag As Integer
            Dim myfont As System.Drawing.Font
            myfont = New System.Drawing.Font("Arial", mvarTagFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

            bubbleBar1.TooltipFont = myfont

            bubbleBarTab1.Buttons.Clear()
            bubbleBar1.TooltipFont = myfont

            RegenOverlay()
            PopupBox("Thumbnail Creation", "Please wait while Compara creates comparison thumbnails", 50)
            Application.DoEvents()
            Dim pagecount As Integer
            pagecount = mvarCompareCount - 1

            If pagecount < 10 Then
                intThumbEnd = pagecount
            Else
                intThumbEnd = intThumbStart + 10 - 1
            End If

            If intThumbStart > 1 Then 'add a previous thumbs to the start of the bubblebar
                bmp = New System.Drawing.Bitmap(APP_Path() & "\back.gif")

                bButton = New DevComponents.DotNetBar.BubbleButton

                bButton.Name = "previous"
                bButton.TooltipText = "previous"
                bButton.Tag = "previous"
                bButton.ImageLarge = bmp
                bButton.Image = bmp

                bubbleBarTab1.Buttons.Add(bButton)
                AddHandler bButton.Click, AddressOf bButton_Click
                bButton.Dispose()
                bmp = Nothing
            End If

            If intThumbEnd > pagecount Then intThumbEnd = pagecount
            If intThumbEnd - intThumbStart > 10 Then intThumbEnd = intThumbStart + 9
            If intThumbEnd > pagecount Then intThumbEnd = pagecount

            For i = intThumbStart To intThumbEnd

                'GdViewer1.GetRectCoordinatesOnDocument(myLeft, myTop, myWidth, myHeight)
                '  GdViewer1.DisplayPage(CompareTags(i).sourcePage)

                ''GdViewer1.CopyRegionToClipboard(CompareTags(i).Left, CompareTags(i).top, CompareTags(i).width, CompareTags(i).height)

                ''myTag = myGD.CreateGdPictureImageFromClipboard
                ''myGD.DrawRectangle(myTag, 1, 1, myGD.GetWidth(myTag) - 2, myGD.GetHeight(myTag) - 2, 3, Color.Blue, False)
                ''myGD.CopyToClipboard(myTag)
                ''CompareTags(i).image = Clipboard.GetImage
                bmp = CompareTags(i).Mergedimage
                bButton = New DevComponents.DotNetBar.BubbleButton

                bButton.Name = CompareTags(i).Name
                bButton.TooltipText = CompareTags(i).Name
                bButton.Tag = "Tag " & i.ToString
                bButton.ImageLarge = bmp
                bButton.Image = bmp

                bubbleBarTab1.Buttons.Add(bButton)
                AddHandler bButton.Click, AddressOf bButton_Click
                bButton.Dispose()
                bmp = Nothing
                myGD.ReleaseGdPictureImage(myTag)
            Next

            If intThumbEnd < pagecount Then 'add a next thumbs to the end of the bubblebar
                bmp = New System.Drawing.Bitmap(APP_Path() & "\forward.gif")

                bButton = New DevComponents.DotNetBar.BubbleButton

                bButton.Name = "next"
                bButton.TooltipText = "next"
                bButton.Tag = "next"
                bButton.ImageLarge = bmp
                bButton.Image = bmp

                bubbleBarTab1.Buttons.Add(bButton)
                AddHandler bButton.Click, AddressOf bButton_Click
                bButton.Dispose()
                bmp = Nothing
            End If
            'now select the first page
            bubbleBarTab1.Visible = True
            bubbleBar1.Refresh()

        Catch ex As Exception
            PopupBox("Button Error", Err.Description)
        End Try
    End Sub

    Private Sub ParseKeywords(ByVal inKeywords As String)
        Try
            Dim i As Integer = 0
            Dim marker As Integer
            Dim TagIdentifiers As String() = inKeywords.Split(New Char() {"]"c})

            ' Use For Each loop over words and display them
            Dim TagIdentifier As String
            For Each TagIdentifier In TagIdentifiers
                'get the name first
                TagIdentifier = Strings.Replace(TagIdentifier, "Compara Tags", "")
                marker = InStr(TagIdentifier, "|")
                Dim Location As String() = TagIdentifier.Split(New Char() {"|"c})
                Dim leftbracket As Integer = Strings.InStr(TagIdentifier, "[")
                If leftbracket < 0 Then Exit For
                CompareTags(i).Name = Strings.Mid(TagIdentifier, leftbracket + 1, marker - leftbracket - 1)
                CompareTags(i).Left = CInt(Location(1))
                CompareTags(i).top = CInt(Location(2))
                CompareTags(i).width = CInt(Location(3))
                CompareTags(i).height = CInt(Location(4))
                i += 1
            Next


        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadThumbsFromPDF(ByVal inPDF As GdPicturePDF)

        Try

            Dim i As Integer
            Dim bmp As System.Drawing.Bitmap
            Dim bButton As DevComponents.DotNetBar.BubbleButton

            bubbleBarTab1.Buttons.Clear()
            PopupBox("Thumbnail Creation", "Please wait while Compara creates comparison thumbnails", 50)
            Application.DoEvents()
            Dim pagecount As Integer
            pagecount = inPDF.GetPageCount - 1

            If pagecount < 10 Then
                intThumbEnd = pagecount
            Else
                intThumbEnd = intThumbStart + 10 - 1
            End If

            If intThumbEnd > pagecount Then intThumbEnd = pagecount
            If intThumbEnd - intThumbStart > 10 Then intThumbEnd = intThumbStart + 9
            If intThumbEnd > pagecount Then intThumbEnd = pagecount

            mvarCompareCount = 0
            'for each page in the pdf, create the thumbs
            For i = 2 To inPDF.GetPageCount
                inPDF.SelectPage(i)
                'exit if we are at the end 
                If i > intThumbEnd + 2 Then Exit For

                Dim myPage As Integer
                myPage = inPDF.RenderPageToGdPictureImage(mvarRenderDPI, False)
                myGD.CopyToClipboard(myPage)
                'draw a border
                myGD.DrawRectangle(myPage, 1, 1, myGD.GetWidth(myPage) - 2, myGD.GetHeight(myPage) - 2, 3, Color.Blue, False)
                myGD.CopyToClipboard(myPage)
                CompareTags(i).Mergedimage = Clipboard.GetImage
                CompareTags(mvarCompareCount).Mergedimage = Clipboard.GetImage
                bmp = CompareTags(mvarCompareCount).Mergedimage
                bButton = New DevComponents.DotNetBar.BubbleButton

                bButton.Name = CompareTags(mvarCompareCount).Name
                bButton.TooltipText = CompareTags(mvarCompareCount).Name
                bButton.Tag = "Tag " & mvarCompareCount.ToString

                bButton.ImageLarge = bmp
                bButton.Image = bmp
                mvarCompareCount += 1
                bubbleBarTab1.Buttons.Add(bButton)
                AddHandler bButton.Click, AddressOf bButton_Click
                bButton.Dispose()
                bmp = Nothing
                myGD.ReleaseGdPictureImage(myPage)
            Next

            If intThumbEnd < inPDF.GetPageCount - 1 Then 'add a next thumbs to the end of the bubblebar
                bmp = New System.Drawing.Bitmap(APP_Path() & "\forward.gif")

                bButton = New DevComponents.DotNetBar.BubbleButton

                bButton.Name = "next"
                bButton.TooltipText = "next"
                bButton.Tag = "next"
                bButton.ImageLarge = bmp
                bButton.Image = bmp

                bubbleBarTab1.Buttons.Add(bButton)
                AddHandler bButton.Click, AddressOf bButton_Click
                bButton.Dispose()
                bmp = Nothing
            End If
            'now select the first page
            bubbleBarTab1.Visible = True
            bubbleBar1.Visible = True
            bubbleBar1.Refresh()
            SliderTransparency.Enabled = False

            'inPDF.CloseDocument()
        Catch ex As Exception
            PopupBox("Button Error", Err.Description)
        End Try
    End Sub
    Private Sub bButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim button As DevComponents.DotNetBar.BubbleButton
        button = TryCast(sender, DevComponents.DotNetBar.BubbleButton)
        Dim TagNum As String
        Dim intTagNum As Integer
        Try
            TagNum = button.Tag


            If TagNum.ToLower = "next" Then

                intThumbStart += 10
                ClearThumbs(1, 999)
                LoadThumbsBubbleBar()
                Exit Sub
            ElseIf TagNum.ToLower = "previous" Then

                intThumbStart -= 10
                ClearThumbs(1, 999)
                LoadThumbsBubbleBar()
                Exit Sub

            End If
            TagNum = Strings.Right(TagNum, Len(TagNum) - 4)
            intTagNum = CInt(TagNum)
            ' thumbBase.SelectItem(CompareTags(intTagNum).sourcePage - 1)

            If GdViewerMerged.Visible Then
                If mvarSourcePage <> CompareTags(intTagNum).sourcePage Then 'we must regen the overlay
                    mvarSourcePage = CompareTags(intTagNum).sourcePage
                    mvarOverlayPage = CompareTags(intTagNum).OverlayPage
                    RegenOverlay()
                End If
                GdViewerMerged.ZoomArea(CompareTags(intTagNum).Left, CompareTags(intTagNum).top, CompareTags(intTagNum).width, CompareTags(intTagNum).height)
                GdViewerMerged.ZoomOUT()
            Else
                GdViewerBase.DisplayPage(CompareTags(intTagNum).sourcePage)
                GdViewerBase.ZoomArea(CompareTags(intTagNum).Left, CompareTags(intTagNum).top, CompareTags(intTagNum).width, CompareTags(intTagNum).height)
                GdViewerBase.ZoomOUT()
            End If
            'GdViewer1.ZoomArea(1461, 821, 876, 414)
            If GdViewerOverlay.Visible Then
                'thumbOverlay.SelectItem(7) 'CompareTags(intTagNum).OverlayPage)
                GdViewerOverlay.DisplayPage(CompareTags(intTagNum).OverlayPage)
                GdViewerOverlay.ZoomArea(CompareTags(intTagNum).Left, CompareTags(intTagNum).top, CompareTags(intTagNum).width, CompareTags(intTagNum).height)
                GdViewerOverlay.ZoomOUT()
            End If
        Catch ex As Exception
            PopupBox("Button Error", Err.Description)
        End Try
    End Sub

    Private Sub ClearThumbs(ByVal start As Integer, ByVal myend As Integer)
        bubbleBarTab1.Buttons.Clear()
    End Sub

    Private Sub btnShowDifferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowDifferences.Click
        bubbleBar1.Visible = True
    End Sub

    Private Sub btnHideTags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHideTags.Click
        bubbleBar1.Visible = False
    End Sub

    Private Sub btnCreateTags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateTags.Click

        mvarCompareCount = 0
        bubbleBarTab1.Buttons.Clear()
        bubbleBar1.Refresh()
        bubbleBar1.Visible = False
        GdViewerMerged.RemoveAllRegions()

        Exit Sub

        'this is the old code for regenerating the tag bubbles
        Me.Cursor = Cursors.WaitCursor
        If mvarCompareCount = 0 Then
            PopupBox("Compare Tag Creator", "No tags are defined")
            Exit Sub
        End If
        LoadThumbsBubbleBar()
        bubbleBar1.Visible = True
        btnShowDifferences.Enabled = True
        btnHideTags.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnPublishTagstoPDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPublishTagstoPDF.Click
        Try

            If Not DocuFiSession.Authorized Then
                PopupBox("License Notification       ", "Publishing Comparison Tags is not supported in the trial version")
                Application.DoEvents()
                System.Threading.Thread.Sleep(3000)
                Exit Sub
            End If
            Dim outimage As String
            If mvarPublishFile = "" Then

                Dim dlg As SaveFileDialog = New SaveFileDialog()
                With dlg
                    .Filter = "  PDF (*.PDF)|*.PDF"
                    .FilterIndex = 1
                    .RestoreDirectory = True
                    .Title = "Publish Comparison document to PDF"
                End With
                Dim res As DialogResult = dlg.ShowDialog()
                If res = Windows.Forms.DialogResult.Cancel Then Exit Sub
                outimage = dlg.FileName
            Else
                outimage = mvarPublishFile

                'if the file already exists, delete it
                If IO.File.Exists(outimage) Then IO.File.Delete(outimage)
                'now make sure the folder exists, create it if it does not
                If IO.Directory.Exists(IO.Path.GetDirectoryName(outimage)) = False Then
                    IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(outimage))
                End If
            End If


            Dim myPDF As New GdPicturePDF

            'set up the progress bar
            Application.DoEvents()
            ProgressBarX1.Maximum = mvarCompareCount + 1
            ProgressBarX1.Visible = True
            ProgressBarX1.Value = 1
            If mergedID = 0 Then RegenOverlay()

            If IO.Path.GetExtension(outimage).ToLower = ".pdf" Then

                myPDF.NewPDF()

                myPDF.SetJpegQuality(100)
                Dim i As Integer
                Dim myKeywords As String = "Compara Tags"
                Dim PageCOunter As Integer = 0
                For i = 0 To mvarCompareCount - 1
                    If mvarSourcePage <> CompareTags(i).sourcePage Then
                        PageCOunter += 1 'store the comparison page number

                        mvarSourcePage = CompareTags(i).sourcePage
                        mvarOverlayPage = CompareTags(i).OverlayPage
                        If i > 0 Then 'save a previous page
                            myPDF.SetKeywords(myKeywords)
                            myPDF.AddImageFromGdPictureImage(mergedID, False, True)
                        End If
                        CompareTags(i).ComparePage = PageCOunter 'store the comparison page number

                        RegenOverlay()
                    Else
                        CompareTags(i).ComparePage = PageCOunter
                    End If

                    myKeywords = myKeywords & "[" & CompareTags(i).Name & "|" & CompareTags(i).Left & "|" & CompareTags(i).top & "|" & CompareTags(i).width & "|" & CompareTags(i).height & "]" & vbCrLf

                    myGD.DrawText(mergedID, CompareTags(i).Name, CompareTags(i).Left, CompareTags(i).top + mvarTagFontSize / 2, mvarTagFontSize, FontStyle.Bold, Color.Red, "Arial", False)
                    myGD.DrawRectangle(mergedID, CompareTags(i).Left, CompareTags(i).top, CompareTags(i).width, CompareTags(i).height, 5, Color.Red, False)

                Next


                ' myGD.ConvertTo8BppQ(mergedID)
                myPDF.AddImageFromGdPictureImage(mergedID, False, True)
                Dim myClipboard As Integer
                'now add the clips as secondary pages
                Dim actionID, BookMarkID As Integer
                Dim pagesBookmarkID As Integer
                Dim myRight, myTop As Integer
                pagesBookmarkID = myPDF.NewBookmark(0, "Compara Tags")
                For i = 0 To mvarCompareCount - 1
                    ProgressBarX1.Value = i + 1
                    myClipboard = myGD.CreateGdPictureImageFromBitmap(CompareTags(i).Mergedimage)
                    myPDF.AddImageFromGdPictureImage(myClipboard, False, True)
                    myGD.ReleaseGdPictureImage(myClipboard)

                    myRight = CompareTags(i).Left + CompareTags(i).width
                    myTop = CompareTags(i).top + CompareTags(i).height
                    actionID = myPDF.NewActionGoTo(PdfDestinationType.DestinationTypeFitB, CompareTags(i).ComparePage, CompareTags(i).Left, myRight, myTop, CompareTags(i).top, 5)

                    BookMarkID = myPDF.NewBookmark(pagesBookmarkID, CompareTags(i).Name)
                    myPDF.SetBookmarkAction(BookMarkID, actionID)

                    myClipboard = myGD.CreateGdPictureImageFromBitmap(CompareTags(i).sourceImage)
                    myPDF.AddImageFromGdPictureImage(myClipboard, False, True)
                    myGD.CopyToClipboard(myClipboard)

                    myGD.ReleaseGdPictureImage(myClipboard)

                    myClipboard = myGD.CreateGdPictureImageFromBitmap(CompareTags(i).DestImage)
                    myPDF.AddImageFromGdPictureImage(myClipboard, False, True)
                    myGD.CopyToClipboard(myClipboard)
                    myGD.ReleaseGdPictureImage(myClipboard)

                Next
                ProgressBarX1.Value = ProgressBarX1.Maximum
                myPDF.SaveToFile(outimage)
                myPDF.CloseDocument()
                'myGD.SaveAsPDF(mergedID, outimage, False, "Compara Results", "DocuFi", "", "", "DocuFi Compara")
                PopupBox("Publishing", "Comparisons were published to " & outimage)
                ProgressBarX1.Visible = False
            End If

        Catch ex As Exception
            PopupBox("Publishing to PDF", "Exception" & ex.Message)
        End Try
    End Sub

    Private Sub btnPublishtoTIF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try


            Dim outimage As String
            Dim dlg As SaveFileDialog = New SaveFileDialog()
            With dlg
                .Filter = "  TIF (*.TIF)|*.TIF"
                .FilterIndex = 1
                .RestoreDirectory = True
                .Title = "Publish Comparison document to TIF"
            End With
            Dim res As DialogResult = dlg.ShowDialog()

            If res = Windows.Forms.DialogResult.Cancel Then Exit Sub
            outimage = dlg.FileName
            If IO.Path.GetExtension(dlg.FileName).ToLower = ".tif" Then

                Dim i As Integer

                For i = 0 To mvarCompareCount - 1
                    myGD.DrawText(mergedID, CompareTags(i).Name, CompareTags(i).Left, CompareTags(i).top - 30, 40, FontStyle.Bold, Color.Red, "Arial", False)
                    myGD.DrawRectangle(mergedID, CompareTags(i).Left, CompareTags(i).top, CompareTags(i).width, CompareTags(i).height, 5, Color.Red, False)
                Next

                myGD.SaveAsTIFF(mergedID, outimage, False, TiffCompression.TiffCompressionLZW)
                PopupBox("Publishing", "Comparisons were published to " & outimage)
            End If

        Catch ex As Exception
            PopupBox("Publishing to TIF", "Exception" & ex.Message)
        End Try
    End Sub

    Private Sub btnAlign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlign.Click

        If Not DocuFiSession.Authorized Then
            PopupBox("License Notification       ", "Aligning documents is not supported in the trial version")
            Application.DoEvents()
            System.Threading.Thread.Sleep(3000)
            Exit Sub
        End If
        GdViewerMerged.AddLineAnnotInteractive(Color.BurlyWood, 0.01, Drawing2D.LineCap.DiamondAnchor, Drawing2D.LineCap.DiamondAnchor, 1)
        DocuFiSession.MeasureMode = 7
    End Sub



    Private Sub GdViewer1_PageDisplayed() Handles GdViewerBase.PageDisplayed
        If GdViewerOverlay.Visible Then


            mvarSourcePage = GdViewerBase.CurrentPage
            If Not mvarConvertMode Then
                'now clear the holders for the previous comparison
                myGD.ReleaseGdPictureImage(overlayID)
                overlayID = 0
                myGD.ReleaseGdPictureImage(sourceID)
                sourceID = 0
            End If

        End If
        If DocuFiSession.IsPDFTrans Then
            lblPages.Text = "Viewing Page " & mvarSourcePage.ToString
        Else
            lblPages.Text = "Comparing Pages " & mvarSourcePage.ToString & " and " & mvarOverlayPage.ToString
        End If

    End Sub

    Private Sub GdViewer2_ScrollViewer() Handles GdViewerOverlay.ScrollViewer

        GdViewerOverlay.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)

        'GdViewerOverlay.GetDisplayedAreaininches(myLeft, myTop, myWidth, myHeight)
        If GdViewerBase.Visible And Not mvarConvertMode Then
            GdViewerBase.ZoomArea(myLeft, myTop, myWidth, myHeight)
            'myTop = GdViewerOverlay.GetHScrollBarPosition()
            'GdViewerBase.SetHScrollBarPosition(myTop)
            'myLeft = GdViewerOverlay.GetVScrollBarPosition()
            'GdViewerBase.SetVScrollBarPosition(myLeft)
        End If

    End Sub



    Private Sub GdViewer1_ScrollViewer() Handles GdViewerBase.ScrollViewer
        Try

            'myTop = GdViewerBase.GetHScrollBarPosition()
            'GdViewerOverlay.SetHScrollBarPosition(myTop)
            'myLeft = GdViewerBase.GetVScrollBarPosition()
            'GdViewerOverlay.SetVScrollBarPosition(myLeft)

            GdViewerBase.GetDisplayedArea(myLeft, myTop, myWidth, myHeight)
            If GdViewerOverlay.Visible And GdViewerOverlay.PageCount > 0 And Not mvarConvertMode Then
                ' MsgBox(GdViewerOverlay.GetDocumentLeft())
                If GdViewerOverlay.GetStat = GdPictureStatus.OK Then
                    GdViewerOverlay.ZoomArea(myLeft, myTop, myWidth, myHeight)
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub GdViewer2_PageDisplayed() Handles GdViewerOverlay.PageDisplayed
        mvarOverlayPage = GdViewerOverlay.CurrentPage

        lblPages.Text = "Comparing Pages " & mvarSourcePage.ToString & " and " & mvarOverlayPage.ToString
    End Sub


    Private Sub btnPreferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreferences.Click, btnPreferences2.Click
        Dim oldBaseColor As Color = mvarBaseColor
        Dim oldoverlaycolor As Color = mvarOverlayColor
        frmPreferences.ShowDialog()


        If mvarBaseColor <> oldBaseColor Then
            If mergedID <> 0 Then
                myGD.SwapColor(mergedID, oldBaseColor, mvarBaseColor, 95)
                GdViewerMerged.DisplayFromGdPictureImage(mergedID)
            End If

        End If
        If mvarOverlayColor <> oldoverlaycolor Then
            If mergedID <> 0 Then
                myGD.SwapColor(mergedID, oldoverlaycolor, mvarOverlayColor, 95)
                GdViewerMerged.DisplayFromGdPictureImage(mergedID)
            End If

        End If
        Select Case mvarUnits
            Case 1
                lblUnits.Text = "Inches"
            Case 2
                lblUnits.Text = "Feet"
            Case 3
                lblUnits.Text = "Meters"
            Case 4

                lblUnits.Text = "Centimeters"
        End Select
    End Sub


    Private Sub txtCircleSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCircleSize.KeyPress
        Try


            If Regex.IsMatch(e.KeyChar, "[.0-9]") = False Then
                PopupBox("Warning", "Please enter a numeric value", 5)
            Else
                mvarCircleSize = CDbl(txtCircleSize.ControlText & e.KeyChar)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnShrink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShrink.Click
        GdViewerBase.MagnifierHeight = GdViewerBase.MagnifierHeight * 0.7
        GdViewerOverlay.MagnifierHeight = GdViewerBase.MagnifierHeight
        GdViewerBase.MagnifierWidth = GdViewerBase.MagnifierWidth * 0.7
        GdViewerOverlay.MagnifierWidth = GdViewerBase.MagnifierWidth
        GdViewerMerged.MagnifierWidth = GdViewerBase.MagnifierWidth
        GdViewerMerged.MagnifierHeight = GdViewerBase.MagnifierHeight
    End Sub

    Private Sub btnEnlarge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnlarge.Click
        GdViewerBase.MagnifierHeight = GdViewerBase.MagnifierHeight * 1.5
        GdViewerOverlay.MagnifierHeight = GdViewerBase.MagnifierHeight
        GdViewerBase.MagnifierWidth = GdViewerBase.MagnifierWidth * 1.5
        GdViewerOverlay.MagnifierWidth = GdViewerBase.MagnifierWidth
        GdViewerMerged.MagnifierWidth = GdViewerBase.MagnifierWidth
        GdViewerMerged.MagnifierHeight = GdViewerBase.MagnifierHeight
    End Sub


    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Nudgeleft = 0
        NudgeTop = 0

        nudgeOverlay2()
    End Sub
    'syncronize the pannels
    Private Sub BasePanel_ExpandedChanged(ByVal sender As Object, ByVal e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles BasePanel.ExpandedChanged
        OverlayPanel.Expanded = BasePanel.Expanded
        If BasePanel.Expanded Then
            BasePanel.Height = GdViewerBase.Height - barStatus.Height
            OverlayPanel.Height = BasePanel.Height
        End If


    End Sub

    Private Sub OverlayPanel_ExpandedChanged(ByVal sender As Object, ByVal e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles OverlayPanel.ExpandedChanged
        BasePanel.Expanded = OverlayPanel.Expanded
        If BasePanel.Expanded Then
            BasePanel.Height = GdViewerBase.Height - barStatus.Height
            OverlayPanel.Height = BasePanel.Height
        End If
    End Sub

    'Private Sub thumbBase_SelectedThumbnailChanged(ByVal Idx As Integer) Handles thumbBase.SelectedThumbnailChanged
    '    myGD.ReleaseGdPictureImage(sourceID)
    '    mvarSourcePage = GdViewer1.CurrentPage
    '    If IO.Path.GetExtension(mvarFileName).ToLower = ".pdf" Then
    '        GdViewer1.DisplayPage(mvarSourcePage)
    '        BasePDF.SelectPage(mvarSourcePage)
    '        sourceID = BasePDF.RenderPageToGdPictureImage(200, False)
    '        myGD.CopyToClipboard(sourceID)
    '    Else
    '        myGD.TiffSelectPage(sourceID, mvarSourcePage)
    '    End If
    'End Sub

    'Private Sub thumbOverlay_SelectedThumbnailChanged(ByVal Idx As Integer) Handles thumbOverlay.SelectedThumbnailChanged

    '    myGD.ReleaseGdPictureImage(overlayID)
    '    mvarOverlayPage = GdViewer2.CurrentPage
    '    If IO.Path.GetExtension(mvarOverlayFile).ToLower = ".pdf" Then
    '        GdViewer2.DisplayPage(mvarOverlayPage)
    '        OverlayPDF.SelectPage(mvarOverlayPage)
    '        overlayID = OverlayPDF.RenderPageToGdPictureImage(200, False)
    '    Else
    '        myGD.TiffSelectPage(overlayID, mvarSourcePage)
    '    End If

    'End Sub

    Private Sub btnNextPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNextPage.Click
        If DocuFiSession.Authorized = False Then
            PopupBox("License Notification       ", "MultiPage Navigation is not supported in the Trial Version")
            Application.DoEvents()
            System.Threading.Thread.Sleep(3000)
            Exit Sub
        End If
        mvarOverlayPage += 1
        Me.Cursor = Cursors.WaitCursor

        If mvarOverlayPage > mvarOverlayPageCount Then mvarOverlayPage = mvarOverlayPageCount

        If mvarOverlayPage <= 0 Then mvarOverlayPage = 1

        mvarSourcePage += 1
        If mvarSourcePage > mvarSourcePageCount Then mvarSourcePage = mvarSourcePageCount

        If BasePDF.GetPageCount = 1 And OverlayPDF.GetPageCount = 1 Then

            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        'now clear the holders for the previous comparison
        myGD.ReleaseGdPictureImage(overlayID)
        overlayID = 0
        myGD.ReleaseGdPictureImage(sourceID)
        sourceID = 0

        If RibbonCompare.IsOnMenu Then
            lblPages.Text = "Comparing Pages " & mvarSourcePage.ToString & " and " & mvarOverlayPage.ToString
        Else
            lblPages.Text = "Viewing Page " & mvarSourcePage.ToString
        End If

        If GdViewerMerged.Visible Then ' need to regenerate the merged view
            sourceID = 0 'clear out the source id to regen the pages

            RegenOverlay()
        Else
            GdViewerBase.Clear()

            GdViewerBase.DisplayPage(mvarSourcePage)
            GdViewerOverlay.Clear()
            GdViewerOverlay.DisplayPage(mvarOverlayPage)
        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnPreviousPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreviousPage.Click
        mvarOverlayPage -= 1
        Me.Cursor = Cursors.WaitCursor
        If mvarOverlayPage <= 0 Then mvarOverlayPage = 1

        mvarSourcePage -= 1
        If mvarSourcePage <= 0 Then mvarSourcePage = 1
        lblPages.Text = "Viewing Pages " & mvarSourcePage.ToString & " and " & mvarOverlayPage.ToString
        If GdViewerMerged.Visible Then ' need to regenerate the merged view
            sourceID = 0 'clear out the source id to regen the pages
            'now clear the holders for the previous comparison
            myGD.ReleaseGdPictureImage(overlayID)
            overlayID = 0
            myGD.ReleaseGdPictureImage(sourceID)
            sourceID = 0
            RegenOverlay()
        Else
            GdViewerBase.DisplayPage(mvarSourcePage)
            GdViewerOverlay.DisplayPage(mvarOverlayPage)
        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ComboBoxItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboResolution.Click
        If DocuFiSession.IsMultipage Then
            If comboResolution.SelectedItem.ToString = "100" Then
                mvarRenderDPI = 100
            ElseIf comboResolution.SelectedItem.ToString = "200" Then
                mvarRenderDPI = 200
            ElseIf comboResolution.SelectedItem.ToString = "72" Then 'added 3.3
                mvarRenderDPI = 72
            ElseIf comboResolution.SelectedItem.ToString = "150" Then
                mvarRenderDPI = 150
            ElseIf comboResolution.SelectedItem.ToString = "300" Then
                mvarRenderDPI = 300
            ElseIf comboResolution.SelectedItem.ToString = "400" Then
                mvarRenderDPI = 400
            Else
                mvarRenderDPI = 150
            End If
            'clear the previous rendered base and overlay images
            myGD.ReleaseGdPictureImage(sourceID)
            myGD.ReleaseGdPictureImage(overlayID)
            sourceID = 0
            mvarSourceGDHandle = 0
        Else
            If blnStartupCompleted Then

                PopupBox("License Validation     .", "You must have a full ComPara license to change rendering resolutions")
                Application.DoEvents()
                System.Threading.Thread.Sleep(3000)
                Exit Sub
            End If
        End If

    End Sub


    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        If DocuFiSession.isCompare Then
            frmAbout.ShowDialog()
        Else
            frmAboutPDFTrans.ShowDialog()
        End If

    End Sub


    Private Sub btnVisit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVisit.Click
        System.Diagnostics.Process.Start("http://www.docufi.com")
    End Sub

    Private Sub btnSaveAllPages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAllPages.Click
        Dim blnSuccess As Boolean = False
        Dim ext As String = ""

        Dim res As System.Windows.Forms.DialogResult
        ext = IO.Path.GetExtension(mvarSourceDocumentFormat).ToLower
        'if its a pdf file, default to tif for convert
        If DocuFiSession.IsPDFTrans Then

            If mvarSourceDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF Then
                saveFileDialog1.Filter = "TIF Files|*.tif|JPEG Files|*.jpg"
            Else
                saveFileDialog1.Filter = "PDF Files|*.pdf|TIF Files|*.tif|JPEG Files|*.jpg"
            End If
        Else
            saveFileDialog1.Filter = "PDF Files|*.pdf|TIF Files|*.tif|JPEG Files|*.jpg"
        End If



        res = saveFileDialog1.ShowDialog()

        If res = Windows.Forms.DialogResult.Cancel Then Exit Sub

        'if it's not saving to pdf, then save the file to tif or jpg
        If IO.Path.GetExtension(saveFileDialog1.FileName).ToLower <> ".pdf" Then

            If DocuFiSession.ExtractTIF Then
                If GDPicExtractTIF(mvarFileName, saveFileDialog1.FileName, chkIsColor.Checked) = False Then
                    PopupBox("TIF Creation Failed", "Failed to create " & IO.Path.GetFileName(OutputFilePath))
                    Exit Sub
                Else
                    blnSuccess = True
                End If
            Else

                If GDPicRenderTIF(mvarFileName, saveFileDialog1.FileName, chkIsColor.Checked) = False Then
                    PopupBox("TIF Creation Failed", "Failed to create " & IO.Path.GetFileName(OutputFilePath))
                    Exit Sub
                Else
                    blnSuccess = True
                End If

            End If
        Else
            gdSaveToPDF(mvarFileName, saveFileDialog1.FileName, chkIsColor.Checked)
        End If
        OutputFilePath = saveFileDialog1.FileName
        If IO.File.Exists(OutputFilePath) Then
            PopupBox("File Conversion Completed", "Successfully created " & IO.Path.GetFileName(OutputFilePath))
            'if the base pdf is greater than 1 page, then display the thumbnail
            If BasePDF.GetPageCount > 0 Then OverlayPanel.Visible = True
            OverlayPanel.TitleText = "Converted Pages"
            lblBase2.Text = "  Source:  " & IO.Path.GetFileName(mvarFileName) & "    Saved As:  " & IO.Path.GetFileName(mvarOverlayFile) & " "

        End If

        'If btnConverttoTIF.Tag = "pdf" Then
        '    If GDPicRenderTIF(mvarFileName, chkIsColor.Checked) = False Then
        '        If GDPicExtractTIF(mvarFileName, chkIsColor.Checked) = False Then
        '            'If VeryPDFExtractTif(mvarFileName, chkIsColor.Checked) = False Then
        '            PopupBox("File Conversion Failed", "Failed to create " & IO.Path.GetFileName(OutputFilePath))
        '            Exit Sub
        '            'End If
        '        End If
        '    End If
        'Else
        '    gdSaveToPDF(mvarFileName, mvarFileName, chkIsColor.Checked)
        'End If
        'If IO.File.Exists(mvarOverlayFile) Then

        '    PopupBox("File Conversion Completed", "Successfully created " & IO.Path.GetFileName(mvarOverlayFile))
        '    'if the base pdf is greater than 1 page, then display the thumbnail
        '    If BasePDF.GetPageCount > 0 Then OverlayPanel.Visible = True
        '    OverlayPanel.TitleText = "Converted Pages"
        '    lblBase2.Text = "  Source:  " & IO.Path.GetFileName(mvarFileName) & "    Converted:  " & IO.Path.GetFileName(mvarOverlayFile) & " "

        'End If
        barStatus.Refresh()
    End Sub




    Private Function GDPicExtractTIF(ByVal inPDF As String, ByVal OutputFilePath As String, ByVal isColor As Boolean) As Boolean
        Try
            Dim PDFFile As String = inPDF
            ' Dim appData As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            Dim appData As String = System.IO.Path.GetTempPath

            Dim myGuid As String = System.Guid.NewGuid.ToString()

            Dim currDate As Date = Now()

            GDPicExtractTIF = False
            Dim oGdPictureImaging As New GdPicture14.GdPictureImaging

            Dim ImageNo As Integer = 0  'Number of extracted bitmaps 
            Dim TiffID As Integer = 0  'the multipage TIFF document ID 
            Dim status As GdPicture14.GdPictureStatus
            blnTIFSaved = False

            'license the gdpicture engine

            If BasePDF.GetPageCount > 0 Then

                BasePDF.SelectPage(1)
                If BasePDF.IsPageImage() = False Then
                    ' BasePDF.Dispose()
                    'Exit Function
                End If
                Dim ExtractPageID As Integer = BasePDF.ExtractPageImage(1)
                If ExtractPageID = 0 Then
                    ' MsgBox("This image did not Extract, Please try a lower resolution")
                    GdPictureDocumentUtilities.ReleaseAllGdPictureImages()
                    Exit Function
                End If
                ' Dim yresolution As Integer = Convert.ToInt32(comboResolution.Text)
                Dim pageWidth As Integer = oGdPictureImaging.GetWidthInches(ExtractPageID)
                Dim pageHeight As Integer = oGdPictureImaging.GetHeightInches(ExtractPageID)
                'lblType.Text = "Dimensions: " & pageWidth.ToString & " by " & pageHeight.ToString & " inches"
                If isColor Then
                    status = oGdPictureImaging.SaveAsJPEG(ExtractPageID, OutputFilePath)
                    oGdPictureImaging.TiffCloseMultiPageFile(ExtractPageID)
                    oGdPictureImaging.ReleaseGdPictureImage(ExtractPageID)
                Else
                    'binarize the image to black and white
                    Call oGdPictureImaging.SetContrast(ExtractPageID, 255)
                    oGdPictureImaging.ConvertTo1BppAT(ExtractPageID, 255)
                    status = oGdPictureImaging.TiffSaveAsMultiPageFile(ExtractPageID, OutputFilePath, GdPicture14.TiffCompression.TiffCompressionCCITT4)

                    oGdPictureImaging.TiffCloseMultiPageFile(ExtractPageID)
                    oGdPictureImaging.ReleaseGdPictureImage(ExtractPageID)
                End If

            End If

            'now see if the file was created 
            If IO.File.Exists(OutputFilePath) Then

                blnTIFSaved = True

                Dim iret As GdPicture14.GdPictureStatus
                GdViewerBase.SilentMode = True
                iret = GdViewerOverlay.DisplayFromFile(OutputFilePath)
                thumbOverlay.LoadFromGdViewer(GdViewerOverlay)
                If iret <> GdPicture14.GdPictureStatus.OK Then
                    MsgBox("Warning, this file cannot be displayed, but may have converted" & vbCrLf & "Please try a lower resolution")
                End If
                GdViewerOverlay.ZoomMode = GdPicture14.ViewerZoomMode.ZoomModeWidthViewer

                GdViewerOverlay.Visible = True
            Else
                blnTIFSaved = False
            End If
            oGdPictureImaging.TiffCloseMultiPageFile(TiffID)
            oGdPictureImaging.ReleaseGdPictureImage(TiffID)

            ProgressBarX1.Visible = False

            Me.Cursor = Cursors.Default
            Return blnTIFSaved
        Catch
            Return False ' MsgBox(Err.Description)
        Finally
            '' Message is displayed since the exception caused
        End Try

    End Function
    Private Function GDPicRenderTIF(ByVal inPDF As String, ByVal outFile As String, ByVal isColor As Boolean, Optional ByVal inPage As Integer = 0) As Boolean
        Try
            Dim PDFFile As String = inPDF
            ' Dim appData As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            Dim appData As String = System.IO.Path.GetTempPath
            Dim startPage As Integer
            Dim endPage As Integer
            Dim myGuid As String = System.Guid.NewGuid.ToString()

            GDPicRenderTIF = False
            Dim oGdPicturePDF As New GdPicture14.GdPicturePDF
            Dim oGdPictureImaging As New GdPicture14.GdPictureImaging

            OutputFilePath = outFile

            Dim SaveAsJpeg As Boolean = False

            If IO.Path.GetExtension(outFile).ToLower = ".jpg" Or IO.Path.GetExtension(outFile).ToLower = ".jpeg" Then
                SaveAsJpeg = True
            End If
            Dim ImageNo As Integer = 0  'Number of extracted bitmaps 


            Dim status As GdPicture14.GdPictureStatus
            blnTIFSaved = False
            GdPictureDocumentUtilities.ReleaseAllGdPictureImages()
            ProgressBarX1.Visible = True
            Me.Cursor = Cursors.WaitCursor
            ' GdViewerBase.Cursor = Cursors.WaitCursor
            'now try to reload it using the rendering method
            If BasePDF.GetCurrentPage > 0 Then
                'If oGdPicturePDF.LoadFromFile(inPDF, False) = GDPicture14.GdPictureStatus.OK Then
                ProgressBarX1.Maximum = BasePDF.GetPageCount
                If DocuFiSession.IsMultipage = False And BasePDF.GetPageCount > 1 Then

                    startPage = 1
                    endPage = 1
                    PopupBox("License Notification       ", "MultiPage support is not licensed, only the first page will be converted")
                    Application.DoEvents()
                    System.Threading.Thread.Sleep(3000)
                Else
                    If inPage = 0 Then
                        startPage = 1
                        endPage = BasePDF.GetPageCount
                    Else
                        startPage = inPage
                        endPage = inPage
                    End If

                End If
                Dim ResOverwrite As Integer = 300

                For i As Integer = startPage To endPage
                    If status = GdPicture14.GdPictureStatus.OK Or True Then
                        ProgressBarX1.Value = i
                        BasePDF.SelectPage(i)
                        'Dim yresolution As Integer = Convert.ToInt32(comboResolution.SelectedItem)
                        Dim RasterizedPageID As Integer

                        If DocuFiSession.ExtractTIF Then
                            RasterizedPageID = BasePDF.RenderPageToGdPictureImage(mvarRenderDPI, i)

                            If oGdPictureImaging.GetHorizontalResolution(RasterizedPageID) > 400 Then
                                oGdPictureImaging.SetHorizontalResolution(RasterizedPageID, ResOverwrite)
                                oGdPictureImaging.SetVerticalResolution(RasterizedPageID, ResOverwrite)
                            End If
                        Else

                            RasterizedPageID = BasePDF.RenderPageToGdPictureImage(mvarRenderDPI, i)

                        End If


                        'lblBlackCount.Text = "BlackPixels: " & oGdPictureImaging.CountColor(RasterizedPageID, Drawing.Color.Black)
                        Dim myGDstat As GdPicture14.GdPictureStatus = oGdPictureImaging.GetStat
                        If RasterizedPageID = 0 Then
                            PopupBox("GD Rendering Warning ", "This image did not render, Please try a lower resolution")

                            Me.Cursor = Cursors.Default
                            ProgressBarX1.Visible = False
                            Exit Function
                        End If
                        'Dim pageWidth As Integer = oGdPictureImaging.GetWidthInches(RasterizedPageID)
                        'Dim pageHeight As Integer = oGdPictureImaging.GetHeightInches(RasterizedPageID)
                        'lblType.Text = "Dimensions: " & pageWidth.ToString & " by " & pageHeight.ToString & " inches"

                        If SaveAsJpeg Then

                            status = oGdPictureImaging.SaveAsJPEG(RasterizedPageID, OutputFilePath)


                        ElseIf isColor = False And Not DocuFiSession.ExtractTIF Then
                            'binarize the image to black and white
                            If DocuFiSession.ThreshMode = 1 Then
                                myGD.ConvertTo1BppSauvola(RasterizedPageID, SensitivityValue / 100.0F, ThresholdValue, 3)
                            Else
                                myGD.ConvertTo1BppSauvola(RasterizedPageID, SensitivityValue / 100.0F, ThresholdValue, 3)

                            End If

                            If i = startPage Then
                                TiffID = myGD.TiffCreateMultiPageFromGdPictureImage(RasterizedPageID)
                            Else
                                myGD.TiffAppendPageFromGdPictureImage(TiffID, RasterizedPageID)
                            End If
                        Else
                            If i = startPage Then
                                TiffID = myGD.TiffCreateMultiPageFromGdPictureImage(RasterizedPageID)
                            Else
                                myGD.TiffAppendPageFromGdPictureImage(TiffID, RasterizedPageID)
                            End If
                        End If

                        myGD.ReleaseGdPictureImage(RasterizedPageID)
                    End If

                Next

                If Not SaveAsJpeg Then
                    myGD.TiffCloseMultiPageFile(TiffID)
                    myGD.TiffSaveMultiPageToFile(TiffID, OutputFilePath, TiffCompression.TiffCompressionAUTO)
                End If


                ProgressBarX1.Visible = False

                '    'lblBitCount.Text = "BitCount: " & oGdPictureImaging.GetBitDepth(TiffID)

            Else
                'MsgBox("Invalid PDF or insufficient resources to convert this PDF")

                Return False
            End If

            'now see if the file was created 
            If IO.File.Exists(OutputFilePath) Then

                blnTIFSaved = True
            Else
                blnTIFSaved = False
            End If

            If blnTIFSaved Then
                GdViewerOverlay.DisplayFromFile(OutputFilePath)
                thumbOverlay.LoadFromGdViewer(GdViewerOverlay)
                GdViewerOverlay.SetZoomFitViewer()
                mvarOverlayPageCount = GdViewerOverlay.PageCount
                lblBase2.Text = "Source File: " & IO.Path.GetFileName(mvarFileName) & "   Converted File: " & IO.Path.GetFileName(OutputFilePath)
            End If
            Me.Cursor = Cursors.Default
            GdViewerBase.Cursor = Cursors.Default
            Return blnTIFSaved
        Catch '
            Me.Cursor = Cursors.Default
            GdViewerBase.Cursor = Cursors.Default
            MsgBox(Err.Description)
        Finally
            '' Message is displayed since the exception caused
            '' by ReadDwgFile is handled.

        End Try

    End Function

    Private Sub btnHelp_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        Try


            If DocuFiSession.isCompare Then
                System.Diagnostics.Process.Start(APP_Path() & "comparaManual.pdf")
            Else
                System.Diagnostics.Process.Start(APP_Path() & "PDFTransManual.pdf")
            End If
        Catch ex As Exception
            MsgBox(" The Help File was not found")
        End Try
    End Sub


    Private Sub RibbonCompare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonCompare.Click
        btnDeletions.Visible = True
        btnAdditions.Visible = True
        btnCommon.Visible = True
        SliderSensitivity.Visible = False
        SliderTransparency.Visible = False
        btnDifferences.Visible = True
        barStatus.Refresh()
        OverlayPanel.Text = "Overlay Pages"
        OverlayPanel.TitleText = "Overlay Pages"
    End Sub

    Private Sub RibbonPDFTrans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonPDFTrans.Click
        btnDeletions.Visible = False
        btnAdditions.Visible = False
        btnCommon.Visible = False
        btnDifferences.Visible = False
        barStatus.Refresh()
        OverlayPanel.Text = "Converted Pages"
        OverlayPanel.TitleText = "Converted Pages"
    End Sub

    Public Sub gdSaveToPDF(ByVal InFileName As String, ByVal outFileName As String, ByVal IsColor As Boolean, Optional ByVal inPage As Integer = 0)

        Try
            Dim myPage As Integer
            Dim myTitle As String
            Dim outFile As String = ""
            Dim m_saveImage As Integer
            Dim startpage As Integer
            Dim endpage As Integer

            myTitle = IO.Path.GetFileNameWithoutExtension(InFileName)

            If InFileName = "" Then
                PopupBox("File Open Warning", "Please open a file first")
                Exit Sub
            End If
            Dim gdPDF As New GdPicture14.GdPicturePDF
            Dim GdPictureImage As New GdPicture14.GdPictureImaging
            gdPDF.NewPDF(batchProps.SaveAsPDFA)

            gdPDF.SetAuthor("DocuFi-ComPara")
            gdPDF.SetTitle(myTitle)
            gdPDF.SetProducer("ComPara")

            Dim pdfString As String

            Dim GDPicturePDFSrc As New GdPicture14.GdPicturePDF
            outFile = outFileName 'IO.Path.ChangeExtension(InFileName, ".pdf")

            If inPage = 0 Then 'all pages
                startpage = 1
                endpage = mvarSourcePageCount
            Else
                startpage = inPage
                endpage = inPage
                OutputFilePath = outFile
            End If

            ProgressBarX1.Visible = True
            ProgressBarX1.Maximum = endpage


            For myPage = startpage To endpage
                'copy the page from the original source document

                ProgressBarX1.Value = myPage
                If myPage = startpage Then
                    'm_CurrentImage = GdPictureImage.TiffCreateMultiPageFromFile(InFileName)
                    'If GdPictureImage.GetStat <> GDPicture14.GdPictureStatus.OK Then
                    '    WriteLog("Warning processing tif in SavetoPDF" & InFileName)
                    'End If
                    gdPDF.NewPDF()
                End If
                If gSourceisPDF Then
                    gdPDF.ClonePage(BasePDF, myPage)
                Else
                    GdPictureImage.TiffSelectPage(mvarSourceGDHandle, myPage)
                    m_saveImage = GdPictureImage.CreateClonedGdPictureImage(mvarSourceGDHandle)

                    If m_saveImage <> 0 Then
                        If DocuFiSession.Authorized = False Then
                            GdPictureImage.DrawText(m_saveImage, "ComPara Trial", 50, 100, 30, FontStyle.Bold, Color.Black, "Arial", True)
                        End If

                        If IsColor = False Then
                            GdPictureImage.ConvertTo1BppAT(m_saveImage)
                        End If
                        pdfString = gdPDF.AddImageFromGdPictureImage(m_saveImage, False, True)
                        'GdPictureImage.CopyToClipboard(m_saveImage)
                        ' GdPictureImage.SaveAsPDF(m_saveImage, outFile, False, "test", "test", "test", "test", "test")
                    End If

                    GdPictureImage.ReleaseGdPictureImage(m_saveImage)
                End If
            Next


            ProgressBarX1.Visible = False
            ' gdPDF.CloseDocument()

            WriteLog("  gdtiftoPDF Rendered to PDF")
            If batchProps.PDFDisallowCutCopy Or batchProps.PDFDisallowModify Or batchProps.PDFDisallowPrint Or batchProps.PDFEncrypt Or batchProps.PDFLinearize Then
                Dim blnCanPrint As Boolean = Not batchProps.PDFDisallowPrint
                Dim blnCancopy As Boolean = Not batchProps.PDFDisallowCutCopy
                Dim blnEncryptt As Boolean = Not batchProps.PDFEncrypt
                Dim blnModify As Boolean = Not batchProps.PDFDisallowModify
                ' Dim blnCanPrint As Boolean = Not BatchProps.PDFDisallowPrint
                Dim pdfEncyrptType As GdPicture14.PdfEncryption = GdPicture14.PdfEncryption.PdfEncryptionNone
                If batchProps.PDFEncrypt Then
                    If batchProps.PDFEncryptType = 3 Then
                        pdfEncyrptType = GdPicture14.PdfEncryption.PdfEncryption128BitAES
                    ElseIf batchProps.PDFEncryptType = 2 Then
                        pdfEncyrptType = GdPicture14.PdfEncryption.PdfEncryption128BitRC4
                    ElseIf batchProps.PDFEncryptType = 4 Then
                        pdfEncyrptType = GdPicture14.PdfEncryption.PdfEncryption256BitAES
                    ElseIf batchProps.PDFEncryptType = 1 Then
                        pdfEncyrptType = GdPicture14.PdfEncryption.PdfEncryption40BitRC4
                    Else
                        pdfEncyrptType = GdPicture14.PdfEncryption.PdfEncryption40BitRC4
                    End If
                End If
                gdPDF.SaveToFile(outFile, pdfEncyrptType, batchProps.PDFUserPassword, batchProps.PDFPassword,
                                blnCanPrint, blnCancopy, blnModify, True, True, blnCancopy, False, blnCanPrint)
            Else
                gdPDF.EnableCompression(True)

                gdPDF.SaveToFile(outFile, True)
            End If
            GdViewerOverlay.DisplayFromFile(outFile)
            thumbOverlay.LoadFromGdViewer(GdViewerOverlay)
            OverlayExtents = gdPDF.GetPageWidth.ToString & " by " & gdPDF.GetPageHeight.ToString
            mvarOverlayPageCount = mvarSourcePageCount
            mvarOverlayFile = outFile
            WriteLog("  gdtiftoPDF Saved to " & IO.Path.GetFileName(outFile))
            gdPDF.CloseDocument()
            gdPDF = Nothing

        Catch ex As Exception
            WriteLog("gdtifToPDF Error converting to PDF " & Err.Description)
        End Try
    End Sub

    Private Sub btnSavePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavePage.Click
        Dim blnSuccess As Boolean = False


        Dim res As System.Windows.Forms.DialogResult
        saveFileDialog1.Filter = "PDF Files|*.pdf|JPEG Files|*.jpg|TIF Files|*.tif"
        res = saveFileDialog1.ShowDialog()

        If res = Windows.Forms.DialogResult.Cancel Then Exit Sub

        'if it's not saving to pdf, then save the file to tif or jpg
        If IO.Path.GetExtension(saveFileDialog1.FileName).ToLower <> ".pdf" Then

            If DocuFiSession.ExtractTIF Then
                If GDPicRenderTIF(mvarFileName, saveFileDialog1.FileName, chkIsColor.Checked, mvarSourcePage) = False Then
                    PopupBox("File Conversion Failed", "Failed to create " & IO.Path.GetFileName(OutputFilePath))
                    Exit Sub
                Else
                    blnSuccess = True
                End If
            Else
                If GDPicRenderTIF(mvarFileName, saveFileDialog1.FileName, chkIsColor.Checked, mvarSourcePage) = False Then
                    PopupBox("File Conversion Failed", "Failed to create " & IO.Path.GetFileName(OutputFilePath))
                    Exit Sub
                Else
                    blnSuccess = True
                End If

            End If
        Else
            gdSaveToPDF(mvarFileName, saveFileDialog1.FileName, chkIsColor.Checked, mvarSourcePage)
        End If

        If IO.File.Exists(OutputFilePath) Then
            PopupBox("File Conversion Completed", "Successfully created " & IO.Path.GetFileName(OutputFilePath))
            'if the base pdf is greater than 1 page, then display the thumbnail
            If BasePDF.GetPageCount > 0 Then OverlayPanel.Visible = True
            OverlayPanel.TitleText = "Converted Pages"
            lblBase2.Text = "  Source:  " & IO.Path.GetFileName(mvarFileName) & "    Saved As:  " & IO.Path.GetFileName(mvarOverlayFile) & " "

        End If

    End Sub


    Private Sub btnReplacePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplacePage.Click, btnReplPage.Click

        Try
            If Not DocuFiSession.Authorized Then
                PopupBox("License Notification       ", "Replacing Pages is not supported in the trial version")
                Application.DoEvents()
                System.Threading.Thread.Sleep(3000)
                Exit Sub
            End If
            If mvarActiveChanges Then
                MsgBox("There are active modifications, please approve before saving")
                Exit Sub
            End If

            Dim outimage As String
            Dim dlg As OpenFileDialog = New OpenFileDialog()
            With dlg
                .Filter = "PDF/TIF Files|*.tif;*.pdf"
                .FilterIndex = 1
                .RestoreDirectory = True
                .Title = "Insert to Current Page"
            End With
            Dim res As DialogResult = dlg.ShowDialog()

            If res = Windows.Forms.DialogResult.Cancel Then Exit Sub

            Dim currentPage As Integer = 0
            Dim PageCount As Integer
            Dim newID As Integer
            Dim i As Integer
            Dim locInsertIsPDf As Boolean = False

            outimage = dlg.FileName

            Dim docFormat As String = IO.Path.GetExtension(mvarCurrentFile)
            If docFormat.ToLower = ".pdf" Then
                'We use the GdPicturePDF class to handle the document
                If OverlayPDF.LoadFromFile(outimage, False) = GdPictureStatus.OK Then
                    mvarOverlayDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF
                    'GdViewerBase.DisplayFromGdPicturePDF(BasePDF)
                    'TiffID = OverlayPDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                End If

            Else
                'We use the GdPictureImaging class to handle the document as an editable multipage tiff image
                'myGD.ReleaseGdPictureImage(TiffID)
                'TiffID = myGD.TiffCreateMultiPageFromFile(outimage)
                If TiffID <> 0 Then
                    mvarOverlayDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatTIFF
                    ' GdViewerBase.DisplayFromGdPictureImage(TiffID)
                End If
            End If

            currentPage = GdViewerBase.CurrentPage
            Dim NewPDFPage As Integer
            If gSourceisPDF Then
                PageCount = GdViewerBase.PageCount

                'replace the pdf page with the new image
                Dim ImportedPageGDPicture As New GdPictureImaging

                If IO.Path.GetExtension(dlg.FileName).ToLower = ".pdf" Then
                    locInsertIsPDf = True
                    NewPDFPage = OverlayPDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                Else
                    NewPDFPage = myGD.CreateGdPictureImageFromFile(dlg.FileName)
                End If

                BasePDF.SelectPage(GdViewerBase.CurrentPage)
                BasePDF.AddImageFromGdPictureImage(NewPDFPage, True, True)

                'swap the new page at the end with the current page
                BasePDF.SwapPages(currentPage, BasePDF.GetPageCount)

                'now delete the last page
                BasePDF.DeletePage(BasePDF.GetPageCount)
                GdViewerBase.DisplayFromGdPicturePDF(BasePDF)
                'regen the thumbs
                thumbBase.ClearAllItems()
                thumbBase.LoadFromGdViewer(GdViewerBase)

            Else
                If IO.Path.GetExtension(dlg.FileName).ToLower = ".pdf" Then
                    locInsertIsPDf = True
                    NewPDFPage = OverlayPDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                Else
                    NewPDFPage = myGD.CreateGdPictureImageFromFile(dlg.FileName)
                End If
                'sourceID = myGD.CreateGdPictureImageFromFile(mvarCurrentFile)

                PageCount = myGD.TiffGetPageCount(mvarSourceGDHandle)
                If PageCount = 0 Then
                    MsgBox("Error opening this document", vbExclamation)
                    Exit Sub
                End If
                Dim ClonePage As Integer
                For i = 1 To PageCount
                    myGD.TiffSelectPage(mvarSourceGDHandle, i)
                    myGD.ReleaseGdPictureImage(ClonePage)
                    ClonePage = myGD.CreateClonedGdPictureImage(mvarSourceGDHandle)

                    If i = currentPage Then
                        If i = 1 Then
                            newID = myGD.TiffCreateMultiPageFromGdPictureImage(NewPDFPage)
                        Else
                            myGD.TiffInsertPageFromGdPictureImage(newID, 999, NewPDFPage)
                        End If

                    Else
                        If i = 1 Then
                            newID = myGD.TiffCreateMultiPageFromGdPictureImage(ClonePage)
                        Else
                            myGD.TiffInsertPageFromGdPictureImage(newID, 999, ClonePage)
                            ' Call myGD.TiffAddToMultiPageFile(newID, clonePage)
                        End If

                    End If

                Next

                myGD.TiffCloseMultiPageFile(newID)

                ' myGD.TiffSaveMultiPageToFile(newID, mvarCurrentFile, GDPicture14.TiffCompression.TiffCompressionCCITT4)
                myGD.ReleaseGdPictureImage(mvarSourceGDHandle)
                mvarSourceGDHandle = newID
                GdViewerBase.CloseDocument()
                thumbBase.ClearAllItems()
                GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
                GdViewerBase.DisplayPage(currentPage)
                thumbBase.LoadFromGdViewer(GdViewerBase)
                ' myGD.TiffCloseMultiPageFile(sourceID)
                GdViewerBase.Redraw()
            End If

            GdViewerBase.DisplayPage(currentPage)
        Catch ex As Exception
            WriteLog("Error replacing page " & Err.Description)
        End Try
    End Sub


#Region "Enhance"
    Private Sub enhStraightenPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles enhStraightenPage.Click
        If mvarActiveChanges Then
            MsgBox("There are active modifications, please approve before saving or save as a working draft copy")
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        Me.GDDeskew()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub enhRemoveNoise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles enhRemoveNoise.Click

        Me.Cursor = Cursors.WaitCursor
        'Me.GDThicken(sldSpeckles.Value)
        Me.GDDespeckle(sldSpeckles.Value)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub enhCropImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles enhCropImage.Click
        Me.Cursor = Cursors.WaitCursor
        Me.GDCrop()
        Me.Cursor = Cursors.Default
    End Sub

    Public Sub GDDespeckle(ByVal inSize As Integer)
        Dim pageCount As Integer

        If mvarActiveChanges Then
            MsgBox("There are active changes that must be approved before proceeding")
            Exit Sub
        End If

        mvarNoiseSize = sldSpeckles.Value
        pageCount = GdViewerBase.PageCount

        If pageCount = 0 Then pageCount = 1

        If mvarSourceGDHandle = 0 Then Exit Sub
        Dim activePage As Integer = GdViewerBase.CurrentPage
        myGD.TiffSelectPage(mvarSourceGDHandle, activePage)
        myGD.FxBitonalDespeckleMore(mvarSourceGDHandle, False)

        myGD.FxBitonalFillHolesHV(mvarSourceGDHandle)

        GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
    End Sub
    Public Sub GDThreshold()
        Dim page As Integer
        Dim pageCount As Integer
        If mvarActiveChanges Then
            MsgBox("There are active changes that must be approved before proceeding")
            Exit Sub
        End If

        mvarNoiseSize = sldSpeckles.Value
        myGD.ReleaseGdPictureImage(sourceID)
        sourceID = myGD.CreateGdPictureImageFromFile(mvarCurrentFile)

        ' myGD.ConvertTo1BppSauvola(sourceID, sldSensitivity.Value / 100.0F, sldThreshold.Value, 3)
        GdViewerBase.DisplayFromGdPictureImage(sourceID)
        Exit Sub
        If IO.File.Exists(mvarTempDoc) Then IO.File.Delete(mvarTempDoc)
        mvarSourceGDHandle = myGD.CreateGdPictureImageFromFile(mvarFileName)
        If mvarSourceGDHandle = 0 Then Exit Sub
        If pageCount = 0 Then pageCount = 1
        For page = 1 To pageCount
            myGD.TiffSelectPage(mvarSourceGDHandle, page)
            myGD.FxBlackNWhiteT(mvarSourceGDHandle, 240)
        Next
        myGD.SaveAsTIFF(mvarSourceGDHandle, mvarTempDoc, GdPicture14.TiffCompression.TiffCompressionCCITT4)
        IO.File.Delete(mvarSourceGDHandle)
        IO.File.Copy(mvarTempDoc, mvarFileName)

    End Sub

    Public Sub GDThicken(ByVal inSize As Integer)

        If mvarActiveChanges Then
            MsgBox("There are active changes that must be approved before proceeding")
            Exit Sub
        End If
        If mvarSourceGDHandle = 0 Then Exit Sub
        Dim activePage As Integer = GdViewerBase.CurrentPage
        'For page = 1 To pageCount
        myGD.TiffSelectPage(mvarSourceGDHandle, activePage)

        myGD.FxBitonalDilate8(mvarSourceGDHandle)
        GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)

    End Sub
    Public Sub GDDeskew()
        Dim pageCount As Integer

        If mvarActiveChanges Then
            MsgBox("There are active changes that must be approved before proceeding")
            Exit Sub
        End If

        pageCount = GdViewerBase.PageCount
        If mvarSourceGDHandle = 0 Then Exit Sub
        Dim activePage As Integer = GdViewerBase.CurrentPage
        'For page = 1 To pageCount
        myGD.TiffSelectPage(mvarSourceGDHandle, activePage)
        myGD.AutoDeskew(mvarSourceGDHandle)

        ' Next

        GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
    End Sub
    Public Sub GDRotate()

        Dim pageCount As Integer

        If mvarActiveChanges Then
            MsgBox("There are active changes that must be approved before proceeding")
            Exit Sub
        End If

        Dim activePage As Integer = GdViewerBase.CurrentPage
        pageCount = GdViewerBase.PageCount
        If mvarSourceGDHandle = 0 Then Exit Sub

        myGD.TiffSelectPage(mvarSourceGDHandle, activePage)
        myGD.Rotate(mvarSourceGDHandle, RotateFlipType.Rotate90FlipNone)

        GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
        'ThumbnailEx1.LoadFromGdViewer(GdViewer1)

        ' ThumbnailEx1.LoadFromGdPictureImage(myGDHandle)

        'ThumbnailEx1.Refresh()
    End Sub
    Public Sub GDsmooth()

        If mvarActiveChanges Then
            MsgBox("There are active changes that must be approved before proceeding")
            Exit Sub
        End If

        If mvarSourceGDHandle = 0 Then Exit Sub

        myGD.TiffSelectPage(mvarSourceGDHandle, GdViewerBase.CurrentPage)
        myGD.FxSmooth(mvarSourceGDHandle)
        myGD.FxDilate(mvarSourceGDHandle) 'myGD.FxErode(myGDHandle)
        myGD.SetContrast(mvarSourceGDHandle, 200)
        myGD.ConvertTo1Bpp(mvarSourceGDHandle)
        myGD.FxErode(mvarSourceGDHandle)
        myGD.SetContrast(mvarSourceGDHandle, 200)
        myGD.ConvertTo1Bpp(mvarSourceGDHandle)
        GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
    End Sub
    Public Sub GDCrop()

        If mvarActiveChanges Then
            MsgBox("There are active changes that must be approved before proceeding")
            Exit Sub
        End If

        If mvarSourceGDHandle = 0 Then Exit Sub

        myGD.TiffSelectPage(mvarSourceGDHandle, GdViewerBase.CurrentPage)
        myGD.CropBorders(mvarSourceGDHandle, GdPicture14.ImagingContext.ContextDocument)


        GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
    End Sub

    Public Sub AutoRotate()

    End Sub

#End Region

    Private Sub btnComParaAppStore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnComParaAppStore.Click
        '  System.Diagnostics.Process.Start("http://apps.exchange.autodesk.com/Detail/Index?id=appstore.exchange.autodesk.com:comparadrawingsetcompareconvertandreview:en")
        System.Diagnostics.Process.Start("http://www.docufi.com/products/edit-compare-convert-pdf-tif/compara-drawing-compare-and-review")
    End Sub

    Private Sub btnReVisaAppStore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReVisaAppStore.Click
        System.Diagnostics.Process.Start("http://www.docufi.com/products/edit-compare-convert-pdf-tif/revisa-pdf-tif-edit-compare")
    End Sub



    Private Sub ButtonItem70_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonItem70.Click
        System.Diagnostics.Process.Start("http://www.docufi.com/products/edit-compare-convert-pdf-tif/pdftrans-annual-license-renewal")

    End Sub



    Private Sub BtnInsertBefore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnInsertBefore.Click

        Try
            If Not DocuFiSession.Authorized Then
                PopupBox("License Notification       ", "Inserting Pages is not supported in the trial version")
                Application.DoEvents()
                System.Threading.Thread.Sleep(3000)
                Exit Sub
            End If
            If mvarActiveChanges Then
                MsgBox("There are active modifications, please approve before saving")
                Exit Sub
            End If

            Dim outimage As String
            Dim dlg As OpenFileDialog = New OpenFileDialog()
            With dlg
                .Filter = "PDF/TIF Files|*.tif;*.pdf"
                .FilterIndex = 1
                .RestoreDirectory = True
                .Title = "Insert to Current Page"
            End With
            Dim res As DialogResult = dlg.ShowDialog()

            If res = Windows.Forms.DialogResult.Cancel Then Exit Sub

            Dim currentPage As Integer = 0
            Dim PageCount As Integer
            Dim newID As Integer
            Dim i As Integer
            Dim locInsertIsPDf As Boolean = False

            outimage = dlg.FileName

            Dim docFormat As String = IO.Path.GetExtension(mvarCurrentFile)
            If docFormat.ToLower = ".pdf" Then
                'We use the GdPicturePDF class to handle the document
                If OverlayPDF.LoadFromFile(outimage, False) = GdPictureStatus.OK Then
                    mvarOverlayDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF
                    'GdViewerBase.DisplayFromGdPicturePDF(BasePDF)

                End If

            Else
                'We use the GdPictureImaging class to handle the document as an editable multipage tiff image
                myGD.ReleaseGdPictureImage(TiffID)
                TiffID = myGD.TiffCreateMultiPageFromFile(outimage)
                If TiffID <> 0 Then
                    mvarOverlayDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatTIFF
                    ' GdViewerBase.DisplayFromGdPictureImage(TiffID)
                End If
            End If

            currentPage = GdViewerBase.CurrentPage
            If gSourceisPDF Then
                PageCount = GdViewerBase.PageCount

                'replace the pdf page with the new image
                Dim ImportedPageGDPicture As New GdPictureImaging
                Dim NewPDFPage As Integer

                If IO.Path.GetExtension(dlg.FileName).ToLower = ".pdf" Then
                    locInsertIsPDf = True
                    NewPDFPage = OverlayPDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                    'NewPDFPage = ImportedPageGDPicture.CreateGdPictureImageFromFile(dlg.FileName)

                Else
                    NewPDFPage = myGD.CreateGdPictureImageFromFile(dlg.FileName)
                End If

                Dim sourcePage As Integer = GdViewerBase.CurrentPage
                BasePDF.SelectPage(GdViewerBase.CurrentPage)
                'add the newly created image to the last page then move it
                BasePDF.AddImageFromGdPictureImage(NewPDFPage, True, True)

                BasePDF.MovePage(BasePDF.GetPageCount, sourcePage)

                'now delete the last page
                GdViewerBase.DisplayFromGdPicturePDF(BasePDF)
                'regen the thumbs
                thumbBase.ClearAllItems()
                thumbBase.LoadFromGdViewer(GdViewerBase)

            Else

                Dim clonePage As Integer 'handle for cloned page

                PageCount = myGD.TiffGetPageCount(mvarSourceGDHandle)
                If myGD.TiffIsMultiPage(mvarSourceGDHandle) = False Then
                    PageCount = 1
                End If

                If PageCount = 0 Then
                    MsgBox("Error opening this document", vbExclamation)
                    Exit Sub
                End If

                For i = 1 To PageCount
                    myGD.TiffSelectPage(mvarSourceGDHandle, i)
                    myGD.ReleaseGdPictureImage(clonePage)
                    clonePage = myGD.CreateClonedGdPictureImage(mvarSourceGDHandle)

                    If i = currentPage Then
                        If i = 1 Then
                            newID = myGD.TiffCreateMultiPageFromGdPictureImage(TiffID)
                            myGD.TiffInsertPageFromGdPictureImage(newID, 9999, clonePage)
                        Else
                            Call myGD.TiffInsertPageFromGdPictureImage(newID, currentPage, TiffID)
                            'now add the former current page to the end
                            myGD.TiffInsertPageFromGdPictureImage(newID, 9999, clonePage)
                        End If

                    Else
                        If i = 1 Then
                            newID = myGD.TiffCreateMultiPageFromGdPictureImage(clonePage)
                        Else
                            Call myGD.TiffInsertPageFromGdPictureImage(newID, 9999, clonePage)
                        End If

                    End If

                Next

                myGD.TiffCloseMultiPageFile(newID)

                ' myGD.TiffSaveMultiPageToFile(newID, mvarCurrentFile, GDPicture14.TiffCompression.TiffCompressionCCITT4)
                myGD.ReleaseGdPictureImage(mvarSourceGDHandle)
                mvarSourceGDHandle = newID
                GdViewerBase.CloseDocument()
                thumbBase.ClearAllItems()
                GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
                GdViewerBase.DisplayPage(currentPage)
                thumbBase.LoadFromGdViewer(GdViewerBase)
                GdViewerBase.Redraw()

            End If
            mvarSourcePageCount = GdViewerBase.PageCount()
            GdViewerBase.DisplayPage(currentPage)
        Catch ex As Exception
            WriteLog("Error replacing page " & Err.Description)
        End Try
    End Sub

    Private Sub btnInsertAfter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertAfter.Click
        Try
            If Not DocuFiSession.Authorized Then
                PopupBox("License Notification       ", "Inserting Pages is not supported in the trial version")
                Application.DoEvents()
                System.Threading.Thread.Sleep(3000)
                Exit Sub
            End If
            If mvarActiveChanges Then
                MsgBox("There are active modifications, please approve before saving")
                Exit Sub
            End If

            Dim outimage As String
            Dim dlg As OpenFileDialog = New OpenFileDialog()
            With dlg
                .Filter = "PDF/TIF Files|*.tif;*.pdf"
                .FilterIndex = 1
                .RestoreDirectory = True
                .Title = "Insert to Current Page"
            End With
            Dim res As DialogResult = dlg.ShowDialog()

            If res = Windows.Forms.DialogResult.Cancel Then Exit Sub

            Dim currentPage As Integer = 0
            Dim PageCount As Integer
            Dim newID As Integer
            Dim i As Integer
            Dim locInsertIsPDf As Boolean = False

            outimage = dlg.FileName

            Dim docFormat As String = IO.Path.GetExtension(mvarCurrentFile)
            If docFormat.ToLower = ".pdf" Then
                'We use the GdPicturePDF class to handle the document
                If OverlayPDF.LoadFromFile(outimage, False) = GdPictureStatus.OK Then
                    mvarOverlayDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF
                    'GdViewerBase.DisplayFromGdPicturePDF(BasePDF)

                End If

            Else
                'We use the GdPictureImaging class to handle the document as an editable multipage tiff image
                myGD.ReleaseGdPictureImage(TiffID)
                TiffID = myGD.TiffCreateMultiPageFromFile(outimage)
                If TiffID <> 0 Then
                    mvarOverlayDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatTIFF
                    ' GdViewerBase.DisplayFromGdPictureImage(TiffID)
                End If
            End If

            currentPage = GdViewerBase.CurrentPage
            If gSourceisPDF Then
                PageCount = GdViewerBase.PageCount

                'replace the pdf page with the new image
                Dim ImportedPageGDPicture As New GdPictureImaging
                Dim NewPDFPage As Integer

                If IO.Path.GetExtension(dlg.FileName).ToLower = ".pdf" Then
                    locInsertIsPDf = True
                    OverlayPDF.LoadFromFile(dlg.FileName, True)
                    NewPDFPage = OverlayPDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
                    'NewPDFPage = ImportedPageGDPicture.CreateGdPictureImageFromFile(dlg.FileName)

                Else
                    NewPDFPage = myGD.CreateGdPictureImageFromFile(dlg.FileName)
                End If
                Dim sourcePage As Integer = GdViewerBase.CurrentPage
                BasePDF.SelectPage(GdViewerBase.CurrentPage)
                BasePDF.AddImageFromGdPictureImage(NewPDFPage, True, True)

                BasePDF.MovePage(BasePDF.GetPageCount, sourcePage + 1)

                'now delete the last page
                GdViewerBase.DisplayFromGdPicturePDF(BasePDF)
                'regen the thumbs
                thumbBase.ClearAllItems()
                thumbBase.LoadFromGdViewer(GdViewerBase)

            Else

                Dim clonePage As Integer 'handle for cloned page

                PageCount = myGD.TiffGetPageCount(mvarSourceGDHandle)
                If myGD.TiffIsMultiPage(mvarSourceGDHandle) = False Then
                    PageCount = 1
                End If

                If PageCount = 0 Then
                    MsgBox("Error opening this document", vbExclamation)
                    Exit Sub
                End If

                myGD.ReleaseGdPictureImage(clonePage)
                clonePage = myGD.CreateClonedGdPictureImage(mvarSourceGDHandle)
                For i = 1 To PageCount
                    myGD.TiffSelectPage(mvarSourceGDHandle, i)


                    If i = currentPage Then
                        If i = 1 Then
                            newID = myGD.TiffCreateMultiPageFromGdPictureImage(clonePage)
                            myGD.TiffInsertPageFromGdPictureImage(newID, 9999, TiffID)
                        Else
                            myGD.TiffInsertPageFromGdPictureImage(newID, 9999, clonePage)
                            Call myGD.TiffInsertPageFromGdPictureImage(newID, 9999, TiffID)

                        End If

                    Else
                        If i = 1 Then
                            newID = myGD.TiffCreateMultiPageFromGdPictureImage(clonePage)
                        Else
                            myGD.TiffInsertPageFromGdPictureImage(newID, 9999, clonePage)
                        End If

                    End If

                Next

                myGD.TiffCloseMultiPageFile(newID)

                ' myGD.TiffSaveMultiPageToFile(newID, mvarCurrentFile, GDPicture14.TiffCompression.TiffCompressionCCITT4)
                myGD.ReleaseGdPictureImage(mvarSourceGDHandle)
                mvarSourceGDHandle = newID
                GdViewerBase.CloseDocument()
                thumbBase.ClearAllItems()
                GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
                GdViewerBase.DisplayPage(currentPage)
                thumbBase.LoadFromGdViewer(GdViewerBase)
                GdViewerBase.Redraw()


            End If
            mvarSourcePageCount = GdViewerBase.PageCount()
            GdViewerBase.DisplayPage(currentPage)
        Catch ex As Exception
            WriteLog("Error replacing page " & Err.Description)
        End Try
    End Sub

    Private Sub DeletePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelPage.Click
        Dim PageNo As Integer = GdViewerBase.CurrentPage

        If Not DocuFiSession.Authorized Then
            PopupBox("License Notification       ", "Deleting Pages is not supported in the trial version")
            Application.DoEvents()
            System.Threading.Thread.Sleep(3000)
            Exit Sub
        End If
        If mvarSourceDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF Then

            BasePDF.DeletePage(PageNo)
            thumbBase.RemoveItem(PageNo - 1)

            mvarSourcePageCount -= 1
            GdViewerBase.DisplayFromGdPicturePDF(BasePDF)
            thumbBase.LoadFromGdViewer(GdViewerBase)


        Else
            If mvarSourceDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatTIFF Then

                myGD.TiffDeletePage(mvarSourceGDHandle, PageNo)
                thumbBase.RemoveItem(PageNo - 1)
                mvarSourcePageCount -= 1
                GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
                thumbBase.LoadFromGdViewer(GdViewerBase)
            End If
        End If

    End Sub


    Private Sub btnRotatePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRotatePage.Click
        If mvarSourceDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatPDF Then

            BasePDF.RotatePage(90)

            GdViewerBase.DisplayFromGdPicturePDF(BasePDF)
        Else
            If mvarSourceDocumentFormat = GdPicture14.DocumentFormat.DocumentFormatTIFF Then
                myGD.Rotate(mvarSourceGDHandle, RotateFlipType.Rotate90FlipNone)
            End If
            GdViewerBase.DisplayFromGdPictureImage(mvarSourceGDHandle)
        End If

        GdViewerBase.DisplayPage(GdViewerBase.CurrentPage)
        thumbBase.LoadFromGdViewer(GdViewerBase)
        GdViewerBase.Redraw()
    End Sub

    Private Sub btnThreshold_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThreshold.Click

        If Not DocuFiSession.Authorized Then
            PopupBox("License Notification       ", "Thresholding is not supported in the trial version")
            Application.DoEvents()
            System.Threading.Thread.Sleep(3000)
            Exit Sub
        End If
        SliderTransparency.Visible = True
        SliderSensitivity.Visible = True
        grpSliderTools.Visible = True
        If gSourceisPDF And mvarSourceGDHandle = 0 Then

            mvarSourceGDHandle = BasePDF.RenderPageToGdPictureImage(mvarRenderDPI, True)
        End If
        Me.Cursor = Cursors.WaitCursor
        RegenConversion()
        Me.Cursor = Cursors.Default
        Me.Refresh()
    End Sub

    Private Sub SliderSensitivity_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SliderSensitivity.MouseMove
        lblBalloon.Visible = True
        lblBalloon.Text = "Sensitivity: " & SliderSensitivity.Value.ToString
    End Sub

    Private Sub SliderSensitivity_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SliderSensitivity.MouseUp
        SensitivityValue = SliderSensitivity.Value
        ThresholdValue = SliderTransparency.Value
        Me.Cursor = Cursors.WaitCursor
        RegenConversion()
        Me.Cursor = Cursors.Default
        lblBalloon.Visible = False
    End Sub

    Private Sub SliderTransparency_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SliderTransparency.MouseUp
        If GdViewerOverlay.Visible = False Then ' it is in overlay mode, so ok to proceed
            nudgeOverlay2()

        End If
    End Sub
    Private Sub btnSaveConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim res As System.Windows.Forms.DialogResult
        saveFileDialog1.Filter = "PDF Files|*.pdf|JPEG Files|*.jpg|TIF Files|*.tif"
        res = saveFileDialog1.ShowDialog()

        If res = Windows.Forms.DialogResult.Cancel Then Exit Sub

        If IO.Path.GetExtension(saveFileDialog1.FileName) = ".jpg" Then
            myGD.SaveAsJPEG(overlayID, saveFileDialog1.FileName)
        ElseIf IO.Path.GetExtension(saveFileDialog1.FileName) = ".tif" Then
            myGD.SaveAsTIFF(overlayID, saveFileDialog1.FileName, TiffCompression.TiffCompressionAUTO)
        Else
            gdSaveToPDF(mvarFileName, saveFileDialog1.FileName, chkIsColor.Checked)
        End If

    End Sub

    Private Sub RegenConversion()
        myGD.ReleaseGdPictureImage(overlayID)
        If GdViewerBase.IsRect Then
            Dim top, left, width, height As Single
            GdViewerBase.GetRectCoordinatesOnDocument(left, top, width, height)
            GdViewerBase.ZoomArea(left, top, width, height)
            overlayID = GdViewerBase.CopyRegionToGdPictureImage(left, top, width, height)

            '  myGD.CopyRegionToClipboard(mvarSourceGDHandle, left, top, width, height)
            '  myGD.CopyToClipboard(mvarSourceGDHandle)
            'GdViewerBase.CopyToClipboard()
            ' overlayID = myGD.CreateGdPictureImageFromClipboard()


        Else
            overlayID = myGD.CreateClonedGdPictureImage(mvarSourceGDHandle)

        End If

        myGD.ConvertTo1BppSauvola(overlayID, SliderSensitivity.Value / 100, SliderTransparency.Value, 3)
        GdViewerOverlay.DisplayFromGdPictureImage(overlayID)
    End Sub

    Private Sub btnOpenforConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenforConvert.Click

        RibbonPDFTrans.Select()
        buttonOpenConvert_Click(sender, e)

    End Sub


    Private Sub btnThresholdArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SliderTransparency.Visible = True
        SliderSensitivity.Visible = True
        GdViewerBase.MouseMode = ViewerMouseMode.MouseModeAreaSelection
        PopupBox("Select a rectangle", "Please select a rectangular region to threshold ")
        Application.DoEvents()
        grpSliderTools.Visible = True

    End Sub

    Private Sub btnCompareViewports_Click(sender As System.Object, e As System.EventArgs) Handles btnCompareViewports.Click
        GdViewerBase.MouseMode = ViewerMouseMode.MouseModeAreaSelection
        GdViewerOverlay.MouseMode = ViewerMouseMode.MouseModeAreaSelection
        PopupBox("Viewport comparing", "Please select a rectangular region to compare ")
        DocuFiSession.MeasureMode = 10
        GdViewerBase.SetRectBorderColor(Color.Red.ToArgb)
        GdViewerOverlay.SetRectBorderColor(Color.Red.ToArgb)
    End Sub




End Class


