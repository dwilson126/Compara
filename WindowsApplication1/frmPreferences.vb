Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class frmPreferences
    Inherits System.Windows.Forms.Form

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub frmPreferences_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        intTagFontSize.Value = mvarTagFontSize
        ColorPickerButton1.SelectedColor = mvarDrawColor
        ColorPickerBase.SelectedColor = mvarbaseColor
        ColorPickerOverllay3.SelectedColor = mvaroverlayColor

        sldSensitivity2.Value = SensitivityValue
        sldThreshold2.Value = ThresholdValue

        chkExtract.Checked = DocuFiSession.ExtractTIF
        chkInvert.Checked = DocuFiSession.InvertExtraction

        If mvarUnits = 1 Then
            rbInches.Checked = True
        ElseIf mvarUnits = 2 Then
            rbFeet.Checked = True
        ElseIf mvarUnits = 3 Then
            rbMeters.Checked = True
        ElseIf mvarUnits = 4 Then
            rbCent.Checked = True
        End If
    End Sub

    Private Sub frmAbout_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'UPGRADE_NOTE: Object frmAbout may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        Me.Dispose()
    End Sub

    Private Sub intTagFontSize_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles intTagFontSize.ValueChanged
        mvarTagFontSize = intTagFontSize.Value
    End Sub

    Private Sub ColorPickerButton1_SelectedColorChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorPickerButton1.SelectedColorChanged
        mvarDrawColor = ColorPickerButton1.SelectedColor
    End Sub

    Private Sub rbInches_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbInches.CheckedChanged
        If rbInches.Checked Then mvarUnits = 1

    End Sub

    Private Sub rbFeet_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFeet.CheckedChanged
        If rbFeet.Checked Then mvarunits = 2
    End Sub

    Private Sub rbMeters_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMeters.CheckedChanged
        If rbMeters.Checked Then mvarunits = 3
    End Sub

    Private Sub rbCent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCent.CheckedChanged
        If rbCent.Checked Then mvarunits = 4
    End Sub

    Private Sub ColorPickerBase_SelectedColorChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorPickerBase.SelectedColorChanged

        mvarBaseColor = ColorPickerBase.SelectedColor
    End Sub

    Private Sub ColorPickerOverllay3_SelectedColorChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorPickerOverllay3.SelectedColorChanged
        mvarOverlayColor = ColorPickerOverllay3.SelectedColor
    End Sub

    Private Sub rbAdaptive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbAdaptive.CheckedChanged
        DocuFiSession.threshmode = 1
    End Sub

    Private Sub rbFixed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFixed.CheckedChanged
        DocuFiSession.threshmode = 2
    End Sub

    Private Sub sldThreshold2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sldThreshold2.Scroll
        ThresholdValue = sldThreshold2.Value
        lblThreshold.Text = "Threshold: " & ThresholdValue.ToString
    End Sub

    Private Sub sldSensitivity2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sldSensitivity2.Scroll
        SensitivityValue = sldSensitivity2.Value
        lblSensitivity.Text = "Sensitivity: " & SensitivityValue.ToString
    End Sub

    Private Sub chkExtract_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkExtract.CheckedChanged
        DocuFiSession.ExtractTif = chkExtract.Checked
    End Sub

    Private Sub chkInvert_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkInvert.CheckedChanged
        DocuFiSession.InvertExtraction = chkInvert.Checked
    End Sub
End Class