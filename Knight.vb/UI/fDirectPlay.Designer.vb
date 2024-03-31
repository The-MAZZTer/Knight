Namespace MZZT.Knight.Games
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fDirectPlay
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
			Dim lDesc As System.Windows.Forms.Label
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fDirectPlay))
			Me.bCancel = New System.Windows.Forms.Button()
			Me.cbMods = New System.Windows.Forms.CheckBox()
			Me.bApply = New System.Windows.Forms.Button()
			Me.bReset = New System.Windows.Forms.Button()
			Me.ec = New MZZT.EventControl()
			lDesc = New System.Windows.Forms.Label()
			Me.SuspendLayout()
			'
			'lDesc
			'
			lDesc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			lDesc.Location = New System.Drawing.Point(9, 9)
			lDesc.Name = "lDesc"
			lDesc.Size = New System.Drawing.Size(383, 107)
			lDesc.TabIndex = 0
			lDesc.Text = resources.GetString("lDesc.Text")
			'
			'bCancel
			'
			Me.bCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.bCancel.Image = Global.My.Resources.Resources.silk_cross
			Me.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
			Me.bCancel.Location = New System.Drawing.Point(320, 202)
			Me.bCancel.Name = "bCancel"
			Me.bCancel.Size = New System.Drawing.Size(75, 24)
			Me.bCancel.TabIndex = 4
			Me.bCancel.Text = "Close"
			Me.bCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
			Me.bCancel.UseVisualStyleBackColor = True
			'
			'cbMods
			'
			Me.cbMods.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.cbMods.AutoSize = True
			Me.cbMods.Checked = True
			Me.cbMods.CheckState = System.Windows.Forms.CheckState.Checked
			Me.cbMods.Location = New System.Drawing.Point(12, 119)
			Me.cbMods.Name = "cbMods"
			Me.cbMods.Size = New System.Drawing.Size(105, 17)
			Me.cbMods.TabIndex = 1
			Me.cbMods.Text = "&Use active mods"
			Me.cbMods.UseVisualStyleBackColor = True
			'
			'bApply
			'
			Me.bApply.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.bApply.Image = Global.My.Resources.Resources.silk_shield
			Me.bApply.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
			Me.bApply.Location = New System.Drawing.Point(12, 142)
			Me.bApply.Name = "bApply"
			Me.bApply.Size = New System.Drawing.Size(383, 24)
			Me.bApply.TabIndex = 2
			Me.bApply.Text = "&Apply settings now"
			Me.bApply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.bApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
			Me.bApply.UseVisualStyleBackColor = True
			'
			'bReset
			'
			Me.bReset.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.bReset.Image = Global.My.Resources.Resources.silk_shield
			Me.bReset.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
			Me.bReset.Location = New System.Drawing.Point(12, 172)
			Me.bReset.Name = "bReset"
			Me.bReset.Size = New System.Drawing.Size(383, 24)
			Me.bReset.TabIndex = 3
			Me.bReset.Text = "&Reset to default"
			Me.bReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.bReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
			Me.bReset.UseVisualStyleBackColor = True
			'
			'fDirectPlay
			'
			Me.AcceptButton = Me.bCancel
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.CancelButton = Me.bCancel
			Me.ClientSize = New System.Drawing.Size(407, 238)
			Me.Controls.Add(Me.bReset)
			Me.Controls.Add(Me.bApply)
			Me.Controls.Add(Me.cbMods)
			Me.Controls.Add(Me.bCancel)
			Me.Controls.Add(lDesc)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "fDirectPlay"
			Me.ShowInTaskbar = False
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "DirectPlay Lobby Support - Knight"
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents bCancel As System.Windows.Forms.Button
        Friend WithEvents cbMods As System.Windows.Forms.CheckBox
        Friend WithEvents bApply As System.Windows.Forms.Button
        Friend WithEvents bReset As System.Windows.Forms.Button
        Friend WithEvents ec As MZZT.EventControl
    End Class
End Namespace
