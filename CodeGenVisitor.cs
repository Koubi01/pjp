using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using pjpproject;

public class CodeGenVisitor : PLCBaseVisitor<PType>
{
    private readonly Dictionary<string, PType> _symbolTable = new();
    private readonly List<string> _instructions = new();

    public List<string> GetInstructions() => _instructions;

    private static string GetTypeCode(PType type) => type switch
    {
        PType.Int => "I",
        PType.Float => "F",
        PType.Bool => "B",
        PType.String => "S",
        PType.File => "File",
        _ => "?"
    };

    private static string GetDefaultValue(PType type) => type switch
    {
        PType.Int => "0",
        PType.Float => "0.0",
        PType.Bool => "false",
        PType.String => "\"\"",
        _ => "?"
    };

    public override PType VisitVariableDecl(PLCParser.VariableDeclContext context)
    {
        var type = context.type().GetText() switch
        {
            "int" => PType.Int,
            "float" => PType.Float,
            "bool" => PType.Bool,
            "string" => PType.String,
            "file" => PType.File,
            _ => PType.Error,
        };

        foreach (var id in context.ID())
        {
            _symbolTable[id.GetText()] = type;
            _instructions.Add($"push {GetTypeCode(type)} {GetDefaultValue(type)}");
            _instructions.Add($"save {id.GetText()}");
        }

        return PType.Error;
    }
    public override PType VisitUnaryMinusExpr(PLCParser.UnaryMinusExprContext context)
    {
        var type = Visit(context.expression());

        if (type == PType.Int || type == PType.Float)
        {
            _instructions.Add($"uminus {GetTypeCode(type)}");
            return type;
        }

        return PType.Error;
    }
    public override PType VisitLiteralExpr(PLCParser.LiteralExprContext context)
    {
        var text = context.literal().GetText();
        if (context.literal().INT() != null)
        {
            _instructions.Add($"push I {text}");
            return PType.Int;
        }
        if (context.literal().FLOAT() != null)
        {
            _instructions.Add($"push F {text}");
            return PType.Float;
        }
        if (context.literal().STRING() != null)
        {
            _instructions.Add($"push S {text}");
            return PType.String;
        }
        if (text == "true" || text == "false")
        {

            _instructions.Add($"push B {text}");
            return PType.Bool;
        }
        return PType.Error;
    }

    public override PType VisitIdExpr(PLCParser.IdExprContext context)
    {
        var name = context.ID().GetText();
        if (_symbolTable.TryGetValue(name, out var type))
        {

            _instructions.Add($"load {name}");
            return type;
        }
        return PType.Error;
    }

    public override PType VisitAssignExpr(PLCParser.AssignExprContext context)
    {
        var leftExpr = context.expression(0);
        var rightExpr = context.expression(1);

        var name = leftExpr.GetText();
        var rightType = Visit(rightExpr);
        var leftType = _symbolTable.ContainsKey(name) ? _symbolTable[name] : rightType;

        if (leftType == PType.Float && rightType == PType.Int)
        {

            _instructions.Add("itof");
        }

        _instructions.Add($"save {name}");
        _instructions.Add($"load {name}");

        if (context.Parent is not PLCParser.AssignExprContext)
        {
   
            _instructions.Add("pop");
        }

        _symbolTable[name] = leftType;
        return leftType;
    }


    public override PType VisitAddExpr(PLCParser.AddExprContext context)
    {
        var leftType = Visit(context.expression(0));
        var rightType = Visit(context.expression(1));
        var op = context.GetChild(1).GetText();

        if (op == ".")
        {
            if (leftType == PType.String || rightType == PType.String)
            {

                _instructions.Add("concat");
                return PType.String;
            }
        }
        else if ((leftType == PType.Int || leftType == PType.Float) &&
                 (rightType == PType.Int || rightType == PType.Float))
        {
            if (leftType == PType.Float && rightType == PType.Int){

                _instructions.Add("itof");}
            if (leftType == PType.Int && rightType == PType.Float){

                _instructions.Insert(_instructions.Count - 1, "itof");}
                

            _instructions.Add(op switch
            {
                "+" => $"add {GetTypeCode(PromoteType(leftType, rightType))}",
                "-" => $"sub {GetTypeCode(PromoteType(leftType, rightType))}",
                _ => "ERROR"
            });

            return PromoteType(leftType, rightType);
        }

        return PType.Error;
    }
    public override PType VisitParenExpr(PLCParser.ParenExprContext context)
    {
        return Visit(context.expression());
    }
    public override PType VisitMulExpr(PLCParser.MulExprContext context)
    {
        var leftType = Visit(context.expression(0));
        var rightType = Visit(context.expression(1));
        var op = context.GetChild(1).GetText();

        if ((leftType == PType.Int || leftType == PType.Float) &&
            (rightType == PType.Int || rightType == PType.Float))
        {
            if (leftType == PType.Float && rightType == PType.Int){

                _instructions.Add("itof");}
            if (leftType == PType.Int && rightType == PType.Float){

                _instructions.Insert(_instructions.Count - 1, "itof");}


            _instructions.Add(op switch
            {
                "*" => $"mul {GetTypeCode(PromoteType(leftType, rightType))}",
                "/" => $"div {GetTypeCode(PromoteType(leftType, rightType))}",
                "%" => "mod",
                _ => "ERROR"
            });

            return PromoteType(leftType, rightType);
        }

        return PType.Error;
    }

    public override PType VisitWriteStmt(PLCParser.WriteStmtContext context)
    {
        int exprCount = 0;
        foreach (var expr in context.expression())
        {
            Visit(expr);
            exprCount++;
        }
        _instructions.Add($"print {exprCount}");
        return PType.Error;
    }

    public override PType VisitReadStmt(PLCParser.ReadStmtContext context)
    {
        foreach (var id in context.ID())
        {
            var name = id.GetText();
            if (_symbolTable.TryGetValue(name, out var type))
            {
                _instructions.Add($"read {GetTypeCode(type)}");
                _instructions.Add($"save {name}");
            }
        }
        return PType.Error;
    }

    private static PType PromoteType(PType a, PType b)
    {
        return (a == PType.Float || b == PType.Float) ? PType.Float : PType.Int;
    }
    public override PType VisitBlock(PLCParser.BlockContext context)
        {
            foreach (var stmt in context.statement())
                Visit(stmt);
            return PType.Error;
        }

        public override PType VisitRelExpr(PLCParser.RelExprContext context)
        {
            var leftType = Visit(context.expression(0));
            var rightType = Visit(context.expression(1));
            var op = context.GetChild(1).GetText();
            string typeCode = GetTypeCode(leftType);

            if (leftType == PType.Int && rightType == PType.Float){
                _instructions.Insert(_instructions.Count - 1, "itof");
                typeCode = GetTypeCode(PromoteType(leftType, rightType));}
            if (leftType == PType.Float && rightType == PType.Int){
                _instructions.Insert(_instructions.Count - 2, "itof");
                typeCode = GetTypeCode(PromoteType(leftType, rightType));}
          
            if (op == "<")
                _instructions.Add($"lt {typeCode}");
            else if (op == ">")
                _instructions.Add($"gt {typeCode}");

            return PType.Bool;
        }


        public override PType VisitEqExpr(PLCParser.EqExprContext context)
        {
            var leftType = Visit(context.expression(0));
            var rightType = Visit(context.expression(1));
            var op = context.GetChild(1).GetText();
            string typeCode = GetTypeCode(leftType);

            if (leftType == PType.Int && rightType == PType.Float){
                _instructions.Insert(_instructions.Count - 1, "itof");
                 typeCode = GetTypeCode(PromoteType(leftType, rightType));}
            if (leftType == PType.Float && rightType == PType.Int){
                _instructions.Insert(_instructions.Count - 2, "itof");
                 typeCode = GetTypeCode(PromoteType(leftType, rightType));}

           
            _instructions.Add($"eq {typeCode}");

            if (op == "!=")
                _instructions.Add("not");

            return PType.Bool;
        }


        public override PType VisitOrExpr(PLCParser.OrExprContext context)
        {
            Visit(context.expression(0));
            Visit(context.expression(1));
            _instructions.Add("or");
            return PType.Bool;
        }

        public override PType VisitAndExpr(PLCParser.AndExprContext context)
        {
            Visit(context.expression(0));
            Visit(context.expression(1));
            _instructions.Add("and");
            return PType.Bool;
        }


        public override PType VisitNotExpr(PLCParser.NotExprContext context)
        {
            Visit(context.expression());
            _instructions.Add("not");
            return PType.Bool;
        }


        private int _labelCounter = 0;

       public override PType VisitIfStmt(PLCParser.IfStmtContext context)
        {
            int elseLabel = _labelCounter++;
            int endLabel = _labelCounter++;

            Visit(context.expression());
            _instructions.Add($"fjmp {elseLabel}");

            Visit(context.statement(0));

            if (context.statement().Length > 1)
            {
                _instructions.Add($"jmp {endLabel}");
                _instructions.Add($"label {elseLabel}");
                Visit(context.statement(1));
                _instructions.Add($"label {endLabel}");
            }
            else
            {
                _instructions.Add($"jmp {endLabel}");
                _instructions.Add($"label {elseLabel}");
                _instructions.Add($"label {endLabel}");
            }

            return PType.Error;
        }


        public override PType VisitWhileStmt(PLCParser.WhileStmtContext context)
        {
            int startLabel = _labelCounter++;
            int endLabel = _labelCounter++;

            _instructions.Add($"label {startLabel}");
            Visit(context.expression()); 
            _instructions.Add($"fjmp {endLabel}");

            Visit(context.statement()); 
            _instructions.Add($"jmp {startLabel}");
            _instructions.Add($"label {endLabel}");

            return PType.Error;
        }

    public override PType VisitFopenStmt(PLCParser.FopenStmtContext context)
    {
        string varName = context.ID().GetText();

        Visit(context.expression());

        _instructions.Add("fopen");
        _instructions.Add($"save {varName}");
        return PType.Error;
    }


    public override PType VisitFappendStmt(PLCParser.FappendStmtContext context)
    {
        int fappendCounter = 0;
        
        string varName = context.ID().GetText();
        _instructions.Add($"load {varName}");
        foreach (var expres in context.expression()){
            Visit(expres);
            fappendCounter++;
        }

        _instructions.Add($"fappend {fappendCounter}");

        return PType.Error;
    }
}
