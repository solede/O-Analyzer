<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_watch
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.BTN_SHOW_LOG = New System.Windows.Forms.Button()
        Me.BTN_CANCEL = New System.Windows.Forms.Button()
        Me.BTN_EXEC = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabTablespace = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TXT_INTERVAL = New System.Windows.Forms.TextBox()
        Me.TXT_DISP_NUM = New System.Windows.Forms.TextBox()
        Me.TABCTL = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.DGV_ALL = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CHK_GRAFH = New System.Windows.Forms.CheckBox()
        Me.CHK_FILE = New System.Windows.Forms.CheckBox()
        Me.CHK_GAMEN = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TXT_USER_DEFINE = New System.Windows.Forms.TextBox()
        Me.CHK_PGA = New System.Windows.Forms.CheckBox()
        Me.CHK_PROC = New System.Windows.Forms.CheckBox()
        Me.CHK_SES = New System.Windows.Forms.CheckBox()
        Me.GroupBox9.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabTablespace.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TABCTL.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.DGV_ALL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.BTN_SHOW_LOG)
        Me.GroupBox9.Controls.Add(Me.BTN_CANCEL)
        Me.GroupBox9.Controls.Add(Me.BTN_EXEC)
        Me.GroupBox9.Location = New System.Drawing.Point(700, 40)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(79, 511)
        Me.GroupBox9.TabIndex = 24
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "コマンド"
        '
        'BTN_SHOW_LOG
        '
        Me.BTN_SHOW_LOG.Location = New System.Drawing.Point(5, 88)
        Me.BTN_SHOW_LOG.Name = "BTN_SHOW_LOG"
        Me.BTN_SHOW_LOG.Size = New System.Drawing.Size(68, 25)
        Me.BTN_SHOW_LOG.TabIndex = 20
        Me.BTN_SHOW_LOG.TabStop = False
        Me.BTN_SHOW_LOG.Text = "ログ(&L)"
        Me.BTN_SHOW_LOG.UseVisualStyleBackColor = True
        '
        'BTN_CANCEL
        '
        Me.BTN_CANCEL.Location = New System.Drawing.Point(6, 57)
        Me.BTN_CANCEL.Name = "BTN_CANCEL"
        Me.BTN_CANCEL.Size = New System.Drawing.Size(68, 25)
        Me.BTN_CANCEL.TabIndex = 19
        Me.BTN_CANCEL.TabStop = False
        Me.BTN_CANCEL.Text = "キャンセル"
        Me.BTN_CANCEL.UseVisualStyleBackColor = True
        '
        'BTN_EXEC
        '
        Me.BTN_EXEC.Location = New System.Drawing.Point(6, 25)
        Me.BTN_EXEC.Name = "BTN_EXEC"
        Me.BTN_EXEC.Size = New System.Drawing.Size(68, 25)
        Me.BTN_EXEC.TabIndex = 18
        Me.BTN_EXEC.TabStop = False
        Me.BTN_EXEC.Text = "実行(&E)"
        Me.BTN_EXEC.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabTablespace)
        Me.TabControl1.Location = New System.Drawing.Point(14, 21)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(680, 530)
        Me.TabControl1.TabIndex = 25
        '
        'TabTablespace
        '
        Me.TabTablespace.Controls.Add(Me.GroupBox2)
        Me.TabTablespace.Controls.Add(Me.TABCTL)
        Me.TabTablespace.Controls.Add(Me.GroupBox1)
        Me.TabTablespace.Controls.Add(Me.GroupBox3)
        Me.TabTablespace.Location = New System.Drawing.Point(4, 22)
        Me.TabTablespace.Name = "TabTablespace"
        Me.TabTablespace.Padding = New System.Windows.Forms.Padding(3)
        Me.TabTablespace.Size = New System.Drawing.Size(672, 504)
        Me.TabTablespace.TabIndex = 0
        Me.TabTablespace.Text = "リソース"
        Me.TabTablespace.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.TXT_INTERVAL)
        Me.GroupBox2.Controls.Add(Me.TXT_DISP_NUM)
        Me.GroupBox2.Location = New System.Drawing.Point(426, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(141, 74)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "表示条件"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "監視間隔(S)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "表示件数"
        '
        'TXT_INTERVAL
        '
        Me.TXT_INTERVAL.Location = New System.Drawing.Point(80, 16)
        Me.TXT_INTERVAL.Name = "TXT_INTERVAL"
        Me.TXT_INTERVAL.Size = New System.Drawing.Size(49, 19)
        Me.TXT_INTERVAL.TabIndex = 4
        Me.TXT_INTERVAL.Text = "4"
        '
        'TXT_DISP_NUM
        '
        Me.TXT_DISP_NUM.Location = New System.Drawing.Point(80, 46)
        Me.TXT_DISP_NUM.Name = "TXT_DISP_NUM"
        Me.TXT_DISP_NUM.Size = New System.Drawing.Size(49, 19)
        Me.TXT_DISP_NUM.TabIndex = 6
        Me.TXT_DISP_NUM.Text = "18"
        '
        'TABCTL
        '
        Me.TABCTL.Controls.Add(Me.TabPage1)
        Me.TABCTL.Controls.Add(Me.TabPage5)
        Me.TABCTL.Controls.Add(Me.TabPage3)
        Me.TABCTL.Location = New System.Drawing.Point(6, 86)
        Me.TABCTL.Name = "TABCTL"
        Me.TABCTL.SelectedIndex = 0
        Me.TABCTL.Size = New System.Drawing.Size(660, 413)
        Me.TABCTL.TabIndex = 7
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.DGV_ALL)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(652, 387)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "全体"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'DGV_ALL
        '
        Me.DGV_ALL.AllowUserToAddRows = False
        Me.DGV_ALL.AllowUserToDeleteRows = False
        Me.DGV_ALL.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_ALL.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DGV_ALL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_ALL.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column4, Me.Column2, Me.Column3})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGV_ALL.DefaultCellStyle = DataGridViewCellStyle2
        Me.DGV_ALL.Location = New System.Drawing.Point(3, 3)
        Me.DGV_ALL.Name = "DGV_ALL"
        Me.DGV_ALL.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_ALL.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DGV_ALL.RowTemplate.Height = 21
        Me.DGV_ALL.Size = New System.Drawing.Size(646, 379)
        Me.DGV_ALL.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = "時刻"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 54
        '
        'Column4
        '
        Me.Column4.HeaderText = "セッション数"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 84
        '
        'Column2
        '
        Me.Column2.HeaderText = "プロセス数"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 79
        '
        'Column3
        '
        Me.Column3.HeaderText = "PGA"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 53
        '
        'TabPage5
        '
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(652, 387)
        Me.TabPage5.TabIndex = 2
        Me.TabPage5.Text = "グラフ"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(652, 387)
        Me.TabPage3.TabIndex = 4
        Me.TabPage3.Text = "PGA"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CHK_GRAFH)
        Me.GroupBox1.Controls.Add(Me.CHK_FILE)
        Me.GroupBox1.Controls.Add(Me.CHK_GAMEN)
        Me.GroupBox1.Location = New System.Drawing.Point(573, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(86, 74)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "出力先"
        '
        'CHK_GRAFH
        '
        Me.CHK_GRAFH.AutoSize = True
        Me.CHK_GRAFH.Checked = True
        Me.CHK_GRAFH.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_GRAFH.Location = New System.Drawing.Point(6, 32)
        Me.CHK_GRAFH.Name = "CHK_GRAFH"
        Me.CHK_GRAFH.Size = New System.Drawing.Size(49, 16)
        Me.CHK_GRAFH.TabIndex = 2
        Me.CHK_GRAFH.Text = "グラフ"
        Me.CHK_GRAFH.UseVisualStyleBackColor = True
        '
        'CHK_FILE
        '
        Me.CHK_FILE.AutoSize = True
        Me.CHK_FILE.Location = New System.Drawing.Point(6, 48)
        Me.CHK_FILE.Name = "CHK_FILE"
        Me.CHK_FILE.Size = New System.Drawing.Size(58, 16)
        Me.CHK_FILE.TabIndex = 3
        Me.CHK_FILE.Text = "ファイル"
        Me.CHK_FILE.UseVisualStyleBackColor = True
        '
        'CHK_GAMEN
        '
        Me.CHK_GAMEN.AutoSize = True
        Me.CHK_GAMEN.Checked = True
        Me.CHK_GAMEN.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_GAMEN.Location = New System.Drawing.Point(6, 16)
        Me.CHK_GAMEN.Name = "CHK_GAMEN"
        Me.CHK_GAMEN.Size = New System.Drawing.Size(48, 16)
        Me.CHK_GAMEN.TabIndex = 1
        Me.CHK_GAMEN.Text = "画面"
        Me.CHK_GAMEN.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.TXT_USER_DEFINE)
        Me.GroupBox3.Controls.Add(Me.CHK_PGA)
        Me.GroupBox3.Controls.Add(Me.CHK_PROC)
        Me.GroupBox3.Controls.Add(Me.CHK_SES)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(414, 74)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "監視項目"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 12)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "ユーザ条件"
        '
        'TXT_USER_DEFINE
        '
        Me.TXT_USER_DEFINE.Location = New System.Drawing.Point(71, 46)
        Me.TXT_USER_DEFINE.Name = "TXT_USER_DEFINE"
        Me.TXT_USER_DEFINE.Size = New System.Drawing.Size(337, 19)
        Me.TXT_USER_DEFINE.TabIndex = 8
        Me.TXT_USER_DEFINE.Text = "4"
        '
        'CHK_PGA
        '
        Me.CHK_PGA.AutoSize = True
        Me.CHK_PGA.Checked = True
        Me.CHK_PGA.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_PGA.Location = New System.Drawing.Point(169, 18)
        Me.CHK_PGA.Name = "CHK_PGA"
        Me.CHK_PGA.Size = New System.Drawing.Size(71, 16)
        Me.CHK_PGA.TabIndex = 6
        Me.CHK_PGA.Text = "合計PGA"
        Me.CHK_PGA.UseVisualStyleBackColor = True
        '
        'CHK_PROC
        '
        Me.CHK_PROC.AutoSize = True
        Me.CHK_PROC.Checked = True
        Me.CHK_PROC.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_PROC.Location = New System.Drawing.Point(90, 21)
        Me.CHK_PROC.Name = "CHK_PROC"
        Me.CHK_PROC.Size = New System.Drawing.Size(73, 16)
        Me.CHK_PROC.TabIndex = 3
        Me.CHK_PROC.Text = "プロセス数"
        Me.CHK_PROC.UseVisualStyleBackColor = True
        '
        'CHK_SES
        '
        Me.CHK_SES.AutoSize = True
        Me.CHK_SES.Checked = True
        Me.CHK_SES.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_SES.Location = New System.Drawing.Point(6, 21)
        Me.CHK_SES.Name = "CHK_SES"
        Me.CHK_SES.Size = New System.Drawing.Size(78, 16)
        Me.CHK_SES.TabIndex = 0
        Me.CHK_SES.Text = "セッション数"
        Me.CHK_SES.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 573)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.GroupBox9.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabTablespace.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TABCTL.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.DGV_ALL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_SHOW_LOG As System.Windows.Forms.Button
    Friend WithEvents BTN_CANCEL As System.Windows.Forms.Button
    Friend WithEvents BTN_EXEC As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabTablespace As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TXT_INTERVAL As System.Windows.Forms.TextBox
    Friend WithEvents TXT_DISP_NUM As System.Windows.Forms.TextBox
    Friend WithEvents TABCTL As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents DGV_ALL As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CHK_GRAFH As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_FILE As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_GAMEN As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TXT_USER_DEFINE As System.Windows.Forms.TextBox
    Friend WithEvents CHK_PGA As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_PROC As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_SES As System.Windows.Forms.CheckBox

End Class
