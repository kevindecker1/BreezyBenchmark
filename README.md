# BreezyBenchmark

BreezyBenchmark is a lightweight .Net package for easy performance testing on your code.

By default, the test summary will be printed to the Output console. There is an optional argument for writing the summary to a txt file. If set to true, it will launch automatically when the test(s) are complete.

There are 4 ways that you can utilize BreezyBenchmark to test your code.

1. Run a specific block of code 

![bbcodeblock](https://user-images.githubusercontent.com/10837928/147970097-467b118e-648d-4a92-a27f-2b767c9934b2.PNG)

2. Run a test on an URL. A  web request is created to call the URL

![bburl](https://user-images.githubusercontent.com/10837928/147970168-235d44ac-8893-49f2-960a-09a29bde06cd.PNG)

3. Add tests to an Instance. When added to an Instance, all tests are printed in the summary. This can be useful for code comparisons (Different code implementations for the same outcome). 
Note: BreezyBenchmarkRunner.Instance.Run needs to be called to execute the tests.

![bbinstance](https://user-images.githubusercontent.com/10837928/147970353-7ee45d2c-3922-4f66-a2ce-cdd24580e123.PNG)

4. Finally, you can run BreezyBenchmark on a class. The BreezyBenchmark attribute needs to be added to each method you wish to test, but this can be changed by setting the onlyRunTaggedMethods parameter in the constructor to false. This will test every method regardless.

![bbclass](https://user-images.githubusercontent.com/10837928/147970816-6eb0e504-01cc-49e6-a93f-614515cab80b.PNG)

![bbclass1](https://user-images.githubusercontent.com/10837928/147970824-728f2d11-65ff-428a-a4a4-602f8a5cbfeb.PNG)

When testing methods, you also have the option of specifying the param values for the test. Make sure the params are in the same order as they appear in the method

![bbparam](https://user-images.githubusercontent.com/10837928/147970997-436a81be-044d-40d6-b979-0d3104a7bf68.PNG)
