grammar PLC;

program: statement* EOF;

statement
    : variableDecl
    | expression ';'
    | readStmt
    | writeStmt
    | ifStmt
    | whileStmt
    | block
    | ';'
    ;

variableDecl: type ID (',' ID)* ';';
type: 'int' | 'float' | 'bool' | 'string';

readStmt: 'read' ID (',' ID)* ';';
writeStmt: 'write' expression (',' expression)* ';';

ifStmt: 'if' '(' expression ')' statement ('else' statement)?;
whileStmt: 'while' '(' expression ')' statement;
block: '{' statement* '}';

expression
    : '!' expression                                       #notExpr    
    | '-' expression                                       #unaryMinusExpr
    | <assoc=left> expression ('*' | '/' | '%') expression #mulExpr    
    | <assoc=left> expression ('+' | '-' | '.') expression #addExpr
    | <assoc=left> expression ('<' | '>') expression       #relExpr
    | <assoc=left> expression ('==' | '!=') expression     #eqExpr
    | <assoc=left> expression '&&' expression              #andExpr
    | <assoc=left> expression '||' expression              #orExpr
    | ID                                                   #idExpr
    | literal                                              #literalExpr
    | <assoc=left> expression '<<' expression              #newOP
    | <assoc=right> expression '=' expression              #assignExpr 
    | '(' expression ')'                                   #parenExpr
    ;

literal
    : INT 
    | FLOAT 
    | STRING 
    | 'true' 
    | 'false'
    ;

ID: [a-zA-Z][a-zA-Z0-9]*;
FLOAT: [0-9]+ '.' [0-9]+;
INT: [0-9]+;
STRING: '"' .*? '"';
COMMENT: '//' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;