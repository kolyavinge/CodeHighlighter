﻿using System.Collections;
using System.Collections.Generic;

namespace CodeHighlighter.Sql;

internal class KeywordsCollection : IEnumerable<string>
{
    private readonly List<string> _keywords = new()
    {
        "ADD",
        "ALL",
        "ALTER",
        "ANY",
        "AS",
        "ASC",
        "AUTHORIZATION",
        "BACKUP",
        "BEGIN",
        "BETWEEN",
        "BREAK",
        "BROWSE",
        "BULK",
        "BY",
        "CASCADE",
        "CASE",
        "CATCH",
        "CHECK",
        "CHECKPOINT",
        "CLOSE",
        "CLUSTERED",
        "COALESCE",
        "COLLATE",
        "COLUMN",
        "COMMIT",
        "COMPUTE",
        "CONSTRAINT",
        "CONTAINS",
        "CONTAINSTABLE",
        "CONTINUE",
        "CONVERT",
        "CREATE",
        "CROSS",
        "CURRENT",
        "CURRENT_DATE",
        "CURRENT_TIME",
        "CURRENT_TIMESTAMP",
        "CURRENT_USER",
        "CURSOR",
        "DATE",
        "DATABASE",
        "DATEFORMAT",
        "DBCC",
        "DEALLOCATE",
        "DECLARE",
        "DEFAULT",
        "DELETE",
        "DENY",
        "DESC",
        "DISK",
        "DISTINCT",
        "DISTRIBUTED",
        "DOUBLE",
        "DROP",
        "DUMP",
        "ELSE",
        "END",
        "ERRLVL",
        "ESCAPE",
        "EXCEPT",
        "EXEC",
        "EXECUTE",
        "EXIT",
        "EXTERNAL",
        "FETCH",
        "FILE",
        "FILLFACTOR",
        "FOR",
        "FOREIGN",
        "FREETEXT",
        "FREETEXTTABLE",
        "FROM",
        "FULL",
        "FUNCTION",
        "GO",
        "GOTO",
        "GRANT",
        "GROUP",
        "HAVING",
        "HOLDLOCK",
        "IDENTITY",
        "IDENTITYCOL",
        "IDENTITY_INSERT",
        "IF",
        "INDEX",
        "INNER",
        "INSERT",
        "INTERSECT",
        "INTO",
        "JOIN",
        "KEY",
        "KILL",
        "LEFT",
        "LIKE",
        "LINENO",
        "LOAD",
        "MERGE",
        "NATIONAL",
        "NOCHECK",
        "NOLOCK",
        "NONCLUSTERED",
        "NULLIF",
        "OF",
        "OFF",
        "OFFSETS",
        "ON",
        "OPEN",
        "OPENDATASOURCE",
        "OPENQUERY",
        "OPENROWSET",
        "OPENXML",
        "OPTION",
        "ORDER",
        "OUTER",
        "OVER",
        "PARTITION",
        "PERCENT",
        "PIVOT",
        "PLAN",
        "PRECISION",
        "PRIMARY",
        "PRINT",
        "PROC",
        "PROCEDURE",
        "PUBLIC",
        "RAISERROR",
        "READ",
        "READTEXT",
        "RECONFIGURE",
        "REFERENCES",
        "REPLICATION",
        "RESTORE",
        "RESTRICT",
        "RETURN",
        "REVERT",
        "REVOKE",
        "RIGHT",
        "ROLLBACK",
        "ROWCOUNT",
        "ROWGUIDCOL",
        "RULE",
        "SAVE",
        "SCHEMA",
        "SECURITYAUDIT",
        "SELECT",
        "SEMANTICKEYPHRASETABLE",
        "SEMANTICSIMILARITYDETAILSTABLE",
        "SEMANTICSIMILARITYTABLE",
        "SESSION_USER",
        "SET",
        "SETUSER",
        "SHUTDOWN",
        "SOME",
        "STATISTICS",
        "SYSNAME",
        "TABLE",
        "TABLESAMPLE",
        "TEXTSIZE",
        "THEN",
        "TO",
        "TOP",
        "TRAN",
        "TRANSACTION",
        "TRIGGER",
        "TRUNCATE",
        "TRY_CONVERT",
        "TSEQUAL",
        "TYPE",
        "TRY",
        "UNION",
        "UNIQUE",
        "UNPIVOT",
        "UPDATE",
        "UPDATETEXT",
        "USE",
        "VALUES",
        "VARYING",
        "VIEW",
        "WAITFOR",
        "WHEN",
        "WHERE",
        "WHILE",
        "WITH",
        "WITHIN",
        "WRITETEXT",
        "BIGINT",
        "INT",
        "INTEGER",
        "SMALLINT",
        "TINYINT",
        "BIT",
        "DECIMAL",
        "NUMERIC",
        "MONEY",
        "SMALLMONEY",
        "FLOAT",
        "REAL",
        "DATETIME",
        "SMALLDATETIME",
        "CHAR",
        "VARCHAR",
        "TEXT",
        "NCHAR",
        "NVARCHAR",
        "NTEXT",
        "BINARY",
        "VARBINARY",
        "IMAGE",
        "CURSOR",
        "SQL_VARIANT",
        "TIMESTAMP",
        "UNIQUEIDENTIFIER",
        "INCLUDE",
        "STATISTICS_NORECOMPUTE",
        "SORT_IN_TEMPDB",
        "DROP_EXISTING",
        "ONLINE",
        "ALLOW_ROW_LOCKS",
        "ALLOW_PAGE_LOCKS",
        "FILLFACTOR",
        "PRIMARY",
        "STATISTICS",
        "PAD_INDEX",
        "IGNORE_DUP_KEY",
        "PRIMARY",
        "QUOTED_IDENTIFIER",
        "ANSI_NULLS",
        "NEXT",
        "OUTPUT",
        "LOCAL",
        "FORWARD_ONLY",
        "STATIC",
        "XML",
        "PATH",
        "NOCOUNT",
        "YMD",
    };

    public IEnumerator<string> GetEnumerator()
    {
        return _keywords.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _keywords.GetEnumerator();
    }
}
