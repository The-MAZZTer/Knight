Namespace MZZT.Knight.Games
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fJOJAOptions
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
            Dim lCommands As System.Windows.Forms.Label
            Me.bOK = New System.Windows.Forms.Button
            Me.bCancel = New System.Windows.Forms.Button
            Me.bApply = New System.Windows.Forms.Button
            Me.ec = New MZZT.EventControl
            Me.tbCommands = New System.Windows.Forms.TextBox
            lCommands = New System.Windows.Forms.Label
            Me.SuspendLayout()
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
            Me.bOK.TabIndex = 8
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
            Me.bCancel.TabIndex = 9
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
            Me.bApply.TabIndex = 10
            Me.bApply.Text = "&Apply"
            Me.bApply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bApply.UseVisualStyleBackColor = True
            '
            'lCommands
            '
            lCommands.AutoSize = True
            lCommands.Location = New System.Drawing.Point(12, 9)
            lCommands.Name = "lCommands"
            lCommands.Size = New System.Drawing.Size(182, 13)
            lCommands.TabIndex = 11
            lCommands.Text = "Console commands to run on startup:"
            '
            'tbCommands
            '
            Me.tbCommands.AcceptsReturn = True
            Me.tbCommands.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbCommands.Location = New System.Drawing.Point(12, 25)
            Me.tbCommands.Multiline = True
            Me.tbCommands.Name = "tbCommands"
            Me.tbCommands.Size = New System.Drawing.Size(270, 156)
            Me.tbCommands.TabIndex = 12
            '
            'fJOJAOptions
            '
            Me.AcceptButton = Me.bOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.bCancel
            Me.ClientSize = New System.Drawing.Size(294, 223)
            Me.Controls.Add(Me.tbCommands)
            Me.Controls.Add(lCommands)
            Me.Controls.Add(Me.bOK)
            Me.Controls.Add(Me.bCancel)
            Me.Controls.Add(Me.bApply)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "fJOJAOptions"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Jedi Outcast Options - Knight"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents bOK As System.Windows.Forms.Button
        Friend WithEvents bCancel As System.Windows.Forms.Button
        Friend WithEvents bApply As System.Windows.Forms.Button
        Friend WithEvents ec As MZZT.EventControl
        Friend WithEvents tbCommands As System.Windows.Forms.TextBox
    End Class
End Namespace