Namespace MZZT
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ucNowLater
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
            Me.bLater = New System.Windows.Forms.Button
            Me.bNow = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'bLater
            '
            Me.bLater.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bLater.Image = Global.My.Resources.Resources.silk_clock
            Me.bLater.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bLater.Location = New System.Drawing.Point(125, 0)
            Me.bLater.Name = "bLater"
            Me.bLater.Size = New System.Drawing.Size(75, 24)
            Me.bLater.TabIndex = 0
            Me.bLater.Text = "&Later"
            Me.bLater.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bLater.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bLater.UseVisualStyleBackColor = True
            '
            'bNow
            '
            Me.bNow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bNow.Image = Global.My.Resources.Resources.silk_tick
            Me.bNow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bNow.Location = New System.Drawing.Point(44, 0)
            Me.bNow.Name = "bNow"
            Me.bNow.Size = New System.Drawing.Size(75, 24)
            Me.bNow.TabIndex = 1
            Me.bNow.Text = "&Now"
            Me.bNow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bNow.UseVisualStyleBackColor = True
            '
            'ucNowLater
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.bNow)
            Me.Controls.Add(Me.bLater)
            Me.Name = "ucNowLater"
            Me.Size = New System.Drawing.Size(200, 24)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents bLater As System.Windows.Forms.Button
        Friend WithEvents bNow As System.Windows.Forms.Button

    End Class
End Namespace
