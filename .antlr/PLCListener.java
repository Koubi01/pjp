// Generated from d:/Progro/pjpproject/PLC.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link PLCParser}.
 */
public interface PLCListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link PLCParser#program}.
	 * @param ctx the parse tree
	 */
	void enterProgram(PLCParser.ProgramContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#program}.
	 * @param ctx the parse tree
	 */
	void exitProgram(PLCParser.ProgramContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterStatement(PLCParser.StatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitStatement(PLCParser.StatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#variableDecl}.
	 * @param ctx the parse tree
	 */
	void enterVariableDecl(PLCParser.VariableDeclContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#variableDecl}.
	 * @param ctx the parse tree
	 */
	void exitVariableDecl(PLCParser.VariableDeclContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#type}.
	 * @param ctx the parse tree
	 */
	void enterType(PLCParser.TypeContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#type}.
	 * @param ctx the parse tree
	 */
	void exitType(PLCParser.TypeContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#readStmt}.
	 * @param ctx the parse tree
	 */
	void enterReadStmt(PLCParser.ReadStmtContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#readStmt}.
	 * @param ctx the parse tree
	 */
	void exitReadStmt(PLCParser.ReadStmtContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#writeStmt}.
	 * @param ctx the parse tree
	 */
	void enterWriteStmt(PLCParser.WriteStmtContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#writeStmt}.
	 * @param ctx the parse tree
	 */
	void exitWriteStmt(PLCParser.WriteStmtContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#ifStmt}.
	 * @param ctx the parse tree
	 */
	void enterIfStmt(PLCParser.IfStmtContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#ifStmt}.
	 * @param ctx the parse tree
	 */
	void exitIfStmt(PLCParser.IfStmtContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#whileStmt}.
	 * @param ctx the parse tree
	 */
	void enterWhileStmt(PLCParser.WhileStmtContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#whileStmt}.
	 * @param ctx the parse tree
	 */
	void exitWhileStmt(PLCParser.WhileStmtContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#block}.
	 * @param ctx the parse tree
	 */
	void enterBlock(PLCParser.BlockContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#block}.
	 * @param ctx the parse tree
	 */
	void exitBlock(PLCParser.BlockContext ctx);
	/**
	 * Enter a parse tree produced by the {@code orExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterOrExpr(PLCParser.OrExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code orExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitOrExpr(PLCParser.OrExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code parenExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterParenExpr(PLCParser.ParenExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code parenExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitParenExpr(PLCParser.ParenExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code eqExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterEqExpr(PLCParser.EqExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code eqExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitEqExpr(PLCParser.EqExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code notExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterNotExpr(PLCParser.NotExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code notExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitNotExpr(PLCParser.NotExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code unaryMinusExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterUnaryMinusExpr(PLCParser.UnaryMinusExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code unaryMinusExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitUnaryMinusExpr(PLCParser.UnaryMinusExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code addExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterAddExpr(PLCParser.AddExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code addExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitAddExpr(PLCParser.AddExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code literalExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterLiteralExpr(PLCParser.LiteralExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code literalExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitLiteralExpr(PLCParser.LiteralExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code mulExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterMulExpr(PLCParser.MulExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code mulExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitMulExpr(PLCParser.MulExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code relExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterRelExpr(PLCParser.RelExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code relExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitRelExpr(PLCParser.RelExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code newOP}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterNewOP(PLCParser.NewOPContext ctx);
	/**
	 * Exit a parse tree produced by the {@code newOP}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitNewOP(PLCParser.NewOPContext ctx);
	/**
	 * Enter a parse tree produced by the {@code assignExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterAssignExpr(PLCParser.AssignExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code assignExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitAssignExpr(PLCParser.AssignExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code idExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterIdExpr(PLCParser.IdExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code idExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitIdExpr(PLCParser.IdExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code andExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterAndExpr(PLCParser.AndExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code andExpr}
	 * labeled alternative in {@link PLCParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitAndExpr(PLCParser.AndExprContext ctx);
	/**
	 * Enter a parse tree produced by {@link PLCParser#literal}.
	 * @param ctx the parse tree
	 */
	void enterLiteral(PLCParser.LiteralContext ctx);
	/**
	 * Exit a parse tree produced by {@link PLCParser#literal}.
	 * @param ctx the parse tree
	 */
	void exitLiteral(PLCParser.LiteralContext ctx);
}