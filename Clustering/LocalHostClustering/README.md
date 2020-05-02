#LocalHost (In-memory) clustering

- An admin Silo is hosted to coordinate all the silo hosts (connected via port :11111) and clients (connected via port :30000)
- Load balancing, activation of grains (i.e creation of actors when request is triggered from client) will be taken care of by Orleans

Deployment order:
1) Admin Silo (dashboard will be hosted on http://localhost:8080)
2) Silo Host instance 1 and instance 2
3) Api Client

Try it out!
1) Run all projects according to the deployment order
2) Hit http:localhost:5001/api/hello and observe that a HelloWorldGrain is activated on either Silo Host instace 1 or instance 2 (navigate to Orleans Dashboard)
3) Shut down the instance which the HelloWorldGrain is created on and hit http:localhost:5001/api/hello again
4) HelloWorldGrain will be activated on the other Silo Host instance
5) Shut down all Silo Host instances, http:localhost:5001/api/hello should throw Internal Server Error
6) Start both Silo Host instances again, observe that http:localhost:5001/api/hello should function normally 
