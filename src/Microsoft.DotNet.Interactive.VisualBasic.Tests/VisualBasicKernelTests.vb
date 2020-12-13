Imports System
Imports System.Threading.Tasks
Imports FluentAssertions
Imports Microsoft.DotNet.Interactive.Commands
Imports Microsoft.DotNet.Interactive.Tests
Imports Xunit
Imports Xunit.Abstractions
Imports Microsoft.DotNet.Interactive.VisualBasic
Imports System.Reflection

Namespace Microsoft.DotNet.Interactive.VisualBasic.Tests
    Public Class VisualBasicKernelTests
        Inherits LanguageKernelTestBase

        Public Sub New(output As ITestOutputHelper)
            MyBase.New(output)
        End Sub

        <Fact>
        Public Async Function Script_state_is_available_within_middleware_pipeline() As Task
            Dim variableCountBeforeEvaluation = 0
            Dim variableCountAfterEvaluation = 0

            Dim kernel = New VisualBasicKernel()
            kernel.AddMiddleware(Async(Command, Context, Continuation), Sub()
                                                                            Dim k = Context.HandlingKernel As VisualBasicKernel
                                                                            Await Continuation(Command, Context)
                                                                            variableCountAfterEvaluation = k.ScriptState.Variables.Length
                                                                        End Sub)

            Await kernel.SendAsync(New SubmitCode("var x = 1;"))

            variableCountBeforeEvaluation.Should().Be(0)
            variableCountAfterEvaluation.Should().Be(1)

        End Function
    End Class

End Namespace

