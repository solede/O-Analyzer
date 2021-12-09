<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Stress
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Stress))
        Me.BTN_SHOW_LOG = New System.Windows.Forms.Button()
        Me.BTN_ABORT = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_CANCEL = New System.Windows.Forms.Button()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_BIND = New System.Windows.Forms.Button()
        Me.BTN_EXECUTE = New System.Windows.Forms.Button()
        Me.LBL_INFO = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.BTN_COPY = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CHK_NO_LOG = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LBL_TIMEOUT_VALUE = New System.Windows.Forms.Label()
        Me.CHK_WAIT_SESSION = New System.Windows.Forms.CheckBox()
        Me.CHK_CONNECT_SYNC = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TXT_SQL = New System.Windows.Forms.TextBox()
        Me.DGV_THREAD = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TXT_LOG = New System.Windows.Forms.TextBox()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.TXT_THREAD_CNT = New CLS_COMMON.RegExTextBox()
        Me.TXT_TIMEOUT_VALUE = New CLS_COMMON.RegExTextBox()
        Me.TXT_EXECUTE_CNT = New CLS_COMMON.RegExTextBox()
        Me.TXT_INTERVAL = New CLS_COMMON.RegExTextBox()
        Me.GroupBox1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DGV_THREAD, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BTN_SHOW_LOG
        '
        Me.BTN_SHOW_LOG.Location = New System.Drawing.Point(6, 111)
        Me.BTN_SHOW_LOG.Name = "BTN_SHOW_LOG"
        Me.BTN_SHOW_LOG.Size = New System.Drawing.Size(68, 25)
        Me.BTN_SHOW_LOG.TabIndex = 3
        Me.BTN_SHOW_LOG.TabStop = False
        Me.BTN_SHOW_LOG.Text = "ログ(&L)"
        Me.BTN_SHOW_LOG.UseVisualStyleBackColor = True
        '
        'BTN_ABORT
        '
        Me.BTN_ABORT.Location = New System.Drawing.Point(6, 80)
        Me.BTN_ABORT.Name = "BTN_ABORT"
        Me.BTN_ABORT.Size = New System.Drawing.Size(68, 25)
        Me.BTN_ABORT.TabIndex = 2
        Me.BTN_ABORT.TabStop = False
        Me.BTN_ABORT.Text = "強制停止"
        Me.BTN_ABORT.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_CANCEL)
        Me.GroupBox1.Controls.Add(Me.BTN_HELP)
        Me.GroupBox1.Controls.Add(Me.BTN_BIND)
        Me.GroupBox1.Controls.Add(Me.BTN_EXECUTE)
        Me.GroupBox1.Controls.Add(Me.BTN_SHOW_LOG)
        Me.GroupBox1.Controls.Add(Me.BTN_ABORT)
        Me.GroupBox1.Location = New System.Drawing.Point(701, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 545)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_CANCEL
        '
        Me.BTN_CANCEL.Location = New System.Drawing.Point(6, 49)
        Me.BTN_CANCEL.Name = "BTN_CANCEL"
        Me.BTN_CANCEL.Size = New System.Drawing.Size(68, 25)
        Me.BTN_CANCEL.TabIndex = 1
        Me.BTN_CANCEL.TabStop = False
        Me.BTN_CANCEL.Text = "キャンセル"
        Me.BTN_CANCEL.UseVisualStyleBackColor = True
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(6, 142)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HELP.TabIndex = 4
        Me.BTN_HELP.TabStop = False
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'BTN_BIND
        '
        Me.BTN_BIND.Location = New System.Drawing.Point(6, 173)
        Me.BTN_BIND.Name = "BTN_BIND"
        Me.BTN_BIND.Size = New System.Drawing.Size(68, 25)
        Me.BTN_BIND.TabIndex = 5
        Me.BTN_BIND.TabStop = False
        Me.BTN_BIND.Text = "バインド(&B)"
        Me.BTN_BIND.UseVisualStyleBackColor = True
        Me.BTN_BIND.Visible = False
        '
        'BTN_EXECUTE
        '
        Me.BTN_EXECUTE.Location = New System.Drawing.Point(6, 18)
        Me.BTN_EXECUTE.Name = "BTN_EXECUTE"
        Me.BTN_EXECUTE.Size = New System.Drawing.Size(68, 25)
        Me.BTN_EXECUTE.TabIndex = 0
        Me.BTN_EXECUTE.TabStop = False
        Me.BTN_EXECUTE.Text = "実行(&E)"
        Me.BTN_EXECUTE.UseVisualStyleBackColor = True
        '
        'LBL_INFO
        '
        Me.LBL_INFO.AutoSize = True
        Me.LBL_INFO.Location = New System.Drawing.Point(5, 556)
        Me.LBL_INFO.Name = "LBL_INFO"
        Me.LBL_INFO.Size = New System.Drawing.Size(23, 12)
        Me.LBL_INFO.TabIndex = 12
        Me.LBL_INFO.Text = "aaa"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(5, 5)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.BTN_COPY)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.TXT_SQL)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DGV_THREAD)
        Me.SplitContainer1.Panel1MinSize = 450
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.TXT_LOG)
        Me.SplitContainer1.Size = New System.Drawing.Size(694, 550)
        Me.SplitContainer1.SplitterDistance = 458
        Me.SplitContainer1.TabIndex = 13
        Me.SplitContainer1.TabStop = False
        '
        'BTN_COPY
        '
        Me.BTN_COPY.Location = New System.Drawing.Point(0, 144)
        Me.BTN_COPY.Name = "BTN_COPY"
        Me.BTN_COPY.Size = New System.Drawing.Size(447, 25)
        Me.BTN_COPY.TabIndex = 2
        Me.BTN_COPY.TabStop = False
        Me.BTN_COPY.Text = "↓グリッドへ一括コピー↓(&C)"
        Me.BTN_COPY.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.CHK_NO_LOG)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.LBL_TIMEOUT_VALUE)
        Me.GroupBox2.Controls.Add(Me.CHK_WAIT_SESSION)
        Me.GroupBox2.Controls.Add(Me.TXT_THREAD_CNT)
        Me.GroupBox2.Controls.Add(Me.CHK_CONNECT_SYNC)
        Me.GroupBox2.Controls.Add(Me.TXT_TIMEOUT_VALUE)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.TXT_EXECUTE_CNT)
        Me.GroupBox2.Controls.Add(Me.TXT_INTERVAL)
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(447, 57)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "実行条件"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "スレッド数"
        '
        'CHK_NO_LOG
        '
        Me.CHK_NO_LOG.AutoSize = True
        Me.CHK_NO_LOG.Location = New System.Drawing.Point(341, 16)
        Me.CHK_NO_LOG.Name = "CHK_NO_LOG"
        Me.CHK_NO_LOG.Size = New System.Drawing.Size(104, 16)
        Me.CHK_NO_LOG.TabIndex = 5
        Me.CHK_NO_LOG.Text = "ログを出力しない"
        Me.CHK_NO_LOG.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(100, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "起動間隔(MS)"
        '
        'LBL_TIMEOUT_VALUE
        '
        Me.LBL_TIMEOUT_VALUE.AutoSize = True
        Me.LBL_TIMEOUT_VALUE.Location = New System.Drawing.Point(5, 37)
        Me.LBL_TIMEOUT_VALUE.Name = "LBL_TIMEOUT_VALUE"
        Me.LBL_TIMEOUT_VALUE.Size = New System.Drawing.Size(73, 12)
        Me.LBL_TIMEOUT_VALUE.TabIndex = 6
        Me.LBL_TIMEOUT_VALUE.Text = "タイムアウト(S)"
        '
        'CHK_WAIT_SESSION
        '
        Me.CHK_WAIT_SESSION.AutoSize = True
        Me.CHK_WAIT_SESSION.Checked = True
        Me.CHK_WAIT_SESSION.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_WAIT_SESSION.Location = New System.Drawing.Point(254, 35)
        Me.CHK_WAIT_SESSION.Name = "CHK_WAIT_SESSION"
        Me.CHK_WAIT_SESSION.Size = New System.Drawing.Size(144, 16)
        Me.CHK_WAIT_SESSION.TabIndex = 10
        Me.CHK_WAIT_SESSION.Text = "完了時にセッションを切断"
        Me.CHK_WAIT_SESSION.UseVisualStyleBackColor = True
        '
        'CHK_CONNECT_SYNC
        '
        Me.CHK_CONNECT_SYNC.AutoSize = True
        Me.CHK_CONNECT_SYNC.Location = New System.Drawing.Point(221, 16)
        Me.CHK_CONNECT_SYNC.Name = "CHK_CONNECT_SYNC"
        Me.CHK_CONNECT_SYNC.Size = New System.Drawing.Size(117, 16)
        Me.CHK_CONNECT_SYNC.TabIndex = 4
        Me.CHK_CONNECT_SYNC.Text = "接続完了時に同期"
        Me.CHK_CONNECT_SYNC.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(121, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "処理回数"
        '
        'TXT_SQL
        '
        Me.TXT_SQL.Location = New System.Drawing.Point(0, 60)
        Me.TXT_SQL.Multiline = True
        Me.TXT_SQL.Name = "TXT_SQL"
        Me.TXT_SQL.Size = New System.Drawing.Size(447, 80)
        Me.TXT_SQL.TabIndex = 1
        Me.TXT_SQL.Text = "SELECT * FROM dual"
        '
        'DGV_THREAD
        '
        Me.DGV_THREAD.AllowUserToAddRows = False
        Me.DGV_THREAD.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_THREAD.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DGV_THREAD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_THREAD.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column4, Me.Column5, Me.Column3, Me.Column6})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_THREAD.DefaultCellStyle = DataGridViewCellStyle2
        Me.DGV_THREAD.Location = New System.Drawing.Point(0, 175)
        Me.DGV_THREAD.Name = "DGV_THREAD"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_THREAD.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DGV_THREAD.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DGV_THREAD.RowTemplate.Height = 21
        Me.DGV_THREAD.Size = New System.Drawing.Size(447, 370)
        Me.DGV_THREAD.StandardTab = True
        Me.DGV_THREAD.TabIndex = 3
        '
        'Column1
        '
        Me.Column1.HeaderText = "ID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 25
        '
        'Column2
        '
        Me.Column2.HeaderText = "SQL文"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 133
        '
        'Column4
        '
        Me.Column4.HeaderText = "開始"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 67
        '
        'Column5
        '
        Me.Column5.HeaderText = "終了"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 67
        '
        'Column3
        '
        Me.Column3.HeaderText = "ステータス"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 130
        '
        'Column6
        '
        Me.Column6.HeaderText = "処理回数"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(0, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 12)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "実行ログ"
        '
        'TXT_LOG
        '
        Me.TXT_LOG.Location = New System.Drawing.Point(0, 20)
        Me.TXT_LOG.Multiline = True
        Me.TXT_LOG.Name = "TXT_LOG"
        Me.TXT_LOG.ReadOnly = True
        Me.TXT_LOG.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TXT_LOG.Size = New System.Drawing.Size(228, 525)
        Me.TXT_LOG.TabIndex = 1
        '
        'TXT_THREAD_CNT
        '
        Me.TXT_THREAD_CNT.ErrorColor = System.Drawing.Color.Red
        Me.TXT_THREAD_CNT.ErrorMessage = "スレッド数は1以上を設定する必要があります"
        Me.TXT_THREAD_CNT.Location = New System.Drawing.Point(62, 14)
        Me.TXT_THREAD_CNT.Name = "TXT_THREAD_CNT"
        Me.TXT_THREAD_CNT.Size = New System.Drawing.Size(30, 19)
        Me.TXT_THREAD_CNT.TabIndex = 1
        Me.TXT_THREAD_CNT.Text = "5"
        Me.TXT_THREAD_CNT.ValidationExpression = "^[1-9][0-9]*$"
        '
        'TXT_TIMEOUT_VALUE
        '
        Me.TXT_TIMEOUT_VALUE.ErrorColor = System.Drawing.Color.Red
        Me.TXT_TIMEOUT_VALUE.ErrorMessage = "タイムアウトは1以上を設定する必要があります"
        Me.TXT_TIMEOUT_VALUE.Location = New System.Drawing.Point(85, 34)
        Me.TXT_TIMEOUT_VALUE.Name = "TXT_TIMEOUT_VALUE"
        Me.TXT_TIMEOUT_VALUE.Size = New System.Drawing.Size(30, 19)
        Me.TXT_TIMEOUT_VALUE.TabIndex = 7
        Me.TXT_TIMEOUT_VALUE.Text = "60"
        Me.TXT_TIMEOUT_VALUE.ValidationExpression = "^[1-9][0-9]*$"
        '
        'TXT_EXECUTE_CNT
        '
        Me.TXT_EXECUTE_CNT.ErrorColor = System.Drawing.Color.Red
        Me.TXT_EXECUTE_CNT.ErrorMessage = "実行回数は1以上を設定する必要があります"
        Me.TXT_EXECUTE_CNT.Location = New System.Drawing.Point(180, 34)
        Me.TXT_EXECUTE_CNT.Name = "TXT_EXECUTE_CNT"
        Me.TXT_EXECUTE_CNT.Size = New System.Drawing.Size(68, 19)
        Me.TXT_EXECUTE_CNT.TabIndex = 9
        Me.TXT_EXECUTE_CNT.Text = "1"
        Me.TXT_EXECUTE_CNT.ValidationExpression = "^[1-9][0-9]*$"
        '
        'TXT_INTERVAL
        '
        Me.TXT_INTERVAL.ErrorColor = System.Drawing.Color.Red
        Me.TXT_INTERVAL.ErrorMessage = "起動間隔は数字を設定する必要があります"
        Me.TXT_INTERVAL.Location = New System.Drawing.Point(183, 14)
        Me.TXT_INTERVAL.Name = "TXT_INTERVAL"
        Me.TXT_INTERVAL.Size = New System.Drawing.Size(31, 19)
        Me.TXT_INTERVAL.TabIndex = 3
        Me.TXT_INTERVAL.Text = "100"
        Me.TXT_INTERVAL.ValidationExpression = "^[0-9][0-9]*$"
        '
        'frm_Stress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.LBL_INFO)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frm_Stress"
        Me.Text = "frm_Stress"
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DGV_THREAD, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BTN_SHOW_LOG As System.Windows.Forms.Button
    Friend WithEvents BTN_ABORT As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_EXECUTE As System.Windows.Forms.Button
    Friend WithEvents LBL_INFO As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents BTN_BIND As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents CHK_WAIT_SESSION As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TXT_SQL As System.Windows.Forms.TextBox
    Friend WithEvents DGV_THREAD As System.Windows.Forms.DataGridView
    Friend WithEvents LBL_TIMEOUT_VALUE As System.Windows.Forms.Label
    Friend WithEvents TXT_LOG As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CHK_CONNECT_SYNC As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TXT_INTERVAL As CLS_COMMON.RegExTextBox
    Friend WithEvents TXT_EXECUTE_CNT As CLS_COMMON.RegExTextBox
    Friend WithEvents TXT_TIMEOUT_VALUE As CLS_COMMON.RegExTextBox
    Friend WithEvents TXT_THREAD_CNT As CLS_COMMON.RegExTextBox
    Friend WithEvents CHK_NO_LOG As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents BTN_COPY As System.Windows.Forms.Button
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
    Friend WithEvents BTN_CANCEL As System.Windows.Forms.Button
End Class
