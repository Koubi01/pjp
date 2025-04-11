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
    : expression '=' expression               #assignExpr
    | expression '||' expression              #orExpr
    | expression '&&' expression              #andExpr
    | expression ('==' | '!=') expression     #eqExpr
    | expression ('<' | '>') expression       #relExpr
    | expression ('+' | '-' | '.') expression #addExpr
    | expression ('*' | '/' | '%') expression #mulExpr
    | '!' expression                          #notExpr
    | '-' expression                          #unaryMinusExpr
    | '(' expression ')'                      #parenExpr
    | literal                                 #literalExpr
    | ID                                      #idExpr
    ;

literal
    : INT 
    | FLOAT 
    | STRING 
    | 'true' 
    | 'false'
    ;

ID: [a-zA-Z][a-zA-Z0-9]*;
INT: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: '"' .*? '"';
WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;
