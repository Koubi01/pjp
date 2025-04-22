grammar PLC;

program: statement* EOF;

statement
    : variableDecl
    | expression ';'
    | readStmt
    | writeStmt
    | ifStmt
    | whileStmt
    | fopenStmt
    | fappendStmt
    | block
    | ';'
    ;

variableDecl: type ID (',' ID)* ';';
type: 'int' | 'float' | 'bool' | 'string' | 'file';

readStmt: 'read' ID (',' ID)* ';';
writeStmt: 'write' expression (',' expression)* ';';

ifStmt: 'if' '(' expression ')' statement ('else' statement)?;
whileStmt: 'while' '(' expression ')' statement;

fopenStmt: 'fopen' ID ',' expression ';';
fappendStmt: 'fappend' ID (',' expression)+ ';';

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
    | FILE
    | 'true' 
    | 'false'
    ;

ID: [a-zA-Z][a-zA-Z0-9]*;
FLOAT: [0-9]+ '.' [0-9]+;
INT: [0-9]+;
FILE: '"' [a-zA-Z][a-zA-Z0-9]* '.' [a-zA-Z] '"';
STRING: '"' .*? '"';
COMMENT: '//' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;