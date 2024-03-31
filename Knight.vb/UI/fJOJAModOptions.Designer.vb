Namespace MZZT.Knight.Games
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fJOJAModOptions
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
            Me.bOK = New System.Windows.Forms.Button
            Me.bCancel = New System.Windows.Forms.Button
            Me.bApply = New System.Windows.Forms.Button
            Me.cbSinglePlayer = New System.Windows.Forms.CheckBox
            Me.ec = New MZZT.EventControl
            Me.SuspendLayout()
            '
            'bOK
            '
            Me.bOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bOK.Image = Global.My.Resources.Resources.silk_tick
            Me.bOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bOK.Location = New System.Drawing.Point(132, 35)
            Me.bOK.Name = "bOK"
            Me.bOK.Size = New System.Drawing.Size(75, 24)
            Me.bOK.TabIndex = 1
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
            Me.bCancel.Location = New System.Drawing.Point(213, 35)
            Me.bCancel.Name = "bCancel"
            Me.bCancel.Size = New System.Drawing.Size(75, 24)
            Me.bCancel.TabIndex = 2
            Me.bCancel.Text = "Close"
            Me.bCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bCancel.UseVisualStyleBackColor = True
            '
            'bApply
            '
            Me.bApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bApply.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bApply.Image = Global.My.Resources.Resources.silk_disk
            Me.bApply.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bApply.Location = New System.Drawing.Point(294, 35)
            Me.bApply.Name = "bApply"
            Me.bApply.Size = New System.Drawing.Size(75, 24)
            Me.bApply.TabIndex = 3
            Me.bApply.Text = "&Apply"
            Me.bApply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bApply.UseVisualStyleBackColor = True
            '
            'cbSinglePlayer
            '
            Me.cbSinglePlayer.AutoSize = True
            Me.cbSinglePlayer.Location = New System.Drawing.Point(12, 12)
            Me.cbSinglePlayer.Name = "cbSinglePlayer"
            Me.cbSinglePlayer.Size = New System.Drawing.Size(357, 17)
            Me.cbSinglePlayer.TabIndex = 0
            Me.cbSinglePlayer.Text = "When mod is double-clicked, run in single player instead of multiplayer."
            Me.cbSinglePlayer.UseVisualStyleBackColor = True
            '
            'fJOJAModOptions
            '
            Me.AcceptButton = Me.bOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.bCancel
            Me.ClientSize = New System.Drawing.Size(381, 71)
            Me.Controls.Add(Me.cbSinglePlayer)
            Me.Controls.Add(Me.bOK)
            Me.Controls.Add(Me.bCancel)
            Me.Controls.Add(Me.bApply)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "fJOJAModOptions"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Mod Options - Knight"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents bOK As System.Windows.Forms.Button
        Friend WithEvents bCancel As System.Windows.Forms.Button
        Friend WithEvents bApply As System.Windows.Forms.Button
        Friend WithEvents cbSinglePlayer As System.Windows.Forms.CheckBox
        Friend WithEvents ec As MZZT.EventControl
    End Class
End Namespace
