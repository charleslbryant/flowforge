I want a workflow that I can have multiple bots using the claude code sdk that I can train to execute various stages of a development process. I might have a PRD bot to create a PRD, a design bot to create a design, a dev bot to create a dev, and a QA bot to create a QA, and a release bot to create a release.

I envision the workflow operating like a pipeline that takes input from user or another bot and then executes its directive in system prompt and then outputs its resuts to the next bot in the pipeline or to the user.

We measure throughput, latency, cost,and accuracy of the pipeline.