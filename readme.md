This is a sample project intended to be roughly similar to a line-of-business application.

# Why?

A lot of the example applications we find on the internet are Todo apps, social apps (twitter/fb clones), and blogs. On the one hand, some of these are too simple to map to realistic domains, on the other there is a weighting towards consumer apps or apps with large, pooled userbases.

Line of business apps make up a huge part of what's out there, helping small business run their inventory and finances, plan production, and more. In some cases these are single client, in-house apps, but often they are Software-as-a-Service applications. The scaling and architecture challenges of these applications tend to focus more on:

* Importing and exporting data in a million artisinal shapes, quickly
* Tenant data (and sometimes process) partioning
* High dollar contracts, for small end-user counts

Where a massive consumer application can A/B test or canary deploy changes, in many LOB platforms millions in revenue can look like only a few hundred active users/week. 

The focus can be very different, changing how we think about user-to-user interaction, performance, consistency, auditability, provable correctness, and more.

# But this is still a sample app

The challenge is we often can't open source these types of application to use as examples, so we have to simplify the problem space and still try to expose some of those complexities.

This is not a real application.

I based this off a [React Admin example site](https://marmelab.com/react-admin/Demos.html), which in this case appears to be a mix of backend concerns behind a storefront that includes product administration, order status views, and order review approval. Think of it as a narrow, squished together view of an ERP and ecommerce admin site.

The example data structures were optimized to make it look very easy to wire up an interface (their specialty), and I tried not to completely recreate the world. That means I did lightly redesign some of the data structures (and may do more, we'll see), I held back on completely reworking it to better separate ERP/WMS/CMS concerns and add in a lot of the missing auditability and such that you would expect.

# What is real-ish so far

The patterns that are intended to be more notable/realistic in this are:

### Monolith
The front-end and back-end are a single repo and deployable artifact b/c we would expect this to be maintained by a single team, not a separated front-end and back-end team. 

If we were going to grow this fake organization/app, other teams would likely be focused on the major WMS and processing pieces that are missing from this sample app. For instance, there's a whole logistics and WMS event pipeline that I'd expect to see providing the projected stock quantities coming in. There's an integration to an ERP or similar that would be the true owner of an API to accept new orders, update statuses on them as it interacts with a WMS or other logistics app, etc. If we consumed those offerings, then we would either split the team along those lines and decide whetehr to split the monolith at that point, or have teams own carved out verticals inside the one app.

### DB Migrations

This application is built for mid-day, zero-downtime deploys via a Continuous Delivery pipeline. Fast feedback, frequent deploys should be the default for any organization doing new work these days (See Accelerate, recent Devops reports, etc).

Secondarily, these migrations should be as easy and lightweight as possible for developers: good developer experience makes it easier and faster to work with a system, ensuring a pit of success where it's easier to follow good patterns and move quickly than not.

### Hexagonal Back-end

The backend service is setup arounf a hexagonal structure. The core is business logic (which, again, is fairly light in this example) and which manages not only the how and what of our domain logic, but also manages the transaction scope of persistence, would be responsible for producing or acting on external events/logs, etc.

On one side of this core, we have persistence via a set of Persistence objects following a basic repository pattern that handles the translation between our more object-y application and the more record-based SQL database.

On the other side of this core, we have partioned APIs to make data available to the front-end, and eventually tooling for sample data, connectivity from other trusted services, etc.

### Partioned APIs

Many folks prefer a single API per service, with business apps I have found that have a minimal, clean API for each purpose (front-end, internal trusted services, public API via a proxy service, etc) make it easier to understand the impact of internal change son external facing APIs and manage the lifecycles explicitly and customize how they are versioned/handled to the consumer type. It also means that an API that is allowed to do things on behalf of users, like an internal service-to-service one, doesn't mix with one that should never allow imprsonation like a front-end app one.

### API->DB Integration Tests

There is a strong pattern of integration testing in this example application, as I've found having easy to use, non-fragile API->DB integration tests is really handy in LOB applications. Typically 2/3s or more of the application will be basic CRUD functionality, which is easy to TDD and shape a clean external interface for when you have a good set of integration tests.

The hard part of this is usually in the setup, management, and clean-up of sample data.

The pattern I'm using here has evolved over multiple systems, and mirrors what I used at ledgex throughout the suite to support tests interacting with multi- and single- tenant paritiioned DBs with almost no test failures over 18 months due to cross-talk, bad clean-up, etc.

A lot of the same tooling used to reset data to a clean state also benefits work for sample environments and provisioning (for multi-tenant/SaaS setups) and the fact that it's used by the integration tests in realtime locally helps bring it to our attention immediately if we haven't included an update to the process when making other changes. 

### DTOs, SummaryDTOs, _Actions_, and Models

I have separate 4 data modeling concerns and used consistent naming in the application to try and keep them separate:

* DTOs - these are the domain language, as an aggregate root that is a cacheable unit (an OrderLine will never be independently mutated, only as part of an Order)
* SummaryDTOs - these summarize DTOs and often combine properties from related ones - they are not cacheable because they are merging multiple atomic pieces of data that have indepent lifetimes
* _Actions_ - These are actions the user or an external system is applying to a DTO. Instead of filling out a full DTO and handing it to us to try and figure out what changed, these mirror only the properties that the user is applying, with some identifying information. Are they on a create screen? They shouldn't be sending us values like what time they created a record (don't get me started on 3rd party timestamps, either). Are they editing one field at a time (should be applied as narrow mutations) or saving a whole screen (should all be submitted as what the user has decided to apply). It also preserve the user intent for even clearr audit trails and logic to explain the history of an item. And also we can have more specific, cleaner logic in the business layer to perform the _Action_ instead of trying to make a mega function that can do anything and then try to back back into the user intent (ex: most Unit of Work implementations).
* Models - sometimes our API needs to talk a different language then the internal ones, Models can be transformations, enveleopes that carry more information, or narrow APIs for command parameter shapes

### Realtime Test Results

...

### more notes coming

# Questions

### Where's your database indexes?

Indeed. With B2B apps, especially if they are single-tenant partitioned DBs, I prefer to start without explicit indexes until we have a good handle on the shape of the database, the query patterns we'll need, and so on. For example, at Ledgex we put off indexing one of our common queries (find the latest reviewed revision of a record) and realized after a point in time that we needed that information so often, that it was better to record the answer at write time on the record and be able to do a direct join then to have an index. We did this by holding off on the performance work until the problem was well defined and we had hit our performance limit on one or more screens, rather than indexing from the very beginning.

Also, once you put in indexes then you also need to spend some time reviewing if you're still using them, etc. If you didn't need them yet, now you have two things you didn't need yet.
