<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Config
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Config))
        Me.DGV_PARAM = New System.Windows.Forms.DataGridView()
        Me.column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_UPDATE = New System.Windows.Forms.Button()
        CType(Me.DGV_PARAM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DGV_PARAM
        '
        Me.DGV_PARAM.AllowUserToAddRows = False
        Me.DGV_PARAM.AllowUserToDeleteRows = False
        Me.DGV_PARAM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_PARAM.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.column1, Me.Column3, Me.column2})
        Me.DGV_PARAM.Location = New System.Drawing.Point(5, 5)
        Me.DGV_PARAM.Name = "DGV_PARAM"
        Me.DGV_PARAM.RowTemplate.Height = 21
        Me.DGV_PARAM.Size = New System.Drawing.Size(682, 554)
        Me.DGV_PARAM.StandardTab = True
        Me.DGV_PARAM.TabIndex = 1
        '
        'column1
        '
        Me.column1.HeaderText = "パラメータ名"
        Me.column1.Name = "column1"
        Me.column1.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.HeaderText = "説明"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'column2
        '
        Me.column2.HeaderText = "値"
        Me.column2.Name = "column2"
        Me.column2.Width = 250
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_HELP)
        Me.GroupBox1.Controls.Add(Me.BTN_UPDATE)
        Me.GroupBox1.Location = New System.Drawing.Point(693, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 547)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(6, 49)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HELP.TabIndex = 3
        Me.BTN_HELP.TabStop = False
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'BTN_UPDATE
        '
        Me.BTN_UPDATE.Location = New System.Drawing.Point(6, 18)
        Me.BTN_UPDATE.Name = "BTN_UPDATE"
        Me.BTN_UPDATE.Size = New System.Drawing.Size(68, 25)
        Me.BTN_UPDATE.TabIndex = 1
        Me.BTN_UPDATE.TabStop = False
        Me.BTN_UPDATE.Text = "更新(&U)"
        Me.BTN_UPDATE.UseVisualStyleBackColor = True
        '
        'frm_Config
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.DGV_PARAM)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frm_Config"
        Me.Text = "パラメータ設定"
        CType(Me.DGV_PARAM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DGV_PARAM As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_UPDATE As System.Windows.Forms.Button
    Friend WithEvents column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
End Class
