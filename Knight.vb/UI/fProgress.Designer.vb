Namespace MZZT.Knight
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fProgress
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
            Me.bCancel = New System.Windows.Forms.Button
            Me.pbProgress = New System.Windows.Forms.ProgressBar
            Me.lDesc = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'bCancel
            '
            Me.bCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bCancel.Image = Global.My.Resources.Resources.silk_cross
            Me.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bCancel.Location = New System.Drawing.Point(207, 62)
            Me.bCancel.Name = "bCancel"
            Me.bCancel.Size = New System.Drawing.Size(75, 24)
            Me.bCancel.TabIndex = 2
            Me.bCancel.Text = "Cancel"
            Me.bCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bCancel.UseVisualStyleBackColor = True
            '
            'pbProgress
            '
            Me.pbProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pbProgress.Location = New System.Drawing.Point(12, 33)
            Me.pbProgress.Name = "pbProgress"
            Me.pbProgress.Size = New System.Drawing.Size(270, 23)
            Me.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
            Me.pbProgress.TabIndex = 1
            '
            'lDesc
            '
            Me.lDesc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lDesc.Location = New System.Drawing.Point(12, 9)
            Me.lDesc.Name = "lDesc"
            Me.lDesc.Size = New System.Drawing.Size(270, 21)
            Me.lDesc.TabIndex = 0
            Me.lDesc.Text = "Preparing to activate/deactivate requested mod(s)..."
            Me.lDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'fProgress
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.bCancel
            Me.ClientSize = New System.Drawing.Size(294, 98)
            Me.Controls.Add(Me.lDesc)
            Me.Controls.Add(Me.pbProgress)
            Me.Controls.Add(Me.bCancel)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "fProgress"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Working - Knight"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents bCancel As System.Windows.Forms.Button
        Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
        Friend WithEvents lDesc As System.Windows.Forms.Label
    End Class
End Namespace
