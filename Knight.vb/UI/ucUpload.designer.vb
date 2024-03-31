Namespace MZZT
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ucUpload
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Me.pbProgress = New System.Windows.Forms.ProgressBar
            Me.bCancel = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'pbProgress
            '
            Me.pbProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pbProgress.Location = New System.Drawing.Point(0, 2)
            Me.pbProgress.Name = "pbProgress"
            Me.pbProgress.Size = New System.Drawing.Size(133, 20)
            Me.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
            Me.pbProgress.TabIndex = 0
            '
            'bCancel
            '
            Me.bCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bCancel.Image = Global.My.Resources.Resources.silk_cross
            Me.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bCancel.Location = New System.Drawing.Point(137, 0)
            Me.bCancel.Name = "bCancel"
            Me.bCancel.Size = New System.Drawing.Size(75, 24)
            Me.bCancel.TabIndex = 1
            Me.bCancel.Text = "Cancel"
            Me.bCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bCancel.UseVisualStyleBackColor = True
            '
            'ucUpload
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.bCancel)
            Me.Controls.Add(Me.pbProgress)
            Me.Name = "ucUpload"
            Me.Size = New System.Drawing.Size(212, 24)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
        Friend WithEvents bCancel As System.Windows.Forms.Button

    End Class
End Namespace
