using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    INT, Float, String, Read, Write, Repeat, Until,If ,ElseIf, Else,End, Then, Return, Endline,
    Dot, Semicolon, Comma,LBraces, RBraces, LParanthesis, RParanthesis, Assign, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp, Equal,
    Idenifier, Number, Comment,AndOP , OrOp
}
namespace Tiny_Compiler
{
    

    public class Token
    {
       public string lex;
       public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        public Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        public Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.INT);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endline);
            
            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("{", Token_Class.LBraces);
            Operators.Add("}", Token_Class.RBraces);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add(":=", Token_Class.Assign);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("=", Token_Class.Equal);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("&&", Token_Class.AndOP);
          
        }

        public void StartScanning(string SourceCode)
        {

            for (int i = 0; i < SourceCode.Length; i++)
            {
                string word = "";
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n') continue;

                if ((CurrentChar >= 'A' && CurrentChar <= 'Z')
                    || (CurrentChar >= 'a' && CurrentChar <= 'z'))
                {
                    while (i < SourceCode.Length && (
                           (CurrentChar >= 'A' && CurrentChar <= 'Z')
                        || (CurrentChar >= 'a' && CurrentChar <= 'z')
                        || (CurrentChar >= '0' && CurrentChar <= '9')
                        || CurrentChar == '_'))
                    {
                        word += CurrentChar;
                        i++;
                        if (i < SourceCode.Length)
                            CurrentChar = SourceCode[i];
                    }
                    i--;
                    FindTokenClass(word);
                }
                else if (CurrentChar == '"') {
                    

                    while (i < SourceCode.Length)
                    {
                        word += CurrentChar;
                        i++;
                        if (i < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[i];
                            if (CurrentChar == '"')
                            {
                                i++;
                                word += CurrentChar;
                                break;
                            }
                        }
                    }
                    i--;
                    FindTokenClass(word);
                }
                else if (i < SourceCode.Length && (
                    (CurrentChar >= '0' && CurrentChar <= '9')
                    || CurrentChar == '.'))
                {
                    while (i < SourceCode.Length && (
                           (CurrentChar >= '0' && CurrentChar <= '9')
                        || CurrentChar == '.'))
                    {
                        word += CurrentChar;
                        i++;
                        if (i < SourceCode.Length)
                            CurrentChar = SourceCode[i];
                    }
                    i--;
                    FindTokenClass(word);
                }
                //Equal
                else if (CurrentChar == ':')
                {
                    word += CurrentChar;
                    if (i < SourceCode.Length - 1)
                    {
                        if (SourceCode[i + 1] == '=')
                        {
                            word += SourceCode[i + 1];
                            i++;
                        }
                    }
                    FindTokenClass(word);
                }
                else if (CurrentChar == '|')
                {
                    word += CurrentChar;
                    if (i < SourceCode.Length - 1)
                    {
                        if (SourceCode[i + 1] == '|')
                        {
                            word += SourceCode[i + 1];
                            i++;
                        }
                    }
                    FindTokenClass(word);
                }
                else if (CurrentChar == '&')
                {
                    word += CurrentChar;
                    if (i < SourceCode.Length - 1)
                    {
                        if (SourceCode[i + 1] == '&')
                        {
                            word += SourceCode[i + 1];
                            i++;
                        }
                    }
                    FindTokenClass(word);
                }
                //Comment
                else if (CurrentChar == '/')
                {
                    if (i < SourceCode.Length - 1)
                    {
                        if (SourceCode[i + 1] != '*')
                        {
                            word += CurrentChar;
                            FindTokenClass(word);
                            continue;
                        }
                    }

                    while (i < SourceCode.Length)
                    {
                        word += CurrentChar;
                        i++;
                        if (i < SourceCode.Length-1)
                        {
                            CurrentChar = SourceCode[i];
                            if (CurrentChar == '*' && SourceCode[i+1]=='/')
                            {
                                i++;
                                word += CurrentChar;
                                word += SourceCode[i];
                                break;
                            }
                        }
                    }
                    FindTokenClass(word);
                    
                }
                else if (CurrentChar == '<')
                {
                    word += CurrentChar;
                    if (i < SourceCode.Length - 1)
                    {
                        if (SourceCode[i + 1] == '>')
                        {
                            i++;
                            word += SourceCode[i];
                        }
                    }
                    FindTokenClass(word);
                }
                else
                {
                    if (i < SourceCode.Length)
                    {
                        word += CurrentChar;
                        FindTokenClass(word);
                    }
                }
            }

            TinyCompiler.TokenStream = Tokens;
        }
    
        void FindTokenClass(string Lex)
        {
            //Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex)) Tok.token_type = ReservedWords[Lex];
            //Is it an identifier?
            else if (isIdentifier(Lex)) Tok.token_type = Token_Class.Idenifier;
            //Is it a Constant?                                         
            else if (isConstant(Lex)) Tok.token_type = Token_Class.Number;
            //Is it an operator?
            else if (Operators.ContainsKey(Lex)) Tok.token_type = Operators[Lex];
            else if (isString(Lex)) Tok.token_type = Token_Class.String;
            else if (isComment(Lex)) Tok.token_type = Token_Class.Comment;
            else
            {
                Errors.Error_List.Add(Lex);
                return;
            }
            Tokens.Add(Tok);
        }

        bool isIdentifier(string lex)
        {
            if (!(
                (lex[0] >= 'a' && lex[0] <= 'z')
                || (lex[0] >= 'A' && lex[0] <= 'Z')
                || lex[0] == '_')
                ) return false;

            for (int i = 0; i < lex.Length; i++)
                if (!(
                    (lex[i] >= 'a' && lex[i] <= 'z')
                    || (lex[i] >= '0' && lex[i] <= '9')
                    || (lex[i] >= 'A' && lex[i] <= 'Z')
                    || lex[i] == '_')
                    ) return false;
            return true;

        }

        bool isConstant(string lex)
        {
            bool dotFound = false;
            for (int i = 0; i < lex.Length; i++)
            {
                if ((lex[i] >= '0' && lex[i] <= '9')
                    || (lex[i] == '.'
                    && !dotFound))
                {
                    if (lex[i] == '.') dotFound = true;
                }
                else return false;
            }
            return true;

        }

        bool isComment(string lex)
        {
            if (lex.Length<4 || !((lex[0] == '/') && (lex[1] == '*') && (lex[lex.Length - 2] == '*') && (lex[lex.Length - 1] == '/'))) return false;
            for (int i =2; i < lex.Length - 5; i++) {
                if (lex[i] == '*' && lex[i + 1] == '/') return false;
                }
            return true;
        }

        bool isString(string lex) {
              if (lex.Length<=1 || !(lex[0]=='"' && lex[lex.Length-1]=='"')
                ) return false;
                        
            for (int i = 1; i < lex.Length - 1; i++)
                if (lex[i] == '"') return false;
            return true;
        }
    }
}
