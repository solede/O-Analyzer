<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Bind
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Bind))
        Me.DGV_BIND = New System.Windows.Forms.DataGridView()
        Me.column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.column2 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_UPDATE = New System.Windows.Forms.Button()
        Me.LBL_INFO = New System.Windows.Forms.Label()
        CType(Me.DGV_BIND, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DGV_BIND
        '
        Me.DGV_BIND.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_BIND.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.column1, Me.Column3, Me.column2, Me.Column4})
        Me.DGV_BIND.Location = New System.Drawing.Point(5, 5)
        Me.DGV_BIND.Name = "DGV_BIND"
        Me.DGV_BIND.RowTemplate.Height = 21
        Me.DGV_BIND.Size = New System.Drawing.Size(490, 426)
        Me.DGV_BIND.StandardTab = True
        Me.DGV_BIND.TabIndex = 1
        '
        'column1
        '
        Me.column1.HeaderText = "バインド変数名"
        Me.column1.Name = "column1"
        Me.column1.Width = 110
        '
        'Column3
        '
        DataGridViewCellStyle1.NullValue = "VARCHAR2"
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle1
        Me.Column3.HeaderText = "タイプ"
        Me.Column3.Items.AddRange(New Object() {"NUMBER", "VARCHAR2", "CLOB"})
        Me.Column3.Name = "Column3"
        Me.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column3.Width = 97
        '
        'column2
        '
        DataGridViewCellStyle2.NullValue = "IN/OUT"
        Me.column2.DefaultCellStyle = DataGridViewCellStyle2
        Me.column2.HeaderText = "IN/OUT"
        Me.column2.Items.AddRange(New Object() {"IN", "OUT", "IN/OUT"})
        Me.column2.Name = "column2"
        Me.column2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.column2.Width = 70
        '
        'Column4
        '
        Me.Column4.HeaderText = "値"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 170
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_HELP)
        Me.GroupBox1.Controls.Add(Me.BTN_UPDATE)
        Me.GroupBox1.Location = New System.Drawing.Point(501, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 426)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(6, 49)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HELP.TabIndex = 2
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
        Me.BTN_UPDATE.Text = "設定(&C)"
        Me.BTN_UPDATE.UseVisualStyleBackColor = True
        '
        'LBL_INFO
        '
        Me.LBL_INFO.AutoSize = True
        Me.LBL_INFO.Location = New System.Drawing.Point(12, 441)
        Me.LBL_INFO.Name = "LBL_INFO"
        Me.LBL_INFO.Size = New System.Drawing.Size(40, 12)
        Me.LBL_INFO.TabIndex = 16
        Me.LBL_INFO.Tag = "label"
        Me.LBL_INFO.Text = "LABEL"
        '
        'frm_Bind
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 462)
        Me.Controls.Add(Me.LBL_INFO)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.DGV_BIND)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(600, 500)
        Me.Name = "frm_Bind"
        Me.Text = "frm_Bind"
        CType(Me.DGV_BIND, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DGV_BIND As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_UPDATE As System.Windows.Forms.Button
    Friend WithEvents LBL_INFO As System.Windows.Forms.Label
    Friend WithEvents column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents column2 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
End Class
