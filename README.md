
# ![image](https://user-images.githubusercontent.com/6662454/167391357-edb02ce2-a63c-439b-be9b-69b4b4796b1c.png) WebReaper


Declarative extensible web scraper in C# with focused web crawler. Easly crawl any web site and parse the data, save structed result to a file, DB, etc.

:exclamation: This is work in progres! API is not stable and will change.

## 📋 Example:

```c#
await new Scraper()
    .WithStartUrl("https://rutracker.org/forum/index.php?c=33")
    .FollowLinks("#cf-33 .forumlink>a") // first level links
    .FollowLinks(".forumlink>a").       // second level links
    .FollowLinks("a.torTopic", ".pg").  // third level links to target pages
    .WithScheme(new Schema {
        new("name", "#topic-title"),
        new("category", "td.nav.t-breadcrumb-top.w100.pad_2>a:nth-child(3)"),
        new Url("torrentLink", ".magnet-link"), // get a link from <a> HTML tag (href attribute)
        new Image("coverImageUrl", ".postImg")  // get a link to the image from HTML <img> tag (src attribute)
    })
    .WriteToJsonFile("result.json")
    .Build()
    .Run();
```

## Features:

* :zap: It's fast
* 🗒 Easy declarative parsing:  new Schema { new("field", ".selector") }
* :page_facing_up: Pagination support:  .FollowLinks("a", ".paginationSelector")
* Saving data to any sinks such as file, database or API
* :earth_americas: Distributed crawling support: provide your implementation of IJobQueueReader, IJobQueueWriter and ICrawledLinkTracker and run your crawler on ony cloud VM, serverless function, on-prem servers, etc
* :octopus: Crowling and parsing Single Page Applications

## Coming soon:

- [ ] Proxy support
- [ ] Azure functions for the distributed crawling
- [ ] Request throttling
- [ ] Autotune for parallelism degree and throttling
- [ ] Ports to NoneJS and GO

