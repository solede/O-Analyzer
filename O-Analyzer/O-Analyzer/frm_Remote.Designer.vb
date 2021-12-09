<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Remote
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Remote))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_EXECUTE = New System.Windows.Forms.Button()
        Me.BTN_SHOW_LOG = New System.Windows.Forms.Button()
        Me.LBL_INFO = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TAB_PREPARE = New System.Windows.Forms.TabPage()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.BTN_PERMISSION = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.DGV_JAVA_PERMISSION = New System.Windows.Forms.DataGridView()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TXT_MEMO = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TXT_PLSQL_PROC = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TXT_JAVA_PROC = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TAB_EXECUTE = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CMB_ENCODE = New System.Windows.Forms.ComboBox()
        Me.CMB_EXEC_PROCESS = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TXT_EXEC_CMD = New System.Windows.Forms.TextBox()
        Me.TXT_JAVA_PROC2 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TXT_RESULT_LOG = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TAB_PREPARE.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.DGV_JAVA_PERMISSION, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TAB_EXECUTE.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_HELP)
        Me.GroupBox1.Controls.Add(Me.BTN_EXECUTE)
        Me.GroupBox1.Controls.Add(Me.BTN_SHOW_LOG)
        Me.GroupBox1.Location = New System.Drawing.Point(701, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 533)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(6, 80)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HELP.TabIndex = 24
        Me.BTN_HELP.TabStop = False
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'BTN_EXECUTE
        '
        Me.BTN_EXECUTE.Location = New System.Drawing.Point(6, 18)
        Me.BTN_EXECUTE.Name = "BTN_EXECUTE"
        Me.BTN_EXECUTE.Size = New System.Drawing.Size(68, 25)
        Me.BTN_EXECUTE.TabIndex = 1
        Me.BTN_EXECUTE.TabStop = False
        Me.BTN_EXECUTE.Text = "実行(&E)"
        Me.BTN_EXECUTE.UseVisualStyleBackColor = True
        '
        'BTN_SHOW_LOG
        '
        Me.BTN_SHOW_LOG.Location = New System.Drawing.Point(6, 49)
        Me.BTN_SHOW_LOG.Name = "BTN_SHOW_LOG"
        Me.BTN_SHOW_LOG.Size = New System.Drawing.Size(68, 25)
        Me.BTN_SHOW_LOG.TabIndex = 2
        Me.BTN_SHOW_LOG.TabStop = False
        Me.BTN_SHOW_LOG.Text = "ログ(&L)"
        Me.BTN_SHOW_LOG.UseVisualStyleBackColor = True
        '
        'LBL_INFO
        '
        Me.LBL_INFO.AutoSize = True
        Me.LBL_INFO.Location = New System.Drawing.Point(7, 541)
        Me.LBL_INFO.Name = "LBL_INFO"
        Me.LBL_INFO.Size = New System.Drawing.Size(40, 12)
        Me.LBL_INFO.TabIndex = 3
        Me.LBL_INFO.Tag = "label"
        Me.LBL_INFO.Text = "LABEL"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TAB_PREPARE)
        Me.TabControl1.Controls.Add(Me.TAB_EXECUTE)
        Me.TabControl1.Location = New System.Drawing.Point(5, 5)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(690, 533)
        Me.TabControl1.TabIndex = 1
        Me.TabControl1.TabStop = False
        '
        'TAB_PREPARE
        '
        Me.TAB_PREPARE.Controls.Add(Me.Label5)
        Me.TAB_PREPARE.Controls.Add(Me.GroupBox4)
        Me.TAB_PREPARE.Controls.Add(Me.GroupBox3)
        Me.TAB_PREPARE.Controls.Add(Me.GroupBox2)
        Me.TAB_PREPARE.Location = New System.Drawing.Point(4, 22)
        Me.TAB_PREPARE.Name = "TAB_PREPARE"
        Me.TAB_PREPARE.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_PREPARE.Size = New System.Drawing.Size(682, 507)
        Me.TAB_PREPARE.TabIndex = 1
        Me.TAB_PREPARE.Text = "準備"
        Me.TAB_PREPARE.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(5, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(436, 12)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "本機能はJAVAストアドプロシージャを使用するためORACLE JVMのインストールが必要です"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.BTN_PERMISSION)
        Me.GroupBox4.Controls.Add(Me.Label11)
        Me.GroupBox4.Controls.Add(Me.DGV_JAVA_PERMISSION)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.TXT_MEMO)
        Me.GroupBox4.Location = New System.Drawing.Point(5, 143)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(671, 358)
        Me.GroupBox4.TabIndex = 11
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "3 JAVAの権限付与(反映には再接続が必要)"
        '
        'BTN_PERMISSION
        '
        Me.BTN_PERMISSION.Location = New System.Drawing.Point(490, 176)
        Me.BTN_PERMISSION.Name = "BTN_PERMISSION"
        Me.BTN_PERMISSION.Size = New System.Drawing.Size(175, 25)
        Me.BTN_PERMISSION.TabIndex = 16
        Me.BTN_PERMISSION.TabStop = False
        Me.BTN_PERMISSION.Text = "確認"
        Me.BTN_PERMISSION.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(6, 182)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(87, 12)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "権限の設定状態"
        '
        'DGV_JAVA_PERMISSION
        '
        Me.DGV_JAVA_PERMISSION.AllowUserToAddRows = False
        Me.DGV_JAVA_PERMISSION.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_JAVA_PERMISSION.Location = New System.Drawing.Point(5, 207)
        Me.DGV_JAVA_PERMISSION.Name = "DGV_JAVA_PERMISSION"
        Me.DGV_JAVA_PERMISSION.RowTemplate.Height = 21
        Me.DGV_JAVA_PERMISSION.Size = New System.Drawing.Size(660, 145)
        Me.DGV_JAVA_PERMISSION.TabIndex = 14
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(6, 15)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(320, 12)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "以下を参考に実行したいOSコマンドに対して権限を付与してください"
        '
        'TXT_MEMO
        '
        Me.TXT_MEMO.Location = New System.Drawing.Point(5, 32)
        Me.TXT_MEMO.Multiline = True
        Me.TXT_MEMO.Name = "TXT_MEMO"
        Me.TXT_MEMO.ReadOnly = True
        Me.TXT_MEMO.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TXT_MEMO.Size = New System.Drawing.Size(660, 136)
        Me.TXT_MEMO.TabIndex = 1
        Me.TXT_MEMO.Text = resources.GetString("TXT_MEMO.Text")
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.TXT_PLSQL_PROC)
        Me.GroupBox3.Controls.Add(Me.Button2)
        Me.GroupBox3.Location = New System.Drawing.Point(5, 86)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(671, 51)
        Me.GroupBox3.TabIndex = 10
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "2 JAVAプロシージャのコールファンクションの作成"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(5, 23)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 12)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "ファンクション名"
        '
        'TXT_PLSQL_PROC
        '
        Me.TXT_PLSQL_PROC.Location = New System.Drawing.Point(115, 20)
        Me.TXT_PLSQL_PROC.Name = "TXT_PLSQL_PROC"
        Me.TXT_PLSQL_PROC.Size = New System.Drawing.Size(171, 19)
        Me.TXT_PLSQL_PROC.TabIndex = 9
        Me.TXT_PLSQL_PROC.Text = "EXECUTE_OS_CMD"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(490, 17)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(175, 25)
        Me.Button2.TabIndex = 3
        Me.Button2.TabStop = False
        Me.Button2.Text = "作成"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.TXT_JAVA_PROC)
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Location = New System.Drawing.Point(5, 29)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(671, 51)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "1 JAVAプロシージャの作成"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(5, 23)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(104, 12)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "JAVAプロシージャ名"
        '
        'TXT_JAVA_PROC
        '
        Me.TXT_JAVA_PROC.Location = New System.Drawing.Point(115, 20)
        Me.TXT_JAVA_PROC.Name = "TXT_JAVA_PROC"
        Me.TXT_JAVA_PROC.Size = New System.Drawing.Size(171, 19)
        Me.TXT_JAVA_PROC.TabIndex = 9
        Me.TXT_JAVA_PROC.Text = "J_EXECUTE_OS_CMD"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(490, 17)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(175, 25)
        Me.Button1.TabIndex = 2
        Me.Button1.TabStop = False
        Me.Button1.Text = "作成"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TAB_EXECUTE
        '
        Me.TAB_EXECUTE.Controls.Add(Me.Label2)
        Me.TAB_EXECUTE.Controls.Add(Me.CMB_ENCODE)
        Me.TAB_EXECUTE.Controls.Add(Me.CMB_EXEC_PROCESS)
        Me.TAB_EXECUTE.Controls.Add(Me.Label4)
        Me.TAB_EXECUTE.Controls.Add(Me.Label3)
        Me.TAB_EXECUTE.Controls.Add(Me.TXT_EXEC_CMD)
        Me.TAB_EXECUTE.Controls.Add(Me.TXT_JAVA_PROC2)
        Me.TAB_EXECUTE.Controls.Add(Me.Label1)
        Me.TAB_EXECUTE.Controls.Add(Me.TXT_RESULT_LOG)
        Me.TAB_EXECUTE.Location = New System.Drawing.Point(4, 22)
        Me.TAB_EXECUTE.Name = "TAB_EXECUTE"
        Me.TAB_EXECUTE.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_EXECUTE.Size = New System.Drawing.Size(682, 507)
        Me.TAB_EXECUTE.TabIndex = 0
        Me.TAB_EXECUTE.Text = "実行"
        Me.TAB_EXECUTE.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(462, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 12)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "サーバー側エンコード"
        '
        'CMB_ENCODE
        '
        Me.CMB_ENCODE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_ENCODE.FormattingEnabled = True
        Me.CMB_ENCODE.Items.AddRange(New Object() {"UTF-8", "MS932", "EUC-JP", "ASCII"})
        Me.CMB_ENCODE.Location = New System.Drawing.Point(570, 7)
        Me.CMB_ENCODE.Name = "CMB_ENCODE"
        Me.CMB_ENCODE.Size = New System.Drawing.Size(105, 20)
        Me.CMB_ENCODE.TabIndex = 9
        '
        'CMB_EXEC_PROCESS
        '
        Me.CMB_EXEC_PROCESS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_EXEC_PROCESS.FormattingEnabled = True
        Me.CMB_EXEC_PROCESS.Items.AddRange(New Object() {" ", "1 アラートログ表示", "2 アラートログのエラー抽出"})
        Me.CMB_EXEC_PROCESS.Location = New System.Drawing.Point(88, 6)
        Me.CMB_EXEC_PROCESS.Name = "CMB_EXEC_PROCESS"
        Me.CMB_EXEC_PROCESS.Size = New System.Drawing.Size(172, 20)
        Me.CMB_EXEC_PROCESS.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(5, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 12)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "定型処理表示"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(109, 12)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "実行コマンド(フルパス)"
        '
        'TXT_EXEC_CMD
        '
        Me.TXT_EXEC_CMD.Location = New System.Drawing.Point(121, 32)
        Me.TXT_EXEC_CMD.Name = "TXT_EXEC_CMD"
        Me.TXT_EXEC_CMD.Size = New System.Drawing.Size(554, 19)
        Me.TXT_EXEC_CMD.TabIndex = 4
        '
        'TXT_JAVA_PROC2
        '
        Me.TXT_JAVA_PROC2.Location = New System.Drawing.Point(346, 6)
        Me.TXT_JAVA_PROC2.Name = "TXT_JAVA_PROC2"
        Me.TXT_JAVA_PROC2.Size = New System.Drawing.Size(110, 19)
        Me.TXT_JAVA_PROC2.TabIndex = 6
        Me.TXT_JAVA_PROC2.Text = "EXECUTE_OS_CMD"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(266, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 12)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "ファンクション名"
        '
        'TXT_RESULT_LOG
        '
        Me.TXT_RESULT_LOG.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TXT_RESULT_LOG.Location = New System.Drawing.Point(5, 60)
        Me.TXT_RESULT_LOG.Multiline = True
        Me.TXT_RESULT_LOG.Name = "TXT_RESULT_LOG"
        Me.TXT_RESULT_LOG.ReadOnly = True
        Me.TXT_RESULT_LOG.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TXT_RESULT_LOG.Size = New System.Drawing.Size(771, 472)
        Me.TXT_RESULT_LOG.TabIndex = 8
        '
        'frm_Remote
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 562)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LBL_INFO)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frm_Remote"
        Me.Text = "リモート実行"
        Me.GroupBox1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TAB_PREPARE.ResumeLayout(False)
        Me.TAB_PREPARE.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.DGV_JAVA_PERMISSION, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TAB_EXECUTE.ResumeLayout(False)
        Me.TAB_EXECUTE.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_EXECUTE As System.Windows.Forms.Button
    Friend WithEvents BTN_SHOW_LOG As System.Windows.Forms.Button
    Friend WithEvents LBL_INFO As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TAB_EXECUTE As System.Windows.Forms.TabPage
    Friend WithEvents TXT_RESULT_LOG As System.Windows.Forms.TextBox
    Friend WithEvents TAB_PREPARE As System.Windows.Forms.TabPage
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TXT_EXEC_CMD As System.Windows.Forms.TextBox
    Friend WithEvents TXT_JAVA_PROC2 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TXT_MEMO As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CMB_EXEC_PROCESS As System.Windows.Forms.ComboBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CMB_ENCODE As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TXT_JAVA_PROC As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TXT_PLSQL_PROC As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents DGV_JAVA_PERMISSION As System.Windows.Forms.DataGridView
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents BTN_PERMISSION As System.Windows.Forms.Button
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
End Class
