grammar Code;

/*Parser rules*/
program: BEGIN NEWLINE (declaration NEWLINE)* lines* END (NEWLINE)* EOF;
lines:((assignment | colonFunc | ifBlock | whileBlock) NEWLINE)+;
ifBlock: IF '(' expression ')' NEWLINE block (NEWLINE ELSE elseIfBlock)?;
elseIfBlock: NEWLINE block | ifBlock;
blockLine: lines*;
block: BEGINIF NEWLINE blockLine ENDIF;
whileBlock: 'WHILE' '(' expression ')'NEWLINE inWhileBlock ;
inWhileBlock: BEGINWHILE NEWLINE lines ENDWHILE;
declaration: DATATYPE declarations;
declarations: terminalDeclaration (','terminalDeclaration)*;
terminalDeclaration: (IDENTIFIER|IDENTIFIER ASSIGN expression);
assignment: assignments ASSIGN ('+'|'-')? expression;
assignments: IDENTIFIER (ASSIGN IDENTIFIER)* ;
colonFunc: COLONFUNCTION COLON ( expression (',' expression)*)?;
constant: CHARLITERAL | INTEGERLITERAL | FLOATLITERAL | BOOLEANLITERAL | STRINGLITERAL;
expression:
     ('+'|'-')? constant	     # constantExpression
	| '(' expression ')'         # parenthesizedExpression
	| 'NOT' expression           # notBoolExpression
	| IDENTIFIER			     # identifierExpression
	| DOLLARSIGNCARRIAGE         # newLineExpression
	| colonFunc			         # colonFuncExpression
	| expression binaryOperation expression # binaryExpression
	| expression logicalOperation expression # logicalExpression
	| expression booleanOperation expression # booleanExpression
	| expression concatenateOperation expression # concatenateExpression;
binaryOperation: '*' | '/' | '%' | '+' | '-';
logicalOperation: 'AND' | 'OR';
booleanOperation: '>' | '<' | '>=' | '<=' | '==' | '<>';
concatenateOperation: '&';

/*Lexer rules*/
BEGIN: 'BEGIN CODE';
END: 'END CODE';
IF: 'IF';
ELSE: 'ELSE';
BEGINIF: 'BEGIN IF';
ENDIF: 'END IF';
BEGINWHILE: 'BEGIN WHILE';
ENDWHILE: 'END WHILE';
DATATYPE: 'BOOL' | 'CHAR' | 'INT' | 'FLOAT';
DOLLARSIGNCARRIAGE: '$';
ASSIGN: '=';
COLON: ':';
BOOLEANLITERAL: 'TRUE' | 'FALSE';
INTEGERLITERAL: [0-9]+;
FLOATLITERAL: [0-9]+ '.' [0-9]+;
CHARLITERAL: '\'' ~('\'') '\'';
STRINGLITERAL: ('"' ~'"'* '"') | ('[' ~']'* ']'+);
WS: [ \t\r]+ -> skip;
NEWLINE: [\r|\n]+;
COLONFUNCTION: 'DISPLAY' | 'SCAN';
IDENTIFIER: [_a-z][a-zA-Z0-9_]* | [a-z][a-zA-Z0-9_]*;
COMMENT: '#' ~[\r\n]* [\r|\n]*-> skip;