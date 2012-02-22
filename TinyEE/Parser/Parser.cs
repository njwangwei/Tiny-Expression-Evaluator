// Generated by TinyPG v1.3 available at www.codeproject.com

using System;
using System.Collections.Generic;

namespace TinyEE
{
    #region Parser

    public partial class Parser 
    {
        private Scanner scanner;
        private ParseTree tree;
        
        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
        }

        public ParseTree Parse(string input)
        {
            tree = new ParseTree();
            return Parse(input, tree);
        }

        public ParseTree Parse(string input, ParseTree tree)
        {
            scanner.Init(input);

            this.tree = tree;
            ParseStart(tree);
            tree.Skipped = scanner.Skipped;

            return tree;
        }

        private void ParseStart(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Start), "Start");
            parent.Nodes.Add(node);


            
            tok = scanner.LookAhead(TokenType.NOT, TokenType.MINUS, TokenType.STRING, TokenType.DECIMAL, TokenType.INTEGER, TokenType.TRUE, TokenType.FALSE, TokenType.NULL, TokenType.LPAREN, TokenType.FUNCTION, TokenType.IDENTIFIER);
            if (tok.Type == TokenType.NOT
                || tok.Type == TokenType.MINUS
                || tok.Type == TokenType.STRING
                || tok.Type == TokenType.DECIMAL
                || tok.Type == TokenType.INTEGER
                || tok.Type == TokenType.TRUE
                || tok.Type == TokenType.FALSE
                || tok.Type == TokenType.NULL
                || tok.Type == TokenType.LPAREN
                || tok.Type == TokenType.FUNCTION
                || tok.Type == TokenType.IDENTIFIER)
            {
                ParseExpression(node);
            }

            
            tok = scanner.Scan(TokenType.EOF);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.EOF) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EOF.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseExpression(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Expression), "Expression");
            parent.Nodes.Add(node);

            ParseOrExpression(node);

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseOrExpression(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.OrExpression), "OrExpression");
            parent.Nodes.Add(node);


            
            ParseAndExpression(node);

            
            tok = scanner.LookAhead(TokenType.OR);
            while (tok.Type == TokenType.OR)
            {

                
                tok = scanner.Scan(TokenType.OR);
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
                if (tok.Type != TokenType.OR) {
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.OR.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    return;
                }

                
                ParseAndExpression(node);
            tok = scanner.LookAhead(TokenType.OR);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseAndExpression(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.AndExpression), "AndExpression");
            parent.Nodes.Add(node);


            
            ParseNotExpression(node);

            
            tok = scanner.LookAhead(TokenType.AND);
            while (tok.Type == TokenType.AND)
            {

                
                tok = scanner.Scan(TokenType.AND);
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
                if (tok.Type != TokenType.AND) {
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.AND.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    return;
                }

                
                ParseNotExpression(node);
            tok = scanner.LookAhead(TokenType.AND);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseNotExpression(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.NotExpression), "NotExpression");
            parent.Nodes.Add(node);


            
            tok = scanner.LookAhead(TokenType.NOT);
            if (tok.Type == TokenType.NOT)
            {
                tok = scanner.Scan(TokenType.NOT);
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
                if (tok.Type != TokenType.NOT) {
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.NOT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    return;
                }
            }

            
            ParseCompare(node);

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseCompare(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Compare), "Compare");
            parent.Nodes.Add(node);


            
            ParseAddition(node);

            
            tok = scanner.LookAhead(TokenType.EQUAL, TokenType.GT, TokenType.LT, TokenType.GTE, TokenType.LTE, TokenType.NOTEQUAL);
            while (tok.Type == TokenType.EQUAL
                || tok.Type == TokenType.GT
                || tok.Type == TokenType.LT
                || tok.Type == TokenType.GTE
                || tok.Type == TokenType.LTE
                || tok.Type == TokenType.NOTEQUAL)
            {

                
                tok = scanner.LookAhead(TokenType.EQUAL, TokenType.GT, TokenType.LT, TokenType.GTE, TokenType.LTE, TokenType.NOTEQUAL);
                switch (tok.Type)
                {
                    case TokenType.EQUAL:
                        tok = scanner.Scan(TokenType.EQUAL);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.EQUAL) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EQUAL.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    case TokenType.GT:
                        tok = scanner.Scan(TokenType.GT);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.GT) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    case TokenType.LT:
                        tok = scanner.Scan(TokenType.LT);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.LT) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.LT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    case TokenType.GTE:
                        tok = scanner.Scan(TokenType.GTE);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.GTE) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GTE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    case TokenType.LTE:
                        tok = scanner.Scan(TokenType.LTE);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.LTE) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.LTE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    case TokenType.NOTEQUAL:
                        tok = scanner.Scan(TokenType.NOTEQUAL);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.NOTEQUAL) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.NOTEQUAL.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    default:
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                        break;
                }

                
                ParseAddition(node);
            tok = scanner.LookAhead(TokenType.EQUAL, TokenType.GT, TokenType.LT, TokenType.GTE, TokenType.LTE, TokenType.NOTEQUAL);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseAddition(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Addition), "Addition");
            parent.Nodes.Add(node);


            
            ParseMultiplication(node);

            
            tok = scanner.LookAhead(TokenType.PLUS, TokenType.MINUS);
            while (tok.Type == TokenType.PLUS
                || tok.Type == TokenType.MINUS)
            {

                
                tok = scanner.LookAhead(TokenType.PLUS, TokenType.MINUS);
                switch (tok.Type)
                {
                    case TokenType.PLUS:
                        tok = scanner.Scan(TokenType.PLUS);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.PLUS) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.PLUS.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    case TokenType.MINUS:
                        tok = scanner.Scan(TokenType.MINUS);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.MINUS) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.MINUS.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    default:
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                        break;
                }

                
                ParseMultiplication(node);
            tok = scanner.LookAhead(TokenType.PLUS, TokenType.MINUS);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseMultiplication(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Multiplication), "Multiplication");
            parent.Nodes.Add(node);


            
            ParsePower(node);

            
            tok = scanner.LookAhead(TokenType.STAR, TokenType.FSLASH);
            while (tok.Type == TokenType.STAR
                || tok.Type == TokenType.FSLASH)
            {

                
                tok = scanner.LookAhead(TokenType.STAR, TokenType.FSLASH);
                switch (tok.Type)
                {
                    case TokenType.STAR:
                        tok = scanner.Scan(TokenType.STAR);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.STAR) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.STAR.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    case TokenType.FSLASH:
                        tok = scanner.Scan(TokenType.FSLASH);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.FSLASH) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.FSLASH.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }
                        break;
                    default:
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                        break;
                }

                
                ParsePower(node);
            tok = scanner.LookAhead(TokenType.STAR, TokenType.FSLASH);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParsePower(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Power), "Power");
            parent.Nodes.Add(node);


            
            ParseNegation(node);

            
            tok = scanner.LookAhead(TokenType.EXPONENT);
            while (tok.Type == TokenType.EXPONENT)
            {

                
                tok = scanner.Scan(TokenType.EXPONENT);
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
                if (tok.Type != TokenType.EXPONENT) {
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EXPONENT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    return;
                }

                
                ParseNegation(node);
            tok = scanner.LookAhead(TokenType.EXPONENT);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseNegation(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Negation), "Negation");
            parent.Nodes.Add(node);


            
            tok = scanner.LookAhead(TokenType.MINUS);
            if (tok.Type == TokenType.MINUS)
            {
                tok = scanner.Scan(TokenType.MINUS);
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
                if (tok.Type != TokenType.MINUS) {
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.MINUS.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    return;
                }
            }

            
            ParseMember(node);

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseMember(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Member), "Member");
            parent.Nodes.Add(node);


            
            ParseBase(node);

            
            tok = scanner.LookAhead(TokenType.DOT, TokenType.LBRACKET);
            while (tok.Type == TokenType.DOT
                || tok.Type == TokenType.LBRACKET)
            {
                tok = scanner.LookAhead(TokenType.DOT, TokenType.LBRACKET);
                switch (tok.Type)
                {
                    case TokenType.DOT:

                        
                        tok = scanner.Scan(TokenType.DOT);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.DOT) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }

                        
                        ParseMemberAccess(node);
                        break;
                    case TokenType.LBRACKET:

                        
                        tok = scanner.Scan(TokenType.LBRACKET);
                        n = node.CreateNode(tok, tok.ToString() );
                        node.Token.UpdateRange(tok);
                        node.Nodes.Add(n);
                        if (tok.Type != TokenType.LBRACKET) {
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.LBRACKET.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                            return;
                        }

                        
                        ParseIndexAccess(node);
                        break;
                    default:
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                        break;
                }
            tok = scanner.LookAhead(TokenType.DOT, TokenType.LBRACKET);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseMemberAccess(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.MemberAccess), "MemberAccess");
            parent.Nodes.Add(node);

            tok = scanner.Scan(TokenType.IDENTIFIER);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.IDENTIFIER) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.IDENTIFIER.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseBase(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Base), "Base");
            parent.Nodes.Add(node);

            tok = scanner.LookAhead(TokenType.STRING, TokenType.DECIMAL, TokenType.INTEGER, TokenType.TRUE, TokenType.FALSE, TokenType.NULL, TokenType.LPAREN, TokenType.FUNCTION, TokenType.IDENTIFIER);
            switch (tok.Type)
            {
                case TokenType.STRING:
                case TokenType.DECIMAL:
                case TokenType.INTEGER:
                case TokenType.TRUE:
                case TokenType.FALSE:
                case TokenType.NULL:
                    ParseLiteral(node);
                    break;
                case TokenType.LPAREN:
                    ParseGroup(node);
                    break;
                case TokenType.FUNCTION:
                    ParseFunctionCall(node);
                    break;
                case TokenType.IDENTIFIER:
                    ParseVariable(node);
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseVariable(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Variable), "Variable");
            parent.Nodes.Add(node);

            tok = scanner.Scan(TokenType.IDENTIFIER);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.IDENTIFIER) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.IDENTIFIER.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseIndexAccess(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.IndexAccess), "IndexAccess");
            parent.Nodes.Add(node);


            
            tok = scanner.LookAhead(TokenType.STRING, TokenType.INTEGER);
            switch (tok.Type)
            {
                case TokenType.STRING:
                    tok = scanner.Scan(TokenType.STRING);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.STRING) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.STRING.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.INTEGER:
                    tok = scanner.Scan(TokenType.INTEGER);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.INTEGER) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.INTEGER.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            
            tok = scanner.Scan(TokenType.RBRACKET);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.RBRACKET) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.RBRACKET.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseFunctionCall(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.FunctionCall), "FunctionCall");
            parent.Nodes.Add(node);


            
            tok = scanner.Scan(TokenType.FUNCTION);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.FUNCTION) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.FUNCTION.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            
            tok = scanner.LookAhead(TokenType.NOT, TokenType.MINUS, TokenType.STRING, TokenType.DECIMAL, TokenType.INTEGER, TokenType.TRUE, TokenType.FALSE, TokenType.NULL, TokenType.LPAREN, TokenType.FUNCTION, TokenType.IDENTIFIER);
            if (tok.Type == TokenType.NOT
                || tok.Type == TokenType.MINUS
                || tok.Type == TokenType.STRING
                || tok.Type == TokenType.DECIMAL
                || tok.Type == TokenType.INTEGER
                || tok.Type == TokenType.TRUE
                || tok.Type == TokenType.FALSE
                || tok.Type == TokenType.NULL
                || tok.Type == TokenType.LPAREN
                || tok.Type == TokenType.FUNCTION
                || tok.Type == TokenType.IDENTIFIER)
            {
                ParseArgumentList(node);
            }

            
            tok = scanner.Scan(TokenType.RPAREN);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.RPAREN) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.RPAREN.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseArgumentList(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ArgumentList), "ArgumentList");
            parent.Nodes.Add(node);


            
            ParseExpression(node);

            
            tok = scanner.LookAhead(TokenType.COMMA);
            while (tok.Type == TokenType.COMMA)
            {

                
                tok = scanner.Scan(TokenType.COMMA);
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
                if (tok.Type != TokenType.COMMA) {
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.COMMA.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    return;
                }

                
                ParseExpression(node);
            tok = scanner.LookAhead(TokenType.COMMA);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseLiteral(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Literal), "Literal");
            parent.Nodes.Add(node);

            tok = scanner.LookAhead(TokenType.STRING, TokenType.DECIMAL, TokenType.INTEGER, TokenType.TRUE, TokenType.FALSE, TokenType.NULL);
            switch (tok.Type)
            {
                case TokenType.STRING:
                    tok = scanner.Scan(TokenType.STRING);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.STRING) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.STRING.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.DECIMAL:
                    tok = scanner.Scan(TokenType.DECIMAL);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.DECIMAL) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DECIMAL.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.INTEGER:
                    tok = scanner.Scan(TokenType.INTEGER);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.INTEGER) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.INTEGER.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.TRUE:
                    tok = scanner.Scan(TokenType.TRUE);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.TRUE) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.TRUE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.FALSE:
                    tok = scanner.Scan(TokenType.FALSE);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.FALSE) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.FALSE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.NULL:
                    tok = scanner.Scan(TokenType.NULL);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.NULL) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.NULL.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseGroup(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Group), "Group");
            parent.Nodes.Add(node);


            
            tok = scanner.Scan(TokenType.LPAREN);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.LPAREN) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.LPAREN.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            
            ParseExpression(node);

            
            tok = scanner.Scan(TokenType.RPAREN);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.RPAREN) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.RPAREN.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }


    }

    #endregion Parser
}