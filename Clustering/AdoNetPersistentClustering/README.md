#AdoNet Clustering

-This clustering example uses the OrleansMembershipTable in a database of your choice to synchronize the state of the silo hosts

Pre-requisite
1) SQL Connection (MS SQLServer, Oracle, MySQL or PostgreSQL)

Databse Setup:
1) Run the scripts in the SQL folder of any Silo Project

Try it out:
1) Start the silo hosts in any order. Cluster is coordinate by the state which is updated in the database table "OrleansMembershipTable"
2) Start ApiClient

Additional notes:
- Grains (by function) are hosted can be hosted across different Silos and scaled as per requirement. For more details about Heterogeneous silos, please check out this article: https://dotnet.github.io/orleans/Documentation/clusters_and_clients/heterogeneous_silos.html
