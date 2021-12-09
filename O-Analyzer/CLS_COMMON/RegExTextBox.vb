Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class RegExTextBox
    Inherits System.Windows.Forms.TextBox

    Protected validationPattern As String
    Protected mErrorMessage As String
    Protected mValidationExpression As Regex
    Protected mErrorColor As Color = Color.Red

    'ユーザ定義エラーメッセージ
    Public Property ErrorMessage() As String
        Get
            Return mErrorMessage
        End Get
        Set(ByVal Value As String)
            mErrorMessage = Value
        End Set
    End Property

    'ユーザ定義エラーカラー
    Public Property ErrorColor() As Color
        Get
            Return mErrorColor
        End Get
        Set(ByVal Value As Color)
            mErrorColor = Value
        End Set
    End Property

    '検証
    Public ReadOnly Property IsValid() As Boolean
        Get
            If Not mValidationExpression Is Nothing Then
                Return mValidationExpression.IsMatch(Me.Text)
            Else
                Return True
            End If
        End Get
    End Property

    '正規表現の設定
    Public Property ValidationExpression() As String
        Get
            Return validationPattern
        End Get
        Set(ByVal Value As String)
            mValidationExpression = New Regex(Value)
            validationPattern = Value
        End Set
    End Property

    'エラー時、正常時のカラー変更イベント
    Protected Overrides Sub OnValidated(ByVal e As System.EventArgs)
        If Not Me.IsValid Then
            Me.ForeColor = mErrorColor
        Else
            Me.ForeColor = DefaultForeColor
        End If

        MyBase.OnValidated(e)
    End Sub
End Class
