# BreezyBenchmark

BreezyBenchmark is a lightweight .Net package for easy performance testing on your code.

By default, the test summary will be printed to the Output console. There is an optional argument for writing the summary to a txt file. If set to true, it will launch automatically when the test(s) are complete.

Also by default, the tests are run on their own thread, so that we can attempt to get the most accurate results on just the code we want to test, and not worry about any main application threads that these tests will be running from.

There are 4 ways that you can utilize BreezyBenchmark to test your code.

1. Run a specific block of code 

![bbcodeblock](https://user-images.githubusercontent.com/10837928/147970097-467b118e-648d-4a92-a27f-2b767c9934b2.PNG)

2. Run a test on an URL. A  web request is created to call the URL

![bburl](https://user-images.githubusercontent.com/10837928/147970168-235d44ac-8893-49f2-960a-09a29bde06cd.PNG)

3. Add tests to an Instance. When added to an Instance, all tests are printed in the summary. This can be useful for code comparisons (Different code implementations for the same outcome). 
Note: BreezyBenchmarkRunner.Instance.Run needs to be called at the end to execute the tests.

![bbinstance1](https://user-images.githubusercontent.com/10837928/147971717-8c6fc2ce-fde4-4046-ae40-43c0f9597396.PNG)

Here is what the summary would look like as the generated txt file for the above Instance tests. Both methods were fast, but the StringBuilder was slightly quicker. However, it used a little more memory.

![bbsummary1](https://user-images.githubusercontent.com/10837928/147971824-49699982-1cea-4fb0-ac6f-5f2d655a6417.PNG)

4. Finally, you can run BreezyBenchmark on a class. The BreezyBenchmark attribute needs to be added to each method you wish to test, but this can be changed by setting the onlyRunTaggedMethods parameter in the method definition to false. This will test every method regardless.

![bbclass](https://user-images.githubusercontent.com/10837928/147970816-6eb0e504-01cc-49e6-a93f-614515cab80b.PNG)

![bbclass1](https://user-images.githubusercontent.com/10837928/147970824-728f2d11-65ff-428a-a4a4-602f8a5cbfeb.PNG)

When testing methods, you also have the option of specifying the param values for the test. Make sure the params are in the same order as they appear in the method

![bbparam](https://user-images.githubusercontent.com/10837928/147970997-436a81be-044d-40d6-b979-0d3104a7bf68.PNG)
