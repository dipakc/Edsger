// The module 'vscode' contains the VS Code extensibility API

'use strict';

//import { workspace, Disposable, ExtensionContext } from 'vscode';
//import { LanguageClient, LanguageClientOptions, SettingMonitor, ServerOptions, TransportKind, InitializeParams } from 'vscode-languageclient';
//import { Trace } from 'vscode-jsonrpc';
import * as vscode from "vscode";
import * as child from "child_process"
import * as fs from "fs";   

// this method is called when your extension is activated
export function activate(context: vscode.ExtensionContext) {

	console.log('EdsgerVSCodeClient is now active!');
	let edsgerPanel = vscode.window.createOutputChannel("EdsgerPanel");
	//Write to output.
	edsgerPanel.appendLine("EdsgerVSCodeClient is now active!");

	function executeTactic(tacticName: string) {
		edsgerPanel.appendLine(tacticName);

		var filepath = vscode.window.activeTextEditor?.document.uri.fsPath;
		var exepath = "/Users/dipakc/SourceRepos/Edsger/Source/EdsgerFSharp/bin/Debug/netcoreapp3.1/EdsgerFSharp.dll"

		var fullcmd = "dotnet " + exepath + " --file " + filepath + " --tactic " + tacticName

		var foo: child.ChildProcess = 
			child.exec(fullcmd, 
				(error: string, stdout: string, stderr: string) => {
					console.log(stdout);
					edsgerPanel.appendLine(stderr);
					edsgerPanel.appendLine(stdout);					
					if (error) {
						console.log('error:' + error);
					}
		        });
	}

	context.subscriptions.push(vscode.commands.registerCommand('EdsgerVSCodeClient.helloWorld', () => {
		// The code you place here will be executed every time your command is executed

		edsgerPanel.appendLine("Hello World from EdsgerVSCodeClient!");
		// Display a message box to the user
		vscode.window.showInformationMessage('Hello World from EdsgerVSCodeClient!');

		var foo: child.ChildProcess = 
			child.exec('pwd', (error: string, stdout: string, stderr: string) => {
				console.log(stdout);
				vscode.window.showInformationMessage(stdout);
	        });
	}));

	context.subscriptions.push(vscode.commands.registerCommand('EdsgerVSCodeClient.Synthesize', () => {
		executeTactic("Synthesize");
	}));
	
	context.subscriptions.push(vscode.commands.registerCommand('EdsgerVSCodeClient.ConjunctAsInv', () => {	
		executeTactic("ConjunctAsInv");
	}));

	context.subscriptions.push(vscode.commands.registerCommand('EdsgerVSCodeClient.Verify', () => {	
		executeTactic("Verify");
	}));

}

// this method is called when your extension is deactivated
export function deactivate() {}
