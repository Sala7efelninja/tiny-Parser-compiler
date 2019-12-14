using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Tiny_Compiler
{

    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            if (TokenStream[InputPointer].token_type == Token_Class.Comment)
                program.Children.Add(Comment_Statement());
            program.Children.Add(Functions());
            program.Children.Add(Main_Function());
            return program;
        }

         Node Main_Function()
        {
            Node main_function = new Node("Main_function");
            main_function.Children.Add(DataType());
            main_function.Children.Add(match(Token_Class.Idenifier));
            main_function.Children.Add(match(Token_Class.LParanthesis));
            main_function.Children.Add(match(Token_Class.RParanthesis));
            main_function.Children.Add(Function_body());
            return main_function;
        }

         Node DataType()
        {
            Node data_type = new Node("Data_Type");

            if (TokenStream[InputPointer].token_type == Token_Class.INT)
                data_type.Children.Add(match(Token_Class.INT));

            if (TokenStream[InputPointer].token_type == Token_Class.Float)
                data_type.Children.Add(match(Token_Class.Float));
            if (TokenStream[InputPointer].token_type == Token_Class.String)
                data_type.Children.Add(match(Token_Class.String));

            return data_type;
        }

        Node Functions()
        {
            Node functions = new Node("Functions");
            if (TokenStream[InputPointer].token_type == Token_Class.Comment)
                functions.Children.Add(Comment_Statement());
            if ((TokenStream[InputPointer].token_type==Token_Class.INT|| TokenStream[InputPointer].token_type == Token_Class.Float|| TokenStream[InputPointer].token_type == Token_Class.String)&& TokenStream[InputPointer+1].lex!="main")
            {
                functions.Children.Add(Function_Statement());
                functions.Children.Add(Functions());
            }
            return functions;
        }

         Node Function_Statement()
        {
            Node function_statement = new Node("Function_Statement");
            function_statement.Children.Add(Function_declaration());
            function_statement.Children.Add(Function_body());

            return function_statement;
        }
       
         Node Function_declaration()
        {
            Node function_declaration = new Node("Function_declaration");
            function_declaration.Children.Add(DataType());
            function_declaration.Children.Add(FunctionName());
            function_declaration.Children.Add(match(Token_Class.LParanthesis));
            function_declaration.Children.Add(Parameter());
            function_declaration.Children.Add(Parameter_list());
            function_declaration.Children.Add(match(Token_Class.RParanthesis));
            return function_declaration;
        }


         Node Parameter()
        {
            Node parameter = new Node("Parameter");
            parameter.Children.Add(DataType());
            parameter.Children.Add(Identifier());
            return parameter;
        }

         Node Identifier()
        {
            Node id = new Node("Identifier");
            id.Children.Add(match(Token_Class.Idenifier));
            return id;
        }

        Node Parameter_list()
        {
            Node parameter_list = new Node("Parameter_list");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                parameter_list.Children.Add(match(Token_Class.Comma));
                parameter_list.Children.Add(Parameter());
                parameter_list.Children.Add(Parameter_list());
            }
            return parameter_list;
        }

        Node FunctionName()
        {
            Node function_name = new Node("FunctionName");
            function_name.Children.Add(Identifier());
            return function_name;
        }

        Node Function_body()
        {
            //dsadsas
            Node function_body = new Node("Function_body");
            function_body.Children.Add(match(Token_Class.LBraces));
            function_body.Children.Add(Statements());
            function_body.Children.Add(Return_Statement());
            function_body.Children.Add(match(Token_Class.RBraces));
            return function_body;
        }

        private Node Return_Statement()
        {
            Node return_statement = new Node("Return_Statement");      
            return_statement.Children.Add(match(Token_Class.Return));
            return_statement.Children.Add(Expression());
            return_statement.Children.Add(match(Token_Class.Semicolon));
            return return_statement;
        }

        private Node Statements()
        {
            Node statements = new Node("Statements");
            statements.Children.Add(Statement());
            statements.Children.Add(this.Statement2());
            return statements;
        }

        private Node Statement2()
        {
            Node statement2 = new Node("Statement2");
            if(TokenStream[InputPointer].token_type==Token_Class.Semicolon)
            {
                statement2.Children.Add(Statement());
                statement2.Children.Add(Statement2());
            }
            return statement2;
        }

        private Node Statement()
        {
            Node statement = new Node("Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Read)
                statement.Children.Add(Read_Statement());
 //           if (TokenStream[InputPointer].token_type == Token_Class.Return)
  //              statement.Children.Add(Return_Statement());
            if (TokenStream[InputPointer].token_type==Token_Class.Write)
                statement.Children.Add(Write_Statement());
            if (TokenStream[InputPointer].token_type == Token_Class.INT|| TokenStream[InputPointer].token_type == Token_Class.Float|| TokenStream[InputPointer].token_type == Token_Class.String)
                statement.Children.Add(Decleration_Statement());
            if (TokenStream[InputPointer].token_type == Token_Class.Assign)
                statement.Children.Add(Assignment_Statement());
            if (TokenStream[InputPointer].token_type == Token_Class.If)
                statement.Children.Add(If_Statement());
            if (TokenStream[InputPointer].token_type == Token_Class.ElseIf)
                statement.Children.Add(Else_If_Statement());
            if (TokenStream[InputPointer].token_type == Token_Class.Else)
                statement.Children.Add(Else_Statement());
            if (TokenStream[InputPointer].token_type == Token_Class.Repeat)
                statement.Children.Add(Repeat_Statement());
            return statement;
        }

        private Node Else_Statement()
        {
            Node else_stat = new Node("Else_Statement");
            else_stat.Children.Add(match(Token_Class.Else));
            else_stat.Children.Add(Statements());
            else_stat.Children.Add(match(Token_Class.End));
            return else_stat;
        }

        private Node Else_If_Statement()
        {
            Node else_if_stat = new Node("Else_If_Statement");
            else_if_stat.Children.Add(match(Token_Class.ElseIf));
            else_if_stat.Children.Add(Condition_Statement());
            else_if_stat.Children.Add(match(Token_Class.Then));
            else_if_stat.Children.Add(Statements());
            else_if_stat.Children.Add(match(Token_Class.End));
            return else_if_stat;
        }

        private Node If_Statement()
        {
            Node if_stat = new Node("If_Statement");
            if_stat.Children.Add(match(Token_Class.If));
            if_stat.Children.Add(Condition_Statement());
            if_stat.Children.Add(match(Token_Class.Then));
            if_stat.Children.Add(Statements());
            if_stat.Children.Add(match(Token_Class.End));
            return if_stat;
        }

        private Node Repeat_Statement()
        {
            Node repeat_statement = new Node("Repeat_Statement");
            repeat_statement.Children.Add(match(Token_Class.Repeat));
            repeat_statement.Children.Add(Statements());
            repeat_statement.Children.Add(match(Token_Class.Until));
            repeat_statement.Children.Add(Condition_Statement());
            return repeat_statement;
        }

        private Node Condition_Statement()
        {
            Node condition_statement = new Node("Condition_Statement");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Condition_List());

            return condition_statement;
        }

        private Node Condition_List()
        {
            Node condition_list = new Node("Condition_List");
            if(TokenStream[InputPointer].token_type==Token_Class.AndOP|| TokenStream[InputPointer].token_type == Token_Class.OrOp)
            {
                condition_list.Children.Add(Boolean_operator());
                condition_list.Children.Add(Condition());
                condition_list.Children.Add(Condition_List());
            }
            return condition_list;
        }

        private Node Comment_Statement()
        {
            Node comment = new Node("Comment_Statement");
            comment.Children.Add(match(Token_Class.Comment));          
            return comment;
        }

        private Node Boolean_operator()
        {
            Node boolean_op = new Node("Boolean_operator");
            if (TokenStream[InputPointer].token_type == Token_Class.AndOP)
                boolean_op.Children.Add(match(Token_Class.AndOP));
            else 
                boolean_op.Children.Add(match(Token_Class.OrOp));
            return boolean_op;
        }

        private Node Condition()
        {
            Node condition = new Node("Condition");
            condition.Children.Add(Identifier());
            condition.Children.Add(Condition_Operator());
            condition.Children.Add(Term());
            return condition;
        }

        private Node Term()
        {
            Node term = new Node("Term");
            if (TokenStream[InputPointer].token_type == Token_Class.Number)
                term.Children.Add(match(Token_Class.Number));
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier&&TokenStream[InputPointer+1].token_type!=Token_Class.LParanthesis)
                term.Children.Add(Identifier());
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
                term.Children.Add(Function_Call());
            return term;
        }

        private Node Function_Call()
        {
            Node function_call = new Node("Function_Call");
            if(TokenStream[InputPointer].token_type==Token_Class.Idenifier)
            function_call.Children.Add(Identifier());
            function_call.Children.Add(match(Token_Class.LParanthesis));
            function_call.Children.Add(Arguments());
            function_call.Children.Add(match(Token_Class.RParanthesis));
            return function_call;
        }

        private Node Arguments()
        {
            Node args = new Node("Arguments");
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                args.Children.Add(Identifier());
                args.Children.Add(Arguments2());
            }
                return args;
        }

        private Node Arguments2()
        {
            Node args2 = new Node("Arguments2");
            if (TokenStream[InputPointer].token_type==Token_Class.Comma)
            {
                args2.Children.Add(match(Token_Class.Comma));
                args2.Children.Add(Identifier());
                args2.Children.Add(Arguments2());
            }
           return args2;
        }

        private Node Condition_Operator()
        {
            Node condition_operator = new Node("Condition_Operator");
            if(TokenStream[InputPointer].token_type==Token_Class.GreaterThanOp)
            condition_operator.Children.Add(match(Token_Class.GreaterThanOp));

            if (TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
                condition_operator.Children.Add(match(Token_Class.LessThanOp));

            if (TokenStream[InputPointer].token_type == Token_Class.Equal)
                condition_operator.Children.Add(match(Token_Class.Equal));

            if (TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
                condition_operator.Children.Add(match(Token_Class.NotEqualOp));
           
            return condition_operator;
        }

    

        private Node Assignment_Statement()
        {
            Node assignment_statement = new Node("Assignment_Statement");
            assignment_statement.Children.Add(Identifier());
            assignment_statement.Children.Add(match(Token_Class.Assign));
            assignment_statement.Children.Add(Expression());
            return assignment_statement;
        }

        private Node Write_Statement()
        {
            Node write_statement = new Node("Write_Statement");
            write_statement.Children.Add(match(Token_Class.Write));
            if (TokenStream[InputPointer].token_type == Token_Class.Endline)
                write_statement.Children.Add(match(Token_Class.Endline));
            else
            write_statement.Children.Add(Expression());

            write_statement.Children.Add(match(Token_Class.Semicolon));
            return write_statement;
        }

        bool checkOP(int index)
        {
            if (TokenStream[index].token_type == Token_Class.PlusOp || TokenStream[index].token_type == Token_Class.MinusOp || TokenStream[index].token_type == Token_Class.MultiplyOp || TokenStream[index].token_type == Token_Class.DivideOp)
                return true;

                return false;
        }
        private Node Expression()
        {
            Node expr = new Node("Expression");

            if (TokenStream[InputPointer].token_type == Token_Class.String)
                expr.Children.Add(match(Token_Class.String));

            else if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis || (TokenStream[InputPointer].token_type == Token_Class.Idenifier && checkOP(InputPointer + 1)))
                expr.Children.Add(Equation());

            else if (TokenStream[InputPointer].token_type == Token_Class.Number|| TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                expr.Children.Add(Term());
           
                return expr;
        }

        private Node Equation()
        {
            Node equ = new Node("Equation");
            equ.Children.Add(Equation2());
            equ.Children.Add(ArthOP());
            equ.Children.Add(Equation2());
            return equ;
        }

        private Node Equation2()
        {
            Node equ2 = new Node("Equation2");
            if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                equ2.Children.Add(match(Token_Class.LParanthesis));
                equ2.Children.Add(Equation());
                equ2.Children.Add(match(Token_Class.RParanthesis));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier && checkOP(InputPointer + 1))
            {
                equ2.Children.Add(match(Token_Class.Idenifier));
                equ2.Children.Add(Equation());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Number || TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                equ2.Children.Add(Term());




            return equ2;
        }
        private Node Decleration_Statement()
        {
            Node dec = new Node("Decleration_Statement");
            dec.Children.Add(DataType());
            dec.Children.Add(ID_List());
            dec.Children.Add(match(Token_Class.Semicolon));
            return dec;
        }

        private Node ID_List()
        {
            Node id_list = new Node("ID_List");
            id_list.Children.Add(ID_List1());
            id_list.Children.Add(ID_List2());
            return id_list;
        }

        private Node ID_List2()
        {
            Node id_list2 = new Node("ID_List2");
            if(TokenStream[InputPointer].token_type==Token_Class.Comma)
            {
                id_list2.Children.Add(match(Token_Class.Comma));
                id_list2.Children.Add(ID_List1());
                id_list2.Children.Add(ID_List());
            }
            return id_list2;
        }

        private Node ID_List1()
        {
            Node id_list1 = new Node("ID_List1");
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer+1].token_type == Token_Class.Assign)
                id_list1.Children.Add(Assignment_Statement());
           else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                id_list1.Children.Add(Identifier());

            return id_list1;
        }

        private Node ArthOP()
        {
            Node arthOP = new Node("ArthOP");
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp)
                arthOP.Children.Add(match(Token_Class.PlusOp));
                
            else if(TokenStream[InputPointer].token_type == Token_Class.MinusOp)
              arthOP.Children.Add(match(Token_Class.MinusOp));

            else if(TokenStream[InputPointer].token_type == Token_Class.MultiplyOp )
                arthOP.Children.Add(match(Token_Class.MultiplyOp));

             else if (TokenStream[InputPointer].token_type == Token_Class.DivideOp)
                arthOP.Children.Add(match(Token_Class.DivideOp));

            return arthOP;

        }

        private Node Read_Statement()
        {
            Node read_statement = new Node("Read_Statement");
            read_statement.Children.Add(match(Token_Class.Read));
            read_statement.Children.Add(Identifier());
            read_statement.Children.Add(match(Token_Class.Semicolon));
            return read_statement;
        }

        public Node match(Token_Class ExpectedToken)
        {

            if (ExpectedToken == TokenStream[InputPointer].token_type)
            {
                InputPointer++;
                Node newNode = new Node(ExpectedToken.ToString());

                return newNode;

            }

            else
            {
                Errors.Error_List.Add(InputPointer+"Parsing Error: Expected "
                    + ExpectedToken.ToString() + " and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}

