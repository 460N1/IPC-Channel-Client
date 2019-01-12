using System;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Permissions;

public class Client
{
    [SecurityPermission(SecurityAction.Demand)] //wtf even is this
    public static void Main(string[] args)
    {
        // Krijo channel
        IpcChannel channel = new IpcChannel();

        // Regjistro channel, percakto sigurine
        System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(channel, false);

        // Regjistro se qka do dergohet, dhe ku
        System.Runtime.Remoting.WellKnownClientTypeEntry remoteType =
            new System.Runtime.Remoting.WellKnownClientTypeEntry(
                typeof(RemoteObject),
                "ipc://localhost:9090/RemoteObject.rem");
        System.Runtime.Remoting.RemotingConfiguration.
            RegisterWellKnownClientType(remoteType);
        // Krijo nje pool te mesazheve
        string objectUri;
        System.Runtime.Remoting.Messaging.IMessageSink messageSink =
            channel.CreateMessageSink(
                "ipc://localhost:9090/RemoteObject.rem", null,
                out objectUri);
        Console.WriteLine("URI e pool te mesazheve eshte {0}.",
            objectUri);
        if (messageSink != null){
            Console.WriteLine("Lloji i pool te mesazheve eshte {0}.",
                messageSink.GetType().ToString());
        }

        // Krijo instanc te RemoteObject
        RemoteObject service = new RemoteObject();

        // Thirr metoden e RemoteObject
        again:
        Console.WriteLine("Klienti po therret remote object.");
        Console.WriteLine("Remote objecti eshte thirrur "+ service.GetCount() + " here.");
        // Mundesi e thjeshte per te perseritur thirrjen
        Console.Write("Deshironi t'e perseritni? (1=po, 0=jo): ");
        int repeat = 1;
        try{repeat = Int32.Parse(Console.ReadLine());}
        catch{repeat = 1;}
        if (repeat==1) goto again;
    }
}
public class RemoteObject : MarshalByRefObject
{
    private int callCount = 0;
    public int GetCount(){
        Console.WriteLine("GetCount has been called.");
        callCount++; return (callCount);
    }
}