using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using BuggyNet.PackageParser;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

public class ClientConnectionDispatcher {
    private readonly IPackageParser packageParser;
    private readonly ClientConnection clientConnection;
    private Thread thread;

    private ConcurrentDictionary<string, Tuple<EventWaitHandle, Holder>> waitingThread = 
        new ConcurrentDictionary<string, Tuple<EventWaitHandle, Holder>>();

    public event EventHandler<IncomingPackageArgs> IncomingPackage;

    public ClientConnectionDispatcher(IPackageParser packageParser, ClientConnection clientConnection) {
        this.packageParser = packageParser;
        this.clientConnection = clientConnection;

        IncomingPackage += ClientConnectionDispatcher_IncomingPackage;
    }
    private void ClientConnectionDispatcher_IncomingPackage(object sender, IncomingPackageArgs incomingPackage) {
        Debug.Log($"Incoming package: {incomingPackage.Package.GetType().Name}");
    }

    public void Start() {
        thread = new Thread(HandlePackageInput);
        thread.Start();
    }
    
    private void HandlePackageInput(object obj) {
        Package package = packageParser.ParsePackageFromStream(clientConnection.Reader);
        IncomingPackage?.Invoke(this, new IncomingPackageArgs(package));
        foreach (var keyValuePair in waitingThread) {
            if (!keyValuePair.Key.Equals(package.GetType().Name))
                continue;
            keyValuePair.Value.Item2.Value = package;
            keyValuePair.Value.Item1.Set();
        }
    }

    public Task<T> WaitForPackage<T>() where T : Package {
        var eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        waitingThread.TryAdd(typeof(T).Name, new Tuple<EventWaitHandle, Holder>(eventWaitHandle, new Holder()));
        Task<T> task = Task.Run(() => {
            eventWaitHandle.WaitOne(TimeSpan.FromSeconds(30));
            waitingThread.TryRemove(typeof(T).Name, out Tuple<EventWaitHandle, Holder> tuple);
            return (T)tuple.Item2.Value;
        });
        return task;
    }

    public void SendPackage(Package package) {
        packageParser.ParsePackageToStream(package, clientConnection.Writer);
    }
}
public class IncomingPackageArgs : EventArgs {
    public Package Package { get; private set; }
    public IncomingPackageArgs() {}
    public IncomingPackageArgs(Package package) {
        this.Package = package;
    }
}

public class IncomingPackage {
    public Package Package { get; set; }
}

public class Holder {
    public object Value { get; set; }
}