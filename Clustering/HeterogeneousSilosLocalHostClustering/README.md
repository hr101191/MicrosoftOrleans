#Heterogeneous Silos Example
- This is an extension of the localhost clustering example. In an actual production example, a single silo which hosts all the grain is barely sufficient. Having the ability to host multiple instances of frequently used grains on selected silos will be the ideal setup

For more details about Heterogeneous silos, please check out this article:
https://dotnet.github.io/orleans/Documentation/clusters_and_clients/heterogeneous_silos.html

Try it out:
1) Start the Admin Silo
2) Start Silo Host Instance 1, 2 and 3 (Instance 1 and 2 hosts the HelloGrain, and instance 3 hosts the GoodbyeGrain)
3) Start ApiClient, and hit http://localhost:5001/api/hello and http://localhost:5001/api/goodbye. You should notice in the Orleans Dashboard (http://localhost:8080) that HelloGrain is actiavted on instance 1 or 2 while GoodbyeGrain is activated on instance 3
4) See the example (http://localhost:5001/api/hello-goodbye) for a simple example on how to communicate between grains.