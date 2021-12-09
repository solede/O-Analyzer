<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Watch
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

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Watch))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_SHOW_LOG = New System.Windows.Forms.Button()
        Me.BTN_CANCEL = New System.Windows.Forms.Button()
        Me.BTN_EXEC = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
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
        Me.buffer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.other = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.CHK_GRAFH = New System.Windows.Forms.CheckBox()
        Me.CHK_FILE = New System.Windows.Forms.CheckBox()
        Me.CHK_GAMEN = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.CHK_OTHER = New System.Windows.Forms.CheckBox()
        Me.CHK_BUFFER = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TXT_USER_DEFINE = New System.Windows.Forms.TextBox()
        Me.CHK_PGA = New System.Windows.Forms.CheckBox()
        Me.CHK_PROC = New System.Windows.Forms.CheckBox()
        Me.CHK_SES = New System.Windows.Forms.CheckBox()
        Me.LBL_INFO = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabTablespace.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TABCTL.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.DGV_ALL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_HELP)
        Me.GroupBox1.Controls.Add(Me.BTN_SHOW_LOG)
        Me.GroupBox1.Controls.Add(Me.BTN_CANCEL)
        Me.GroupBox1.Controls.Add(Me.BTN_EXEC)
        Me.GroupBox1.Location = New System.Drawing.Point(700, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 539)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(6, 111)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HELP.TabIndex = 26
        Me.BTN_HELP.TabStop = False
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'BTN_SHOW_LOG
        '
        Me.BTN_SHOW_LOG.Location = New System.Drawing.Point(6, 80)
        Me.BTN_SHOW_LOG.Name = "BTN_SHOW_LOG"
        Me.BTN_SHOW_LOG.Size = New System.Drawing.Size(68, 25)
        Me.BTN_SHOW_LOG.TabIndex = 20
        Me.BTN_SHOW_LOG.TabStop = False
        Me.BTN_SHOW_LOG.Text = "ログ(&L)"
        Me.BTN_SHOW_LOG.UseVisualStyleBackColor = True
        '
        'BTN_CANCEL
        '
        Me.BTN_CANCEL.Location = New System.Drawing.Point(6, 49)
        Me.BTN_CANCEL.Name = "BTN_CANCEL"
        Me.BTN_CANCEL.Size = New System.Drawing.Size(68, 25)
        Me.BTN_CANCEL.TabIndex = 19
        Me.BTN_CANCEL.TabStop = False
        Me.BTN_CANCEL.Text = "キャンセル"
        Me.BTN_CANCEL.UseVisualStyleBackColor = True
        '
        'BTN_EXEC
        '
        Me.BTN_EXEC.Location = New System.Drawing.Point(6, 18)
        Me.BTN_EXEC.Name = "BTN_EXEC"
        Me.BTN_EXEC.Size = New System.Drawing.Size(68, 25)
        Me.BTN_EXEC.TabIndex = 18
        Me.BTN_EXEC.TabStop = False
        Me.BTN_EXEC.Text = "実行(&E)"
        Me.BTN_EXEC.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 10000
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabTablespace)
        Me.TabControl1.Location = New System.Drawing.Point(5, 5)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(680, 539)
        Me.TabControl1.TabIndex = 0
        Me.TabControl1.TabStop = False
        '
        'TabTablespace
        '
        Me.TabTablespace.Controls.Add(Me.GroupBox2)
        Me.TabTablespace.Controls.Add(Me.TABCTL)
        Me.TabTablespace.Controls.Add(Me.GroupBox4)
        Me.TabTablespace.Controls.Add(Me.GroupBox3)
        Me.TabTablespace.Location = New System.Drawing.Point(4, 22)
        Me.TabTablespace.Name = "TabTablespace"
        Me.TabTablespace.Padding = New System.Windows.Forms.Padding(3)
        Me.TabTablespace.Size = New System.Drawing.Size(672, 513)
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
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "表示条件"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "監視間隔(S)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "表示件数"
        '
        'TXT_INTERVAL
        '
        Me.TXT_INTERVAL.Location = New System.Drawing.Point(80, 16)
        Me.TXT_INTERVAL.Name = "TXT_INTERVAL"
        Me.TXT_INTERVAL.Size = New System.Drawing.Size(49, 19)
        Me.TXT_INTERVAL.TabIndex = 1
        Me.TXT_INTERVAL.Text = "4"
        '
        'TXT_DISP_NUM
        '
        Me.TXT_DISP_NUM.Location = New System.Drawing.Point(80, 46)
        Me.TXT_DISP_NUM.Name = "TXT_DISP_NUM"
        Me.TXT_DISP_NUM.Size = New System.Drawing.Size(49, 19)
        Me.TXT_DISP_NUM.TabIndex = 3
        Me.TXT_DISP_NUM.Text = "18"
        '
        'TABCTL
        '
        Me.TABCTL.Controls.Add(Me.TabPage1)
        Me.TABCTL.Controls.Add(Me.TabPage2)
        Me.TABCTL.Controls.Add(Me.TabPage3)
        Me.TABCTL.Location = New System.Drawing.Point(6, 86)
        Me.TABCTL.Name = "TABCTL"
        Me.TABCTL.SelectedIndex = 0
        Me.TABCTL.Size = New System.Drawing.Size(660, 413)
        Me.TABCTL.TabIndex = 3
        Me.TABCTL.TabStop = False
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
        Me.DGV_ALL.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column4, Me.Column2, Me.Column3, Me.buffer, Me.other, Me.Column5})
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
        Me.DGV_ALL.StandardTab = True
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
        'buffer
        '
        Me.buffer.HeaderText = "SGA(buffer)"
        Me.buffer.Name = "buffer"
        Me.buffer.ReadOnly = True
        Me.buffer.Width = 91
        '
        'other
        '
        Me.other.HeaderText = "SGA(other)"
        Me.other.Name = "other"
        Me.other.ReadOnly = True
        Me.other.Width = 87
        '
        'Column5
        '
        Me.Column5.HeaderText = "ユーザ定義"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 84
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(652, 387)
        Me.TabPage2.TabIndex = 2
        Me.TabPage2.Text = "セッション プロセス"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(652, 387)
        Me.TabPage3.TabIndex = 4
        Me.TabPage3.Text = "メモリ"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.CHK_GRAFH)
        Me.GroupBox4.Controls.Add(Me.CHK_FILE)
        Me.GroupBox4.Controls.Add(Me.CHK_GAMEN)
        Me.GroupBox4.Location = New System.Drawing.Point(573, 6)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(86, 74)
        Me.GroupBox4.TabIndex = 2
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "出力先"
        '
        'CHK_GRAFH
        '
        Me.CHK_GRAFH.AutoSize = True
        Me.CHK_GRAFH.Checked = True
        Me.CHK_GRAFH.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_GRAFH.Location = New System.Drawing.Point(6, 48)
        Me.CHK_GRAFH.Name = "CHK_GRAFH"
        Me.CHK_GRAFH.Size = New System.Drawing.Size(49, 16)
        Me.CHK_GRAFH.TabIndex = 1
        Me.CHK_GRAFH.Text = "グラフ"
        Me.CHK_GRAFH.UseVisualStyleBackColor = True
        '
        'CHK_FILE
        '
        Me.CHK_FILE.AutoSize = True
        Me.CHK_FILE.Location = New System.Drawing.Point(6, 32)
        Me.CHK_FILE.Name = "CHK_FILE"
        Me.CHK_FILE.Size = New System.Drawing.Size(58, 16)
        Me.CHK_FILE.TabIndex = 2
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
        Me.CHK_GAMEN.TabIndex = 0
        Me.CHK_GAMEN.Text = "画面"
        Me.CHK_GAMEN.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.CHK_OTHER)
        Me.GroupBox3.Controls.Add(Me.CHK_BUFFER)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.TXT_USER_DEFINE)
        Me.GroupBox3.Controls.Add(Me.CHK_PGA)
        Me.GroupBox3.Controls.Add(Me.CHK_PROC)
        Me.GroupBox3.Controls.Add(Me.CHK_SES)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(414, 74)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "監視項目"
        '
        'CHK_OTHER
        '
        Me.CHK_OTHER.AutoSize = True
        Me.CHK_OTHER.Checked = True
        Me.CHK_OTHER.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_OTHER.Location = New System.Drawing.Point(323, 21)
        Me.CHK_OTHER.Name = "CHK_OTHER"
        Me.CHK_OTHER.Size = New System.Drawing.Size(81, 16)
        Me.CHK_OTHER.TabIndex = 4
        Me.CHK_OTHER.Text = "SGA(other)"
        Me.CHK_OTHER.UseVisualStyleBackColor = True
        '
        'CHK_BUFFER
        '
        Me.CHK_BUFFER.AutoSize = True
        Me.CHK_BUFFER.Checked = True
        Me.CHK_BUFFER.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_BUFFER.Location = New System.Drawing.Point(238, 21)
        Me.CHK_BUFFER.Name = "CHK_BUFFER"
        Me.CHK_BUFFER.Size = New System.Drawing.Size(85, 16)
        Me.CHK_BUFFER.TabIndex = 3
        Me.CHK_BUFFER.Text = "SGA(buffer)"
        Me.CHK_BUFFER.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 12)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "ユーザ定義"
        '
        'TXT_USER_DEFINE
        '
        Me.TXT_USER_DEFINE.Location = New System.Drawing.Point(71, 46)
        Me.TXT_USER_DEFINE.Name = "TXT_USER_DEFINE"
        Me.TXT_USER_DEFINE.Size = New System.Drawing.Size(337, 19)
        Me.TXT_USER_DEFINE.TabIndex = 6
        '
        'CHK_PGA
        '
        Me.CHK_PGA.AutoSize = True
        Me.CHK_PGA.Checked = True
        Me.CHK_PGA.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_PGA.Location = New System.Drawing.Point(161, 21)
        Me.CHK_PGA.Name = "CHK_PGA"
        Me.CHK_PGA.Size = New System.Drawing.Size(71, 16)
        Me.CHK_PGA.TabIndex = 2
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
        Me.CHK_PROC.TabIndex = 1
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
        'LBL_INFO
        '
        Me.LBL_INFO.AutoSize = True
        Me.LBL_INFO.Location = New System.Drawing.Point(16, 551)
        Me.LBL_INFO.Name = "LBL_INFO"
        Me.LBL_INFO.Size = New System.Drawing.Size(40, 12)
        Me.LBL_INFO.TabIndex = 2
        Me.LBL_INFO.Tag = "label"
        Me.LBL_INFO.Text = "LABEL"
        '
        'frm_Watch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.LBL_INFO)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frm_Watch"
        Me.Text = "DB監視"
        Me.GroupBox1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabTablespace.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TABCTL.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.DGV_ALL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_SHOW_LOG As System.Windows.Forms.Button
    Friend WithEvents BTN_CANCEL As System.Windows.Forms.Button
    Friend WithEvents BTN_EXEC As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabTablespace As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents CHK_PGA As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_PROC As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_SES As System.Windows.Forms.CheckBox
    Friend WithEvents DGV_ALL As System.Windows.Forms.DataGridView
    Friend WithEvents LBL_INFO As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents CHK_FILE As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_GAMEN As System.Windows.Forms.CheckBox
    Friend WithEvents TABCTL As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TXT_DISP_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TXT_INTERVAL As System.Windows.Forms.TextBox
    Friend WithEvents CHK_GRAFH As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TXT_USER_DEFINE As System.Windows.Forms.TextBox
    'Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    'Friend WithEvents Chart2 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents CHK_OTHER As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_BUFFER As System.Windows.Forms.CheckBox
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents buffer As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents other As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
