using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

internal class SyntaxWalker : CSharpSyntaxWalker
{
    private readonly IReadOnlyCollection<SymbolAnalyzer> _symbolAnalyzers;
    private readonly SemanticModel _semanticModel;
    private readonly ModelBuilder _modelBuilder;

    public SyntaxWalker(IReadOnlyCollection<SymbolAnalyzer> symbolAnalyzers, SemanticModel semanticModel,
        ModelBuilder modelBuilder, SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node)
        : base(depth)
    {
        _symbolAnalyzers = symbolAnalyzers;
        _semanticModel = semanticModel;
        _modelBuilder = modelBuilder;
    }

    public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitNamespaceDeclaration(node);
    }

    public override void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitFileScopedNamespaceDeclaration(node);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitClassDeclaration(node);
    }

    public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitRecordDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitStructDeclaration(node);
    }

    public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitEnumDeclaration(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitInterfaceDeclaration(node);
    }

    public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitDelegateDeclaration(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitMethodDeclaration(node);
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitPropertyDeclaration(node);
    }

    public override void VisitParameter(ParameterSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) ?? throw new InvalidOperationException();
        Analyze(symbol);
        base.VisitParameter(node);
    }

    public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        foreach (var variable in node.Variables)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(variable) ?? throw new InvalidOperationException();
            switch (symbol)
            {
                case IFieldSymbol fieldSymbol:
                    Analyze(fieldSymbol);
                    break;
                case ILocalSymbol localSymbol:
                    Analyze(localSymbol);
                    break;
            }
        }
        base.VisitVariableDeclaration(node);
    }

    private void Analyze<TSymbol>(TSymbol symbol)
        where TSymbol : ISymbol
    {
        foreach (var analyzer in _symbolAnalyzers.OfType<SymbolAnalyzer<TSymbol>>())
            analyzer.Analyze(symbol, _modelBuilder);
    }
}