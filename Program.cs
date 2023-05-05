using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.Linq;
using System.CommandLine.Rendering;
using System.CommandLine.IO;

// Create a list to store notes
var notes = new List<string>();

var addCommand = new Command("add", "Add a new note")
{
    new Argument<string>("content", "The content of the note")
};

var listCommand = new Command("list", "List all notes");

var removeCommand = new Command("remove", "Remove a note by ID")
{
    new Argument<int>("id", "The ID of the note to remove")
};

var searchCommand = new Command("search", "Search for notes by keyword")
{
    new Argument<string>("keyword", "The keyword to search for")
};

// Define command handlers
addCommand.Handler = CommandHandler.Create<string>((string content) =>
{
    notes.Add(content);
    Console.WriteLine("Note added successfully.");
});

listCommand.Handler = CommandHandler.Create(() =>
{
    Console.WriteLine("Notes:");
    for (int i = 0; i < notes.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {notes[i]}");
    }
});

removeCommand.Handler = CommandHandler.Create<int>((int id) =>
{
    if (id > 0 && id <= notes.Count)
    {
        notes.RemoveAt(id - 1);
        Console.WriteLine("Note removed successfully.");
    }
    else
    {
        Console.WriteLine("Invalid note ID.");
    }
});

searchCommand.Handler = CommandHandler.Create<string>((string keyword) =>
{
    Console.WriteLine($"Notes containing '{keyword}':");
    var results = notes.Where(note => note.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    for (int i = 0; i < results.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {results[i]}");
    }
});

// Build and invoke the parser
var rootCommand = new RootCommand("A simple note-taking CLI application.")
{
    addCommand,
    listCommand,
    removeCommand,
    searchCommand
};

var builder = new CommandLineBuilder(rootCommand);
builder.UseDefaults();
builder.UseHelp();
builder.UseVersionOption();
var parser = builder.Build();


var region = new Region(0, 0, Console.WindowWidth,Console.WindowHeight);
var console = new SystemConsole();
var consoleRenderer = new ConsoleRenderer(console, OutputMode.Ansi);

consoleRenderer.RenderToRegion(
                        $"{ForegroundColorSpan.Green()}Hello,{ForegroundColorSpan.Red()}world!{ForegroundColorSpan.Reset()}",
                        region);


//await parser.InvokeAsync(args);
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (input == "exit")
    {
        break;
    }

    await parser.InvokeAsync(input);
}