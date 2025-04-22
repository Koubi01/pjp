using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using Antlr4.Runtime;


namespace pjpproject
{
    public enum PType { Int, Float, Bool, String, File ,Error }

    public class TypeCheckerVisitor : PLCBaseVisitor<PType>
    {
        private readonly Dictionary<string, PType> _symbolTable = new();

        public override PType VisitVariableDecl(PLCParser.VariableDeclContext context)
        {
            var declaredType = GetTypeFromContext(context.type());
            foreach (var id in context.ID())
            {
                string varName = id.GetText();
                if (_symbolTable.ContainsKey(varName))
                {
                    Errors.ReportError(id.Symbol, $"Variable '{varName}' is already declared.");
                }
                else
                {
                    _symbolTable[varName] = declaredType;
                }
            }
            return PType.Error;
        }

        public override PType VisitIdExpr(PLCParser.IdExprContext context)
        {
            string varName = context.ID().GetText();
            if (!_symbolTable.TryGetValue(varName, out var type))
            {
                Errors.ReportError(context.ID().Symbol, $"Variable '{varName}' is not declared.");
                return PType.Error;
            }
            return type;
        }

        public override PType VisitLiteralExpr(PLCParser.LiteralExprContext context)
        {
            var lit = context.literal();
           
            if (lit.INT() != null) return PType.Int;
            if (lit.FLOAT() != null) return PType.Float;
            if (lit.STRING() != null) return PType.String;
            if (lit.GetText() == "true" || lit.GetText() == "false") return PType.Bool;

            return PType.Error;
        }
        public override PType VisitAssignExpr(PLCParser.AssignExprContext context)
        {
            var left = context.expression(0);
            var rightType = Visit(context.expression(1));

            if (left is not PLCParser.IdExprContext)
            {
                Errors.ReportError(left.Start, "Left side of assignment must be a variable.");
                return PType.Error;
            }

            var leftVarName = left.GetText();
            if (!_symbolTable.TryGetValue(leftVarName, out var varType))
            {
                Errors.ReportError(context.Start, $"Variable '{leftVarName}' is not declared.");
                return PType.Error;
            }
            if (!IsAssignable(varType, rightType))
            {
                Errors.ReportError(context.Start, $"Cannot assign {rightType} to variable of type {varType}.");
                return PType.Error;
            }

            return varType;
        }
        public override PType VisitParenExpr(PLCParser.ParenExprContext context)
        {
            return Visit(context.expression());
        }

       public override PType VisitNewOP(PLCParser.NewOPContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            if (left != PType.String)
            {
                Errors.ReportError(context.expression(0).Start, $"Left operand of '<<' must be a string, got {left}.");
                return PType.Error;
            }

            if (right == PType.String || right == PType.Int || right == PType.Float || right == PType.Bool)
            {
                return PType.String;
            }

            Errors.ReportError(context.expression(1).Start, $"Right operand of '<<' must be a printable type (int, float, bool, string), got {right}.");
            return PType.Error;
        }

        public override PType VisitAddExpr(PLCParser.AddExprContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var op = context.GetChild(1).GetText();

            if (op == ".")
            {
                if (left == PType.String && right == PType.String) return PType.String;
            }
            else if ((left == PType.Int || left == PType.Float) && (right == PType.Int || right == PType.Float))
            {
                return PromoteType(left, right);
            }

            Errors.ReportError(context.Start, $"Invalid operands for '{op}': {left}, {right}");
            return PType.Error;
        }

        public override PType VisitMulExpr(PLCParser.MulExprContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var op = context.GetChild(1).GetText();

            if (op == "%" && left == PType.Int && right == PType.Int)
                return PType.Int;
            if ((left == PType.Int || left == PType.Float) && (right == PType.Int || right == PType.Float) && op != "%")
                return PromoteType(left, right);

            Errors.ReportError(context.Start, $"Invalid operands for '{op}': {left}, {right}");
            return PType.Error;
        }

        public override PType VisitRelExpr(PLCParser.RelExprContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            if (AreBothNumeric(left, right))
                return PType.Bool;

            Errors.ReportError(context.Start, "Relational operators require numeric operands.");
            return PType.Error;
        }

        public override PType VisitEqExpr(PLCParser.EqExprContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            if (left == PType.Error || right == PType.Error)
            {
                return PType.Error;
            }              

            if (left == right || AreBothNumeric(left, right))
            {
                return PType.Bool;
            }

            Errors.ReportError(context.Start, $"Cannot compare {left} and {right}.");
            return PType.Error;
        }

        public override PType VisitOrExpr(PLCParser.OrExprContext context)
        {
            return CheckBoolBinary(context, "||");
        }

        public override PType VisitAndExpr(PLCParser.AndExprContext context)
        {
            return CheckBoolBinary(context, "&&");
        }

        public override PType VisitNotExpr(PLCParser.NotExprContext context)
        {
            var operand = Visit(context.expression());

            if (operand != PType.Bool)
            {
                Errors.ReportError(context.Start, "Operator '!' requires a boolean operand.");
                return PType.Error;
            }

            return PType.Bool;
        }

        public override PType VisitUnaryMinusExpr(PLCParser.UnaryMinusExprContext context)
        {
            var operand = Visit(context.expression());
            if (operand == PType.Int || operand == PType.Float)
                return operand;

            Errors.ReportError(context.Start, "Unary minus requires numeric operand.");
            return PType.Error;
        }

        public override PType VisitIfStmt(PLCParser.IfStmtContext context)
        {
            var condType = Visit(context.expression());
            if (condType != PType.Bool)
                Errors.ReportError(context.expression().Start, "Condition in 'if' must be boolean.");

            Visit(context.statement(0));
            if (context.statement().Length > 1)
                Visit(context.statement(1));

            return PType.Error;
        }

        public override PType VisitWhileStmt(PLCParser.WhileStmtContext context)
        {
            var condType = Visit(context.expression());
            if (condType != PType.Bool)
                Errors.ReportError(context.expression().Start, "Condition in 'while' must be boolean.");

            Visit(context.statement());
            return PType.Error;
        }

        public override PType VisitBlock(PLCParser.BlockContext context)
        {
            foreach (var stmt in context.statement())
                Visit(stmt);
            return PType.Error;
        }

        public override PType VisitWriteStmt(PLCParser.WriteStmtContext context)
        {
            foreach (var expr in context.expression())
                Visit(expr);
            return PType.Error;
        }

        public override PType VisitReadStmt(PLCParser.ReadStmtContext context)
        {
            foreach (var id in context.ID())
            {
                string name = id.GetText();
                if (!_symbolTable.ContainsKey(name))
                    Errors.ReportError(id.Symbol, $"Variable '{name}' is not declared.");
            }
            return PType.Error;
        }

        private PType GetTypeFromContext(PLCParser.TypeContext ctx)
        {
            return ctx.GetText() switch
            {
                "int" => PType.Int,
                "float" => PType.Float,
                "bool" => PType.Bool,
                "string" => PType.String,
                _ => PType.Error,
            };
        }

        private static bool AreBothNumeric(PType a, PType b)
        {
            return (a == PType.Int || a == PType.Float) && (b == PType.Int || b == PType.Float);
        }

        private static PType PromoteType(PType a, PType b) =>
            (a == PType.Float || b == PType.Float) ? PType.Float : PType.Int;

        private static bool IsAssignable(PType target, PType value)
        {
            if (target == value) return true;
            if (target == PType.Float && value == PType.Int) return true;
            return false;
        }

        private PType CheckBoolBinary(ParserRuleContext context, string op)
        {
            var left = Visit(context.GetRuleContext<PLCParser.ExpressionContext>(0));
            var right = Visit(context.GetRuleContext<PLCParser.ExpressionContext>(1));
            if (left == PType.Bool && right == PType.Bool)
                return PType.Bool;

            Errors.ReportError(context.Start, $"Operator '{op}' requires boolean operands.");
            return PType.Error;
        }
    }
}
