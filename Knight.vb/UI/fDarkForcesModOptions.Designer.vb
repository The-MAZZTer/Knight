Namespace MZZT.Knight.Games
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fDarkForcesModOptions
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
            Dim lDFBRIEF As System.Windows.Forms.Label
            Dim lFTEXTCRA As System.Windows.Forms.Label
            Dim lOthers As System.Windows.Forms.Label
            Me.bOK = New System.Windows.Forms.Button
            Me.bCancel = New System.Windows.Forms.Button
            Me.bApply = New System.Windows.Forms.Button
            Me.cbDFBRIEF = New System.Windows.Forms.ComboBox
            Me.cbFTEXTCRA = New System.Windows.Forms.ComboBox
            Me.clbOthers = New System.Windows.Forms.CheckedListBox
            Me.ec = New MZZT.EventControl
            lDFBRIEF = New System.Windows.Forms.Label
            lFTEXTCRA = New System.Windows.Forms.Label
            lOthers = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'lDFBRIEF
            '
            lDFBRIEF.AutoSize = True
            lDFBRIEF.Location = New System.Drawing.Point(12, 15)
            lDFBRIEF.Name = "lDFBRIEF"
            lDFBRIEF.Size = New System.Drawing.Size(68, 13)
            lDFBRIEF.TabIndex = 0
            lDFBRIEF.Text = "Briefing LFD:"
            '
            'lFTEXTCRA
            '
            lFTEXTCRA.AutoSize = True
            lFTEXTCRA.Location = New System.Drawing.Point(12, 42)
            lFTEXTCRA.Name = "lFTEXTCRA"
            lFTEXTCRA.Size = New System.Drawing.Size(82, 13)
            lFTEXTCRA.TabIndex = 2
            lFTEXTCRA.Text = "Text crawl LFD:"
            '
            'lOthers
            '
            lOthers.AutoSize = True
            lOthers.Location = New System.Drawing.Point(12, 66)
            lOthers.Name = "lOthers"
            lOthers.Size = New System.Drawing.Size(83, 13)
            lOthers.TabIndex = 4
            lOthers.Text = "Other used files:"
            '
            'bOK
            '
            Me.bOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bOK.Image = Global.My.Resources.Resources.silk_tick
            Me.bOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bOK.Location = New System.Drawing.Point(45, 187)
            Me.bOK.Name = "bOK"
            Me.bOK.Size = New System.Drawing.Size(75, 24)
            Me.bOK.TabIndex = 6
            Me.bOK.Text = "OK"
            Me.bOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bOK.UseVisualStyleBackColor = True
            '
            'bCancel
            '
            Me.bCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bCancel.Image = Global.My.Resources.Resources.silk_cross
            Me.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bCancel.Location = New System.Drawing.Point(126, 187)
            Me.bCancel.Name = "bCancel"
            Me.bCancel.Size = New System.Drawing.Size(75, 24)
            Me.bCancel.TabIndex = 7
            Me.bCancel.Text = "Close"
            Me.bCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bCancel.UseVisualStyleBackColor = True
            '
            'bApply
            '
            Me.bApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bApply.Image = Global.My.Resources.Resources.silk_disk
            Me.bApply.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bApply.Location = New System.Drawing.Point(207, 187)
            Me.bApply.Name = "bApply"
            Me.bApply.Size = New System.Drawing.Size(75, 24)
            Me.bApply.TabIndex = 8
            Me.bApply.Text = "&Apply"
            Me.bApply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bApply.UseVisualStyleBackColor = True
            '
            'cbDFBRIEF
            '
            Me.cbDFBRIEF.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cbDFBRIEF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbDFBRIEF.FormattingEnabled = True
            Me.cbDFBRIEF.Location = New System.Drawing.Point(101, 12)
            Me.cbDFBRIEF.Name = "cbDFBRIEF"
            Me.cbDFBRIEF.Size = New System.Drawing.Size(181, 21)
            Me.cbDFBRIEF.Sorted = True
            Me.cbDFBRIEF.TabIndex = 1
            '
            'cbFTEXTCRA
            '
            Me.cbFTEXTCRA.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cbFTEXTCRA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbFTEXTCRA.FormattingEnabled = True
            Me.cbFTEXTCRA.Location = New System.Drawing.Point(101, 39)
            Me.cbFTEXTCRA.Name = "cbFTEXTCRA"
            Me.cbFTEXTCRA.Size = New System.Drawing.Size(181, 21)
            Me.cbFTEXTCRA.Sorted = True
            Me.cbFTEXTCRA.TabIndex = 3
            '
            'clbOthers
            '
            Me.clbOthers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.clbOthers.CheckOnClick = True
            Me.clbOthers.FormattingEnabled = True
            Me.clbOthers.IntegralHeight = False
            Me.clbOthers.Location = New System.Drawing.Point(101, 66)
            Me.clbOthers.Name = "clbOthers"
            Me.clbOthers.Size = New System.Drawing.Size(181, 115)
            Me.clbOthers.Sorted = True
            Me.clbOthers.TabIndex = 5
            '
            'fDarkForcesModOptions
            '
            Me.AcceptButton = Me.bOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.bCancel
            Me.ClientSize = New System.Drawing.Size(294, 223)
            Me.Controls.Add(Me.clbOthers)
            Me.Controls.Add(lOthers)
            Me.Controls.Add(lFTEXTCRA)
            Me.Controls.Add(lDFBRIEF)
            Me.Controls.Add(Me.cbFTEXTCRA)
            Me.Controls.Add(Me.cbDFBRIEF)
            Me.Controls.Add(Me.bOK)
            Me.Controls.Add(Me.bCancel)
            Me.Controls.Add(Me.bApply)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "fDarkForcesModOptions"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Mod Options - Knight"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents bOK As System.Windows.Forms.Button
        Friend WithEvents bCancel As System.Windows.Forms.Button
        Friend WithEvents bApply As System.Windows.Forms.Button
        Friend WithEvents cbDFBRIEF As System.Windows.Forms.ComboBox
        Friend WithEvents cbFTEXTCRA As System.Windows.Forms.ComboBox
        Friend WithEvents clbOthers As System.Windows.Forms.CheckedListBox
        Friend WithEvents ec As MZZT.EventControl
    End Class
End Namespace
