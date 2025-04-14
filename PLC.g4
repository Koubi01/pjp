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
    : expression '||' expression              #orExpr
    | expression '&&' expression              #andExpr
    | expression ('==' | '!=') expression     #eqExpr
    | expression ('<' | '>') expression       #relExpr
    | expression ('+' | '-' | '.') expression #addExpr
    | expression ('*' | '/' | '%') expression #mulExpr    
    | '-' expression                          #unaryMinusExpr
    | '!' expression                          #notExpr    
    | ID                                      #idExpr
    | literal                                 #literalExpr
    | <assoc=right> expression '=' expression #assignExpr 
    | '(' expression ')'                      #parenExpr
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