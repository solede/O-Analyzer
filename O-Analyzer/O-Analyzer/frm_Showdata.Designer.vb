<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_showdata
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_showdata))
        Me.BTN_EXECUTE = New System.Windows.Forms.Button()
        Me.LBL_INFO = New System.Windows.Forms.Label()
        Me.BTN_CSV = New System.Windows.Forms.Button()
        Me.BTN_COPY = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_RESTART = New System.Windows.Forms.Button()
        Me.BTN_SHOW_LOG = New System.Windows.Forms.Button()
        Me.BTN_CANCEL = New System.Windows.Forms.Button()
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.TAB_DATA = New System.Windows.Forms.TabPage()
        Me.TXT_MAX_DISP_CNT = New CLS_COMMON.RegExTextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TXT_MIN_DISP_CNT = New CLS_COMMON.RegExTextBox()
        Me.CHK_COLSIZE_REGULATE = New System.Windows.Forms.CheckBox()
        Me.DGV_DATA = New System.Windows.Forms.DataGridView()
        Me.lbl_column = New System.Windows.Forms.Label()
        Me.TXT_KOMOKU = New System.Windows.Forms.TextBox()
        Me.lbl_joken = New System.Windows.Forms.Label()
        Me.TXT_JOKEN = New System.Windows.Forms.TextBox()
        Me.TAB_COL_STAT = New System.Windows.Forms.TabPage()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DGV_COL_STAT = New System.Windows.Forms.DataGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DGV_HISTOGRAM = New System.Windows.Forms.DataGridView()
        Me.TAB_SOURCE = New System.Windows.Forms.TabPage()
        Me.TXT_SOURCE = New System.Windows.Forms.RichTextBox()
        Me.GroupBox1.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.TAB_DATA.SuspendLayout()
        CType(Me.DGV_DATA, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_COL_STAT.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.DGV_COL_STAT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DGV_HISTOGRAM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_SOURCE.SuspendLayout()
        Me.SuspendLayout()
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
        Me.LBL_INFO.Location = New System.Drawing.Point(10, 541)
        Me.LBL_INFO.Name = "LBL_INFO"
        Me.LBL_INFO.Size = New System.Drawing.Size(24, 12)
        Me.LBL_INFO.TabIndex = 6
        Me.LBL_INFO.Text = "info"
        '
        'BTN_CSV
        '
        Me.BTN_CSV.Location = New System.Drawing.Point(6, 111)
        Me.BTN_CSV.Name = "BTN_CSV"
        Me.BTN_CSV.Size = New System.Drawing.Size(68, 25)
        Me.BTN_CSV.TabIndex = 3
        Me.BTN_CSV.TabStop = False
        Me.BTN_CSV.Text = "CSV(&O)"
        Me.BTN_CSV.UseVisualStyleBackColor = True
        '
        'BTN_COPY
        '
        Me.BTN_COPY.Location = New System.Drawing.Point(6, 80)
        Me.BTN_COPY.Name = "BTN_COPY"
        Me.BTN_COPY.Size = New System.Drawing.Size(68, 25)
        Me.BTN_COPY.TabIndex = 2
        Me.BTN_COPY.TabStop = False
        Me.BTN_COPY.Text = "コピー(&C)"
        Me.BTN_COPY.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_HELP)
        Me.GroupBox1.Controls.Add(Me.BTN_RESTART)
        Me.GroupBox1.Controls.Add(Me.BTN_SHOW_LOG)
        Me.GroupBox1.Controls.Add(Me.BTN_CANCEL)
        Me.GroupBox1.Controls.Add(Me.BTN_EXECUTE)
        Me.GroupBox1.Controls.Add(Me.BTN_CSV)
        Me.GroupBox1.Controls.Add(Me.BTN_COPY)
        Me.GroupBox1.Location = New System.Drawing.Point(700, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 530)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(6, 173)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HELP.TabIndex = 25
        Me.BTN_HELP.TabStop = False
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'BTN_RESTART
        '
        Me.BTN_RESTART.Location = New System.Drawing.Point(6, 49)
        Me.BTN_RESTART.Name = "BTN_RESTART"
        Me.BTN_RESTART.Size = New System.Drawing.Size(68, 25)
        Me.BTN_RESTART.TabIndex = 19
        Me.BTN_RESTART.TabStop = False
        Me.BTN_RESTART.Text = "再開(&R)"
        Me.BTN_RESTART.UseVisualStyleBackColor = True
        '
        'BTN_SHOW_LOG
        '
        Me.BTN_SHOW_LOG.Location = New System.Drawing.Point(6, 142)
        Me.BTN_SHOW_LOG.Name = "BTN_SHOW_LOG"
        Me.BTN_SHOW_LOG.Size = New System.Drawing.Size(68, 25)
        Me.BTN_SHOW_LOG.TabIndex = 4
        Me.BTN_SHOW_LOG.TabStop = False
        Me.BTN_SHOW_LOG.Text = "ログ(&L)"
        Me.BTN_SHOW_LOG.UseVisualStyleBackColor = True
        '
        'BTN_CANCEL
        '
        Me.BTN_CANCEL.Location = New System.Drawing.Point(5, 49)
        Me.BTN_CANCEL.Name = "BTN_CANCEL"
        Me.BTN_CANCEL.Size = New System.Drawing.Size(68, 25)
        Me.BTN_CANCEL.TabIndex = 1
        Me.BTN_CANCEL.TabStop = False
        Me.BTN_CANCEL.Text = "キャンセル"
        Me.BTN_CANCEL.UseVisualStyleBackColor = True
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.TAB_DATA)
        Me.tabControl1.Controls.Add(Me.TAB_COL_STAT)
        Me.tabControl1.Controls.Add(Me.TAB_SOURCE)
        Me.tabControl1.Location = New System.Drawing.Point(5, 5)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(682, 530)
        Me.tabControl1.TabIndex = 0
        Me.tabControl1.TabStop = False
        '
        'TAB_DATA
        '
        Me.TAB_DATA.Controls.Add(Me.TXT_MAX_DISP_CNT)
        Me.TAB_DATA.Controls.Add(Me.Label3)
        Me.TAB_DATA.Controls.Add(Me.Label4)
        Me.TAB_DATA.Controls.Add(Me.TXT_MIN_DISP_CNT)
        Me.TAB_DATA.Controls.Add(Me.CHK_COLSIZE_REGULATE)
        Me.TAB_DATA.Controls.Add(Me.DGV_DATA)
        Me.TAB_DATA.Controls.Add(Me.lbl_column)
        Me.TAB_DATA.Controls.Add(Me.TXT_KOMOKU)
        Me.TAB_DATA.Controls.Add(Me.lbl_joken)
        Me.TAB_DATA.Controls.Add(Me.TXT_JOKEN)
        Me.TAB_DATA.Location = New System.Drawing.Point(4, 22)
        Me.TAB_DATA.Name = "TAB_DATA"
        Me.TAB_DATA.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_DATA.Size = New System.Drawing.Size(674, 504)
        Me.TAB_DATA.TabIndex = 0
        Me.TAB_DATA.Text = "データ"
        Me.TAB_DATA.UseVisualStyleBackColor = True
        '
        'TXT_MAX_DISP_CNT
        '
        Me.TXT_MAX_DISP_CNT.ErrorColor = System.Drawing.Color.Red
        Me.TXT_MAX_DISP_CNT.ErrorMessage = "表示終了行は0以上である必要があります"
        Me.TXT_MAX_DISP_CNT.Location = New System.Drawing.Point(612, 31)
        Me.TXT_MAX_DISP_CNT.MaxLength = 8
        Me.TXT_MAX_DISP_CNT.Name = "TXT_MAX_DISP_CNT"
        Me.TXT_MAX_DISP_CNT.Size = New System.Drawing.Size(57, 19)
        Me.TXT_MAX_DISP_CNT.TabIndex = 8
        Me.TXT_MAX_DISP_CNT.Text = "99999999"
        Me.TXT_MAX_DISP_CNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TXT_MAX_DISP_CNT.ValidationExpression = "^[0-9]*$"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(495, 34)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 12)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "表示行"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(594, 36)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(17, 12)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "～"
        '
        'TXT_MIN_DISP_CNT
        '
        Me.TXT_MIN_DISP_CNT.ErrorColor = System.Drawing.Color.Red
        Me.TXT_MIN_DISP_CNT.ErrorMessage = "表示開始行は0以上である必要があります"
        Me.TXT_MIN_DISP_CNT.Location = New System.Drawing.Point(536, 31)
        Me.TXT_MIN_DISP_CNT.MaxLength = 8
        Me.TXT_MIN_DISP_CNT.Name = "TXT_MIN_DISP_CNT"
        Me.TXT_MIN_DISP_CNT.Size = New System.Drawing.Size(57, 19)
        Me.TXT_MIN_DISP_CNT.TabIndex = 6
        Me.TXT_MIN_DISP_CNT.Text = "0"
        Me.TXT_MIN_DISP_CNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TXT_MIN_DISP_CNT.ValidationExpression = "^[0-9]*$"
        '
        'CHK_COLSIZE_REGULATE
        '
        Me.CHK_COLSIZE_REGULATE.AutoSize = True
        Me.CHK_COLSIZE_REGULATE.Location = New System.Drawing.Point(562, 5)
        Me.CHK_COLSIZE_REGULATE.Name = "CHK_COLSIZE_REGULATE"
        Me.CHK_COLSIZE_REGULATE.Size = New System.Drawing.Size(106, 16)
        Me.CHK_COLSIZE_REGULATE.TabIndex = 2
        Me.CHK_COLSIZE_REGULATE.Text = "列幅の自動調整"
        Me.CHK_COLSIZE_REGULATE.UseVisualStyleBackColor = True
        '
        'DGV_DATA
        '
        Me.DGV_DATA.AllowUserToAddRows = False
        Me.DGV_DATA.AllowUserToDeleteRows = False
        Me.DGV_DATA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_DATA.Location = New System.Drawing.Point(5, 60)
        Me.DGV_DATA.Name = "DGV_DATA"
        Me.DGV_DATA.ReadOnly = True
        Me.DGV_DATA.RowTemplate.Height = 21
        Me.DGV_DATA.Size = New System.Drawing.Size(666, 439)
        Me.DGV_DATA.StandardTab = True
        Me.DGV_DATA.TabIndex = 9
        '
        'lbl_column
        '
        Me.lbl_column.AutoSize = True
        Me.lbl_column.Location = New System.Drawing.Point(5, 5)
        Me.lbl_column.Name = "lbl_column"
        Me.lbl_column.Size = New System.Drawing.Size(29, 12)
        Me.lbl_column.TabIndex = 0
        Me.lbl_column.Text = "項目"
        '
        'TXT_KOMOKU
        '
        Me.TXT_KOMOKU.Location = New System.Drawing.Point(40, 2)
        Me.TXT_KOMOKU.Name = "TXT_KOMOKU"
        Me.TXT_KOMOKU.Size = New System.Drawing.Size(516, 19)
        Me.TXT_KOMOKU.TabIndex = 1
        '
        'lbl_joken
        '
        Me.lbl_joken.AutoSize = True
        Me.lbl_joken.Location = New System.Drawing.Point(5, 34)
        Me.lbl_joken.Name = "lbl_joken"
        Me.lbl_joken.Size = New System.Drawing.Size(29, 12)
        Me.lbl_joken.TabIndex = 3
        Me.lbl_joken.Text = "条件"
        '
        'TXT_JOKEN
        '
        Me.TXT_JOKEN.Location = New System.Drawing.Point(40, 31)
        Me.TXT_JOKEN.Name = "TXT_JOKEN"
        Me.TXT_JOKEN.Size = New System.Drawing.Size(449, 19)
        Me.TXT_JOKEN.TabIndex = 4
        '
        'TAB_COL_STAT
        '
        Me.TAB_COL_STAT.Controls.Add(Me.SplitContainer1)
        Me.TAB_COL_STAT.Location = New System.Drawing.Point(4, 22)
        Me.TAB_COL_STAT.Name = "TAB_COL_STAT"
        Me.TAB_COL_STAT.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_COL_STAT.Size = New System.Drawing.Size(674, 504)
        Me.TAB_COL_STAT.TabIndex = 1
        Me.TAB_COL_STAT.Text = "列情報"
        Me.TAB_COL_STAT.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(5, 5)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DGV_COL_STAT)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.DGV_HISTOGRAM)
        Me.SplitContainer1.Size = New System.Drawing.Size(665, 495)
        Me.SplitContainer1.SplitterDistance = 200
        Me.SplitContainer1.TabIndex = 11
        Me.SplitContainer1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "列情報"
        '
        'DGV_COL_STAT
        '
        Me.DGV_COL_STAT.AllowUserToAddRows = False
        Me.DGV_COL_STAT.AllowUserToDeleteRows = False
        Me.DGV_COL_STAT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_COL_STAT.Location = New System.Drawing.Point(0, 15)
        Me.DGV_COL_STAT.Name = "DGV_COL_STAT"
        Me.DGV_COL_STAT.ReadOnly = True
        Me.DGV_COL_STAT.RowTemplate.Height = 21
        Me.DGV_COL_STAT.Size = New System.Drawing.Size(657, 164)
        Me.DGV_COL_STAT.TabIndex = 10
        Me.DGV_COL_STAT.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 12)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "ヒストグラム統計"
        '
        'DGV_HISTOGRAM
        '
        Me.DGV_HISTOGRAM.AllowUserToAddRows = False
        Me.DGV_HISTOGRAM.AllowUserToDeleteRows = False
        Me.DGV_HISTOGRAM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_HISTOGRAM.Location = New System.Drawing.Point(0, 15)
        Me.DGV_HISTOGRAM.Name = "DGV_HISTOGRAM"
        Me.DGV_HISTOGRAM.ReadOnly = True
        Me.DGV_HISTOGRAM.RowTemplate.Height = 21
        Me.DGV_HISTOGRAM.Size = New System.Drawing.Size(657, 250)
        Me.DGV_HISTOGRAM.TabIndex = 11
        Me.DGV_HISTOGRAM.TabStop = False
        '
        'TAB_SOURCE
        '
        Me.TAB_SOURCE.Controls.Add(Me.TXT_SOURCE)
        Me.TAB_SOURCE.Location = New System.Drawing.Point(4, 22)
        Me.TAB_SOURCE.Name = "TAB_SOURCE"
        Me.TAB_SOURCE.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_SOURCE.Size = New System.Drawing.Size(674, 504)
        Me.TAB_SOURCE.TabIndex = 2
        Me.TAB_SOURCE.Text = "ソース"
        Me.TAB_SOURCE.UseVisualStyleBackColor = True
        '
        'TXT_SOURCE
        '
        Me.TXT_SOURCE.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TXT_SOURCE.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TXT_SOURCE.Location = New System.Drawing.Point(3, 3)
        Me.TXT_SOURCE.Name = "TXT_SOURCE"
        Me.TXT_SOURCE.ReadOnly = True
        Me.TXT_SOURCE.Size = New System.Drawing.Size(668, 498)
        Me.TXT_SOURCE.TabIndex = 0
        Me.TXT_SOURCE.Text = ""
        '
        'frm_showdata
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 562)
        Me.Controls.Add(Me.tabControl1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LBL_INFO)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frm_showdata"
        Me.Text = "showdata"
        Me.GroupBox1.ResumeLayout(False)
        Me.tabControl1.ResumeLayout(False)
        Me.TAB_DATA.ResumeLayout(False)
        Me.TAB_DATA.PerformLayout()
        CType(Me.DGV_DATA, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_COL_STAT.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.DGV_COL_STAT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DGV_HISTOGRAM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_SOURCE.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BTN_EXECUTE As System.Windows.Forms.Button
    Friend WithEvents LBL_INFO As System.Windows.Forms.Label
    Friend WithEvents BTN_CSV As System.Windows.Forms.Button
    Friend WithEvents BTN_COPY As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_CANCEL As System.Windows.Forms.Button
    Friend WithEvents BTN_SHOW_LOG As System.Windows.Forms.Button
    Friend WithEvents tabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TAB_DATA As System.Windows.Forms.TabPage
    Friend WithEvents DGV_DATA As System.Windows.Forms.DataGridView
    Friend WithEvents lbl_column As System.Windows.Forms.Label
    Friend WithEvents TXT_KOMOKU As System.Windows.Forms.TextBox
    Friend WithEvents lbl_joken As System.Windows.Forms.Label
    Friend WithEvents TXT_JOKEN As System.Windows.Forms.TextBox
    Friend WithEvents TAB_COL_STAT As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents DGV_COL_STAT As System.Windows.Forms.DataGridView
    Friend WithEvents DGV_HISTOGRAM As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BTN_RESTART As System.Windows.Forms.Button
    Friend WithEvents CHK_COLSIZE_REGULATE As System.Windows.Forms.CheckBox
    Friend WithEvents TXT_MAX_DISP_CNT As CLS_COMMON.RegExTextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TXT_MIN_DISP_CNT As CLS_COMMON.RegExTextBox
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
    Friend WithEvents TAB_SOURCE As System.Windows.Forms.TabPage
    Friend WithEvents TXT_SOURCE As System.Windows.Forms.RichTextBox
End Class
