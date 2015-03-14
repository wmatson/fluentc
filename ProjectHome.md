The project is a scripting language (FluentC) that aims to resemble natural language and a paired application from which to run scripts written in FluentC.
(Sorry about my fomatting, it did not port well to wiki markup, included in the repository is a word document with the original formatting and further details)

---

# Specifications #
FluentC supports three value types: Numbers, Strings, and Conditions. Numbers allow the four basic arithmetic operations for both integral and floating-point values. Strings hold all other data in textual representations. If a user attempts operations that mix Strings and Numbers, the attempt results in a String. Conditions indicate whether the conditional expression they hold is satisfied as being a true condition. As they are an indication rather than a concrete type, the value of a Condition may not be set explicitly.

## Number-valued expressions ##
Number-valued expressions may exist in any of the following forms where x and y are any decimal values written in the form ‘0.0’ or the form ‘-0.0’ where 0 is any number of digits 0-9 or variables containing such values. FluentC evaluates number-valued expressions using order of operations.
```
#	Form	Value
1	x	x
2	x+x	x added to x
3	x+y	x added to y
4	x-x	x subtracted from x
5	x-y	y subtracted from x
6	x*x	x multiplied by x
7	x*y	x multiplied by y
8	x/x	x divided by x
9	x/y	x divided by y
10	(x)	x
```

## String-valued expressions ##
String-valued expressions may exist in any of the following forms where x and y are any number of ASCII characters surrounded by double quote marks (“) or variables containing such values.
```
#	Form	Value
1	x+x	x appended to the end of x
2	x+y	y appended to the end of x
3	x	x
```

## Condition-Valued Expressions ##
Condition-valued expressions may exist in any of the following forms where x and y are valued expressions or variables of any type.
```
#	Form	                               Case in which satisfied given x and y
                                  Numbers	        Strings  	                Conditions
1	x is larger than y	  x is greater than y	x is after y alphabetically	Invalid
2	x is smaller than y	  x is less than y	x is before y alphabetically	Invalid
3	x is the same as x	  Always                Always	                        Always
4	x is the same as y	  x is equal to y	x is the same as y	        x is the same as y
5	It is not the case that x Invalid	        Invalid	                        x is not true
6	x > y	                  x is greater than y	x is after y alphabetically	Invalid
7	x < y	                  x is less than y	x is before y alphabetically	Invalid
8	x = x	                  Always                Always 	                        Always
9	x = y	                  x is equal to y       x is the same as y	         x is the same as y
10	!x	                  Invalid               Invalid	                         x is not true
11	(x)	                  Invalid	        Invalid	                         x is true
```

## Variable Declaration, Assignment, and Deletion ##
> Let x exist. – Declares a new typeless variable where x is the variable’s name.
> Let x be value. – Assigns value to x where value is any valued expression and x is a variable’s name. Assigning a value to an undeclared variable declares the variable and assigns value to it.
> Forget x. – Removes the variable x from the available cache of variables.

## Non-Valued Function Declaration and Invocation ##
> How to function-name with parameter-names: statement-block. – Declares a new function where function-name is the function’s name, parameter-names is a comma-separated list of parameter names, and statement-block is any valid block of statements.
> function-name with parameter-values. – Invokes the function identified by function-name where parameter-values is a comma-separated list of expressions that are supplied as parameters. The with keyword is optional if there are fewer than two parameters.

## Native Non-Valued Functions ##
> Tell me x. - Reveals the value of the expression x to the user.

## Valued Function Declaration and Invocation ##
> How to know function-name with parameter-names: statement-block. expression! – Declares a new function where function-name is the function’s name, parameter-names is a comma-separated list of parameter names, statement-block is any valid block of statements, and expression is any valid expression.
function-name with parameter-values. – Invokes the function identified by function-name where parameter-values is a comma-separated list of expressions (optionally paired with names) that is supplied as parameters for the function. The with keyword is optional if there are fewer than two parameters.

## Native Valued Functions ##
> Ask me for a number with the prompt x. – Prompts the user for a number, displaying the value of the expression x as a prompt.
> Ask me for a string with the prompt x. – Prompts the user for a string, displaying the value of the expression x as a prompt.
> Give me the part of the string with the source string s, the starting index x, the ending index y. – Returns the substring of the source string expression s starting at the index indicated by the number expression x (inclusive) and ending at the index indicated by the number expression y (exclusive).
> Give me the length of the string s. – Returns the length of the string s as a number.

## Parameters within Functions ##
> FluentC treats parameters as variables within functions. There is an implied variable deletion at the end of each function declaration for each parameter.

## Statements, Statement Blocks and Comments ##
A statement is any variable declaration, assignment, or deletion; a statement may also be any function invocation. All statements must end with a ‘.’ except within a statement block. In such a case, the last statement must end with a ‘.’ and all preceding statements end with a ‘;’.
Any set of characters ending with a ‘?’ is a comment and is ignored by the engine.
FluentC allows comments only before or after statements.
Some example statements:
> Let x be 7.
> Tell me “Hello World”.
An example statement block:
> Let x exist; Let x be 7+92; Let x be x\*7; Tell me x; Forget x.
An example comment surrounded by statements:
> Let x exist. The previous statement declared a variable and the following assigns a value to it? Let x be 27.


## Conditionals and Looping ##
FluentC expresses conditional statements and loops in any of the following forms where condition is a Condition-Valued Expression and statement-block is any valid block of statements.
1.	If condition, statement-block.
2.	While condition... statement-block.
3.	Do... statement-block. ...Until condition.

## Duplicate variable and function names ##
FluentC does not define behavior where available variables, parameters, and/or functions share one name.

## Whitespace ##
FluentC ignores whitespace outside of statements; FluentC also ignores whitespace between conditionals, loops, function declarations, and their respective statement blocks.

## Language Context Free Grammars ##
In the following list of context-free grammars for FluentC all regular expressions are followed by “(regular expression)” and all language keywords do not appear on the left side of any grammar.
```
whitespace -> \s+ (regular expression)
number_expression -> number
number_expression -> number_expression + number_expression
number_expression -> number_expression - number_expression
number_expression -> number_expression * number_expression
number_expression -> number_expression / number_expression
number_expression -> (number_expression)
number_expression -> variable_name
number -> whole_number.whole_number
number -> whole_number
number -> -whole_number
number -> -whole_number.whole_number
number -> .whole_number
number -> -.whole_number
whole_number -> digit
whole_number -> digit,whole_number
digit -> \d (regular expression)
string_expression -> string_expression + string_expression
string_expression -> “.*” (regular expression)
string_expression -> variable_name
nonconditional_expression -> string_expression
nonconditional_expression -> number_expression
conditional_expression -> nonconditional_expression = nonconditional_expression
conditional_expression -> nonconditional _expression > nonconditional _expression
conditional_expression -> nonconditional _expression < nonconditional _expression
conditional_expression -> nonconditional _expression is larger than nonconditional _expression
conditional_expression -> nonconditional _expression is smaller than nonconditional _expression
conditional_expression -> nonconditional _expression is the same as nonconditional _expression
conditional_expression -> conditional_expression = conditional_expression
conditional_expression -> (conditional_expression)
conditional_expression -> It is not the case that conditional_expression
expression -> nonconditional_expression
expression -> conditional_expression
expression -> valued_function_invocation
variable_declaration -> Let variable_name exist
variable_name -> \b[^;.?,]+\b (regular expression)
variable_assignment -> Let variable_name be expression
variable_deletion -> Forget variable_name
statement_part -> variable_declaration
statement_part -> variable_assignment
statement_part -> variable_deletion
comment -> variable_name?
statement -> statement_part.
statement -> statement_part;whitespace statement
statement -> conditional_statement
statement -> looping_statement
statement -> function_invocation
statement -> comment statement
statement -> statement comment
conditional_statement -> If conditional_expression,whitespace statement
looping_statement -> While conditional_expression...whitespace statement
looping_statement -> Do...whitespace multiple_statements whitespace...Until conditional_expression.
multiple_statements -> statement
multiple_statements -> multiple_statements whitespace statement
function_declaration -> How to variable_name:whitespace statement
valued_function_declaration -> How to know variable_name:whitespace statement whitespace expression!
function_declaration -> How to variable_name with parameter_list:whitespace statement
valued_function_declaration -> How to know variable_name with parameter_list: whitespace multiple_statements whitespace expression!
parameter_list -> parameter
parameter_list -> parameter_list,whitespace parameter
parameter -> variable_name
function_invocation -> variable_name.
function_invocation -> variable_name expression.
function_invocation -> variable_name with expression_list.
expression_list -> expression
expression_list -> the parameter expression
expression_list -> expression_list, whitespace expression_list
valued_function_invocation -> variable_name
valued_function_invocation -> variable_name expression
valued_function_invocation -> variable_name with expression_list
script -> valued_function_declaration
script -> multiple_statements
```