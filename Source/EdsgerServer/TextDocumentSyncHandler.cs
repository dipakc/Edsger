using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace EdsgerServer
{
  internal class TextDocumentSyncHandler: ITextDocumentSyncHandler
  {
    public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }

    TextDocumentChangeRegistrationOptions IRegistration<TextDocumentChangeRegistrationOptions>.GetRegistrationOptions()
    {
      throw new System.NotImplementedException();
    }

    public void SetCapability(SynchronizationCapability capability)
    {
      throw new System.NotImplementedException();
    }

    public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }

    TextDocumentRegistrationOptions IRegistration<TextDocumentRegistrationOptions>.GetRegistrationOptions()
    {
      throw new System.NotImplementedException();
    }

    public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }

    public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }

    TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions>.GetRegistrationOptions()
    {
      throw new System.NotImplementedException();
    }

    public TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
    {
      throw new System.NotImplementedException();
    }
  }
}